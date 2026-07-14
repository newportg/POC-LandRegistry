using AutoMapper;
//using Azure.Identity;
//using Azure.Security.KeyVault.Certificates;
//using Azure.Security.KeyVault.Secrets;
using KnightFrank.Hub.LandRegistry.Common.Models;
using KnightFrank.Hub.LandRegistry.Service.Requests;
using KnightFrank.Hub.LandRegistry.Service.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace KnightFrank.Hub.LandRegistry.Service
{
    public class LandRegistrySvc : ILandRegistrySvc
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LandRegistrySvc> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly ICertificateProvider _certificateProvider;
        private readonly X509Certificate2? _clientCertificate;
        private readonly IServiceProvider _serviceProvider;

        public LandRegistrySvc(IMapper mapper, ILogger<LandRegistrySvc> logger, ICertificateProvider certificateProvider, IServiceProvider serviceProvider)
        {
            _mapper = mapper;
            _logger = logger;
            _certificateProvider = certificateProvider ?? throw new ArgumentNullException(nameof(certificateProvider));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(2, retryAttempt =>
                {
                    var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                    _logger.LogInformation($"Waiting {timeToWait.TotalSeconds} seconds");
                    return timeToWait;
                });

            //            // Load certs
            //            if( !LoadCert())
            //            {
            //                throw new Exception("Unable to load Certificate.");
            //            }
            // Load cert (sync-block to keep ctor simple). Prefer lazy/async initialization if your app supports it.
            var certName = Environment.GetEnvironmentVariable("CertName");
            if (!string.IsNullOrEmpty(certName))
            {
                _clientCertificate = _certificateProvider.GetCertificateAsync(certName).GetAwaiter().GetResult();
                if (_clientCertificate == null)
                {
                    throw new Exception("Unable to load Certificate.");
                }
            }
            else
            {
                _logger.LogWarning("CertName environment variable not set; no client certificate loaded.");
            }

            _logger.LogInformation("Land Registry initialized");
        }

        public async Task<LandRegistryDto> FindProperty(LandRegistryDto request)
        {
            //ServiceFactory factory = ServiceFactory.GetServiceFactory(request);


            ServiceFactory factory = GetServiceFactory(request);
            LandRegistry landRegistry = factory.GetService();

            try
            {
                landRegistry.Map(request);          // Map the Request to the Service XML
                landRegistry.Validate();            // Validate the xml 
                await _retryPolicy.ExecuteAsync(async () => await landRegistry.Request()); // Interact with the LandRegistry service

                return landRegistry.Response();     // maps the response xml into a json DTO - dont understand just map
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                if( ex.InnerException != null )
                    _logger.LogError(ex.InnerException.Message);
                request.SystemError = new Error
                {
                    Description = ex.Message
                };
                return request;
            }
        }

        //--------------------------
        // Helpers

        public ServiceFactory GetServiceFactory(LandRegistryDto request)
        {
            // Resolve concrete factory instances from DI so all request objects are DI-managed.
            return request.RequestType switch
            {
                RequestTypes.ApplicationEnquiry => _serviceProvider.GetRequiredService<ApplicationEnquiryFactory>(),
                RequestTypes.LCBankruptcySearch => _serviceProvider.GetRequiredService<LLCBankruptySearchFactory>(),
                RequestTypes.DischargeActivity => _serviceProvider.GetRequiredService<DischargeActivityFactory>(),
                RequestTypes.EnquiryByPropertyDescription => _serviceProvider.GetRequiredService<EnquiryByPropertyDescriptionFactory>(),
                RequestTypes.LCFullSearch => _serviceProvider.GetRequiredService<LCFullSearchFactory>(),
                RequestTypes.OfficialCopyTitleKnown => _serviceProvider.GetRequiredService<OfficialCopyTitleKnownFactory>(),
                RequestTypes.OfficialSearchWhole => _serviceProvider.GetRequiredService<OfficialSearchWholeFactory>(),
                RequestTypes.OfficialSearchPart => _serviceProvider.GetRequiredService<OfficialSearchPartFactory>(),

                RequestTypes.PollApplicationEnquiry => _serviceProvider.GetRequiredService<PollApplicationEnquiryFactory>(),
                RequestTypes.PollLCBankruptcySearch => _serviceProvider.GetRequiredService<PollLLCBankruptySearchFactory>(),
                RequestTypes.PollDischargeActivity => _serviceProvider.GetRequiredService<PollDischargeActivityFactory>(),
                RequestTypes.PollEnquiryByPropertyDescription => _serviceProvider.GetRequiredService<PollPropertyDescriptionEnquiryFactory>(),
                RequestTypes.PollLCFullSearch => _serviceProvider.GetRequiredService<PollLCFullSearchFactory>(),
                RequestTypes.PollOfficialSearchWhole => _serviceProvider.GetRequiredService<PollOfficialSearchWholeFactory>(),
                RequestTypes.PollOfficialSearchPart => _serviceProvider.GetRequiredService<PollOfficialSearchPartFactory>(),

                _ => _serviceProvider.GetRequiredService<EnquiryByPropertyDescriptionFactory>(),
            };
        }

        //public ServiceFactory GetServiceFactory(LandRegistryDto request)
        //{
        //    return request.RequestType switch
        //    {
        //        // Interactive requests
        //        RequestTypes.ApplicationEnquiry => new ApplicationEnquiryFactory(_mapper, _logger, _clientCertificate),
        //        RequestTypes.LCBankruptcySearch => new LLCBankruptySearchFactory(_mapper, _logger),
        //        RequestTypes.DischargeActivity => new DischargeActivityFactory(_mapper, _logger),
        //        RequestTypes.EnquiryByPropertyDescription => new EnquiryByPropertyDescriptionFactory(_mapper, _logger),
        //        RequestTypes.LCFullSearch =>  new LCFullSearchFactory(_mapper, _logger),
        //        RequestTypes.OfficialCopyTitleKnown => new OfficialCopyTitleKnownFactory(_mapper, _logger),
        //        RequestTypes.OfficialSearchWhole => new OfficialSearchWholeFactory(_mapper, _logger),
        //        RequestTypes.OfficialSearchPart => new OfficialSearchPartFactory(_mapper, _logger),

        //        // Polling requests
        //        RequestTypes.PollApplicationEnquiry => new PollApplicationEnquiryFactory(_mapper, _logger),
        //        RequestTypes.PollLCBankruptcySearch => new PollLLCBankruptySearchFactory(_mapper, _logger),
        //        RequestTypes.PollDischargeActivity => new PollDischargeActivityFactory(_mapper, _logger),
        //        RequestTypes.PollEnquiryByPropertyDescription => new PollPropertyDescriptionEnquiryFactory(_mapper, _logger),
        //        RequestTypes.PollLCFullSearch => new PollLCFullSearchFactory(_mapper, _logger),
        //        RequestTypes.PollOfficialSearchWhole => new PollOfficialSearchWholeFactory(_mapper, _logger),
        //        RequestTypes.PollOfficialSearchPart => new PollOfficialSearchPartFactory(_mapper, _logger),

        //        // Default
        //        _ => new EnquiryByPropertyDescriptionFactory(_mapper, _logger),
        //    };
        //}

        //private void LoadCertChain()
        //{
        //    //X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
        //    //store.Open(OpenFlags.ReadOnly);
        //    //var col = store.Certificates.Find(X509FindType.FindByIssuerName, "Land Registry CA", false);
        //    //_logger.LogInformation("Root :" + Convert.ToBase64String(col[0].Export(X509ContentType.Cert)));
        //    //store.Close();

        //    //store = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser);
        //    //store.Open(OpenFlags.ReadOnly);
        //    //col = store.Certificates.Find(X509FindType.FindByThumbprint, "8296b1a212c3b8c1d3e6972e955f7d4f04cf0cd5", false);
        //    //_logger.LogInformation("2018 :" + Convert.ToBase64String(col[0].Export(X509ContentType.Cert)));
        //    //store.Close();

        //    //store = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser);
        //    //store.Open(OpenFlags.ReadOnly);
        //    //col = store.Certificates.Find(X509FindType.FindByThumbprint, "4b3432ad2bf8c36d06f5ffb9939047a6813378fd", false);
        //    //_logger.LogInformation("2020 :" + Convert.ToBase64String(col[0].Export(X509ContentType.Cert)));
        //    //store.Close();

        //    //LoadCert(StoreName.Root, Environment.GetEnvironmentVariable("LandRegistryRootCA"));
        //    //LoadCert(StoreName.CertificateAuthority, Environment.GetEnvironmentVariable("LandRegistry2020IssuingCA"));
        //    ////LoadCert(StoreName.CertificateAuthority, Environment.GetEnvironmentVariable("LandRegistry2018IssuingCA"));
        //    //LoadCert(StoreName.My, Environment.GetEnvironmentVariable("LandRegistryBGVendor"));

        //    LoadCert();
        //}

        //private static void LoadCert(StoreName store, string cert)
        //{
        //    // Create cert from Pem
        //    var bytes = Convert.FromBase64String(cert);
        //    var certificate = new X509Certificate2(bytes, string.Empty);

        //    // Save into store
        //    X509Store certStore = new X509Store(store, StoreLocation.CurrentUser);
        //    certStore.Open(OpenFlags.ReadWrite);
        //    certStore.Add(certificate);
        //    certStore.Close();
        //}

        //private static string ExportToPEM(X509Certificate2 cert)
        //{
        //    StringBuilder builder = new StringBuilder();

        //    builder.AppendLine("-----BEGIN CERTIFICATE-----");
        //    builder.AppendLine(Convert.ToBase64String(cert.Export(X509ContentType.Cert)));   //, Base64FormattingOptions.InsertLineBreaks); 
        //    builder.AppendLine("-----END CERTIFICATE-----");

        //    return builder.ToString();
        //}

        //private static X509Certificate2 GetCertificate(string thumbprint, ILogger log)
        //{
        //    if (string.IsNullOrEmpty(thumbprint))
        //    {
        //        log.LogError("GetCertificate: Thumbprint Cannot be null");
        //        return null;
        //    }

        //    X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //    try
        //    {
        //        store.Open(OpenFlags.ReadOnly);
        //        log.LogInformation("Enumerating certificates");
        //        foreach (var cert in store.Certificates)
        //        {
        //            log.LogInformation(cert.Subject);
        //        }
        //        var col = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
        //        if (col == null || col.Count == 0)
        //        {
        //            return null;
        //        }
        //        return col[0];
        //    }
        //    finally
        //    {
        //        store.Close();
        //    }
        //}

        //private string SerializeObject<T>(T obj)
        //{
        //    _logger.LogInformation("SerializeObject");

        //    try
        //    {
        //        string xmlString = null;
        //        MemoryStream memoryStream = new MemoryStream();
        //        XmlSerializer xs = new XmlSerializer(typeof(T));
        //        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
        //        xs.Serialize(xmlTextWriter, obj);
        //        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
        //        xmlString = ByteArrayToUTF8String(memoryStream.ToArray()); return xmlString;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        //private string ByteArrayToUTF8String(byte[] characters)
        //{
        //    UTF8Encoding encoding = new UTF8Encoding();
        //    string constructedString = encoding.GetString(characters);
        //    return (constructedString);
        //}


        //private BasicHttpBinding Binding()
        //{
        //    var myBinding = new BasicHttpBinding
        //    {
        //        Name = "CertPortBinding",
        //        CloseTimeout = TimeSpan.FromMinutes(1),
        //        OpenTimeout = TimeSpan.FromMinutes(1),
        //        ReceiveTimeout = TimeSpan.FromMinutes(10),
        //        SendTimeout = TimeSpan.FromMinutes(1),
        //        BypassProxyOnLocal = false,
        //        MaxBufferPoolSize = 524288,
        //        MaxReceivedMessageSize = 65536000,
        //        TextEncoding = Encoding.UTF8,
        //        UseDefaultWebProxy = true
        //    };
        //    myBinding.Security.Mode = BasicHttpSecurityMode.Transport; // SecurityMode.Transport;
        //    myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
        //    myBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
        //    myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName; //MessageCredentialType.UserName;
        //    myBinding.ReaderQuotas.MaxDepth = 32;
        //    myBinding.ReaderQuotas.MaxStringContentLength = 65536000;
        //    myBinding.ReaderQuotas.MaxArrayLength = 16384;
        //    myBinding.ReaderQuotas.MaxBytesPerRead = 4096;
        //    myBinding.ReaderQuotas.MaxNameTableCharCount = 16384;

        //    return myBinding;
        //}

        //private HttpClientHandler GetHttpClientHandler()
        //{
        //    _logger.LogInformation("GetHttpClientHandler");
        //    _logger.LogInformation("LandRegistry_Certificates :" + Environment.GetEnvironmentVariable("LandRegistryCertificates"));

        //    var credentials = new NetworkCredential(Environment.GetEnvironmentVariable("LandRegistryUserId"), Environment.GetEnvironmentVariable("LandRegistryPassword"));
        //    var cert = GetCertificate(Environment.GetEnvironmentVariable("LandRegistryCertificates"), _logger);
        //    if (cert != null)
        //    {
        //        _logger.LogInformation("Certificate chain valid ? :" + cert.Verify().ToString());
        //        _logger.LogInformation("Private Key :" + cert.PrivateKey.ToString());

        //        var httpClientHandler = new HttpClientHandler();
        //        httpClientHandler.ClientCertificates.Add(cert);
        //        httpClientHandler.Credentials = credentials;

        //        return httpClientHandler;
        //    }

        //    return null;
        //}

        //private bool LoadCert()
        //{
        //    _logger.LogInformation("LoadCert");


        //    // Does it exist already
        //    //var rtn = GetCertificate(Environment.GetEnvironmentVariable("LandRegistryCertificates"));
        //    //if (rtn != null)
        //    //{
        //    //    _logger.LogInformation("Certificate already exist");
        //    //    _logger.LogInformation($"Expires : {CalculateExpirationTime(DateTime.Parse(rtn.GetExpirationDateString()))}");

        //    //    return true;
        //    //}

        //    // Load the certificate from the secret value
        //    _logger.LogInformation($"LoadCert : CertName : {Environment.GetEnvironmentVariable("CertName")}");

        //    var cert = KeyVault(Environment.GetEnvironmentVariable("CertName"));
        //    _logger.LogInformation($"LoadCert : Cert :{cert}");

        //    if (cert != null)
        //    {
        //        // Save Cert
        //        if( !SaveCert(cert, Environment.GetEnvironmentVariable("LandRegistryCertificates")))
        //        {
        //            return false;
        //        }
        //        _logger.LogInformation($"Expires : {CalculateExpirationTime(DateTime.Parse(cert.GetExpirationDateString()))}");
        //        return true;
        //    }
        //    else
        //    {
        //        _logger.LogError($"Certificate with thumbprint {Environment.GetEnvironmentVariable("LandRegistryCertificates")} was not found");
        //        return false;
        //    }
        //}

        //private bool SaveCert(X509Certificate2 cert, string certThumbprint)
        //{
        //    _logger.LogInformation("SaveCert");
        //    if (cert == null || string.IsNullOrEmpty(certThumbprint))
        //    {
        //        _logger.LogInformation("SaveCert : Invalid parameter");
        //        return false; 
        //    }

        //    // Does it exist already
        //    var rtn = GetCertificate( certThumbprint);
        //    if (rtn != null) 
        //    {
        //        _logger.LogInformation("Certificate already exist");
        //        return true;
        //    }

        //    try
        //    {
        //        // Save It
        //        using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
        //        {
        //            _logger.LogInformation("SaveCert : certStore");

        //            certStore.Open(OpenFlags.ReadWrite);

        //            // N.B
        //            // certStore.Certificates.Add() will save to memory
        //            // certStore.Add(cert) will save to certificate store - need to verify that the az Func still work.
        //            //var idx = certStore.Certificates.Add(cert);
        //            //_logger.LogInformation($"Certificate stored at idx : {idx}");
        //            certStore.Add(cert);
        //            certStore.Close();

        //            // lets make sure we can find it.
        //            rtn = GetCertificate(certThumbprint);

        //            if (rtn != null)
        //            {
        //                _logger.LogInformation("SaveCert : cert true");
        //                return true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"SaveCert : {ex.Message}");
        //    }

        //    _logger.LogError($"Couldnt store certificate");
        //    return false;
        //}
        //private X509Certificate2? GetCertificate(string certThumbprint)
        //{
        //    _logger.LogInformation("GetCertificate");
        //    if ( string.IsNullOrEmpty(certThumbprint))
        //    {
        //        _logger.LogInformation("GetCertificate : Invalid parameter");
        //        return null;
        //    }

        //    var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //    _logger.LogInformation("GetCertificate : certStore");

        //    certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        //    X509Certificate2Collection certCollection = certStore.Certificates
        //        .Find(X509FindType.FindByThumbprint, certThumbprint, false);

        //    _logger.LogInformation($"GetCertificate : certCollection {certCollection.Count}");
        //    certStore.Close();

        //    foreach (var certificate in certCollection)
        //    {
        //        _logger.LogInformation($"GetCertificate : {certificate.Thumbprint}");
        //    }

        //    // Get the first cert with the thumbprint
        //    X509Certificate2? cert = certCollection.OfType<X509Certificate2>().FirstOrDefault();
        //    if (cert is null)
        //    {
        //        _logger.LogInformation("GetCertificate : cert is null");
        //        return null;
        //        //throw new Exception($"Certificate with thumbprint {certThumbprint} was not found");
        //    }

        //    // Use certificate
        //    _logger.LogInformation($"Issuer : {cert.Issuer}");
        //    _logger.LogInformation($"FriendlyName : {cert.FriendlyName}");
        //    return cert;

        //    //using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
        //    //{
        //    //    _logger.LogInformation("GetCertificate : certStore");

        //    //    certStore.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

        //    //    X509Certificate2Collection certCollection = certStore.Certificates.Find(
        //    //                                X509FindType.FindByThumbprint, certThumbprint, false);

        //    //    _logger.LogInformation($"GetCertificate : certCollection {certCollection.Count}");
        //    //    certStore.Close();

        //    //    foreach (var certificate in certCollection)
        //    //    {
        //    //        _logger.LogInformation($"GetCertificate : {certificate.Thumbprint}");
        //    //    }

        //    //    // Get the first cert with the thumbprint
        //    //    X509Certificate2? cert = certCollection.OfType<X509Certificate2>().FirstOrDefault();
        //    //    if (cert is null)
        //    //    {
        //    //        _logger.LogInformation("GetCertificate : cert is null");
        //    //        return null;
        //    //        //throw new Exception($"Certificate with thumbprint {certThumbprint} was not found");
        //    //    }

        //    //    // Use certificate
        //    //    _logger.LogInformation($"Issuer : {cert.Issuer}");
        //    //    _logger.LogInformation($"FriendlyName : {cert.FriendlyName}");
        //    //    return cert;
        //    //}
        //}
        //public X509Certificate2? KeyVault(string key)
        //{
        //    if (string.IsNullOrEmpty(key))
        //        return null;

        //    _logger.LogInformation($"KeyVault : {key}");

        //    try
        //    {
        //        // Initialize AzureServiceTokenProvider for Managed Identity
        //        var credential = new DefaultAzureCredential();
        //        _logger.LogInformation($"KeyVault : DefaultAzureCredential : {credential}");

        //        // Create a SecretClient to interact with Key Vault
        //        _logger.LogInformation($"KeyVault : KeyVaultUri : {Environment.GetEnvironmentVariable("KeyVaultUri")}");
        //        var keyVaultUri = new Uri(Environment.GetEnvironmentVariable("KeyVaultUri"));
        //        var certClient = new CertificateClient(keyVaultUri, credential);
        //        _logger.LogInformation($"KeyVault : certClient : {certClient}");

        //        // Retrieve the certificate secret from Key Vault
        //        KeyVaultCertificateWithPolicy certificate = certClient.GetCertificate(key);

        //        // Check if the private key is exportable
        //        if (certificate.Policy?.Exportable == true)
        //        {
        //            _logger.LogInformation("KeyVault : Exportable");

        //            // Retrieve the secret containing the private key
        //            var secretClient = new SecretClient(keyVaultUri, credential);
        //            KeyVaultSecret secret = secretClient.GetSecretAsync(key).Result;

        //            _logger.LogInformation($"KeyVault : secret.Value : {secret.Value}");

        //            // Convert base64-encoded secret value to bytes
        //            byte[] pfxBytes = Convert.FromBase64String(secret.Value);

        //            // VerifyCertificate()
        //            if (VerifyCertificate(pfxBytes))
        //            {
        //                // Create an X509Certificate2 object from the PFX bytes
        //                return new X509Certificate2(pfxBytes, string.Empty, X509KeyStorageFlags.Exportable);
        //            }
        //            return new X509Certificate2(pfxBytes, string.Empty, X509KeyStorageFlags.Exportable);
        //            //                    return null;
        //        }
        //        else
        //        {
        //            _logger.LogInformation("KeyVault : NOT Exportable");

        //            // If private key is not exportable, return a certificate with only the public key
        //            return new X509Certificate2(certificate.Cer);
        //        }
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        _logger.LogError("Check the function has permission to access the KeyVault certificate store!.");
        //        _logger.LogError($"{ex.Message}");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Check the function has permission to access the KeyVault certificate store!.");
        //        _logger.LogError($"{ex.Message}");
        //    }
        //    return null;
        //}
        //public bool VerifyCertificate(byte[] certificateBytes)
        //{
        //    _logger.LogInformation($"VerifyCertificate : ");
        //    var chain = new X509Chain();

        //    // Create a collection to hold the certificates
        //    X509Certificate2Collection certificateCollection = new X509Certificate2Collection();
        //    certificateCollection.Import(certificateBytes, string.Empty, X509KeyStorageFlags.PersistKeySet);

        //    // Iterate through the certificates in the collection
        //    foreach (X509Certificate2 certificate in certificateCollection)
        //    {
        //        // Do something with each certificate (e.g., validate, store, etc.)
        //        Console.WriteLine($"Certificate Subject: {certificate.Subject}");
        //        Console.WriteLine($"Thumbprint: {certificate.Thumbprint}");
        //        Console.WriteLine();
        //        if (!certificate.Thumbprint.Equals(Environment.GetEnvironmentVariable("LandRegistryCertificates")))
        //            chain.ChainPolicy.ExtraStore.Add(certificate);
        //    }

        //    // You can alter how the chain is built/validated.
        //    chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
        //    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.IgnoreWrongUsage;
        //    chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

        //    // Do the preliminary validation.
        //    var primaryCert = new X509Certificate2(certificateCollection[0]);
        //    if( chain.Build(primaryCert))
        //    {
        //        _logger.LogInformation("VerifyCertificate : Chain built successfully");

        //        Console.WriteLine("Chain Information");
        //        Console.WriteLine("Chain revocation flag: {0}", chain.ChainPolicy.RevocationFlag);
        //        Console.WriteLine("Chain revocation mode: {0}", chain.ChainPolicy.RevocationMode);
        //        Console.WriteLine("Chain verification flag: {0}", chain.ChainPolicy.VerificationFlags);
        //        Console.WriteLine("Chain verification time: {0}", chain.ChainPolicy.VerificationTime);
        //        Console.WriteLine("Chain status length: {0}", chain.ChainStatus.Length);
        //        Console.WriteLine("Chain application policy count: {0}", chain.ChainPolicy.ApplicationPolicy.Count);
        //        Console.WriteLine("Chain certificate policy count: {0} {1}", chain.ChainPolicy.CertificatePolicy.Count, Environment.NewLine);

        //        //Output chain element information.
        //        Console.WriteLine("Chain Element Information");
        //        Console.WriteLine("Number of chain elements: {0}", chain.ChainElements.Count);
        //        Console.WriteLine("Chain elements synchronized? {0} {1}", chain.ChainElements.IsSynchronized, Environment.NewLine);

        //        foreach (X509ChainElement element in chain.ChainElements)
        //        {
        //            Console.WriteLine("Element issuer name: {0}", element.Certificate.Issuer);
        //            Console.WriteLine("Element certificate valid until: {0}", element.Certificate.NotAfter);
        //            Console.WriteLine("Element certificate is valid: {0}", element.Certificate.Verify());
        //            Console.WriteLine("Element error status length: {0}", element.ChainElementStatus.Length);
        //            Console.WriteLine("Element information: {0}", element.Information);
        //            Console.WriteLine("Number of element extensions: {0}{1}", element.Certificate.Extensions.Count, Environment.NewLine);

        //            if (chain.ChainStatus.Length > 1)
        //            {
        //                for (int index = 0; index < element.ChainElementStatus.Length; index++)
        //                {
        //                    Console.WriteLine(element.ChainElementStatus[index].Status);
        //                    Console.WriteLine(element.ChainElementStatus[index].StatusInformation);
        //                }
        //            }
        //            Console.WriteLine("----");
        //        }
        //        return true;
        //    }

        //    // Make sure we have the same number of elements.
        //    if (chain.ChainElements.Count != chain.ChainPolicy.ExtraStore.Count + 1)
        //        return false;

        //    return true;
        //}

        //public static string CalculateExpirationTime(DateTime expiryDate)
        //{
        //    var currentDate = DateTime.Now;
        //    var dateDifference = (expiryDate - currentDate);

        //    if (dateDifference.Days >= 1)
        //        return $"{dateDifference.Days} day(s) remained";
        //    else if (dateDifference.Hours >= 1)
        //        return $"{dateDifference.Hours} hour(s) remained";
        //    else if (dateDifference.Minutes >= 1)
        //        return $"{dateDifference.Minutes} minute(s) remained";
        //    else if (dateDifference.TotalSeconds >= 1)
        //        return $"{dateDifference.Seconds} second(s) remained";

        //    return "Expired!";
        //}

    }
}

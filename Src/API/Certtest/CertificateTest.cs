using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Certtest
{
    public class CertificateTest
    {
        private readonly ILogger<CertificateTest> _logger;

        public CertificateTest(ILogger<CertificateTest> logger)
        {
            _logger = logger;
        }

        [Function("CertificateTest")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestData req)
        {
            _logger.LogInformation("CertificateTest Does the Land Registry Thumbnail exist function");
            var response = req.CreateResponse();

            bool isRunningInIisExpress = Process.GetCurrentProcess()
                                .ProcessName.ToLower().Contains("iisexpress");
            _logger.LogInformation($"Is Running In Iis Express {isRunningInIisExpress}");
            _logger.LogInformation($"ProcessName {Process.GetCurrentProcess().ProcessName.ToLower()}");

            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            //_logger.LogInformation($"Find : All");
            //var certificateCollection = store.Certificates.;
            //foreach (var certificate in certificateCollection)
            //{
            //    _logger.LogInformation($"Issuer : {certificate.Issuer}");
            //    _logger.LogInformation($"Thumbprint : {certificate.Thumbprint}");
            //}

            _logger.LogInformation($"Find : B10D6788259CA89F7309A07C334B1B2DE4B7D520");
            var certificateCollection = store.Certificates.Find(X509FindType.FindByThumbprint, "B10D6788259CA89F7309A07C334B1B2DE4B7D520", false);
            foreach (var certificate in certificateCollection)
            {
                _logger.LogInformation($"Issuer : {certificate.Issuer}");
                _logger.LogInformation($"Thumbprint : {certificate.Thumbprint}");
            }

            store.Close();

            response.StatusCode = HttpStatusCode.OK;
            await response.WriteStringAsync("whooo");
            return response;
        }
    }
}

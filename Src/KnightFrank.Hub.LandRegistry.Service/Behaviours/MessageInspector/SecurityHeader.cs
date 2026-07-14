using System;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace KnightFrank.Hub.LandRegistry.Service
{
    public class SecurityHeader : MessageHeader
    {
        private readonly string _username;
        private readonly string _nonce;
        private readonly string _created;
        private readonly string _password;

        public SecurityHeader(string username, string password, DateTime created)
        {
            _username = username;
            _nonce = CalculateNonce();
            _created = created.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            _password = password;
        }


        [XmlRoot(ElementName = "Password", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public class Password
        {
            [XmlAttribute(AttributeName = "Type")] public string Type { get; set; }
            [XmlText] public string Text { get; set; }
        }

        [XmlRoot(ElementName = "Nonce", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public class Nonce
        {
            [XmlAttribute(AttributeName = "EncodingType")]
            public string EncodingType { get; set; }

            [XmlText] public string Text { get; set; }
        }

        [XmlRoot(ElementName = "UsernameToken", Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
        public class UsernameToken
        {
            [XmlElement(ElementName = "Username",
                Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
            public string Username { get; set; }

            [XmlElement(ElementName = "Password",
                Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
            public Password Password { get; set; }

            [XmlElement(ElementName = "Nonce",
                Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd")]
            public Nonce Nonce { get; set; }

            [XmlElement(ElementName = "Created",
                Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd")]
            public string Created { get; set; }

            [XmlAttribute(AttributeName = "Id",
                Namespace = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd")]
            public string Id { get; set; }
        }

        public override string Name { get; } = "Security";

        public override string Namespace { get; } = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            var serializer = new XmlSerializer(typeof(UsernameToken));
            var pass = CreateHashedPassword(_nonce, _created, _password);
            serializer.Serialize(writer,
                new UsernameToken
                {
                    Username = _username,
                    Password = new Password
                    {
                        Text = pass,
                        Type =
                            "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest"
                    },
                    Nonce = new Nonce
                    {
                        Text = _nonce,
                        EncodingType =
                            "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary"
                    },
                    Created = _created

                });
        }

        private static string CalculateNonce()
        {
            //Allocate a buffer
            var byteArray = new byte[32];
            //Generate a cryptographically random set of bytes
            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(byteArray);
            }
            //Base64 encode and then return
            return Convert.ToBase64String(byteArray);
        }

        private static string CreateHashedPassword(string nonceStr, string created, string password)
        {
            var nonce = Convert.FromBase64String(nonceStr);
            var createdBytes = Encoding.UTF8.GetBytes(created);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var combined = new byte[createdBytes.Length + nonce.Length + passwordBytes.Length];
            Buffer.BlockCopy(nonce, 0, combined, 0, nonce.Length);
            Buffer.BlockCopy(createdBytes, 0, combined, nonce.Length, createdBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combined, nonce.Length + createdBytes.Length, passwordBytes.Length);

            return Convert.ToBase64String(SHA1.Create().ComputeHash(combined));
        }
    }

    //< soapenv:Header >
    //    < wsse:Security xmlns:wsse = "http://docs.oasisopen.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd" >
    //        < wsse:UsernameToken >
    //            < wsse:Username > userName here </ wsse:Username >
    //            < wsse:Password type = "http://docs.oasis-open.org/wss/2004/01/oasis200401-wss-username-token-profile-1.0#PasswordText" > passwordhere </ wsse:Password >
    //        </ wsse:UsernameToken >
    //    </ wsse:Security >
    //    < i18n:international xmlns:i18n = "http://www.w3.org/2005/09/ws-i18n" >
    //        < i18n:locale > en </ i18n:locale >
    //    </ i18n:international >
    //</ soapenv:Header >


    // Example user
    //ClientCredentials clientCredentials = new ClientCredentials();
    //clientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, Environment.GetEnvironmentVariable("LandRegistryCertificates"));
    //        clientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerTrust;

    //        var client = new DaylistEnquiryV2_0ServiceClient(Binding(), new EndpointAddress("https://bgtest.landregistry.gov.uk/b2b/BGStubService/DaylistEnquiryV2_0WebService"));
    //client.ChannelFactory.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
    //        client.ChannelFactory.Endpoint.EndpointBehaviors.Add(clientCredentials);

    //        //client.Endpoint.EndpointBehaviors.Add(new SimpleEndpointBehavior());

    //        //var opContext = new OperationContext(client.InnerChannel);
    //        //var soapSecurityHeader = new SoapSecurityHeader(Environment.GetEnvironmentVariable("LandRegistryUserId"), Environment.GetEnvironmentVariable("LandRegistryPassword"));
    //        //// Adding the security header
    //        //opContext.OutgoingMessageHeaders.Add(soapSecurityHeader);
    //        //var prevOpContext = OperationContext.Current;
    //        //OperationContext.Current = opContext;

    public class SoapSecurityHeader : MessageHeader
    {
        private readonly string _password, _username, _nonce, _createdDate;

        public SoapSecurityHeader(string username, string password, string nonce = "", string created = "")
        {
            _password = password;
            _username = username;
            _nonce = nonce;
            _createdDate = created;
        }

        public override string Name
        {
            get { return "Security"; }
        }

        public override string Namespace
        {
            get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
        }

        protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("wsse", Name, Namespace);
            //writer.WriteXmlnsAttribute("wsse", Namespace);
            //writer.WriteXmlAttribute("mustUnderstand", "1");
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("wsse", "UsernameToken", Namespace);

            writer.WriteStartElement("wsse", "Username", Namespace);
            writer.WriteValue(_username);
            writer.WriteEndElement();

            writer.WriteStartElement("wsse", "Password", Namespace);
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
            writer.WriteValue(_password);
            writer.WriteEndElement();

            writer.WriteEndElement();

            writer.WriteStartElement("i18n", "international", "http://www.w3.org/2005/09/ws-i18n");

            writer.WriteStartElement("i18n", "locale", "http://www.w3.org/2005/09/ws-i18n");
            writer.WriteValue("en");
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}

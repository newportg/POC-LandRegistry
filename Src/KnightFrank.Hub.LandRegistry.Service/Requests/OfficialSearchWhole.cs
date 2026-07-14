using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using KnightFrank.Hub.LandRegistry.Service.Behaviours.SoapHeader;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceReference;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace KnightFrank.Hub.LandRegistry.Service.Requests
{
    // Concrete Creator 
    public class OfficialSearchWholeFactory : ServiceFactory
    {
        private readonly X509Certificate2? _clientCertificate;

        public OfficialSearchWholeFactory(IMapper mapper, ILogger<LandRegistrySvc> logger, X509Certificate2? clientCertificate = null)
        {
            _mapper = mapper;
            _logger = logger;
            _clientCertificate = clientCertificate;
        }

        public override LandRegistry GetService()
        {
            return new OfficialSeacrhWhole(_mapper, _logger);
        }
    }

    public class PollOfficialSearchWholeFactory : ServiceFactory
    {
        private readonly X509Certificate2? _clientCertificate;

        public PollOfficialSearchWholeFactory(IMapper mapper, ILogger<LandRegistrySvc> logger, X509Certificate2? clientCertificate = null)
        {
            _mapper = mapper;
            _logger = logger;
            _clientCertificate = clientCertificate;
        }

        public override LandRegistry GetService()
        {
            return new PollOfficialSearchWhole(_mapper, _logger);
        }
    }

    // Concrete 
    public class OfficialSeacrhWhole : LandRegistry
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LandRegistrySvc> _logger;
        private readonly X509Certificate2? _clientCertificate;
        private LandRegistryDto _dto;
        private RequestOfficialSearchOfWholeWithPriorityV2_1Type _request;
        private processOS1RequestResponse1 _response;

        public OfficialSeacrhWhole(IMapper mapper, ILogger<LandRegistrySvc> logger, X509Certificate2? clientCertificate = null)
        {
            _mapper = mapper;
            _logger = logger;
            _clientCertificate = clientCertificate;
        }

        public override void Validate()
        {
            _logger.LogInformation("Validate");
        }

        public override void Map(LandRegistryDto landRegistryDto)
        {
            _logger.LogInformation("Map - Request");

            _dto = landRegistryDto;

            // Map JSON to XML
            _request = _mapper.Map<RequestOfficialSearchOfWholeWithPriorityV2_1Type>(_dto);
            _logger.LogInformation(JsonConvert.SerializeObject(_request));
        }

        public override async Task Request()
        {
            _logger.LogInformation("Request");

            var client = GetWsClient();
            _response = await client.processOS1RequestAsync(_request);
            //client.CloseAsync();
        }

        public override LandRegistryDto Response()
        {
            _logger.LogInformation("Map - Response");

            // Map back into original object
            return _mapper.Map<processOS1RequestResponse1, LandRegistryDto>(_response, _dto);
        }

        private OfficialSearchV2_1ServiceClient GetWsClient()
        {
            _logger.LogInformation("GetWsClient");

            ClientCredentials clientCredentials = new ClientCredentials();
            //            clientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, Environment.GetEnvironmentVariable("LandRegistryCertificates"));
            // If we have an in-memory certificate (e.g. from Key Vault using EphemeralKeySet), assign it directly.
            if (_clientCertificate != null)
            {
                clientCredentials.ClientCertificate.Certificate = _clientCertificate;
                _logger.LogDebug("Using in-memory client certificate (thumbprint={thumbprint})", _clientCertificate.Thumbprint);
            }
            else
            {
                // Fallback to store-based lookup — sanitize thumbprint (remove spaces) before lookup
                var thumb = Environment.GetEnvironmentVariable("LandRegistryCertificates") ?? string.Empty;
                thumb = thumb.Replace(" ", string.Empty).ToUpperInvariant();
                if (!string.IsNullOrEmpty(thumb))
                {
                    clientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, thumb);
                    _logger.LogDebug("Using certificate from store (thumbprint={thumb})", thumb);
                }
                else
                {
                    _logger.LogWarning("No client certificate provided and LandRegistryCertificates env var is empty");
                }
            }

            clientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerTrust;

            var client = new OfficialSearchV2_1ServiceClient(Binding(), new EndpointAddress(Environment.GetEnvironmentVariable("LandRegistryApplicationEnquiry")));
            client.ChannelFactory.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
            client.ChannelFactory.Endpoint.EndpointBehaviors.Add(clientCredentials);
            client.ChannelFactory.Endpoint.EndpointBehaviors.Add(new HMLRBGMessageEndpointBehavior(Environment.GetEnvironmentVariable("LandRegistryUserId"), Environment.GetEnvironmentVariable("LandRegistryPassword"), _logger));

            //client.OpenAsync();

            return client;
        }

    }

    public class PollOfficialSearchWhole : LandRegistry
    {
        private readonly IMapper _mapper;
        private readonly ILogger<LandRegistrySvc> _logger;
        private readonly X509Certificate2? _clientCertificate;
        private LandRegistryDto _dto;
        private PollRequestType _request;
        private getResponseResponse9 _response;

        public PollOfficialSearchWhole(IMapper mapper, ILogger<LandRegistrySvc> logger, X509Certificate2? clientCertificate = null)
        {
            _mapper = mapper;
            _logger = logger;
            _clientCertificate = clientCertificate;
        }

        public override void Validate()
        {
            _logger.LogInformation("Validate");
        }

        public override void Map(LandRegistryDto landRegistryDto)
        {
            _logger.LogInformation("Map - Request");

            _dto = landRegistryDto;

            // Map JSON to XML
            _request = _mapper.Map<PollRequestType>(_dto);
            _logger.LogInformation(JsonConvert.SerializeObject(_request));
        }

        public override async Task Request()
        {
            _logger.LogInformation("Request");

            var client = GetWsClient();
            _response = await client.getResponseAsync(_request);
        }

        public override LandRegistryDto Response()
        {
            _logger.LogInformation("Map - Response");

            // Map back into original object
            return _mapper.Map<getResponseResponse9, LandRegistryDto>(_response, _dto);
        }

        private OfficialSearchV2_0PollServiceClient GetWsClient()
        {
            _logger.LogInformation("GetWsClient");

            ClientCredentials clientCredentials = new ClientCredentials();
            //            clientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, Environment.GetEnvironmentVariable("LandRegistryCertificates"));
            // If we have an in-memory certificate (e.g. from Key Vault using EphemeralKeySet), assign it directly.
            if (_clientCertificate != null)
            {
                clientCredentials.ClientCertificate.Certificate = _clientCertificate;
                _logger.LogDebug("Using in-memory client certificate (thumbprint={thumbprint})", _clientCertificate.Thumbprint);
            }
            else
            {
                // Fallback to store-based lookup — sanitize thumbprint (remove spaces) before lookup
                var thumb = Environment.GetEnvironmentVariable("LandRegistryCertificates") ?? string.Empty;
                thumb = thumb.Replace(" ", string.Empty).ToUpperInvariant();
                if (!string.IsNullOrEmpty(thumb))
                {
                    clientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindByThumbprint, thumb);
                    _logger.LogDebug("Using certificate from store (thumbprint={thumb})", thumb);
                }
                else
                {
                    _logger.LogWarning("No client certificate provided and LandRegistryCertificates env var is empty");
                }
            }

            clientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.PeerTrust;

            var client = new OfficialSearchV2_0PollServiceClient(Binding(), new EndpointAddress(Environment.GetEnvironmentVariable("LandRegistryApplicationEnquiry")));
            client.ChannelFactory.Endpoint.EndpointBehaviors.Remove(typeof(ClientCredentials));
            client.ChannelFactory.Endpoint.EndpointBehaviors.Add(clientCredentials);
            client.ChannelFactory.Endpoint.EndpointBehaviors.Add(new HMLRBGMessageEndpointBehavior(Environment.GetEnvironmentVariable("LandRegistryUserId"), Environment.GetEnvironmentVariable("LandRegistryPassword"), _logger));

            //client.OpenAsync();

            return client;
        }

    }
}

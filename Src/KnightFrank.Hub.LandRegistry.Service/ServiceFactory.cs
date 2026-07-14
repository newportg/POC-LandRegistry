using AutoMapper;
using KnightFrank.Hub.LandRegistry.Common.Models;
using KnightFrank.Hub.LandRegistry.Service.Requests;
using Microsoft.Extensions.Logging;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Hub.LandRegistry.Service
{
    public abstract class ServiceFactory
    {
        protected IMapper _mapper;
        protected ILogger<LandRegistrySvc> _logger;
        public abstract LandRegistry GetService();

        // This is cleaner, BUT you will need to use DI for mapper/logger for the request factories
        //
        //public static ServiceFactory GetServiceFactory(LandRegistryDto request)
        //{
        //    return request.RequestType switch
        //    {
        //        // Interactive requests
        //        RequestTypes.ApplicationEnquiry => new ApplicationEnquiryFactory(_mapper, _logger),
        //        RequestTypes.LCBankruptcySearch => new LLCBankruptySearchFactory(_mapper, _logger),
        //        RequestTypes.DischargeActivity => new DischargeActivityFactory(_mapper, _logger),
        //        RequestTypes.EnquiryByPropertyDescription => new EnquiryByPropertyDescriptionFactory(_mapper, _logger),
        //        RequestTypes.LCFullSearch => new LCFullSearchFactory(_mapper, _logger),
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
    }

    public abstract class LandRegistry
    {
        public abstract void Map(LandRegistryDto landRegistryDto);
        public abstract void Validate();
        public abstract Task Request();
        public abstract LandRegistryDto Response();
        protected static Binding Binding()
        {
            var myBinding = new BasicHttpBinding()
            {
                Name = "CertPortBinding",
                CloseTimeout = TimeSpan.FromMinutes(1),
                OpenTimeout = TimeSpan.FromMinutes(1),
                ReceiveTimeout = TimeSpan.FromMinutes(10),
                SendTimeout = TimeSpan.FromMinutes(1),
                BypassProxyOnLocal = false,
                MaxBufferPoolSize = 524288,
                MaxReceivedMessageSize = 65536000,
                TextEncoding = Encoding.UTF8,
                UseDefaultWebProxy = true
            };
            myBinding.Security.Mode = BasicHttpSecurityMode.Transport; // SecurityMode.Transport;
            myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            //myBinding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None; // default
            //myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName; //MessageCredentialType.UserName;

            //myBinding.ReaderQuotas.MaxDepth = 32;
            //myBinding.ReaderQuotas.MaxStringContentLength = 65536000;
            //myBinding.ReaderQuotas.MaxArrayLength = 16384;
            //myBinding.ReaderQuotas.MaxBytesPerRead = 4096;
            //myBinding.ReaderQuotas.MaxNameTableCharCount = 16384;

            return myBinding;
        }
    }
}

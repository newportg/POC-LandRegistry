using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum RequestTypes
    {
        ApplicationEnquiry,
        LCBankruptcySearch,
        DischargeActivity,
        EnquiryByPropertyDescription,
        LCFullSearch,
        OfficialCopyTitleKnown,
        OfficialSearchWhole,
        OfficialSearchPart,

        PollApplicationEnquiry,
        PollLCBankruptcySearch,
        PollDischargeActivity,
        PollLCFullSearch,
        PollEnquiryByPropertyDescription,
        PollOfficialSearchWhole,
        PollOfficialSearchPart
    }


    //"../WSDL/OfficialCopyWithSummaryV2_1WebService.wsdl",
    //"../WSDL/SearchOfIndexMapV2_0WebService.wsdl",

    //"../WSDL/SearchOfIndexMapV2_0PollRequestWebService.wsdl",


    public class LandRegistryDto
    {
        public RequestTypes RequestType { get; set; } = RequestTypes.EnquiryByPropertyDescription;
        public Request Request { get; set; }
        public Response Response { get; set; }
        public Error SystemError { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LandRegistryDto dto
                   && RequestType == dto.RequestType
                   && EqualityComparer<Request>.Default.Equals(Request, dto.Request)
                   && EqualityComparer<Response>.Default.Equals(Response, dto.Response);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RequestType, Request, Response);
        }

        public static bool operator ==(LandRegistryDto left, LandRegistryDto right)
        {
            return EqualityComparer<LandRegistryDto>.Default.Equals(left, right);
        }

        public static bool operator !=(LandRegistryDto left, LandRegistryDto right)
        {
            return !(left == right);
        }

        public static LandRegistryDto GetRequest(RequestTypes requestType, string json = null)
        {
            var ldto = new LandRegistryDto();

            switch (requestType)
            {
                case RequestTypes.ApplicationEnquiry:
                    ldto.RequestType = RequestTypes.ApplicationEnquiry;
                    break;
                case RequestTypes.LCBankruptcySearch:
                    ldto.RequestType = RequestTypes.LCBankruptcySearch;
                    break;
                case RequestTypes.DischargeActivity:
                    ldto.RequestType = RequestTypes.DischargeActivity;
                    break;
                case RequestTypes.EnquiryByPropertyDescription:
                    ldto.RequestType = RequestTypes.EnquiryByPropertyDescription;
                    break;
                case RequestTypes.LCFullSearch:
                    ldto.RequestType = RequestTypes.LCFullSearch;
                    break;
                case RequestTypes.OfficialCopyTitleKnown:
                    ldto.RequestType = RequestTypes.OfficialCopyTitleKnown;
                    break;
                case RequestTypes.OfficialSearchWhole:
                    ldto.RequestType = RequestTypes.OfficialSearchWhole;
                    break;
                case RequestTypes.OfficialSearchPart:
                    ldto.RequestType = RequestTypes.OfficialSearchPart;
                    break;

                case RequestTypes.PollApplicationEnquiry:
                    ldto.RequestType = RequestTypes.PollApplicationEnquiry;
                    break;
                case RequestTypes.PollLCBankruptcySearch:
                    ldto.RequestType = RequestTypes.PollLCBankruptcySearch;
                    break;
                case RequestTypes.PollDischargeActivity:
                    ldto.RequestType = RequestTypes.PollDischargeActivity;
                    break;
                case RequestTypes.PollLCFullSearch:
                    ldto.RequestType = RequestTypes.PollLCFullSearch;
                    break;
                case RequestTypes.PollEnquiryByPropertyDescription:
                    ldto.RequestType = RequestTypes.PollEnquiryByPropertyDescription;
                    break;
                case RequestTypes.PollOfficialSearchWhole:
                    ldto.RequestType = RequestTypes.PollOfficialSearchWhole;
                    break;
                case RequestTypes.PollOfficialSearchPart:
                    ldto.RequestType = RequestTypes.PollOfficialSearchPart;
                    break;
                default:
                    break;
            }

            if( !string.IsNullOrEmpty(json))
                ldto.Request = JsonConvert.DeserializeObject<Request>(json);
            return ldto;
        }
    }
}

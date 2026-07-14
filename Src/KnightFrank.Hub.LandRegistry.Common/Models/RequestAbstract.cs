//using Newtonsoft.Json;
//using System;
//using System.Linq;

//namespace KnightFrank.Hub.LandRegistry.Common.Models
//{
//    public class Fred
//    {
//        public static RQ GetRequest(RequestTypes requestType)
//        {
//            switch (requestType)
//            {
//                //case RequestTypes.ApplicationEnquiry:
//                //    return new RequestApplicationEnquiry();
//                //case RequestTypes.LCBankruptcySearch:
//                //    return new RequestLCBankruptcySearch();
//                //case RequestTypes.DischargeActivity:
//                //    return new RequestDischargeActivity();
//                case RequestTypes.EnquiryByPropertyDescription:
//                    return new RequestSearchByPropertyDescription();
//                //case RequestTypes.LCFullSearch:
//                //    return new RequestLCFullSearch();
//                //case RequestTypes.OfficialCopyTitleKnown:
//                //    return new RequestTitleKnown();
//                //case RequestTypes.OfficialSearchWhole:
//                //    return new RequestOfficialSearchWhole();

//                //case RequestTypes.PollOfficialSearchWhole:
//                //    return new RequestPoll();
//                default:
//                    break;
//            }
//            return null;
//        }

//        public static RQ GetRequest(RequestTypes requestType, string json)
//        {
//            switch (requestType)
//            {
//                case RequestTypes.EnquiryByPropertyDescription:
//                    return JsonConvert.DeserializeObject<RequestSearchByPropertyDescription>(json);

//                default:
//                    break;
//            }
//            return null;
//        }
//    }
//    public abstract class RQ
//    {
//        public RequestTypes RequestType { get; set; }

//        public Identity Reference { get; set; } = new Identity();

//        public abstract RQ Deserialise(string json);
//        public abstract bool Validate();
//    }

//    public class RequestSearchByPropertyDescription : RQ
//    {
//        public RequestSearchByPropertyDescription() 
//        {
//            RequestType = RequestTypes.EnquiryByPropertyDescription;    
//        }

//        public Property Property { get; set; }

//        public override RQ Deserialise(string json)
//        {
//            return JsonConvert.DeserializeObject<RequestSearchByPropertyDescription>(json);
//        }
//        public override bool Validate()
//        {
//            return false;
//        }
//    }

//    //public class RequestTitleKnown : RQ
//    //{
//    //    public override RQ Deserialise(string json)
//    //    {
//    //        return JsonConvert.DeserializeObject<RequestTitleKnown>(json);
//    //    }

//    //    public override bool Validate()
//    //    {
//    //        return false;
//    //    }

//    //    private readonly string[] allowedOC = new string[] { "OC1", "OC2" };
//    //    private readonly string[] allowedROC = new string[] { "RegisterOnly", "TitleOnly", "RegisterAndTitlePlan", "CI", "CIAndRegister" };
//    //    private string officialCopy = "OC1";
//    //    private string requestedOfficialCopy = string.Empty;

//    //    private decimal expectedPrice;
//    //    public decimal ExpectedPrice
//    //    {
//    //        get
//    //        {
//    //            return expectedPrice;
//    //        }
//    //        set
//    //        {
//    //            expectedPrice = value;
//    //        }
//    //    }
//    //    public Contact Contact { get; set; } = new Contact();
//    //    public DocumentInfo[] DocumentInfo { get; set; }
//    //    public string Tenure { get; set; }
//    //    public string TitleNumber { get; set; }
//    //    public string PropertyDescription { get; set; }
//    //    public string OfficialCopy
//    //    {
//    //        get
//    //        {
//    //            return officialCopy;
//    //        }
//    //        set
//    //        {
//    //            if (!allowedOC.Any(x => x == value))
//    //                throw new ArgumentException("Not valid - OfficialCopy");
//    //            officialCopy = value;
//    //        }
//    //    }
//    //    public string RequestedOfficialCopy
//    //    {
//    //        get
//    //        {
//    //            return requestedOfficialCopy;
//    //        }
//    //        set
//    //        {
//    //            if (!allowedROC.Any(x => x == value))
//    //                throw new ArgumentException("Not valid - requestedOfficialCopy");
//    //            requestedOfficialCopy = value;
//    //        }
//    //    }
//    //    public bool ContinueIfFeeExceeds { get; set; } = false;
//    //    public bool ClosedAndContinued { get; set; } = false;
//    //    public bool PendingApps { get; set; } = false;
//    //    public bool FirstRegistration { get; set; } = false;
//    //    public bool SendBackdated { get; set; } = false;
//    //    public decimal[] CertificateInFormCI { get; set; }
//    //}

//    //public class RequestOfficialSearchWhole : RQ
//    //{
//    //    private decimal expectedPrice;
//    //    public decimal ExpectedPrice
//    //    {
//    //        get
//    //        {
//    //            return expectedPrice;
//    //        }
//    //        set
//    //        {
//    //            expectedPrice = value;
//    //        }
//    //    }
//    //    public Contact[] Contact { get; set; } = new Contact[1];
//    //    public string TitleNumber { get; set; }
//    //    public PrioritySearch PrioritySearch { get; set; }
//    //    public AlternativeDespatchDetails AlternativeDespatchDetails { get; set; }
//    //}
//    //public class RequestLCFullSearch : RQ
//    //{
//    //    private decimal expectedPrice;
//    //    public decimal ExpectedPrice
//    //    {
//    //        get
//    //        {
//    //            return expectedPrice;
//    //        }
//    //        set
//    //        {
//    //            expectedPrice = value;
//    //        }
//    //    }
//    //    public Contact Contact { get; set; } = new Contact();
//    //    public SearchParty SearchParty { get; set; }
//    //    public AlternativeDespatchDetails AlternativeDespatchDetails { get; set; }
//    //}
//    //public class RequestLCBankruptcySearch : RQ
//    //{
//    //    private decimal expectedPrice;
//    //    public decimal ExpectedPrice
//    //    {
//    //        get
//    //        {
//    //            return expectedPrice;
//    //        }
//    //        set
//    //        {
//    //            expectedPrice = value;
//    //        }
//    //    }
//    //    public bool ContinueIfFeeExceeds { get; set; } = false;
//    //    public Contact Contact { get; set; } = new Contact();
//    //    public SearchParty SearchParty { get; set; }
//    //    public AlternativeDespatchDetails AlternativeDespatchDetails { get; set; }
//    //}
//    //public class RequestDischargeActivity : RQ
//    //{
//    //    public bool ClosedAndContinued { get; set; } = false;
//    //    public string TitleNumber { get; set; }
//    //    public DateTime ChargeDate { get; set; }
//    //    public string ApplicationReference { get; set; } = null;
//    //}
//    //public class RequestApplicationEnquiry : RQ
//    //{
//    //    public bool ClosedAndContinued { get; set; } = false;
//    //    public string TitleNumber { get; set; }

//    //}

//    //public class RequestPoll : RQ { }
//}

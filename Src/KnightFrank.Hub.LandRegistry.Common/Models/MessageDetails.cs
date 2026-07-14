using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public abstract class MessageDetails
    {
    }

    public class MessageMessageDetails : MessageDetails
    {
        public string Description { get; set; }
        public Title Title { get; set; }
        public Title[] Titles { get; set; }

        public DateTime DateTime { get; set; }
        public ApplicationEnquiry[] ApplicationEnquiry { get; set; }
        public RecentApplication RecentApplication { get; set; }

        public string HMLRReference { get; set; }
        public decimal ActualPrice { get; set; }
        public Attachment Attachment { get; set; }
        public string ResultTypeCode { get; set; }

        public DateTime ResultDateTime { get; set; }
        public string TitleNumber { get; set; }
        public int DischargeCount { get; set; }
        public Discharges[] Discharges { get; set; }
    }

    //public class DayListEnquiry
    //{
    //    public string Applicant { get; set; }
    //    public string ApplicationReference { get; set; }
    //    public string ApplicationType { get; set; }
    //    public string CustomerReference { get; set; }
    //    public string DepositReason { get; set; }
    //    public bool DepositReasonSpecified { get; set; }
    //    public string LodgedBy { get; set; }
    //    public string OLALodgedFor { get; set; }
    //    public DateTime PriorityDate { get; set; }
    //    public DateTime PriorityTime { get; set; }
    //    public string PropertyDescription { get; set; }
    //    public string SearchCertificateNumber { get; set; }
    //    public string SearchInterest { get; set; }
    //    public bool SearchInterestSpecified { get; set; }

    //    public override global::System.Boolean Equals(global::System.Object obj)
    //    {
    //        return obj is DayListEnquiry enquiry &&
    //               Applicant == enquiry.Applicant &&
    //               ApplicationReference == enquiry.ApplicationReference &&
    //               ApplicationType == enquiry.ApplicationType &&
    //               CustomerReference == enquiry.CustomerReference &&
    //               DepositReason == enquiry.DepositReason &&
    //               DepositReasonSpecified == enquiry.DepositReasonSpecified &&
    //               LodgedBy == enquiry.LodgedBy &&
    //               OLALodgedFor == enquiry.OLALodgedFor &&
    //               EqualityComparer<DateTime>.Default.Equals(PriorityDate, enquiry.PriorityDate) &&
    //               EqualityComparer<DateTime>.Default.Equals(PriorityTime, enquiry.PriorityTime) &&
    //               PropertyDescription == enquiry.PropertyDescription &&
    //               SearchCertificateNumber == enquiry.SearchCertificateNumber &&
    //               SearchInterest == enquiry.SearchInterest &&
    //               SearchInterestSpecified == enquiry.SearchInterestSpecified;
    //    }
    //    public override global::System.Int32 GetHashCode()
    //    {
    //        HashCode hash = new HashCode();
    //        hash.Add(Applicant);
    //        hash.Add(ApplicationReference);
    //        hash.Add(ApplicationType);
    //        hash.Add(CustomerReference);
    //        hash.Add(DepositReason);
    //        hash.Add(DepositReasonSpecified);
    //        hash.Add(LodgedBy);
    //        hash.Add(OLALodgedFor);
    //        hash.Add(PriorityDate);
    //        hash.Add(PriorityTime);
    //        hash.Add(PropertyDescription);
    //        hash.Add(SearchCertificateNumber);
    //        hash.Add(SearchInterest);
    //        hash.Add(SearchInterestSpecified);
    //        return hash.ToHashCode();
    //    }

    //    public static global::System.Boolean operator ==(DayListEnquiry left, DayListEnquiry right)
    //    {
    //        return EqualityComparer<DayListEnquiry>.Default.Equals(left, right);
    //    }

    //    public static global::System.Boolean operator !=(DayListEnquiry left, DayListEnquiry right)
    //    {
    //        return !(left == right);
    //    }
    //}


}

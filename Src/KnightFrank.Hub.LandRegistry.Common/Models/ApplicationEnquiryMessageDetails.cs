using AutoMapper.Execution;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class ApplicationEnquiryMessageDetails : MessageDetails
    {
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public Title Title { get; set; }
        public Title[] Titles { get; set; }

        public ApplicationEnquiry[] ApplicationEnquiry { get; set; }
        public RecentApplication RecentApplication { get; set; }


        public override bool Equals(object obj)
        {
            //var foo = obj as ApplicationEnquiryMessageDetails;
            //return foo != null;
            //   // && Description == foo.Description; 

            if (!(obj is ApplicationEnquiryMessageDetails))
                return false;

            ApplicationEnquiryMessageDetails response = (ApplicationEnquiryMessageDetails)obj;

            //return obj is ApplicationEnquiryMessageDetails copy
            //        && Description == copy.Description;

            //&& EqualityComparer<Title>.Default.Equals(Title, copy.Title)
            //&& EqualityComparer<RecentApplication>.Default.Equals(RecentApplication, copy.RecentApplication);


            // Check element-wise equality
            if (ApplicationEnquiry != null)
            {
                if (ApplicationEnquiry.Length != response.ApplicationEnquiry.Length)
                    return false;

                for (int i = 0; i < this.ApplicationEnquiry.Length; i++)
                {
                    if (!EqualityComparer<ApplicationEnquiry>.Default.Equals(this.ApplicationEnquiry[i], response.ApplicationEnquiry[i]))
                        return false;
                }
            }

            return obj is ApplicationEnquiryMessageDetails copy
                   //&& DateTime == copy.DateTime
                   && EqualityComparer<Title>.Default.Equals(Title, copy.Title)
                   && EqualityComparer<RecentApplication>.Default.Equals(RecentApplication, copy.RecentApplication);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description);
        }

        public static bool operator ==(ApplicationEnquiryMessageDetails left, ApplicationEnquiryMessageDetails right)
        {
            return EqualityComparer<ApplicationEnquiryMessageDetails>.Default.Equals(left, right);
        }

        public static bool operator !=(ApplicationEnquiryMessageDetails left, ApplicationEnquiryMessageDetails right)
        {
            return !(left == right);
        }
    }

    public class ApplicationEnquiry
    {
        public string ApplicationType { get; set; }
        public bool Expedited { get; set; }
        public string ApplicationReference { get; set; }
        public DateTime PriorityDate { get; set; }
        public DateTime PriorityTime { get; set; }
        public string Applicant { get; set; }
        public ApplicationProgress ApplicationProgress { get; set; }
        public string CustomerReference { get; set; }
        public string ApplicationReceivedBy { get; set; }
        public string PropertyDescription { get; set; }
        public string LodgedBy { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ApplicationEnquiry))
                return false;

            ApplicationEnquiry response = (ApplicationEnquiry)obj;

            if (ApplicationType == response.ApplicationType 
                   && Expedited == response.Expedited 
                   && ApplicationReference == response.ApplicationReference
                   //&& EqualityComparer<DateTime>.Default.Equals(PriorityDate, response.PriorityDate) 
                   //&& EqualityComparer<DateTime>.Default.Equals(PriorityTime, response.PriorityTime) 
                   && Applicant == response.Applicant
                   && ApplicationProgress == response.ApplicationProgress
                   && CustomerReference == response.CustomerReference 
                   && PropertyDescription == response.PropertyDescription )
                   //&& LodgedBy == response.LodgedBy)
                return true;
            return false;


            //return obj is ApplicationEnquiry enquiry &&
            //       ApplicationType == enquiry.ApplicationType &&
            //       Expedited == enquiry.Expedited &&
            //       ApplicationReference == enquiry.ApplicationReference &&
            //       //EqualityComparer<DateTime>.Default.Equals(PriorityDate, enquiry.PriorityDate) &&
            //       //EqualityComparer<DateTime>.Default.Equals(PriorityTime, enquiry.PriorityTime) &&
            //       Applicant == enquiry.Applicant &&
            //       CustomerReference == enquiry.CustomerReference &&
            //       LodgedBy == enquiry.LodgedBy &&
            //       PropertyDescription == enquiry.PropertyDescription &&
            //       LodgedBy == enquiry.LodgedBy;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(ApplicationType);
            hash.Add(Expedited);
            hash.Add(ApplicationReference);
            hash.Add(PriorityDate);
            hash.Add(PriorityTime);
            hash.Add(Applicant);
            hash.Add(CustomerReference);
            hash.Add(PropertyDescription);
            hash.Add(LodgedBy);
            return hash.ToHashCode();
        }

        public static bool operator ==(ApplicationEnquiry left, ApplicationEnquiry right)
        {
            return EqualityComparer<ApplicationEnquiry>.Default.Equals(left, right);
        }

        public static bool operator !=(ApplicationEnquiry left, ApplicationEnquiry right)
        {
            return !(left == right);
        }
    }

    public class ApplicationProgress
    {
        public string Description { get; set; }
        public Correspondence[] Correspondence { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ApplicationProgress))
                return false;

            ApplicationProgress response = (ApplicationProgress)obj;

            // Check element-wise equality
            if (Correspondence != null)
            {
                if (this.Correspondence.Length != response.Correspondence.Length)
                    return false;

                for (int i = 0; i < this.Correspondence.Length; i++)
                {
                    if (!EqualityComparer<Correspondence>.Default.Equals(this.Correspondence[i], response.Correspondence[i]))
                        return false;
                }
            }

            return Description == response.Description;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Description);
            return hash.ToHashCode();
        }

        public static bool operator ==(ApplicationProgress left, ApplicationProgress right)
        {
            return EqualityComparer<ApplicationProgress>.Default.Equals(left, right);
        }

        public static bool operator !=(ApplicationProgress left, ApplicationProgress right)
        {
            return !(left == right);
        }

    }

    public class Correspondence
    {
        public string CorrespondenceType { get; set; }
        public DateTime RequestedOnDate { get; set; }
        public DateTime IssuedOnDate { get; set; }
        public DateTime ExpiresOnDate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Correspondence))
                return false;

            return obj is Correspondence enquiry
                && CorrespondenceType == enquiry.CorrespondenceType;
                //&& EqualityComparer<DateTime>.Default.Equals(RequestedOnDate, enquiry.RequestedOnDate)
                //&& EqualityComparer<DateTime>.Default.Equals(IssuedOnDate, enquiry.IssuedOnDate)
                //&& EqualityComparer<DateTime>.Default.Equals(ExpiresOnDate, enquiry.ExpiresOnDate);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(CorrespondenceType);
            return hash.ToHashCode();
        }

        public static bool operator ==(Correspondence left, Correspondence right)
        {
            return EqualityComparer<Correspondence>.Default.Equals(left, right);
        }

        public static bool operator !=(Correspondence left, Correspondence right)
        {
            return !(left == right);
        }
    }

    public class RecentApplication
    {
        public string EndReason { get; set; }
        public DateTime EndDate { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is RecentApplication))
                return false;

            return obj is RecentApplication copy
                    && EndReason == copy.EndReason;
            //&& EndDate == copy.EndDate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EndReason, EndDate);
        }

        public static bool operator ==(RecentApplication left, RecentApplication right)
        {
            return EqualityComparer<RecentApplication>.Default.Equals(left, right);
        }

        public static bool operator !=(RecentApplication left, RecentApplication right)
        {
            return !(left == right);
        }

    }

}

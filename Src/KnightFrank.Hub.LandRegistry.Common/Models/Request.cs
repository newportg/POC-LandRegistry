using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class Request
    {
        /*
         * Basic info Id, external, customer, Contact, DespatchDetails
         * Request
         *  Property
         *      Flags
         *      Applicant[] { Proprieter, Applicant}
         *  Search
         *      Flags
         *      Applicants
         *      Contacts[]
         */


        public Identity Reference { get; set; } = new Identity();
        public Property Property { get; set; }
        public Applicant Applicant { get; set; }
        public Flags Flags { get; set; }
        public DocumentInfo[] DocumentInfo { get; set; }
        public Contact Contact { get; set; } = new Contact();
        public Other Other { get; set; }
        public ApplicationReference ApplicationReference { get; set; }
        public SearchParty SearchParty { get; set; }
        public AlternativeDespatchDetails AlternativeDespatchDetails { get; set; }

        public PrioritySearch PrioritySearch { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Request request
                && Reference == request.Reference
                && Property == request.Property
                && Applicant == request.Applicant
                && Flags == request.Flags
                && EqualityComparer<DocumentInfo[]>.Default.Equals(DocumentInfo, request.DocumentInfo)
                && Contact == request.Contact
                && Other == request.Other
                && ApplicationReference == request.ApplicationReference
                && SearchParty == request.SearchParty
                && AlternativeDespatchDetails == request.AlternativeDespatchDetails;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Reference);
            hash.Add(Property);
            hash.Add(Applicant);
            hash.Add(Flags);
            hash.Add(DocumentInfo);
            hash.Add(Contact);
            hash.Add(Other);
            hash.Add(ApplicationReference);
            hash.Add(SearchParty);
            hash.Add(AlternativeDespatchDetails);
            return hash.ToHashCode();
        }

        public static bool operator ==(Request left, Request right)
        {
            return EqualityComparer<Request>.Default.Equals(left, right);
        }

        public static bool operator !=(Request left, Request right)
        {
            return !(left == right);
        }
    }
    public class Identity
    {
        public string UniqueMsgId { get; set; } = Guid.NewGuid().ToString();
        public string ExternalRef { get; set; }
        public string CustomerRef { get; set; }
        public string AllocatedBy { get; set; }
        public string Description { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Identity copy
                && ExternalRef == copy.ExternalRef
                && CustomerRef == copy.CustomerRef;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            //hash.Add(UniqueMsgId);
            hash.Add(ExternalRef);
            hash.Add(CustomerRef);
            return hash.ToHashCode();
        }

        public static bool operator ==(Identity left, Identity right)
        {
            return EqualityComparer<Identity>.Default.Equals(left, right);
        }

        public static bool operator !=(Identity left, Identity right)
        {
            return !(left == right);
        }
    }
    public class Property
    {
        private decimal expectedPrice;

        public Property()
        {
            _ = decimal.TryParse(Environment.GetEnvironmentVariable("LandRegistryExpectedPrice"), out expectedPrice);
        }

        public string PropertyName { get; set; }
        public string PropertyNumber { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public string Tenure { get; set; }
        public string TitleNumber { get; set; }
        public decimal ExpectedPrice 
        {
            get
            {
                return expectedPrice;
            }
            set
            {
                expectedPrice = value;
            }
        }
        public string PropertyDescription { get; set; }
        public string LocalAuthority { get; set; }
        public string CurrencyID { get; set; }
        public decimal GrossPriceAmount { get; set; } = decimal.Zero;
        public decimal NetPriceAmount { get; set; } = decimal.Zero;
        public decimal VATAmount {  get; set; } = decimal.Zero;
        public DateTime ChargeDate { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Property request
                   && PropertyName == request.PropertyName
                   && PropertyNumber == request.PropertyNumber
                   && Line1 == request.Line1
                   && Line2 == request.Line2
                   && Line3 == request.Line3
                   && City == request.City
                   && County == request.County
                   && PostCode == request.PostCode
                   && Tenure == request.Tenure
                   && TitleNumber == request.TitleNumber
                   && ExpectedPrice == request.ExpectedPrice
                   && PropertyDescription == request.PropertyDescription
                   && LocalAuthority == request.LocalAuthority;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(PropertyName);
            hash.Add(PropertyNumber);
            hash.Add(Line1);
            hash.Add(Line2);
            hash.Add(Line3);
            hash.Add(City);
            hash.Add(County);
            hash.Add(PostCode);
            hash.Add(Tenure);
            hash.Add(TitleNumber);
            hash.Add(PropertyDescription);
            hash.Add(ExpectedPrice); 
            hash.Add(LocalAuthority);
            return hash.ToHashCode();
        }

        public static bool operator ==(Property left, Property right)
        {
            return EqualityComparer<Property>.Default.Equals(left, right);
        }

        public static bool operator !=(Property left, Property right)
        {
            return !(left == right);
        }
    }

    public enum SearchType : ushort
    {
        Individual = 10,
        Company = 20,
        Complex = 30,
        LocalAuthority = 40
    }
    public class SearchParty
    {
        // All Search - should be in flags
        public bool ContinueIfFeeExceeds { get; set; } = false;

        public SearchType SearchType { get; set; }
        public Applicant[] Applicants {  get; set; }

        public string[] County { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SearchParty request
                   && ContinueIfFeeExceeds == request.ContinueIfFeeExceeds
                   && Applicants == request.Applicants;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(ContinueIfFeeExceeds);
            hash.Add(Applicants);
            return hash.ToHashCode();
        }

        public static bool operator ==(SearchParty left, SearchParty right)
        {
            return EqualityComparer<SearchParty>.Default.Equals(left, right);
        }

        public static bool operator !=(SearchParty left, SearchParty right)
        {
            return !(left == right);
        }
    }

    public class Applicant
    {
        public string CompanyName { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string Middle { get; set; }
        public string ComplexName { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public string Client
        {
            get { return this.Forename + " " + this.Surname; }
        }

        public override bool Equals(object obj)
        {
            return obj is Applicant copy
                && CompanyName == copy.CompanyName
                && Surname == copy.Surname
                && Forename == copy.Forename
                && Middle == copy.Middle
                && ComplexName == copy.ComplexName
                && From == copy.From
                && To == copy.To;
        }
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(CompanyName);
            hash.Add(Surname);
            hash.Add(Forename);
            hash.Add(Middle);
            hash.Add(ComplexName);
            hash.Add(From);
            hash.Add(To);
            return hash.ToHashCode();
        }

        public static bool operator ==(Applicant left, Applicant right)
        {
            return EqualityComparer<Applicant>.Default.Equals(left, right);
        }

        public static bool operator !=(Applicant left, Applicant right)
        {
            return !(left == right);
        }

    }
    public class Flags
    {
        private readonly string[] allowedOC = new string[] { "OC1", "OC2" };
        private readonly string[] allowedROC = new string[] { "RegisterOnly", "TitleOnly", "RegisterAndTitlePlan", "CI", "CIAndRegister", "" };
        private string officialCopy = "OC1";
        private string requestedOfficialCopy = string.Empty;

        public Flags()
        {
            officialCopy = "OC1";
            requestedOfficialCopy = string.Empty;
        }

        public string OfficialCopy
        {
            get
            {
                return officialCopy;
            }
            set
            {
                if (!allowedOC.Any(x => x == value))
                    throw new ArgumentException("Not valid - OfficialCopy");
                officialCopy = value;
            }
        }
        public string RequestedOfficialCopy
        {
            get
            {
                return requestedOfficialCopy;
            }
            set
            {
                if (!allowedROC.Any(x => x == value))
                    throw new ArgumentException("Not valid - requestedOfficialCopy");
                requestedOfficialCopy = value;
            }
        }
        public bool ContinueIfFeeExceeds { get; set; } = false;
        public bool ClosedAndContinued { get; set; } = false;
        public bool PendingApps { get; set; } = false;
        public bool FirstRegistration { get; set; } = false;
        public bool SendBackdated { get; set; } = false;
        public decimal[] CertificateInFormCI { get; set; }

        /* 
         *  Other Flags
            Type
            ContinueOOH
            SkipPartialMatch
            SkipHistoricalMatch
            ApplicationRef
            ContinueIf
            ChargeDate
            PriorityType
            RegisterProprietor
            ApplicantNames
            AttachmentDetails
            TitlePlanDetails
            EstatePlanDetails
            TimeShareDetails
            AlternativeAddressDetails
         */

        public override bool Equals(object obj)
        {
            return obj is Flags copy
                && OfficialCopy == copy.OfficialCopy
                && RequestedOfficialCopy == copy.RequestedOfficialCopy
                && ContinueIfFeeExceeds == copy.ContinueIfFeeExceeds
                && ClosedAndContinued == copy.ClosedAndContinued
                && PendingApps == copy.PendingApps
                && FirstRegistration == copy.FirstRegistration
                && SendBackdated == copy.SendBackdated
                && CertificateInFormCI == copy.CertificateInFormCI;
        }
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(OfficialCopy);
            hash.Add(ContinueIfFeeExceeds);
            hash.Add(ClosedAndContinued);
            hash.Add(PendingApps);
            hash.Add(FirstRegistration);
            hash.Add(SendBackdated);
            hash.Add(CertificateInFormCI);
            return hash.ToHashCode();
        }

        public static bool operator ==(Flags left, Flags right)
        {
            return EqualityComparer<Flags>.Default.Equals(left, right);
        }

        public static bool operator !=(Flags left, Flags right)
        {
            return !(left == right);
        }
    }
    public class DocumentInfo
    {
        private readonly string[] allowedDoc = new string[] { "Abstract", "Agreement", "Assent",
                "Assignment", "Charge","Conveyance","Deed",
                "Indenture","Lease","Licence","Plan",
                "Sub - Charge","Transfer","Other","Commonhold Community Statement",
                "Memorandum and Articles of Association","Surrender of Development Rights",
                "Termination Document"};

        private string docType;

        public string DocumentType
        {
            get
            {
                return docType;
            }
            set
            {
                if (!allowedDoc.Any(x => x == value))
                    throw new ArgumentException("Not valid - DocumentType");
                docType = value;
            }
        }
        public DateTime DocumentDate { get; set; }
        public string AdditionalInfo { get; set; }
        public string TitleNumberFiledUnder { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj is DocumentInfo copy
                   && DocumentType == copy.DocumentType
                   && DocumentDate == copy.DocumentDate
                   && AdditionalInfo == copy.AdditionalInfo;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(DocumentType);
            hash.Add(DocumentDate);
            hash.Add(AdditionalInfo);
            return hash.ToHashCode();
        }

        public static bool operator ==(DocumentInfo left, DocumentInfo right)
        {
            return EqualityComparer<DocumentInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(DocumentInfo left, DocumentInfo right)
        {
            return !(left == right);
        }
    }
    public class Contact
    {
        public Contact()
        {
            Name = Environment.GetEnvironmentVariable("LandRegistryContactName"); 
            Telephone = Environment.GetEnvironmentVariable("LandRegistryContactPhone");
        }

        public string Name { get; }
        public string Telephone { get; }

        //public string Name { get { return Environment.GetEnvironmentVariable("LandRegistryContactName"); } }
        //public string Telephone { get { return Environment.GetEnvironmentVariable("LandRegistryContactPhone"); } }

        public override bool Equals(object obj)
        {
            return obj is Contact
                && Name == Name
                && Telephone == Telephone;
        }
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Name);
            hash.Add(Telephone);
            return hash.ToHashCode();
        }

        public static bool operator ==(Contact left, Contact right)
        {
            return EqualityComparer<Contact>.Default.Equals(left, right);
        }

        public static bool operator !=(Contact left, Contact right)
        {
            return !(left == right);
        }
    }
    public class Other
    {
        /*
         *  Reference
            FeeInPence
            GeoJson
            PropertyDescription
            RefreshSearchReference
            ResponseFormat
            ProprietorApplicantDeatils
            ApplicantNames
            Attachment
            Identifier

         */
    }
    public class AlternativeDespatchDetails
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string[] Address { get; set; }
        public string PostCode { get; set; }
        public string DXNumber { get; set; }  
        public string ExchangeName { get; set; }

    }
    public class ApplicationReference
    {
        public string Reference { get; set; } = null;
    }

    public class PrioritySearch
    {
        public string PriorityType { get; set; }
        public string PropertyDescription { get; set; }
        public PropertyIdentification PropertyIdentification { get; set; }
        public DateTime SearchFrom { get; set; }
        public bool ContinueIfFeeExceeds { get; set; } = false;
        public bool ContinueIfNameMismatch { get; set; } = false;
        public bool ContinueIfDeveloperTitle { get; set; } = false;
        public bool ContinueIfPendingSearchesOfPart { get; set; } = false;
        public string PropreietorOrFirstApplicant { get; set; } 
        public string ApplicantNames { get; set; }
        public TimeshareDetails TimeshareDetails { get; set; }

        public override bool Equals(object obj)
        {
            return obj is PrioritySearch copy
                && PriorityType == copy.PriorityType
                && PropertyDescription == copy.PropertyDescription
                && SearchFrom == copy.SearchFrom
                && ContinueIfFeeExceeds == copy.ContinueIfFeeExceeds
                && ContinueIfNameMismatch == copy.ContinueIfNameMismatch
                && ContinueIfDeveloperTitle == copy.ContinueIfDeveloperTitle
                && ContinueIfPendingSearchesOfPart == copy.ContinueIfPendingSearchesOfPart
                && PropreietorOrFirstApplicant == copy.PropreietorOrFirstApplicant
                && ApplicantNames == copy.ApplicantNames;

        }
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(PriorityType);
            hash.Add(PropertyDescription);
            hash.Add(SearchFrom);
            hash.Add(ContinueIfFeeExceeds);
            hash.Add(ContinueIfNameMismatch);
            hash.Add(ContinueIfDeveloperTitle);
            hash.Add(ContinueIfPendingSearchesOfPart);
            hash.Add(PropreietorOrFirstApplicant);
            hash.Add(ApplicantNames);
            return hash.ToHashCode();
        }

        public static bool operator ==(PrioritySearch left, PrioritySearch right)
        {
            return EqualityComparer<PrioritySearch>.Default.Equals(left, right);
        }

        public static bool operator !=(PrioritySearch left, PrioritySearch right)
        {
            return !(left == right);
        }

    }

    public class PropertyIdentification
    {
        public string PropertyDescription { get; set; } = null;
        public TitlePlan TitlePlan { get; set; } = null;
        public EstatePlan EstatePlan { get; set; } = null;
        public PlanAttachment PlanAttachment { get; set; } = null;

    }

    public class TitlePlan
    {
        public string TitlePlanNumber { get; set; }
        public string Reference { get; set; }
    }

    public class EstatePlan
    {
        public DateTime ApprovalDate { get; set; }
        public string PlotDetails { get; set; }
    }
    public class PlanAttachment
    {
        public string Title {  get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public string CopyrightNotices { get; set; }
        public BinaryObjectType BinaryObject { get; set; }
    }

    public class TimeshareDetails
    {
        public bool TimeshareLease { get; set; } = false;
        public string TimePeriod { get; set; }
    }

}

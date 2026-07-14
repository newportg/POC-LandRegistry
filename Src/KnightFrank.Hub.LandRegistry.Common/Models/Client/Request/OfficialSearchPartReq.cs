using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Request
{
    public class OfficialSearchPartReq
    {
        public RequestTypes RequestType { get; } = RequestTypes.OfficialSearchPart;

        [Description("Identity")]
        [Required, ValidateObject]
        public Identity Identity { get; set; }

        [Description("Property")]
        [Required, ValidateObject]
        public OfficialSearchPartReq_Property Property { get; set; }

        [ValidateObject]
        public OfficialSearchPartReq_PrioritySearch PrioritySearch { get; set; }
    }

    public class OfficialSearchPartReq_Property
    {
        [Description("The customers expected fee value.")]
        [RegularExpression(@"^[0-9]*(\.[0-9]{0,2})?$", ErrorMessage = "ExpectedPrice: decimal to 2 decimal places")]
        public decimal ExpectedPrice { get; set; }

        [Description("A Title Number is a unique number assigned to a parcel of land by Land Registry.  ")]
        [StringLength(9, MinimumLength = 1, ErrorMessage = "Title Number: string between 1 and 9 characters")]
        [RegularExpression(@"[A-Z]{0,3}[0-9]{1,6}[ZT]?", ErrorMessage = "Title Number: string between 1 and 9 characters: Fmt [A-Z]{0,3}[0-9]{1,6}[ZT]")]
        public string TitleNumber { get; set; }
    }

    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum PriorityType { Purchase = 10, Lease = 20, Charge = 30}

    public class OfficialSearchPartReq_PrioritySearch
    {
        [Description("The customer's selection type for the search. ")]
        [Required]
        public PriorityType PriorityType { get; set; }
        public string PropertyDescription { get; set; }

        [Required, ValidateObject]
        public OfficialSearchPartReq_PropertyIdentification PropertyIdentification { get; set; }

        [Description("Search From")]
        //[DataType(DataType.Date)]
        //[RegularExpression(@"(18|19|20)\d\d\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])", ErrorMessage = "SearchFrom Date: Fmt YYYY-MM-DD")]
        public DateTime SearchFrom { get; set; }

        [Description("If true, tells LR to continue with search if the expected input fee is less than the actual cost of the search.")]
        [Required]
        public bool ContinueIfFeeExceeds { get; set; } = false;

        [Description("The search will continue and a fee will be charged.")]
        [Required]
        public bool ContinueIfNameMismatch { get; set; } = false;

        //[Description("The customer's selection type for the search. ")]
        //public bool ContinueIfDeveloperTitle { get; set; } = false;
        //public bool ContinueIfPendingSearchesOfPart { get; set; } = false;

        [Description("The proprietors details.")]
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "PropreietorOrFirstApplicant : The name of the proprietor.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "PropreietorOrFirstApplicant : The name of the proprietor.")]
        public string PropreietorOrFirstApplicant { get; set; }

        [Description("The name of the applicant.")]
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "ApplicantNames : The name of the applicant.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "ApplicantNames : The name of the applicant.")]
        public string ApplicantNames { get; set; }

        [Description("Timeshare Details")]
        public OfficialSearchPartReq_TimeshareDetails TimeshareDetails { get; set; }

    }

    [OnlyOnePropertyAttribute("PropertyDescription", "TitlePlan", "EstatePlan", "PlanAttachment", ErrorMessage = "You must supply ONLY one PropertyIdentification value")]
    public class OfficialSearchPartReq_PropertyIdentification
    {
        [Description("Property description of the subject property.")]
        [StringLength(130, MinimumLength = 1, ErrorMessage = "PropertyDescription : Property description.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "PropertyDescription : Property description.")]
        public string PropertyDescription { get; set; }
        public OfficialSearchPartReq_TitlePlan TitlePlan { get; set; }
        public OfficialSearchPartReq_EstatePlan EstatePlan { get; set; }

        [Description("An electronic copy of a document that may be attached to a message. ")]
        public OfficialSearchPartReq_PlanAttachment PlanAttachment { get; set; }
    }

    public class OfficialSearchPartReq_TitlePlan
    {
        [Description("Number of title plan to be searched")]
        [StringLength(9, MinimumLength = 1, ErrorMessage = "TitlePlanNumber : string between 1 and 9 characters")]
        [RegularExpression(@"[A-Z]{0,3}[0-9]{1,6}[ZT]?", ErrorMessage = "TitlePlanNumber : string between 1 and 9 characters: Fmt [A-Z]{0,3}[0-9]{1,6}[ZT]")]
        public string TitlePlanNumber { get; set; }

        [Description("The reference for the title plan")]
        [RegularExpression(@".*\S.*", ErrorMessage = "Reference : title plan reference")]
        public string Reference { get; set; }
    }

    public class OfficialSearchPartReq_EstatePlan
    {
        [Description("Approval Date")]
        [DataType(DataType.Date)]
        [RegularExpression(@"(18|19|20)\d\d\-(0[1-9]|1[012])\-(0[1-9]|[12][0-9]|3[01])", ErrorMessage = "Approval Date Date: Fmt YYYY-MM-DD")]
        public DateTime ApprovalDate { get; set; }

        [Description("Plot details of an approved estate plan that relates to the search ")]
        [StringLength(25, MinimumLength = 1, ErrorMessage = "PlotDetails : The details of the plot.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "PlotDetails : The details of the plot.")]
        public string PlotDetails { get; set; }
    }
    public class OfficialSearchPartReq_PlanAttachment
    {
        [Description("The full name of the file being sent")]
        [Required]
        public string Title { get; set; }
        public string Date { get; set; }

        [Description("A description of the contents of the attached file or document")]
        [RegularExpression(@".*\S.*", ErrorMessage = "Description : Document description.")]
        public string Description { get; set; }
        public string CopyrightNotices { get; set; }

        [Description("A set of finite-length sequences of binary octets.")]
        //[Required]
        public BinaryObjectType BinaryObject { get; set; }
    }

    public class OfficialSearchPartReq_TimeshareDetails
    {
        [Description("Indicates there is a discontinuous timeshare lease on the subject property ")]
        public bool TimeshareLease { get; set; } = false;

        [Description("The amount of time that has demised on a timeshare lease ")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "TimePeriod : The name of the applicant.")]
        [RegularExpression(@".*\S.*", ErrorMessage = "TimePeriod : The name of the applicant.")]
        public string TimePeriod { get; set; }
    }

}

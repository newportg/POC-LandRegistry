using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Request
{
    public class ApplicationEnquiryReq
    {
        public RequestTypes RequestType { get; set; } = RequestTypes.ApplicationEnquiry;

        [Description("Identity")]
        [Required]
        public Identity Identity { get; set; }

        [Description("Property")]
        [Required]
        public ApplicationEnquiryReq_Property Property { get; set; }

        [Description("The Land Registry’s unique application reference ")]
//        [MinLength(1)]
        [MaxLength(7)]
        //        [Required]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ApplicationReference { get; set; }
    }

    public class ApplicationEnquiryReq_Property
    {

        [Description("TitleNumber")]
        [MinLength(1)]
        [MaxLength(9)]
        [RegularExpression(@"[A-Z]{0,3}[0-9]{1,6}[ZT]?")]
        public string TitleNumber { get; set; }

        [Description("Continue If Title Is Closed And Continued Indicator")]
        public bool ContinueIfTitleIsClosedAndContinuedIndicator { get; set; } = false;
    }
}

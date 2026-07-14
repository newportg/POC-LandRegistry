using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Request
{
    public class DischargeActivityReq
    {
        public RequestTypes RequestType { get; set; } = RequestTypes.DischargeActivity;

        [Description("Identity")]
        [Required]
        public Identity Identity { get; set; }

        [Description("Property")]
        [Required]
        public DischargeActivityReq_Property Property { get; set; }
    }


    public class DischargeActivityReq_Property
    {

        [Description("TitleNumber")]
        [MinLength(1)]
        [MaxLength(9)]
        [RegularExpression(@"[A-Z]{0,3}[0-9]{1,6}[ZT]?")]
        public string TitleNumber { get; set; }

        [Description("Continue If Title Is Closed And Continued Indicator")]
        public bool ContinueIfTitleIsClosedAndContinuedIndicator { get; set; } = false;

        [Description("ChargeDate")]
        [DataType(DataType.Date)]
        public DateTime ChargeDate { get; set; }
    }
}

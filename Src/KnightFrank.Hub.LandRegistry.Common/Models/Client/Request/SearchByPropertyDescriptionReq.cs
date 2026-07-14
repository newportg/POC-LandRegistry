using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Request
{
    public class SearchByPropertyDescriptionReq
    {
        public RequestTypes RequestType { get; } = RequestTypes.EnquiryByPropertyDescription;

        [Description("Identity")]
        [Required, ValidateObject]
        public Identity Identity { get; set; }

        [Description("Property")]
        [Required, ValidateObject]
        public SearchByPropertyDescriptionReq_Property Property { get; set; }

    }

    public class SearchByPropertyDescriptionReq_Property
    {
        [Description("The name of the house or building")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "BuildingName: string between 1 and 50 characters")]
        [RegularExpression(@".*\S.*", ErrorMessage = "BuildingName: string between 1 and 50 characters")]
        public string BuildingName { get; set; }

        [Description("The number of the house or building")]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "BuildingNumber: string between 1 and 5 characters")]
        [RegularExpression(@".*\S.*", ErrorMessage = "BuildingNumber: string between 1 and 5 characters")]
        public string BuildingNumber { get; set; }

        [Description("The name of the street where the target property is located")]
        [StringLength(80, MinimumLength = 1, ErrorMessage = "StreetName: string between 1 and 80 characters")]
        [RegularExpression(@".*\S.*", ErrorMessage = "StreetName: string between 1 and 80 characters")]
        public string StreetName { get; set; }

        [Description("The name of the city where the target property is located")]
        [StringLength(35, MinimumLength = 1, ErrorMessage = "CityName: string between 1 and 35 characters")]
        [RegularExpression(@".*\S.*", ErrorMessage = "CityName: string between 1 and 35 characters")]
        public string CityName { get; set; }

        [Description("The postcode where the target property is located")]
        [StringLength(8, MinimumLength = 1, ErrorMessage = "PostcodeZone: string between 1 and 8 characters")]
        [RegularExpression(@"[A-Z]{1,2}[0-9R][0-9A-Z]? [0-9][A-Z-[CIKMOV]]{2}", ErrorMessage = "PostcodeZone: Invalid Format [A-Z][A-Z][0-9][0-9A-Z] [0-9][A-Z][A-Z]")]
        [DataType(DataType.PostalCode)]
        public string PostcodeZone { get; set; }

    }
}

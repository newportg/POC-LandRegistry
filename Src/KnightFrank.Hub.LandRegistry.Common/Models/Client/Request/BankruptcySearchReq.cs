using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KnightFrank.Hub.LandRegistry.Common.Models.Client.Request
{
    public class BankruptcySearchReq
    {
        public RequestTypes RequestType { get; set; } = RequestTypes.LCBankruptcySearch;

        [Description("Identity")]
        [Required]
        public Identity Identity { get; set; }

        [Description("Information about price")]
        public ExpectedPrice ExpectedPrice { get; set; }

        [Description("The information relevant to methods of communication for this Contact.")]
        [Required]
        public Contact Contact { get; set; }

        [Description("An application for a Land Charges Bankruptcy Search against Private Individuals and Complex Names.")]
        [Required]
        public LandChargesBankruptcySearch LandChargesBankruptcySearch { get; set; }

        [Description("Alternate Delivery Address")]
        public AlternativeDespatchDetails AlternativeDespatchDetails { get; set; }
    }

    public class ExpectedPrice
    {

        [Description(">The gross price is the price including the VAT. If the VAT is zero then the Gross and Net Price will be the same")]
        public AmountType GrossPriceAmount { get; set; }

        [Description(">The Net Price is the amount before the VAT is added on. If the VAT is zero then the Gross and Net Price will be the same")]
        public AmountType NetPriceAmount { get; set; }

        [Description("This is the amount of the VAT")]
        public AmountType VATAmount { get; set; }
    }

    public class AmountType
    {
        [Description("Currency")]
        public string Currency { get; set; }

        [Description("Amount")]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }
    }

    public class Contact
    {
        [Description("The name of this contact person or department.")]
        [RegularExpression(@".*\S.*")]
        [Required]
        public string Name { get; set; }

        [Description("Telephone")]
        [RegularExpression(@".*\S.*")]
        [Required]
        public string Telephone { get; set; }
    }

    public class LandChargesBankruptcySearch
    {
        [Description("Telephone")]
        [Required]
        public BankruptcySearchComplexName BankruptcySearchComplexName { get; set; }

        [Required]
        public string BankruptcySearchPrivateIndividual { get; set; }

        [Description("Continue If Title Is Closed And Continued Indicator")]
        public bool ContinueIfTitleIsClosedAndContinuedIndicator { get; set; } = false;
    }

    public class BankruptcySearchComplexName
    {
        [Required]
        public string BankruptcySearchTypeCode { get; } = "30";

        [MinLength(1)]
        [MaxLength(6)]
        public ComplexName[] BankruptcySearchParty { get; set; }
    }

    public class BankruptcySearchPrivateIndividual
    {
        [Required]
        public string BankruptcySearchTypeCode { get; } = "10";

        [MinLength(1)]
        [MaxLength(6)]
        [Required]
        public PrivateIndividual[] BankruptcySearchParty { get; set; }
    }

    public class ComplexName
    {
        [MinLength(1)]
        [MaxLength(90)]
        [RegularExpression(".*\\S.*")]
        [Required]
        public string Name { get; set; }
    }

    public class PrivateIndividual
    {
        [MinLength(1)]
        [MaxLength(90)]
        [RegularExpression("([A-Za-z\\-'])([A-Za-z\\- '])*([A-Za-z\\-'])")]
        [Required]
        public string Forename { get; set; }

        [MinLength(1)]
        [MaxLength(89)]
        [RegularExpression("([A-Za-z\\-'])([A-Za-z\\- '])*([A-Za-z\\-'])")]
        [Required]
        public string Surname { get; set; }
    }

    public class AlternativeDespatchDetails
    {

        [Description(" Name of firm/company results will be addressed to.")]
        [MinLength(1)]
        [MaxLength(70)]
        [RegularExpression(@"[A-Za-z09\\s~!&quot;@#$%'\(\)\*\+,\-\./:;=&gt;\?\[\\\]_\{\}\^&#xa3;]*")]
        public string AlternativeDespatchName { get; set; }

        [Description(" Text reference to be displayed on results.")]
        [MinLength(1)]
        [MaxLength(25)]
        [RegularExpression(@"[A-Za-z09\s~!&quot;@#$%'\(\)\*\+,\-\./:;=&gt;\?\[\\\]_\{\}\^&#xa3;]*")]
        public string AlternativeDespatchReference { get; set; }

        public AlternativeDespatchAddress AlternativeDespatchAddress { get; set; }
    }

    public class AlternativeDespatchAddress
    {
        public DXDetails DXDetails { get; set; }
        public PostalAddress PostalAddress { get; set; }
    }

    public class DXDetails
    {
        [Description(">A unique identifier for a delivery point for organisations using the Document Exchange service.")]
        public string DXNumber { get; set; }

        public string ExchangeName { get; set; }
    }

    public class PostalAddress
    {
        [Description("A line of the address for the alternative despatch location. ")]
        [RegularExpression(@".*\S.* ")]
        public string Line1 { get; set; }
        [Description("A line of the address for the alternative despatch location. ")]
        [RegularExpression(@".*\S.* ")]
        public string Line2 { get; set; }
        [Description("A line of the address for the alternative despatch location. ")]
        [RegularExpression(@".*\S.* ")]
        public string Line3 { get; set; }
        [Description("A line of the address for the alternative despatch location. ")]
        [RegularExpression(@".*\S.* ")]
        public string Line4 { get; set; }
        [Description("A line of the address for the alternative despatch location. ")]
        [RegularExpression(@".*\S.* ")]
        public string Line5 { get; set; }

        [Description("A valid postcode for the alternative address.")]
        [RegularExpression(@".*\S.* ")]
        public string Postcode { get; set; }
    }
}

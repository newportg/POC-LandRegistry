namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class FindResponse
    {
        public string Status { get; set; }
        public string AddressId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyNumber { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }

        //public Response Response { get; set; }
    }
}


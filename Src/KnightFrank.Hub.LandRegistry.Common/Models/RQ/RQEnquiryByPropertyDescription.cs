//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KnightFrank.Hub.LandRegistry.Common.Models.RQ
//{
//    public class RQEnquiryByPropertyDescription
//    {
//        private decimal expectedPrice;

//        public RQEnquiryByPropertyDescription()
//        {
//            _ = decimal.TryParse(Environment.GetEnvironmentVariable("LandRegistryExpectedPrice"), out expectedPrice);
//        }

//        public string PropertyName { get; set; }
//        public string PropertyNumber { get; set; }
//        public string Line1 { get; set; }
//        public string Line2 { get; set; }
//        public string Line3 { get; set; }
//        public string City { get; set; }
//        public string County { get; set; }
//        public string PostCode { get; set; }
//        public string Tenure { get; set; }
//        public string TitleNumber { get; set; }
//        public decimal ExpectedPrice
//        {
//            get
//            {
//                return expectedPrice;
//            }
//            set
//            {
//                expectedPrice = value;
//            }
//        }
//        public string PropertyDescription { get; set; }
//        public string LocalAuthority { get; set; }
//        public string CurrencyID { get; set; }
//        public decimal GrossPriceAmount { get; set; } = decimal.Zero;
//        public decimal NetPriceAmount { get; set; } = decimal.Zero;
//        public decimal VATAmount { get; set; } = decimal.Zero;
//        public DateTime ChargeDate { get; set; }

//        public override bool Equals(object obj)
//        {
//            return obj is RQEnquiryByPropertyDescription request
//                   && PropertyName == request.PropertyName
//                   && PropertyNumber == request.PropertyNumber
//                   && Line1 == request.Line1
//                   && Line2 == request.Line2
//                   && Line3 == request.Line3
//                   && City == request.City
//                   && County == request.County
//                   && PostCode == request.PostCode
//                   && Tenure == request.Tenure
//                   && TitleNumber == request.TitleNumber
//                   && ExpectedPrice == request.ExpectedPrice
//                   && PropertyDescription == request.PropertyDescription
//                   && LocalAuthority == request.LocalAuthority;
//        }

//        public override int GetHashCode()
//        {
//            HashCode hash = new HashCode();
//            hash.Add(PropertyName);
//            hash.Add(PropertyNumber);
//            hash.Add(Line1);
//            hash.Add(Line2);
//            hash.Add(Line3);
//            hash.Add(City);
//            hash.Add(County);
//            hash.Add(PostCode);
//            hash.Add(Tenure);
//            hash.Add(TitleNumber);
//            hash.Add(PropertyDescription);
//            hash.Add(ExpectedPrice);
//            hash.Add(LocalAuthority);
//            return hash.ToHashCode();
//        }

//        public static bool operator ==(RQEnquiryByPropertyDescription left, RQEnquiryByPropertyDescription right)
//        {
//            return EqualityComparer<RQEnquiryByPropertyDescription>.Default.Equals(left, right);
//        }

//        public static bool operator !=(RQEnquiryByPropertyDescription left, RQEnquiryByPropertyDescription right)
//        {
//            return !(left == right);
//        }
//    }
//}

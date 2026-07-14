using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class EnquiryByPropertyDescriptionMessageDetails : MessageDetails
    {
        public string Description { get; set; }
        public Title[] Titles { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is EnquiryByPropertyDescriptionMessageDetails))
                return false;

            EnquiryByPropertyDescriptionMessageDetails response = (EnquiryByPropertyDescriptionMessageDetails)obj;

            // Check reference equality (same object)
            if (ReferenceEquals(this.Titles, response.Titles))
                return true;

            // Check element-wise equality
            if (this.Titles.Length != response.Titles.Length)
                return false;

            for (int i = 0; i < this.Titles.Length; i++)
            {
                if (!EqualityComparer<Title>.Default.Equals(this.Titles[i], response.Titles[i]))
                    return false;
            }

            return Description == response.Description;
        }

        public override global::System.Int32 GetHashCode()
        {
            return HashCode.Combine(Description, Titles);
        }

        public static global::System.Boolean operator ==(EnquiryByPropertyDescriptionMessageDetails left, EnquiryByPropertyDescriptionMessageDetails right)
        {
            return EqualityComparer<EnquiryByPropertyDescriptionMessageDetails>.Default.Equals(left, right);
        }

        public static global::System.Boolean operator !=(EnquiryByPropertyDescriptionMessageDetails left, EnquiryByPropertyDescriptionMessageDetails right)
        {
            return !(left == right);
        }
    }

    public class Title
    {
        public string TitleNumber { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public string TenureTypeCode { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Title title
                   && TitleNumber == title.TitleNumber
                   //&& Description == title.Description 
                   && EqualityComparer<Address>.Default.Equals(Address, title.Address)
                   && TenureTypeCode == title.TenureTypeCode;
        }

        public override global::System.Int32 GetHashCode()
        {
            return HashCode.Combine(TitleNumber, Description, Address, TenureTypeCode);
        }

        public static global::System.Boolean operator ==(Title left, Title right)
        {
            return EqualityComparer<Title>.Default.Equals(left, right);
        }

        public static global::System.Boolean operator !=(Title left, Title right)
        {
            return !(left == right);
        }
    }

    public class Address
    {
        public string BuildingName { get; set; }
        public string SubBuildingName { get; set; }
        public string BuildingNumber { get; set; }
        public string StreetName { get; set; }
        public string CityName { get; set; }
        public string PostcodeZone { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Address address
                   //&& BuildingName == address.BuildingName
                   //&& SubBuildingName == address.SubBuildingName
                   && BuildingNumber == address.BuildingNumber
                   && StreetName == address.StreetName
                   && CityName == address.CityName
                   && PostcodeZone == address.PostcodeZone;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(BuildingName, SubBuildingName, BuildingNumber, StreetName, CityName, PostcodeZone);
        }

        public static global::System.Boolean operator ==(Address left, Address right)
        {
            return EqualityComparer<Address>.Default.Equals(left, right);
        }

        public static global::System.Boolean operator !=(Address left, Address right)
        {
            return !(left == right);
        }
    }
}

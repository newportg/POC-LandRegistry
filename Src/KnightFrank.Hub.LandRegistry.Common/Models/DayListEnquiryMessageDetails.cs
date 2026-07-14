using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class DayListEnquiryMessageDetails : MessageDetails
    {
        public string Description { get; set; }

        public override bool Equals(object obj)
        {
            return obj is DayListEnquiryMessageDetails details &&
                   Description == details.Description;
        }

        //public override bool Equals(object obj)
        //{
        //    if (!(obj is DayListEnquiryMessageDetails))
        //        return false;
        //    var other = obj as DayListEnquiryMessageDetails;
        //    if (Description != other.Description)
        //        return false;
        //    return true;
        //}

        public override int GetHashCode()
        {
            return HashCode.Combine(Description);
        }

        public static bool operator ==(DayListEnquiryMessageDetails left, DayListEnquiryMessageDetails right)
        {
            return EqualityComparer<DayListEnquiryMessageDetails>.Default.Equals(left, right);
        }

        public static bool operator !=(DayListEnquiryMessageDetails left, DayListEnquiryMessageDetails right)
        {
            return !(left == right);
        }
    }
}

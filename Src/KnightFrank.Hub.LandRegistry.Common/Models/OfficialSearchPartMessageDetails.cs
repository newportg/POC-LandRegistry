using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class OfficialSearchPartMessageDetails : MessageDetails
    {
        public string Description { get; set; }
        public string HMLRReference { get; set; }
        public decimal ActualPrice { get; set; }
        public Attachment Attachment { get; set; }
        public string ResultTypeCode { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is OfficialSearchPartMessageDetails))
                return false;

            return obj is OfficialSearchPartMessageDetails copy
                   && ActualPrice == copy.ActualPrice
                   && EqualityComparer<Attachment>.Default.Equals(Attachment, copy.Attachment)
                   && ResultTypeCode == copy.ResultTypeCode;
            //&& Description == copy.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description);
        }

        public static bool operator ==(OfficialSearchPartMessageDetails left, OfficialSearchPartMessageDetails right)
        {
            return EqualityComparer<OfficialSearchPartMessageDetails>.Default.Equals(left, right);
        }

        public static bool operator !=(OfficialSearchPartMessageDetails left, OfficialSearchPartMessageDetails right)
        {
            return !(left == right);
        }
    }
}

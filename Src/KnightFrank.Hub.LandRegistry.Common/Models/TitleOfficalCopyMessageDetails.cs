using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class TitleOfficalCopyMessageDetails : MessageDetails
    {
        public string Description { get; set; }
        public string HMLRReference { get; set; }
        public decimal ActualPrice { get; set; }
        public Attachment Attachment { get; set; }
        public string ResultTypeCode { get; set; }


        public override bool Equals(object obj)
        {
            if (!(obj is TitleOfficalCopyMessageDetails))
                return false;

            return obj is TitleOfficalCopyMessageDetails copy
                   && ActualPrice == copy.ActualPrice
                   && EqualityComparer<Attachment>.Default.Equals(Attachment, copy.Attachment)
                   && ResultTypeCode == copy.ResultTypeCode;
            //&& Description == copy.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Description);
        }

        public static bool operator ==(TitleOfficalCopyMessageDetails left, TitleOfficalCopyMessageDetails right)
        {
            return EqualityComparer<TitleOfficalCopyMessageDetails>.Default.Equals(left, right);
        }

        public static bool operator !=(TitleOfficalCopyMessageDetails left, TitleOfficalCopyMessageDetails right)
        {
            return !(left == right);
        }
    }
    public class Attachment
    {
        public BinaryObjectType EmbeddedFileBinaryObject { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string CopyrightNotices { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Attachment attachment
                   && EqualityComparer<BinaryObjectType>.Default.Equals(EmbeddedFileBinaryObject, attachment.EmbeddedFileBinaryObject);
            //&& Title == attachment.Title 
            //&& Description == attachment.Description
            //&& Date == attachment.Date
            //&& CopyrightNotices == attachment.CopyrightNotices;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(EmbeddedFileBinaryObject, Title, Description, Date, CopyrightNotices);
        }

        public static bool operator ==(Attachment left, Attachment right)
        {
            return EqualityComparer<Attachment>.Default.Equals(left, right);
        }

        public static bool operator !=(Attachment left, Attachment right)
        {
            return !(left == right);
        }
    }
    public class BinaryObjectType
    {
#pragma warning disable IDE1006 // Naming Styles
        public string filename { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public string format { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public string mimeCode { get; set; }
#pragma warning restore IDE1006 // Naming Styles
#pragma warning disable IDE1006 // Naming Styles
        public string characterSetCode { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        public byte[] Value { get; set; }

        public override bool Equals(object obj)
        {
            return obj is BinaryObjectType type
                   && format == type.format;
            //&& filename == type.filename 
            //&& mimeCode == type.mimeCode
            //&& characterSetCode == type.characterSetCode;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(filename, format, mimeCode, characterSetCode);
        }

        public static bool operator ==(BinaryObjectType left, BinaryObjectType right)
        {
            return EqualityComparer<BinaryObjectType>.Default.Equals(left, right);
        }

        public static bool operator !=(BinaryObjectType left, BinaryObjectType right)
        {
            return !(left == right);
        }
    }
}

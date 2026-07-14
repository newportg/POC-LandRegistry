using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class Error
    {
        public string Code { get; set; }
        public string Description { get; set; }

        public override global::System.Boolean Equals(global::System.Object obj)
        {
            return obj is Error error &&
                   Code == error.Code &&
                   Description == error.Description;
        }

        //public override bool Equals(object obj)
        //{
        //    if (!(obj is Error))
        //        return false;

        //    var other = obj as Error;

        //    if (Code != other.Code
        //        || Description != other.Description)
        //        return false;

        //    return true;
        //}

        public override global::System.Int32 GetHashCode()
        {
            return HashCode.Combine(Code, Description);
        }

        public static global::System.Boolean operator ==(Error left, Error right)
        {
            return EqualityComparer<Error>.Default.Equals(left, right);
        }

        public static global::System.Boolean operator !=(Error left, Error right)
        {
            return !(left == right);
        }
    }
}

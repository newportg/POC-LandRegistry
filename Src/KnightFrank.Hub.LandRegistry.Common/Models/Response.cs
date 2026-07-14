using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class Response
    {
        public string Status { get; set; }
        public Acknowledgement Acknowledgement { get; set; }
        public Rejection Rejection { get; set; }
        public Results Results { get; set; }
    }

    public class Acknowledgement
    {
        public string UniqueMsgId { get; set; }
        public DateTime ExpectedResponseDateTime { get; set; }
        public string MessageDescription { get; set; }
        public string HMLRReference { get; set; }
        public override bool Equals(object obj)
        {
            return obj is Acknowledgement copy
                //&& UniqueMsgId == copy.UniqueMsgId
                //&& ExpectedResponseDateTime == copy.ExpectedResponseDateTime
                && MessageDescription.StartsWith(copy.MessageDescription);
                //&& HMLRReference == copy.HMLRReference;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(UniqueMsgId);
            //hash.Add(ExpectedResponseDateTime);
            hash.Add(MessageDescription);
            //hash.Add(HMLRReference);
            return hash.ToHashCode();
        }

        public static bool operator ==(Acknowledgement left, Acknowledgement right)
        {
            return EqualityComparer<Acknowledgement>.Default.Equals(left, right);
        }

        public static bool operator !=(Acknowledgement left, Acknowledgement right)
        {
            return !(left == right);
        }
    }

    public class Rejection
    {
        public string ExternalReference { get; set; }
        public RejectionResponse RejectionResponse { get; set; }

        public override global::System.Boolean Equals(global::System.Object obj)
        {
            return obj is Rejection rejection &&
                   ExternalReference == rejection.ExternalReference &&
                   RejectionResponse == RejectionResponse;
        }

        public override global::System.Int32 GetHashCode()
        {
            return HashCode.Combine(ExternalReference, RejectionResponse);
        }

        public static global::System.Boolean operator ==(Rejection left, Rejection right)
        {
            return EqualityComparer<Rejection>.Default.Equals(left, right);
        }

        public static global::System.Boolean operator !=(Rejection left, Rejection right)
        {
            return !(left == right);
        }
    }

    public class Results 
    {
        public string ExternalReference { get; set; }
        public MessageDetails MessageDetails { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Results copy
                && ExternalReference == copy.ExternalReference
                //&& MessageDetails == copy.MessageDetails;
                && EqualityComparer<MessageDetails>.Default.Equals(MessageDetails, copy.MessageDetails);
        }

        public override global::System.Int32 GetHashCode()
        {
            return HashCode.Combine(ExternalReference, MessageDetails);
        }

        public static global::System.Boolean operator ==(Results left, Results right)
        {
            return EqualityComparer<Results>.Default.Equals(left, right);
        }

        public static global::System.Boolean operator !=(Results left, Results right)
        {
            return !(left == right);
        }
    }

    public class RejectionResponse
    {
        public string Reason { get; set; }
        public string Code { get; set; }
        public Error[] Errors { get; set; }
        public override global::System.Boolean Equals(global::System.Object obj)
        {
            return obj is RejectionResponse rejection &&
                   Reason == rejection.Reason &&
                   Code == rejection.Code &&
                   EqualityComparer<Error[]>.Default.Equals(Errors, rejection.Errors);
        }

        public override global::System.Int32 GetHashCode()
        {
            return HashCode.Combine(Reason, Code, Errors);
        }

        public static global::System.Boolean operator ==(RejectionResponse left, RejectionResponse right)
        {
            return EqualityComparer<RejectionResponse>.Default.Equals(left, right);
        }

        public static global::System.Boolean operator !=(RejectionResponse left, RejectionResponse right)
        {
            return !(left == right);
        }
    }

}

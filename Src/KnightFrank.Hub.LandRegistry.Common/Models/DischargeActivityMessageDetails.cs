using System;
using System.Collections.Generic;

namespace KnightFrank.Hub.LandRegistry.Common.Models
{
    public class DischargeActivityMessageDetails : MessageDetails
    {
        public string Description { get; set; }
        public DateTime ResultDateTime { get; set; }
        public string TitleNumber { get; set; }
        public int DischargeCount { get; set; }
        public Discharges[] Discharges { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is DischargeActivityMessageDetails))
                return false;

            DischargeActivityMessageDetails response = (DischargeActivityMessageDetails)obj;

            // Check element-wise equality
            if (Discharges != null)
            {
                if (Discharges.Length != response.Discharges.Length)
                    return false;

                for (int i = 0; i < this.Discharges.Length; i++)
                {
                    if (!EqualityComparer<Discharges>.Default.Equals(this.Discharges[i], response.Discharges[i]))
                        return false;
                }
            }

            return obj is DischargeActivityMessageDetails copy
                   && TitleNumber == copy.TitleNumber
                   && DischargeCount == copy.DischargeCount;
        //           && ResultDateTime == copy.ResultDateTime
        //           && Description == copy.Description
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TitleNumber, DischargeCount);
        }

        public static bool operator ==(DischargeActivityMessageDetails left, DischargeActivityMessageDetails right)
        {
            return EqualityComparer<DischargeActivityMessageDetails>.Default.Equals(left, right);
        }

        public static bool operator !=(DischargeActivityMessageDetails left, DischargeActivityMessageDetails right)
        {
            return !(left == right);
        }
    }

    public class Discharges
    {
        public DateTime DischargeDate { get; set; }
        public string[] ChargeProprietors { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Discharges))
                return false;

            return obj is Discharges copy
                   && DischargeDate == copy.DischargeDate
                   && ChargeProprietors == ChargeProprietors;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DischargeDate);
        }

        public static bool operator ==(Discharges left, Discharges right)
        {
            return EqualityComparer<Discharges>.Default.Equals(left, right);
        }

        public static bool operator !=(Discharges left, Discharges right)
        {
            return !(left == right);
        }
    }
}

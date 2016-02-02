using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// DateTimeScope Ê±¼ä·¶Î§¡£
    /// zhuweisky
    /// </summary>
    [Serializable]
    public class DateTimeScope
    {
        #region Ctor
        public DateTimeScope() { }
        public DateTimeScope(DateTime start, DateTime end)
        {
            this.StartDate = start;
            this.EndDate = end;            
        } 
        #endregion   

        #region StartDate
        private DateTime startDate = DateTime.MinValue;
        public DateTime StartDate
        {
            get { return startDate; }
            set 
            {
                this.startDate = value;
            }
        } 
        #endregion

        #region EndDate
        private DateTime endDate = DateTime.MaxValue;
        public DateTime EndDate
        {
            get { return endDate; }
            set 
            {
                this.endDate = value;
            }
        } 
        #endregion

        #region Contains
        public bool Contains(DateTime target)
        {
            if ((target >= this.startDate) && (target <= this.endDate))
            {
                return true;
            }

            return false;
        } 
        #endregion

        #region operator
        public static bool operator ==(DateTimeScope left, DateTimeScope right)
        {
            if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
            {
                return true;
            }

            if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
            {
                return false;
            }

            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            bool b1 = (left.startDate == right.startDate);
            bool b2 = (left.endDate == right.endDate);

            return b1 && b2;
        }

        public static bool operator !=(DateTimeScope left, DateTimeScope right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            DateTimeScope target = obj as DateTimeScope;
            if (target == null)
            {
                return false;
            }

            return this == target;
        }
        #endregion
    }
}

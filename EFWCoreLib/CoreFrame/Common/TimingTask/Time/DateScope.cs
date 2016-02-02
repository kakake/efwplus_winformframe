using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// DateScope 日期范围
    /// zhuweisky 2007.03.15
    /// </summary>
    [Serializable]
    public class DateScope
    {
        #region ctor
        public DateScope()
        {
        }

        public DateScope(Date s, Date e)
        {
            this.start = s;
            this.end = e;
        } 
        #endregion

        #region start
        private Date start = new Date(2000,1,1);
        public Date Start
        {
            get { return start; }
            set { start = value; }
        } 
        #endregion

        #region End
        private Date end = new Date(2100,12,31);
        public Date End
        {
            get { return end; }
            set { end = value; }
        } 
        #endregion

        #region Contains
        /// <summary>
        /// Contains 目标时刻是否在时间范围内
        /// </summary>       
        public bool Contains(Date target)
        {
            bool bStart = this.start.CompareTo(target) <= 0;
            bool bEnd = this.end.CompareTo(target) >= 0;

            return bStart && bEnd;
        }

        public bool Contains(DateTime target)
        {
            return this.Contains(new Date(target));
        }
        #endregion

        #region operator
        public static bool operator ==(DateScope left, DateScope right)
        {
            if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
            {
                return false;
            }

            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            bool b1 = (left.Start.CompareTo(right.Start) == 0);
            bool b2 = (left.End.CompareTo(right.End) == 0);

            return b1 && b2;
        }

        public static bool operator !=(DateScope left, DateScope right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            DateScope target = obj as DateScope;
            if (target == null)
            {
                return false;
            }

            return this == target;
        }
        #endregion
    }
}

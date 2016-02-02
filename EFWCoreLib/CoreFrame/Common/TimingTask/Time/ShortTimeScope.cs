using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{  
    /// <summary>
    /// ShortTimeScope 时间范围。
    /// zhuweisky 2007.01.08
    /// </summary>
    [Serializable]
    public class ShortTimeScope
    {
        #region Ctor
        public ShortTimeScope() { }
        public ShortTimeScope(ShortTime first, ShortTime later)
        {
            this.shortTimeStart = first;
            this.shortTimeEnd = later;

            if (first.CompareTo(later) >= 0)
            {
                throw new Exception("the parameter later must be greatter than first!");
            }
        }        
        #endregion

        #region ShortTimeStart
        private ShortTime shortTimeStart = new ShortTime(0, 0, 0);
        public ShortTime ShortTimeStart
        {
            get { return shortTimeStart; }
            set { shortTimeStart = value; }
        }
        #endregion

        #region ShortTimeEnd
        private ShortTime shortTimeEnd = new ShortTime(24, 0, 0);
        public ShortTime ShortTimeEnd
        {
            get { return shortTimeEnd; }
            set { shortTimeEnd = value; }
        }
        #endregion

        #region Contains
        /// <summary>
        /// Contains 目标时刻是否在时间范围内
        /// </summary>       
        public bool Contains(ShortTime target)
        {
            bool bStart = this.shortTimeStart.CompareTo(target) <= 0;
            bool bEnd = this.shortTimeEnd.CompareTo(target) >= 0;

            return bStart && bEnd;
        }

        public bool Contains(DateTime target)
        {
            return this.Contains(new ShortTime(target));
        }
        #endregion

        #region operator
        public static bool operator ==(ShortTimeScope left, ShortTimeScope right)
        {
            if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
            {
                return false;
            }

            if (object.ReferenceEquals(left, right))
            {
                return true;
            }

            bool b1 = (left.ShortTimeStart.CompareTo(right.ShortTimeStart) == 0);
            bool b2 = (left.ShortTimeEnd.CompareTo(right.ShortTimeEnd) == 0);

            return b1 && b2;
        }

        public static bool operator !=(ShortTimeScope left, ShortTimeScope right)
        {
            return !(left == right) ;
        }

        public override bool Equals(object obj)
        {
            ShortTimeScope target = obj as ShortTimeScope;
            if (target == null)
            {
                return false;
            }

            return this == target;
        }
        #endregion
    }
}

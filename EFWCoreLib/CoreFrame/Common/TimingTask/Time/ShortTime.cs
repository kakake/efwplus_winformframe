using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// ShortTime 用于指定小时、分钟、秒。比如上班时间、下班时间。
    /// zhuweisky 2007.01.08
    /// </summary>
    [Serializable]
    public class ShortTime : IComparable<ShortTime>
    {
        #region Ctor
        public ShortTime() { }
        public ShortTime(int h, int m, int s)
        {
            this.Hour = h;
            this.Minute = m;
            this.Second = s;
        }

        public ShortTime(DateTime time)
        {
            this.Hour = time.Hour;
            this.Minute = time.Minute;
            this.Second = time.Second;
        }
        #endregion

        #region Property
        #region Hour
        private int hour = 0;
        public int Hour
        {
            get { return hour; }
            set
            {
                hour = value;
                this.hour = (this.hour > 23) ? 23 : this.hour;
                this.hour = (this.hour < 0) ? 0 : this.hour;

            }
        }
        #endregion

        #region Minute
        private int minute = 0;
        public int Minute
        {
            get { return minute; }
            set
            {
                minute = value;
                this.minute = (this.minute > 59) ? 59 : this.minute;
                this.minute = (this.minute < 0) ? 0 : this.minute;
            }
        }
        #endregion

        #region Second
        private int second = 0;
        public int Second
        {
            get { return second; }
            set
            {
                second = value;
                this.second = (this.second > 59) ? 59 : this.second;
                this.second = (this.second < 0) ? 0 : this.second;
            }
        }
        #endregion 

        #region ShortTimeString
        public string ShortTimeString
        {
            set
            {
                string[] ary = value.Split(':');
                this.Hour = int.Parse(ary[0]);
                this.Minute = int.Parse(ary[1]);
                this.Second = int.Parse(ary[2]);
            }
        } 
        #endregion
        #endregion

        #region GetDateTime
        public DateTime GetDateTime()
        {
            DateTime now = DateTime.Now;
            return this.GetDateTime(now.Year, now.Month, now.Day);
        }

        public DateTime GetDateTime(int year, int month, int day)
        {
            DateTime now = DateTime.Now;
            return new DateTime(year, month, day, this.hour, this.minute, this.second);
        }
        #endregion

        #region IsOnTime
        /// <summary>
        /// IsOnTime 目标时间是否与当前对象所表示的时间的差值是否在maxToleranceInSecs范围之内。
        /// </summary>       
        public bool IsOnTime(DateTime target, int maxToleranceInSecs)
        {
            DateTime dt = this.GetDateTime(target.Year, target.Month, target.Day);
            bool onTime = TimeHelper.IsOnTime(dt, target, maxToleranceInSecs);
            if (onTime)
            {
                return true;
            }

            onTime = TimeHelper.IsOnTime(dt.AddDays(1), target, maxToleranceInSecs);
            if (onTime)
            {
                return true;
            }

            onTime = TimeHelper.IsOnTime(dt.AddDays(-1), target, maxToleranceInSecs);
            if (onTime)
            {
                return true;
            }

            return false;
        } 
        #endregion

        #region IComparable<ShortTime> 成员
        public int CompareTo(ShortTime other)
        {
            if ((this.hour == other.hour) && (this.minute == other.minute) && (this.second == other.second))
            {
                return 0;
            }

            #region Compare
            int deltHour = this.hour - other.hour;
            int deltMin = this.minute - other.minute;
            int deltSec = this.second - other.second;
            if (deltHour > 0)
            {
                return 1;
            }

            if (deltHour < 0)
            {
                return -1;
            }

            if (deltMin > 0)
            {
                return 1;
            }

            if (deltMin < 0)
            {
                return -1;
            }

            if (deltSec > 0)
            {
                return 1;
            }

            if (deltSec < 0)
            {
                return -1;
            }

            return 0;
            #endregion
        }

        #endregion

        #region ToString
        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", this.hour, this.minute, this.second);
        } 
        #endregion
    }
}

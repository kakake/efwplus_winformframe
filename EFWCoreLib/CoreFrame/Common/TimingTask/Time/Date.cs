using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// Date 表示年月日。
    /// </summary>
    [Serializable]
    public class Date : IComparable<Date>
    {
        #region Ctor
        public Date()
            : this(DateTime.Now)
        {
        }

        public Date(DateTime dt)
        {
            this.Year = dt.Year;
            this.Month = dt.Month;
            this.day = dt.Day;
        }

        public Date(int y, int m, int d)
            : this(new DateTime(y, m, d)) //借助DateTime来验证参数的合法性
        {
        }
        #endregion

        #region Property
        #region Year
        private int year = 1900;
        public int Year
        {
            get { return year; }
            set { year = value; }
        }
        #endregion

        #region Month
        private int month = 1;
        public int Month
        {
            get { return month; }
            set 
            {
                month = value;
                if (month > 12)
                {
                    month = 12;
                }
                if (month < 1)
                {
                    month = 1;
                }               
            }
        }
        #endregion

        #region Day
        private int day = 1;
        public int Day
        {
            get { return day; }
            set
            {
                DateTime temp = new DateTime(this.year, this.month, value);//借助DateTime来验证value的合法性
                this.day = value;
            }
        }
        #endregion 

        #region DateString
        public string DateString
        {
            set
            {
                string[] ary = value.Split('-');
                this.Year = int.Parse(ary[0]);
                this.Month = int.Parse(ary[1]);
                this.Day = int.Parse(ary[2]);
            }
        } 
        #endregion
        #endregion        

        #region ToString
        public override string ToString()
        {
            return string.Format("{0}-{1}-{2}", this.year, this.month, this.day);
        } 
        #endregion

        #region IsSameDate
        public bool IsSameDate(DateTime dt)
        {
            return this.CompareTo(new Date(dt)) == 0;
        } 
        #endregion

        #region IComparable<Date> 成员

        public int CompareTo(Date other)
        {
            if ((this.year == other.year) && (this.month == other.month) && (this.day == other.day))
            {
                return 0;
            }

            #region Compare
            int deltYear = this.year - other.year;
            int deltMon = this.month - other.month;
            int deltDay = this.day - other.day;
            if (deltYear > 0)
            {
                return 1;
            }

            if (deltYear < 0)
            {
                return -1;
            }

            if (deltMon > 0)
            {
                return 1;
            }

            if (deltMon < 0)
            {
                return -1;
            }

            if (deltDay > 0)
            {
                return 1;
            }

            if (deltDay < 0)
            {
                return -1;
            }

            return 0;
            #endregion
        }

        #endregion

        #region ToDateInteger
        public int ToDateInteger()
        {
            return this.Year * 10000 + this.Month * 100 + this.Day;
        } 
        #endregion

        #region ToDateTime
        public DateTime ToDateTime(int hour, int minute, int second)
        {
            return new DateTime(this.year, this.month, this.day, hour, minute, second);
        }

        public DateTime ToDateTime()
        {
            return this.ToDateTime(0, 0, 0);
        }
        #endregion

        #region AddDays
        public Date AddDays(int days)
        {
            DateTime dt = new DateTime(this.year, this.month, this.day);
            DateTime newDt = dt.AddDays(days);

            return new Date(newDt);
        } 
        #endregion

        #region Static
        #region ConvertToDateInteger
        public static int ConvertToDateInteger(DateTime dt)
        {
            return dt.Year * 10000 + dt.Month * 100 + dt.Day;
        }
        #endregion

        #region ConvertFromDateInteger
        public static DateTime ConvertFromDateInteger(int theDate)
        {
            return Date.ConvertFromDateInteger(theDate, 0, 0, 0);
        }

        public static DateTime ConvertFromDateInteger(int theDate, int hour, int minute, int second)
        {
            int year = theDate / 10000;
            int month = (theDate % 10000) / 100;
            int day = (theDate % 10000) % 100;

            return new DateTime(year, month, day, hour, minute, second);
        }
        #endregion 
        #endregion
    }
}

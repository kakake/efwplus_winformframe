using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// Week 周。表示某一个星期。
    /// zhuweisky 2009.05.20
    /// </summary>
    [Serializable]
    public class Week
    {
        #region Ctor
        public Week()
            : this(DateTime.Now)
        { }

        public Week(DateTime dt)
        {
            this.monday = this.GetLastMondayDate(dt);
        }  
      
        public Week(Date _monday)
        {
            this.monday = _monday ;
        }

        #region GetLastMondayDate
        /// <summary>
        /// GetLastMondayDate 在获取离dt最近的一个周一。
        /// </summary>  
        private Date GetLastMondayDate(DateTime dt)
        {
            DateTime temp = dt;

            while (temp.DayOfWeek != DayOfWeek.Monday)
            {
                temp = temp.AddDays(-1);
            }

            return new Date(temp);
        }
        #endregion       
        #endregion

        #region Monday
        private Date monday ;
        public Date Monday
        {
            get { return monday; }           
        } 
        #endregion           

        #region GetPreviousWeek
        public Week GetPreviousWeek()
        {
            return new Week(this.monday.AddDays(-7));
        } 
        #endregion

        #region GetNextWeek
        public Week GetNextWeek()
        {
            return new Week(this.monday.AddDays(7));
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            return string.Format("Monday:{0}", this.monday);
        } 
        #endregion

        #region static Current
        public static Week Current()
        {
            return new Week();
        }
        #endregion
    }
}

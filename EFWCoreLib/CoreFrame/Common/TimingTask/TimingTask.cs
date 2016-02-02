using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// TimingTask 定时任务。封装了定时任务的执行频率、执行时间、和要执行的目标方法。
    /// </summary>
    [Serializable]
    public class TimingTask
    {
        [NonSerialized]
        private DateTime lastRightTime = DateTime.Parse("2000-01-01 00:00:00");

        #region TimingTaskExcuter
        private ITimingTaskExcuter timingTaskExcuter;
        public ITimingTaskExcuter TimingTaskExcuter
        {
            get { return timingTaskExcuter; }
            set { timingTaskExcuter = value; }
        } 
        #endregion

        #region TimingTaskType
        private TimingTaskType timingTaskType = TimingTaskType.PerDay;        
        public TimingTaskType TimingTaskType
        {
            get { return timingTaskType; }
            set { timingTaskType = value; }
        } 
        #endregion

        #region ExcuteTime
        private ShortTime excuteTime = new ShortTime();
        /// <summary>
        /// ExcuteTime 任务执行的具体时刻。如果TimingTaskType为PerHour，则将忽略ExcuteTime的Hour属性。
        /// </summary>
        public ShortTime ExcuteTime
        {
            get { return excuteTime; }
            set { excuteTime = value; }
        } 
        #endregion

        #region DayOfWeek
        private DayOfWeek dayOfWeek = DayOfWeek.Monday;
        /// <summary>
        /// DayOfWeek 该属性只有在TimingTaskType为PerWeek时才有效，表示在周几执行。
        /// </summary>
        public DayOfWeek DayOfWeek
        {
            get { return dayOfWeek; }
            set { dayOfWeek = value; }
        } 
        #endregion

        #region Day
        private int day = 1;
        /// <summary>
        /// Day 该属性只有在TimingTaskType为PerMonth时才有效，表示在每月的几号执行。
        /// </summary>
        public int Day
        {
            get { return day; }
            set { day = value; }
        } 
        #endregion

        #region IsOnTime
        public bool IsOnTime(int checkSpanSeconds, DateTime now)
        {
            #region 防止在临界点时，执行两次
            TimeSpan span = now - this.lastRightTime;
            if (span.TotalMilliseconds < checkSpanSeconds * 1000 * 2)
            {
                return false;
            } 
            #endregion
            
            bool isOnTime = false;

            #region Switch
            switch (this.timingTaskType)
            {
                case TimingTaskType.PerHour:
                    {
                        ShortTime temp = new ShortTime(now.Hour, this.excuteTime.Minute, this.excuteTime.Second);
                        isOnTime = temp.IsOnTime(now, checkSpanSeconds);

                        if (!isOnTime)
                        {
                            ShortTime temp2 = new ShortTime(now.AddHours(1).Hour, this.excuteTime.Minute, this.excuteTime.Second);
                            isOnTime = temp2.IsOnTime(now, checkSpanSeconds);
                        }

                        if (!isOnTime)
                        {
                            ShortTime temp3 = new ShortTime(now.AddHours(-1).Hour, this.excuteTime.Minute, this.excuteTime.Second);
                            isOnTime = temp3.IsOnTime(now, checkSpanSeconds);
                        }

                        break;
                    }
                case TimingTaskType.PerDay:
                    {
                        isOnTime = this.excuteTime.IsOnTime(now, checkSpanSeconds);
                        break;
                    }
                case TimingTaskType.PerWeek:
                    {
                        if (now.DayOfWeek != this.dayOfWeek)
                        {
                            isOnTime = false;
                        }
                        else
                        {
                            isOnTime = this.excuteTime.IsOnTime(now, checkSpanSeconds);
                        }
                        break;
                    }
                case TimingTaskType.PerMonth:
                    {
                        if (now.Day != this.day)
                        {
                            isOnTime = false;
                        }
                        else
                        {
                            isOnTime = this.excuteTime.IsOnTime(now, checkSpanSeconds);
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            } 
            #endregion

            if (isOnTime)
            {
                this.lastRightTime = now;
            }

            return isOnTime;
        } 
        #endregion       
    }
}

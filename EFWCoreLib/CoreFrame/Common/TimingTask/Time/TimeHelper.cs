using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// TimeHelper 与DateTime相关的工具类
    /// </summary>
    public static class TimeHelper
    {
        #region IsOnTime
        /// <summary>
        /// IsOnTime 时间val与requiredTime之间的差值是否在maxToleranceInSecs范围之内。
        /// </summary>        
        public static bool IsOnTime(DateTime requiredTime, DateTime val, int maxToleranceInSecs)
        {
            TimeSpan span = val - requiredTime;
            double spanMilliseconds = span.TotalMilliseconds < 0 ? (-span.TotalMilliseconds) : span.TotalMilliseconds;

            return (spanMilliseconds <= (maxToleranceInSecs * 1000));
        }

        /// <summary>
        /// IsOnTime 对于循环调用，时间val与startTime之间的差值(>0)对cycleSpanInSecs求余数的结果是否在maxToleranceInSecs范围之内。
        /// </summary>        
        public static bool IsOnTime(DateTime startTime, DateTime val, int cycleSpanInSecs, int maxToleranceInSecs)
        {
            TimeSpan span = val - startTime;
            double spanMilliseconds = span.TotalMilliseconds ;
            double residual = spanMilliseconds % (cycleSpanInSecs * 1000);

            return (residual <= (maxToleranceInSecs * 1000));
        } 
        #endregion
    }
}

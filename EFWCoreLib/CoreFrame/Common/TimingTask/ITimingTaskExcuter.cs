using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// ITimingTaskExcuter 定时任务执行者。即当定时时刻到达时，将执行该接口的ExcuteOnTime方法。
    /// </summary>
    public interface ITimingTaskExcuter
    {
        /// <summary>
        /// ExcuteOnTime 实现该方法时最好截获可能抛出的所有异常，如果有未截获的异常抛出，将会被忽略。
        /// </summary>        
        void ExcuteOnTime(DateTime dt);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// TimingTaskType 定时任务的类型 
    /// </summary>
    [EnumDescription("定时任务的类型")]
    public enum TimingTaskType
    {
        [EnumDescription("每小时一次")]
        PerHour,
        [EnumDescription("每天一次")]
        PerDay,
        [EnumDescription("每周一次")]
        PerWeek,
        [EnumDescription("每月一次")]
        PerMonth
    }
}

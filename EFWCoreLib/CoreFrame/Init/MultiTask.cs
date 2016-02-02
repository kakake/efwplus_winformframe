using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using EFWCoreLib.CoreFrame.Common;

namespace EFWCoreLib.CoreFrame.Init
{
    /// <summary>
    /// 定制任务
    /// </summary>
    public abstract class MultiTask : MarshalByRefObject
    {
        //public static List<TimingTask> taskList = new List<TimingTask>();
        /// <summary>
        /// 导入任务
        /// TaskContent task = NewObject<TaskContent>("");
        /// TimingTask timing = new TimingTask();
        /// timing.TimingTaskExcuter = task;
        /// timing.TimingTaskType = TimingTaskType.PerDay;
        /// timing.ExcuteTime = new ShortTime(0, 0, 0);
        /// taskList.Add(timing);
        /// </summary>
        public abstract void LoadTask(List<TimingTask> taskList);

        public static void Init(IUnityContainer container, List<TimingTask> taskList)
        {
            taskList.Clear();

            IEnumerable<MultiTask> comms = container.ResolveAll<MultiTask>();

            int count = 0;
            foreach (MultiTask comm in comms)
            {
                comm.LoadTask(taskList);
                count++;
            }

            if (count > 0)
            {
                TimingTaskManager taskmanager = new TimingTaskManager();
                taskmanager.TaskList = taskList;
                taskmanager.Initialize();
            }
        }
    }
    /// <summary>
    /// 执行的任务内容
    /// </summary>
    public abstract class TaskContent : ITimingTaskExcuter
    {
        public abstract void ExcuteOnTime(DateTime dt);
    }
}

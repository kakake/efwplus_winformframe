using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// TimingTaskManager 用于管理所有的定时任务，并在时间到达时，异步执行任务。
    /// </summary>
    public class TimingTaskManager
    {
        private object locker = new object();
        private Timer timer;     

        #region TaskList
        private IList<TimingTask> taskList = new List<TimingTask>();
        public IList<TimingTask> TaskList
        {
            get { return taskList; }
            set { taskList = value ?? new List<TimingTask>(); }
        }        
        #endregion

        #region TimerSpanInSecs
        private int timerSpanInSecs = 1;//sec
        public int TimerSpanInSecs
        {
            get { return timerSpanInSecs; }
            set 
            {                
                timerSpanInSecs = value;
                if (timerSpanInSecs < 1)
                {
                    timerSpanInSecs = 1;
                }
            }
        }
        #endregion       

        #region Initialize
        public void Initialize()
        {
            this.timer = new Timer(new TimerCallback(this.Worker), null, this.timerSpanInSecs * 1000, this.timerSpanInSecs * 1000);
        } 
        #endregion

        #region Worker
        private void Worker(object state)
        {
            DateTime now = DateTime.Now;

            lock (this.locker)
            {
                foreach (TimingTask task in this.taskList)
                {
                    if (task.IsOnTime(this.timerSpanInSecs, now))
                    {
                        CbDateTime cb = new CbDateTime(task.TimingTaskExcuter.ExcuteOnTime);
                        cb.BeginInvoke(now, null, null); //异步执行任务
                    }
                }
            }          
        }
        #endregion

        #region RegisterTask
        private void RegisterTask(TimingTask task)
        {
            lock (this.locker)
            {
                this.taskList.Add(task);
            }
        } 
        #endregion

        #region UnRegisterTask
        private void UnRegisterTask(TimingTask task)
        {
            lock (this.locker)
            {
                this.taskList.Remove(task);
            }
        } 
        #endregion
    }
}

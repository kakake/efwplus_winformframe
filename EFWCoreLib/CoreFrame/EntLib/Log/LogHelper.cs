using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace EFWCoreLib.CoreFrame.EntLib.Log
{
    /// <summary>
    /// 日志操作类
    /// </summary>
    public static class LogHelper
    {
        private static LogWriter logw = ZhyContainer.CreateLog();
        private static TraceManager traceMgr = ZhyContainer.CreateTrace();
        private static LogEntry loge = new LogEntry();
        /// <summary>
        /// 开始跟踪
        /// </summary>
        /// <returns></returns>
        public static Tracer StartTrace()
        {
            return traceMgr.StartTrace(Category.FileLog);
        }
        /// <summary>
        /// 结束跟踪
        /// </summary>
        /// <param name="trace"></param>
        public static void EndTrace(Tracer trace)
        {
            trace.Dispose();
        }

    }
}

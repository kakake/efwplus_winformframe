using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace EFWCoreLib.CoreFrame.EntLib.Log.TraceListeners
{
    /// <summary>
    /// 控制台跟踪侦听类
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class ConsoleTraceListener : CustomTraceListener
    {
        /// <summary>
        /// 写指定消息
        /// </summary>
        /// <param name="message">消息</param>
        public override void Write(string message)
        {
            Console.Write(message);
        }
        /// <summary>
        /// 写指定消息，后跟当前行结束符
        /// </summary>
        /// <param name="message">消息</param>
        public override void WriteLine(string message)
        {
            Console.Out.WriteLine(message);
        }
    }
}

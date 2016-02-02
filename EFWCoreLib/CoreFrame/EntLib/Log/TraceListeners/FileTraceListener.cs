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
    /// 文件跟踪侦听类
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class FileTraceListener : CustomTraceListener
    {
        /// <summary>
        /// 写指定消息
        /// </summary>
        /// <param name="message">消息</param>
        public override void Write(string message)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 写指定消息，后跟当前行结束符
        /// </summary>
        /// <param name="message">消息</param>
        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;


namespace EFWCoreLib.CoreFrame.EntLib.Log.Formatters
{
    /// <summary>
    /// 文本格式化类
    /// </summary>
    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class ZhyTextFormatter : LogFormatter
    {
        private NameValueCollection Attributes = null;
        /// <summary>
        /// 创建ZhyTextFormatter的实例
        /// </summary>
        /// <param name="attributes"></param>
        public ZhyTextFormatter(NameValueCollection attributes)
        {
            this.Attributes = attributes;
        }
        /// <summary>
        /// 格式化
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public override string Format(LogEntry log)
        {
            throw new NotImplementedException();
        }
    }
}

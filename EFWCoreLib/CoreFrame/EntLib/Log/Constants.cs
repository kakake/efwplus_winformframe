using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.EntLib.Log
{
    /// <summary>
    /// 日志优先级
    /// </summary>
    public struct Priority
    {
        /// <summary>
        /// 最低级
        /// </summary>
        public const int Lowest = 0;
        /// <summary>
        /// 低级
        /// </summary>
        public const int Low = 1;
        /// <summary>
        /// 一般
        /// </summary>
        public const int Normal = 2;
        /// <summary>
        /// 高级
        /// </summary>
        public const int High = 3;
        /// <summary>
        /// 最高级
        /// </summary>
        public const int Highest = 4;
    }
    /// <summary>
    /// 日志分类
    /// </summary>
    public struct Category
    {
        /// <summary>
        /// 文件日志
        /// </summary>
        public const string FileLog = "FileLog";
        /// <summary>
        /// 控制台日志
        /// </summary>
        public const string ConsoleLog = "ConsoleLog";
        /// <summary>
        /// 数据库日志
        /// </summary>
        public const string DatabaseLog = "DatabaseLog";
        /// <summary>
        /// 电子邮件日志
        /// </summary>
        public const string EmailLog = "EmailLog";
    }
}

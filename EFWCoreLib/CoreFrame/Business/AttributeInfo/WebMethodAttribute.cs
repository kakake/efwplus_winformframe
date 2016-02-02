using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Business.AttributeInfo
{
    [AttributeUsageAttribute(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class WebMethodAttribute : Attribute
    {
        private string _openDBNames;
        /// <summary>
        /// 打开数据库，中间用,号隔开
        /// </summary>
        public string OpenDBKeys
        {
            get { return _openDBNames; }
            set { _openDBNames = value; }
        }

        string _memo;
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
    }
}

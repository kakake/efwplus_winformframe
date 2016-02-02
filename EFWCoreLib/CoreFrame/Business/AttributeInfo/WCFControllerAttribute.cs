using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Business.AttributeInfo
{
    /// <summary>
    /// WCF服务对象自定义标签
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class WCFControllerAttribute : Attribute
    {
        string _memo;
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
    }

    
}

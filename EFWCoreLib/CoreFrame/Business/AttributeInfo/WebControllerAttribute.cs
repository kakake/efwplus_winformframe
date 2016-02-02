using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Business.AttributeInfo
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class WebControllerAttribute : Attribute
    {
        string _memo;
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
    }

    
}

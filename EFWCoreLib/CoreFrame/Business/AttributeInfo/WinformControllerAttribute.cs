using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Business.AttributeInfo
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class WinformControllerAttribute:Attribute
    {
        //string _defaultName;
        ///// <summary>
        ///// 菜单名称
        ///// </summary>
        //public string DefaultName
        //{
        //    get { return _defaultName; }
        //    set { _defaultName = value; }
        //}

        private string _defaultViewName;
        /// <summary>
        /// 菜单对应打开界面
        /// </summary>
        public string DefaultViewName
        {
            get { return _defaultViewName; }
            set { _defaultViewName = value; }
        }

        string _memo;
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
    }
}

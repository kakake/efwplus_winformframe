using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Business.AttributeInfo
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class WinformViewAttribute : Attribute
    {
        private string _name;
        /// <summary>
        /// 界面名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //private bool _defaultShow;
        //public bool DefaultView
        //{
        //    get { return _defaultShow; }
        //    set { _defaultShow = value; }
        //}
        
        //private Type _viewType;
        ///// <summary>
        ///// 界面对象类型
        ///// </summary>
        //public Type ViewType
        //{
        //    get { return _viewType; }
        //    set { _viewType = value; }
        //}

        private string _dllName;
        /// <summary>
        /// 界面存放的DLL
        /// </summary>
        public string DllName
        {
            get { return _dllName; }
            set { _dllName = value; }
        }

        private string _viewTypeName;
        /// <summary>
        /// 界面类型名称
        /// </summary>
        public string ViewTypeName
        {
            get { return _viewTypeName; }
            set { _viewTypeName = value; }
        }

        string _memo;
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
    }
}

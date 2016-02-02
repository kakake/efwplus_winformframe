using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinMainUIFrame.Entity;
using EFWCoreLib.WinformFrame.Controller;
using System.Windows.Forms;

namespace WinMainUIFrame.Winform.IView
{
    public interface IfrmMain : IBaseView
    {
        string UserName { set; }
        string DeptName { set; }
        string WorkName { set; }

        List<BaseModule> modules { get; set; }
        List<BaseMenu> menus { get; set; }
        List<BaseDept> depts { get; set; }

        void showSysMenu();
        void ShowForm(WinMenu menu);
        void ShowForm(Form form, string tabName, string tabId);
        void ShowRightForm(Form form, int width, bool Collapsed);
        void CloseForm(string tabId);

        void showDebugMenu();
        //void InitMessageForm();
        //void ShowMessageForm();
    }

    public class WinMenu
    {
        private int _menuId;

        public int MenuId
        {
            get { return _menuId; }
            set { _menuId = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _dllName;
        /// <summary>
        /// 对应的Dll
        /// </summary>
        public string PluginName
        {
            get { return _dllName; }
            set { _dllName = value; }
        }
        private string _funName;
        /// <summary>
        /// 对应的函数名
        /// </summary>
        public string ControllerName
        {
            get { return _funName; }
            set { _funName = value; }
        }

        private string _urlPath;
        /// <summary>
        /// Web页面路径
        /// </summary>
        public string UrlPath
        {
            get { return _urlPath; }
            set { _urlPath = value; }
        }

        private int _moduleId;

        public int ModuleId
        {
            get { return _moduleId; }
            set { _moduleId = value; }
        }
        private int _pMenuId;

        public int PMenuId
        {
            get { return _pMenuId; }
            set { _pMenuId = value; }
        }
        private int _isToolBar;
        /// <summary>
        /// 是否显示在工具栏上
        /// </summary>
        public int IsToolBar
        {
            get { return _isToolBar; }
            set { _isToolBar = value; }
        }
        private int _isOutlookBar;
        /// <summary>
        /// 是否显示在Web首页
        /// </summary>
        public int IsOutlookBar
        {
            get { return _isOutlookBar; }
            set { _isOutlookBar = value; }
        }
        private string _memo;

        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }


        private int _sortId;

        public int SortId
        {
            get { return _sortId; }
            set { _sortId = value; }
        }
    }
}

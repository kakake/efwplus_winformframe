/*
 *控制器的目的：
 *使界面对象与服务对象达到隔离和重用的目的 
 *所以控制器是把界面对象与服务对象组合一些业务功能、一些菜单。
 *如果一个界面有两个菜单那就分开建两个控制器对象。
 * 
 */


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Services.Protocols;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.EntLib;
using System.Windows.Forms;

namespace EFWCoreLib.WinformFrame.Controller
{
 
    /// <summary>
    /// Winform控制器基类
    /// 
    /// </summary>
    public class WinformController : AbstractController
    {
        //实例化此控制器的菜单ID
        //public int MenuId { get; set; }

        /// <summary>
        /// 获取页面子权限
        /// </summary>
        //public DataTable GetPageRight
        //{
        //    get
        //    {
        //        DataTable data = (DataTable)ExecuteFun.invoke(oleDb,"getPageRight", MenuId, LoginUserInfo.UserId);
        //        return data;
        //    }
        //}

        protected override SysLoginRight GetUserInfo()
        {
            if (EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser") != null)
            {
                return (SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser");
            }
            return base.GetUserInfo();
        }

        internal IBaseViewBusiness _defaultView;

        public IBaseViewBusiness DefaultView
        {
            get { return _defaultView; }
            set { _defaultView = value; }
        }

        private Dictionary<string, IBaseViewBusiness> _iBaseView;
        public Dictionary<string, IBaseViewBusiness> iBaseView
        {
            get { return _iBaseView; }
            set
            {
                _iBaseView = value;
                foreach (KeyValuePair<string, IBaseViewBusiness> val in _iBaseView)
                {
                    val.Value.InvokeController = new ControllerEventHandler(UI_ControllerEvent);
                }
            }
        }

        /// <summary>
        /// 创建WinformController的实例
        /// </summary>
        public WinformController()
        {
            
        }
        /// <summary>
        /// 界面控制事件
        /// </summary>
        /// <param name="eventname">事件名称</param>
        /// <param name="objs">参数数组</param>
        /// <returns></returns>
        public virtual object UI_ControllerEvent(string eventname, params object[] objs)
        {
            try
            {
                switch (eventname)
                {
                    case "Show":
                        if (objs.Length == 1)
                        {
                            Form form = objs[0] as Form;
                            string tabName = form.Text;
                            string tabId = "view" + form.GetHashCode();
                            InvokeController("WinMainUIFrame", "LoginController", "ShowForm", form, tabName, tabId);
                        }
                        else if (objs.Length == 2)
                        {
                            Form form = objs[0] as Form;
                            string tabName = objs[1].ToString();
                            string tabId = "view" + form.GetHashCode();
                            InvokeController("WinMainUIFrame", "LoginController", "ShowForm", form, tabName, tabId);
                        }
                        else if (objs.Length == 3)
                        {
                            InvokeController("WinMainUIFrame", "LoginController", "ShowForm", objs);
                        }
                        return true;
                    case "Close":
                        if (objs[0] is Form)
                        {
                            string tabId = "view" + objs[0].GetHashCode();
                            InvokeController("WinMainUIFrame", "LoginController", "CloseForm", tabId);
                        }
                        else
                        {
                            InvokeController("WinMainUIFrame", "LoginController", "CloseForm", objs);
                        }
                        return true;
                    case "Exit":
                        AppGlobal.AppExit();
                        return null;
                    case "this":
                        return this;
                }

                MethodInfo meth = ControllerHelper.CreateMethodInfo(_pluginName + "@" + this.GetType().Name, eventname);
                return meth.Invoke(this, objs);
            }
            catch (Exception err)
            {
                //记录错误日志
                ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                throw new Exception(err.Message);
            }
        }

        public bool InitFinish = false;//是否完成初始化
        /// <summary>
        /// 初始化全局web服务参数对象
        /// </summary>
        public virtual void Init() { }

        public virtual IBaseViewBusiness GetView(string frmName)
        {
            return iBaseView[frmName];
        }

        /// <summary>
        /// 执行控制器
        /// </summary>
        /// <returns></returns>
        public Object InvokeController(string puginName, string controllerName, string methodName, params object[] objs)
        {
            try
            {
                WinformController icontroller = ControllerHelper.CreateController(puginName + "@" + controllerName);
                MethodInfo meth = ControllerHelper.CreateMethodInfo(puginName + "@" + controllerName, methodName);
                if (meth == null) throw new Exception("调用的方法名不存在");
                return meth.Invoke(icontroller, objs);
            }
            catch (Exception err)
            {
                //记录错误日志
                ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                throw new Exception(err.Message);
            }
        }
    }
}

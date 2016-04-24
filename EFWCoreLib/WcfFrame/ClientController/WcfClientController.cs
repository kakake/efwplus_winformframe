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
using System.ServiceModel;
using System.Text;
using System.Web.Services.Protocols;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame.WcfService;
using EFWCoreLib.WcfFrame.WcfService.Contract;
using Newtonsoft.Json;
using EFWCoreLib.CoreFrame.Common;
using System.Windows.Forms;

namespace EFWCoreLib.WcfFrame.ClientController
{
 
    /// <summary>
    /// Winform控制器基类
    /// 
    /// </summary>
    public class WcfClientController : AbstractController
    {
        //实例化此控制器的菜单ID
        //public int MenuId { get; set; }

        /// <summary>
        /// 获取页面子权限
        /// </summary>
        //public DataTable GetPageRight
        //{
        //    get;set;
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
        public WcfClientController()
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
                            InvokeController("WcfMainUIFrame", "wcfclientLoginController", "ShowForm", form, tabName, tabId);
                        }
                        else if (objs.Length == 2)
                        {
                            Form form = objs[0] as Form;
                            string tabName = objs[1].ToString();
                            string tabId = "view" + form.GetHashCode();
                            InvokeController("WcfMainUIFrame", "wcfclientLoginController", "ShowForm", form, tabName, tabId);
                        }
                        else if (objs.Length == 3)
                        {
                            InvokeController("WcfMainUIFrame", "wcfclientLoginController", "ShowForm", objs);
                        }
                        return true;
                    case "Close":
                        if (objs[0] is Form)
                        {
                            string tabId = "view" + objs[0].GetHashCode();
                            InvokeController("WcfMainUIFrame", "wcfclientLoginController", "CloseForm", tabId);
                        }
                        else
                        {
                            InvokeController("WcfMainUIFrame", "wcfclientLoginController", "CloseForm", objs);
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
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                if(err.InnerException!=null)
                    throw new Exception(err.InnerException.Message);
                throw new Exception(err.Message);
            }
        }

        public bool InitFinish = false;
        /// <summary>
        /// 初始化全局web服务参数对象
        /// </summary>
        public virtual void Init() { }

        public virtual IBaseViewBusiness GetView(string frmName)
        {
            return iBaseView[frmName];
        }
       
        /// <summary>
        /// 客户端标识
        /// </summary>
        public string ClientID
        {
            get
            {
                return EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("WCFClientID").ToString();
            }
        }

        /// <summary>
        /// wcf服务对象
        /// </summary>
        public IWCFHandlerService WCFService
        {
            get
            {
                IWCFHandlerService _wcfService = EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("WCFService") as IWCFHandlerService;
                return _wcfService;
            }
        }

        /// <summary>
        /// WCF双通道回调客户端
        /// </summary>
        public static EFWCoreLib.WcfFrame.ClientController.ReplyClientCallBack replyClientCallBack
        {
            get
            {
                return AppGlobal.cache.GetData("ClientService") as EFWCoreLib.WcfFrame.ClientController.ReplyClientCallBack;
            }
        }

        public virtual Object InvokeWCFService(string controller, string method)
        {
            return InvokeWCFService(controller, method, "[]");
        }

        public virtual Object InvokeWCFService(string controller, string method, string jsondata)
        {
            if(string.IsNullOrEmpty(jsondata))jsondata="[]";
            string retJson= WcfClientManage.Request(_pluginName+"@"+controller, method, jsondata);

            object Result = JavaScriptConvert.DeserializeObject(retJson);
            int ret = Convert.ToInt32(((Newtonsoft.Json.JavaScriptObject)(Result))["flag"]);
            string msg = ((Newtonsoft.Json.JavaScriptObject)(Result))["msg"].ToString();
            if (ret == 1)
            {
                throw new Exception(msg);
            }
            else
            {
                return ((Newtonsoft.Json.JavaScriptObject)(Result))["data"];
            }
        }

        public virtual Object InvokeWCFServiceCompress(string controller, string method, string jsondata)
        {
            if (string.IsNullOrEmpty(jsondata)) jsondata = "[]";
            jsondata = ZipComporessor.Compress(jsondata);//压缩传入参数
            string retJson= WcfClientManage.Request(_pluginName+"@"+controller, method, jsondata);
         
            object Result = JavaScriptConvert.DeserializeObject(retJson);
            int ret = Convert.ToInt32(((Newtonsoft.Json.JavaScriptObject)(Result))["flag"]);
            string msg = ((Newtonsoft.Json.JavaScriptObject)(Result))["msg"].ToString();
            if (ret == 1)
            {
                throw new Exception(msg);
            }
            else
            {
                //解压输出结果
                return JavaScriptConvert.DeserializeObject(ZipComporessor.Decompress(((Newtonsoft.Json.JavaScriptArray)((Newtonsoft.Json.JavaScriptObject)(Result))["data"])[0].ToString()));
            }
        }

        public virtual IAsyncResult InvokeWCFServiceAsync(string controller, string method, string jsondata, Action<Object> action)
        {
            if (string.IsNullOrEmpty(jsondata)) jsondata = "[]";
            Action<string> retAction = delegate(string retJson)
            {
                object Result = JavaScriptConvert.DeserializeObject(retJson);
                int ret = Convert.ToInt32(((Newtonsoft.Json.JavaScriptObject)(Result))["flag"]);
                string msg = ((Newtonsoft.Json.JavaScriptObject)(Result))["msg"].ToString();
                if (ret == 1)
                {
                    throw new Exception(msg);
                }
                else
                {
                    action(((Newtonsoft.Json.JavaScriptObject)(Result))["data"]);
                }
            };
            return WcfClientManage.RequestAsync(_pluginName + "@" + controller, method, jsondata, retAction);
        }


        /// <summary>
        /// 执行控制器
        /// </summary>
        /// <returns></returns>
        public Object InvokeController(string puginName, string controllerName, string methodName, params object[] objs)
        {
            try
            {
                WcfClientController icontroller = ControllerHelper.CreateController(puginName + "@" + controllerName);
                MethodInfo meth = ControllerHelper.CreateMethodInfo(puginName + "@" + controllerName, methodName);
                return meth.Invoke(icontroller, objs);
            }
            catch (Exception err)
            {
                //记录错误日志
                EFWCoreLib.CoreFrame.EntLib.ZhyContainer.CreateException().HandleException(err, "HISPolicy");
                throw new Exception(err.Message);
            }
        }
       
    }
}

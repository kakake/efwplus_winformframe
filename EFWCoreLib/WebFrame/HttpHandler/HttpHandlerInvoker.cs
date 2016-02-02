using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
using System.Reflection;

namespace EFWCoreLib.WebFrame.HttpHandler
{
    /// <summary>
    /// 执行控制器
    /// </summary>
    public class HttpHandlerInvoker
    {
        private string _controllername;
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName
        {
            get { return _controllername; }
            set { _controllername = value; }
        }

        private string _methodname;
        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName
        {
            get { return _methodname; }
            set { _methodname = value; }
        }


        /// <summary>
        /// 执行命令
        /// </summary>
        public void CmdInvoke(HttpContext context)
        {
            WebHttpController controller = ControllerHelper.CreateController(ControllerName);
            controller.context = context;
            BeginInvoke(context, controller);
            MethodInfo mi = ControllerHelper.CreateMethodInfo(ControllerName, MethodName, controller); //获得执行方法
            mi.Invoke(controller, null); //带参数方法的调用并返回值
            EndInvoke(context, controller);

            ControllerHelper.ControllerWrite(controller);
        }

        private void BeginInvoke(HttpContext context, WebHttpController iController)
        {
            //界面传值到后台
            iController.ViewResult = "";
            iController.JsonResult = "";

            iController.sessionData = new System.Collections.Generic.Dictionary<string, object>();
            iController.PutOutData = new System.Collections.Generic.Dictionary<string, object>();
            iController.ClearKey = new System.Collections.Generic.List<string>();
            iController.ParamsData = new System.Collections.Generic.Dictionary<string, string>();
            iController.FormData = new System.Collections.Generic.Dictionary<string, string>();

            iController.ViewData = new Dictionary<string, object>();

            if (context.Request.Params != null)//获取Param值
            {
                for (int i = 0; i < context.Request.Params.Count; i++)
                {
                    string key = context.Request.Params.Keys[i];
                    if (key != null && key != "")
                        iController.ParamsData.Add(key, context.Request.Params[key].ToString());
                    if (key == "ASP.NET_SessionId")//过滤Web应用服务参数
                        break;
                }
            }

            if (context.Request.Form != null)//获取Form值
            {
                for (int i = 0; i < context.Request.Form.Count; i++)
                {
                    string key = context.Request.Form.Keys[i];
                    if (key != null && key != "")
                        iController.FormData.Add(key, context.Request.Form[key].ToString());
                }
            }

            if (context.Session.Count > 0)//Session数据传入后台
            {
                for (int i = 0; i < context.Session.Count; i++)
                {
                    if (iController.sessionData.ContainsKey(context.Session.Keys[i].ToString()))
                    {
                        iController.sessionData.Remove(context.Session.Keys[i].ToString());
                    }
                    iController.sessionData.Add(context.Session.Keys[i].ToString(), context.Session[i]);
                }
            }
        }

        private void EndInvoke(HttpContext context, WebHttpController iController)
        {
            //后台传出的数据重新绑定给Session
            if (iController.PutOutData != null && iController.PutOutData.Count > 0)
            {
                //Session.Clear();//清除所有Session
                for (int i = 0; i < iController.PutOutData.Count; i++)
                {
                    context.Session[iController.PutOutData.ToArray()[i].Key] = iController.PutOutData.ToArray()[i].Value;
                }
            }

            //后台传出要清除的Session
            if (iController.ClearKey != null)
            {
                for (int i = 0; i < iController.ClearKey.Count; i++)
                {
                    context.Session.Remove(iController.ClearKey[i]);
                }
            }
        }
    }
}

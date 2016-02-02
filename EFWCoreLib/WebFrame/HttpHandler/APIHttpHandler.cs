//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Web;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
using System.Web.UI;
using System.Text;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.EntLib;

namespace EFWCoreLib.WebFrame.HttpHandler
{
    /// <summary>
    /// Http请求处理对象
    /// </summary>
    public class APIHttpHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 您将需要在您网站的 web.config 文件中配置此处理程序，
        /// 并向 IIS 注册此处理程序，然后才能进行使用。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                if (AppGlobal.IsRun == false) throw new Exception("系统未正常启动！");
                string sController = context.Request["controller"].ToString().Trim();
                string sMethod = sController == "UploadifyController" ? "Upload" : context.Request["method"].ToString().Trim();
                if (!String.IsNullOrEmpty(sController) && !String.IsNullOrEmpty(sMethod))
                {
                    HttpHandlerInvoker invoker = new HttpHandlerInvoker();
                    invoker.ControllerName = sController;
                    invoker.MethodName = sMethod;
                    invoker.CmdInvoke(context);
                }
                else
                {
                    context.Response.Write("error\r"+"控制器名称或方法名称不能为空！");//命令错误
                }
            }
            catch (Exception err)
            {
                context.Response.Write("exception\r"+err.Message);//执行异常
                if(err.InnerException!=null)
                    context.Response.Write("\r" + err.InnerException.Message);//执行异常
                //记录错误日志
                ZhyContainer.CreateException().HandleException(err, "HISPolicy");
            }
            finally
            {
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}

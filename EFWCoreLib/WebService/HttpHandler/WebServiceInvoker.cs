/*
 * WebService服务代码不用单独建立一个webservice服务项目存放，可以和Controller程序集的代码合并在一起
 * 1.修改web.config的配置
 * <add verb="*" path="*.asmx" validate="false" type="CoreFrame.Common.WebServiceInvoker, CoreFrame"/>
 * 
 * 2.在Controller程序集中service代码和controller代码一样不能加命名空间,都继承于AbstractService
 * public class LoginService : AbstractService
 * 
 * 3.Url地址按照这种方式就能找到对应的服务
 * http://localhost:1375/UIFrame/LoginService.asmx
 * 
*/
using System;
using System.Web;
using System.Web.Services.Protocols;
using System.Reflection;
using EFWCoreLib.CoreFrame.Init;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Web.Services;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using EFWCoreLib.CoreFrame.Plugin;

namespace EFWCoreLib.WebFrame.HttpHandler
{
    /// <summary>
    /// WebService处理对象
    /// </summary>
    public class WebServiceInvoker : IHttpHandlerFactory
    {
        private Type GetWebService(HttpRequest request, string serviceName)
        {
            Type serviceType = null;
            ModulePlugin mp;
            serviceType = AppPluginManage.GetPluginWebServicesAttributeInfo(serviceName, out mp).ServiceType;
            return serviceType;
        }

        public void ReleaseHandler(IHttpHandler handler)
        {
            var wshf = new System.Web.Services.Protocols.WebServiceHandlerFactory();
            wshf.ReleaseHandler(handler);
        }

        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            var webServiceType = GetWebService(context.Request, GetServiceName(url));
            var wshf = new System.Web.Services.Protocols.WebServiceHandlerFactory();
            var coreGetHandler = typeof(WebServiceHandlerFactory).GetMethod("CoreGetHandler", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var httpHandler = (IHttpHandler)coreGetHandler.Invoke(wshf, new object[] { webServiceType, context, context.Request, context.Response });
            return httpHandler;
        }

        public static string GetServiceName(string url)
        {
            int index = url.LastIndexOf("/");
            int index2 = url.Substring(index).IndexOf(".");
            return url.Substring(index + 1, index2 - 1);
        }
    }
}

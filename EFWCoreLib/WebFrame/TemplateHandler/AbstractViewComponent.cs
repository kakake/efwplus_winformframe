using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.EntLib.Aop;
using EFWCoreLib.CoreFrame.DbProvider;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.WebFrame.HttpHandler.Controller;

namespace EFWCoreLib.WebFrame.TemplateHandler
{
    public abstract class AbstractViewComponent : AbstractController, IAopOperator
    {
        protected Dictionary<string, Object> ViewData = null;

        protected System.Collections.Generic.Dictionary<string, Object> sessionData = null;
        protected System.Collections.Generic.Dictionary<string, Object> PutOutData = null;
        protected System.Collections.Generic.Dictionary<string, string> ParamsData = null;
        protected System.Collections.Generic.Dictionary<string, string> FormData = null;
        public List<string> ClearKey = null;

        public AbstractViewComponent() { }

        protected override SysLoginRight GetUserInfo()
        {
            if (sessionData != null && sessionData.ContainsKey("RoleUser"))
            {
                return (SysLoginRight)sessionData["RoleUser"];
            }
            return base.GetUserInfo();
        }

        #region IAopOperator 成员

        public void PreProcess(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input)
        {
            ViewData = (input.Target as WebHttpController).ViewData;
            _oleDb = (input.Target as WebHttpController).oleDb;
            _container = (input.Target as WebHttpController).GetUnityContainer();
            _cache = (input.Target as WebHttpController).GetCache();

            sessionData = (input.Target as WebHttpController).sessionData;
            PutOutData = (input.Target as WebHttpController).PutOutData;
            ParamsData = (input.Target as WebHttpController).ParamsData;
            FormData = (input.Target as WebHttpController).FormData;
            ClearKey = (input.Target as WebHttpController).ClearKey;
            LoadViewData();
        }

        public void PostProcess(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input, Microsoft.Practices.Unity.InterceptionExtension.IMethodReturn result)
        {
            //throw new NotImplementedException();
        }

        #endregion

        public abstract void LoadViewData();
    }

#if !CSHARP30
    public abstract class AbstractRazorComponent : AbstractController, IAopOperator
    {

        protected Dictionary<string, Object> ViewData = null;

        protected System.Collections.Generic.Dictionary<string, Object> sessionData = null;
        protected System.Collections.Generic.Dictionary<string, Object> PutOutData = null;
        protected System.Collections.Generic.Dictionary<string, string> ParamsData = null;
        protected System.Collections.Generic.Dictionary<string, string> FormData = null;
        public List<string> ClearKey = null;

        public AbstractRazorComponent() { }

        protected override SysLoginRight GetUserInfo()
        {
            if (sessionData != null && sessionData.ContainsKey("RoleUser"))
            {
                return (SysLoginRight)sessionData["RoleUser"];
            }
            return base.GetUserInfo();
        }

        #region IAopOperator 成员

        public void PreProcess(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input)
        {
            ViewData = (input.Target as WebHttpController).ViewData;
            _oleDb = (input.Target as WebHttpController).oleDb;
            _container = (input.Target as WebHttpController).GetUnityContainer();
            _cache = (input.Target as WebHttpController).GetCache();

            sessionData = (input.Target as WebHttpController).sessionData;
            PutOutData = (input.Target as WebHttpController).PutOutData;
            ParamsData = (input.Target as WebHttpController).ParamsData;
            FormData = (input.Target as WebHttpController).FormData;
            ClearKey = (input.Target as WebHttpController).ClearKey;
            LoadViewData();

            string filepath = GetFilePath();
            //FileInfo fileinfo = new FileInfo(AppGlobal.AppRootPath+filepath);
            //if (fileinfo.Exists == false)
            //    throw new Exception(fileinfo.Name + "文件不存在！");
            //string html = File.ReadAllText(fileinfo.FullName);
            //RazorEngine.Razor.Compile(html, filepath);
            if (ViewData.ContainsKey("Razor_Include") == false)
            {
                ViewData.Add("Razor_Include", filepath);
            }
            else
            {
                string paths = ViewData["Razor_Include"].ToString();
                ViewData["Razor_Include"] = paths + "|" + filepath;
            }
        }

        public void PostProcess(Microsoft.Practices.Unity.InterceptionExtension.IMethodInvocation input, Microsoft.Practices.Unity.InterceptionExtension.IMethodReturn result)
        {
            //throw new NotImplementedException();
        }

        #endregion
        public abstract string GetFilePath();
        public abstract void LoadViewData();
    }
#endif
}

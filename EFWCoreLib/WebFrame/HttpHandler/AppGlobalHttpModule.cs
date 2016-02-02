using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EFWCoreLib.CoreFrame.Init;

namespace EFWCoreLib.WebFrame.HttpHandler
{
    /// <summary>
    /// web系统启动调用此对象
    /// </summary>
    public class AppGlobalHttpModule : IHttpModule
    {

        #region IHttpModule 成员

        public void Dispose()
        {
            AppGlobal.AppEnd();
        }
        private HttpApplication _context;
        public void Init(HttpApplication context)
        {
            _context = context;
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            AppGlobal.AppRootPath = _context.Server.MapPath("~/");
            AppGlobal.AppStart();
        }

        #endregion
    }
}

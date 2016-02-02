//2011.10.11 添加模板功能
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Init;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.Business;
using System.Text;

namespace EFWCoreLib.WebFrame.HttpHandler.Controller
{
    /// <summary>
    /// WebHttpController控制器
    /// </summary>
    public class WebHttpController : AbstractController
    {
        public HttpContext context { get; set; }

        /// <summary>
        /// Ajax请求返回Json数据
        /// </summary>
        public string JsonResult { get; set; }
        /// <summary>
        /// URL请求界面
        /// </summary>
        public string ViewResult { get; set; }
        /// <summary>
        /// 界面数据
        /// </summary>
        public Dictionary<string, Object> ViewData { get; set; }

        protected override SysLoginRight GetUserInfo()
        {
            if (sessionData != null && sessionData.ContainsKey("RoleUser"))
            {
                return (SysLoginRight)sessionData["RoleUser"];
            }
            return base.GetUserInfo();
        }

        //封装的页面子权限
        //public DataTable GetPageRight(int MenuId)
        //{
        //    DataTable data = (DataTable)ExecuteFun.invoke(oleDb, "getPageRight", MenuId, LoginUserInfo.UserId);
        //    return data;
        //}

        private System.Collections.Generic.Dictionary<string, Object> _sessionData;
        /// <summary>
        /// Session数据传入后台
        /// </summary>
        public System.Collections.Generic.Dictionary<string, Object> sessionData
        {
            get
            {
                return _sessionData;
            }
            set
            {
                _sessionData = value;
            }
        }

        private System.Collections.Generic.Dictionary<string, Object> _putOutData;
        /// <summary>
        /// 后台传出数据到Session数据
        /// </summary>
        public System.Collections.Generic.Dictionary<string, Object> PutOutData
        {
            get
            {
                return _putOutData;
            }
            set
            {
                _putOutData = value;
            }
        }

        private List<string> _clearKey;
        /// <summary>
        /// 清除Session的数据
        /// </summary>
        public List<string> ClearKey
        {
            get { return _clearKey; }
            set { _clearKey = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _paramsData;
        /// <summary>
        /// Url参数传递数据
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> ParamsData
        {
            get { return _paramsData; }
            set { _paramsData = value; }
        }

        private System.Collections.Generic.Dictionary<string, string> _formData;
        /// <summary>
        /// Form提交的数据
        /// </summary>
        public System.Collections.Generic.Dictionary<string, string> FormData
        {
            get { return _formData; }
            set { _formData = value; }
        }


        public string RetSuccess()
        {
            return RetSuccess(null, null);
        }

        public string RetSuccess(string info)
        {
            return RetSuccess(info, null);
        }

        public string RetSuccess(string info, string data)
        {
            info = info == null ? "" : info;
            data = data == null ? "\"\"" : data;
            return "{\"ret\":0,\"msg\":" + "\"" + info + "\"" + ",\"data\":" + data + "}";
        }

        public string RetError()
        {
            return RetError(null, null);
        }

        public string RetError(string info)
        {
            return RetError(info, null);
        }

        public string RetError(string info, string data)
        {
            info = info == null ? "" : info;
            data = data == null ? "\"\"" : data;
            return "{\"ret\":1,\"msg\":" + "\"" + info + "\"" + ",\"data\":" + data + "}";
        }

        public string ToUrl(string url)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=\"javascript\" type=\"text/javascript\">\n");
            sb.Append("window.location.href='" + url + "'\n");
            sb.Append("</script>\n");
            return sb.ToString();
        }
    }
}
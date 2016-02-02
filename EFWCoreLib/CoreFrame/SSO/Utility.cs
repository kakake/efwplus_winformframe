using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.SSO
{
    public class Utility
    {
        /// <summary>
        /// 客户端ip
        /// </summary>
        public static string RemoteIp
        {
            get
            {
                string realRemoteIP = "";
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    realRemoteIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
                }
                if (string.IsNullOrEmpty(realRemoteIP))
                {
                    realRemoteIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return realRemoteIP;
            }
        }
        /// <summary>
        /// 服务器Ip
        /// </summary>
        public static string HostIp
        {
            get
            {
                return System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
            }
        }
    }
}

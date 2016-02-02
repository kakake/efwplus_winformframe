using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace EFWCoreLib.CoreFrame.Init.AttributeManager
{
    public class WebServicesManager 
    {
        public static void LoadAttribute(List<string> BusinessDll, ICacheManager cache, string pluginName)
        {
            List<WebServicesAttributeInfo> webserviceList = new List<WebServicesAttributeInfo>();

            for (int k = 0; k < BusinessDll.Count; k++)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(BusinessDll[k]);
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    WebServiceAttribute[] webS = ((WebServiceAttribute[])types[i].GetCustomAttributes(typeof(WebServiceAttribute), true));
                    if (webS.Length > 0)
                    {
                        WebServicesAttributeInfo wsa = new WebServicesAttributeInfo();
                        wsa.ServiceName = types[i].Name;
                        wsa.ServiceType = types[i];
                        webserviceList.Add(wsa);
                    }
                }
            }

            cache.Add(pluginName+"@"+GetCacheKey(), webserviceList);
        }

        public static string GetCacheKey()
        {
            return "webServicesAttributeList";
        }

        public static Object GetAttributeInfo(ICacheManager cache, string pluginName)
        {
            return cache.GetData(pluginName+"@"+GetCacheKey());
        }

        public static void ClearAttributeData(ICacheManager cache, string pluginName)
        {
            cache.Remove(pluginName+"@"+GetCacheKey());
        }
    }

    public class WebServicesAttributeInfo
    {
        public string ServiceName { get; set; }
        public Type ServiceType { get; set; }
    }
}

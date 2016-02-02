using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using System.Reflection;
using EFWCoreLib.CoreFrame.Plugin;

namespace EFWCoreLib.CoreFrame.Init.AttributeManager
{
    public class WebControllerManager 
    {
        public static void LoadAttribute(List<string> BusinessDll,ModulePlugin mp)
        {
            List<WebControllerAttributeInfo> cmdControllerList = new List<WebControllerAttributeInfo>();

            for (int k = 0; k < BusinessDll.Count; k++)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(BusinessDll[k]);
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    WebControllerAttribute[] webC = ((WebControllerAttribute[])types[i].GetCustomAttributes(typeof(WebControllerAttribute), true));

                    if (webC.Length > 0)
                    {
                        WebControllerAttributeInfo cmdC = new WebControllerAttributeInfo();
                        cmdC.controllerName = types[i].Name;
                        //cmdC.webController = (AbstractController)Activator.CreateInstance(types[i], null);
                        //cmdC.webController = (AbstractController)FactoryModel.GetObject(types[i], mp.database, mp.container, mp.cache, mp.plugin.name,null);
                        cmdC.webControllerType = types[i];
                        cmdC.MethodList = new List<WebMethodAttributeInfo>();

                        MethodInfo[] property = types[i].GetMethods();
                        for (int n = 0; n < property.Length; n++)
                        {
                            WebMethodAttribute[] WebM = (WebMethodAttribute[])property[n].GetCustomAttributes(typeof(WebMethodAttribute), true);
                            if (WebM.Length > 0)
                            {
                                WebMethodAttributeInfo cmdM = new WebMethodAttributeInfo();
                                cmdM.methodName = property[n].Name;
                                cmdM.methodInfo = property[n];
                                if (WebM[0].OpenDBKeys != null && WebM[0].OpenDBKeys.ToString().Trim() != "")
                                    cmdM.dbkeys = WebM[0].OpenDBKeys.Split(new char[] { ',' }).ToList();
                                cmdC.MethodList.Add(cmdM);
                            }
                        }

                        cmdControllerList.Add(cmdC);
                    }
                }
            }

            mp.cache.Add(mp.plugin.name+"@"+GetCacheKey(), cmdControllerList);
        }

        public static string GetCacheKey()
        {
            return "webControllerAttributeList";
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

    public class WebControllerAttributeInfo
    {
        public string controllerName { get; set; }
        public AbstractController webController { get; set; }
        public Type webControllerType { get; set; }
        public List<WebMethodAttributeInfo> MethodList { get; set; }
    }

    public class WebMethodAttributeInfo
    {
        public string methodName { get; set; }
        public System.Reflection.MethodInfo methodInfo { get; set; }
        public List<string> dbkeys { get; set; }
    }
}

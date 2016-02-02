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
    public class WcfControllerManager 
    {
        public static void LoadAttribute(List<string> BusinessDll, ModulePlugin mp)
        {
            List<WcfControllerAttributeInfo> cmdControllerList = new List<WcfControllerAttributeInfo>();

            for (int k = 0; k < BusinessDll.Count; k++)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(BusinessDll[k]);
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    WCFControllerAttribute[] webC = ((WCFControllerAttribute[])types[i].GetCustomAttributes(typeof(WCFControllerAttribute), true));

                    if (webC.Length > 0)
                    {
                        WcfControllerAttributeInfo cmdC = new WcfControllerAttributeInfo();
                        cmdC.controllerName = types[i].Name;
                        //cmdC.wcfController = (AbstractController)Activator.CreateInstance(types[i], null);
                        //cmdC.wcfController = (AbstractController)FactoryModel.GetObject(types[i], mp.database, mp.container, mp.cache, mp.plugin.name, null);
                        cmdC.wcfControllerType = types[i];
                        cmdC.MethodList = new List<WcfMethodAttributeInfo>();

                        MethodInfo[] property = types[i].GetMethods();
                        for (int n = 0; n < property.Length; n++)
                        {
                            WCFMethodAttribute[] WcfM = (WCFMethodAttribute[])property[n].GetCustomAttributes(typeof(WCFMethodAttribute), true);
                            if (WcfM.Length > 0)
                            {
                                WcfMethodAttributeInfo cmdM = new WcfMethodAttributeInfo();
                                cmdM.methodName = property[n].Name;
                                cmdM.methodInfo = property[n];
                                if (WcfM[0].OpenDBKeys != null && WcfM[0].OpenDBKeys.ToString().Trim() != "")
                                    cmdM.dbkeys = WcfM[0].OpenDBKeys.Split(new char[] { ',' }).ToList();
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
            return "wcfControllerAttributeList";
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

    public class WcfControllerAttributeInfo
    {
        public string controllerName { get; set; }
        public AbstractController wcfController { get; set; }
        public Type wcfControllerType { get; set; }
        public List<WcfMethodAttributeInfo> MethodList { get; set; }
    }

    public class WcfMethodAttributeInfo
    {
        public string methodName { get; set; }
        public System.Reflection.MethodInfo methodInfo { get; set; }
        public List<string> dbkeys { get; set; }
    }
}

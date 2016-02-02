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
    public class WinformControllerManager 
    {
        public static void LoadAttribute(List<string> BusinessDll,ModulePlugin mp)
        {
            List<WinformControllerAttributeInfo> cmdControllerList = new List<WinformControllerAttributeInfo>();

            for (int k = 0; k < BusinessDll.Count; k++)
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(BusinessDll[k]);
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    WinformControllerAttribute[] winC = ((WinformControllerAttribute[])types[i].GetCustomAttributes(typeof(WinformControllerAttribute), true));

                    if (winC.Length > 0)
                    {
                        WinformControllerAttributeInfo cmdC = new WinformControllerAttributeInfo();
                        cmdC.controllerName = types[i].Name;
                        cmdC.defaultViewName = winC[0].DefaultViewName;
                        //cmdC.winformController = (AbstractController)Activator.CreateInstance(types[i], null);
                        cmdC.winformController = (AbstractController)FactoryModel.GetObject(types[i], mp.database, mp.container, mp.cache, mp.plugin.name, null);
                        cmdC.MethodList = new List<WinformMethodAttributeInfo>();
                        cmdC.ViewList = new List<WinformViewAttributeInfo>();

                        MethodInfo[] property = types[i].GetMethods();
                        for (int n = 0; n < property.Length; n++)
                        {
                            WinformMethodAttribute[] WinM = (WinformMethodAttribute[])property[n].GetCustomAttributes(typeof(WinformMethodAttribute), true);
                            if (WinM.Length > 0)
                            {
                                WinformMethodAttributeInfo cmdM = new WinformMethodAttributeInfo();
                                cmdM.methodName = property[n].Name;
                                cmdM.methodInfo = property[n];
                                if (WinM[0].OpenDBKeys != null && WinM[0].OpenDBKeys.ToString().Trim() != "")
                                    cmdM.dbkeys = WinM[0].OpenDBKeys.Split(new char[] { ',' }).ToList();
                                cmdC.MethodList.Add(cmdM);
                            }
                        }

                        WinformViewAttribute[] viewAttribute = (WinformViewAttribute[])types[i].GetCustomAttributes(typeof(WinformViewAttribute), true);
                        for (int n = 0; n < viewAttribute.Length; n++)
                        {
                            WinformViewAttributeInfo winView = new WinformViewAttributeInfo();
                            winView.Name = viewAttribute[n].Name;
                            winView.DllName = viewAttribute[n].DllName;
                            winView.ViewTypeName = viewAttribute[n].ViewTypeName;
                            winView.IsDefaultView = winView.Name == cmdC.defaultViewName ? true : false;

                            Assembly _assembly = Assembly.LoadFrom(winView.DllName);
                            winView.ViewType = _assembly.GetType(winView.ViewTypeName, false, true);
                            cmdC.ViewList.Add(winView);
                        }
                        cmdControllerList.Add(cmdC);
                    }
                }
            }

            mp.cache.Add(mp.plugin.name+"@"+GetCacheKey(), cmdControllerList);
        }

        public static string GetCacheKey()
        {
            return "winformControllerAttributeList";
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

    public class WinformControllerAttributeInfo
    {
        public string controllerName { get; set; }
        public string defaultViewName { get; set; }
        public AbstractController winformController { get; set; }
        public List<WinformMethodAttributeInfo> MethodList { get; set; }
        public List<WinformViewAttributeInfo> ViewList { get; set; }
    }

    public class WinformViewAttributeInfo
    {
        public string Name { get; set; }
        public string DllName { get; set; }
        public string ViewTypeName { get; set; }
        public bool IsDefaultView { get; set; }
        public Type ViewType { get; set; }
    }

    public class WinformMethodAttributeInfo
    {
        public string methodName { get; set; }
        public System.Reflection.MethodInfo methodInfo { get; set; }
        public List<string> dbkeys { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.Plugin;

namespace EFWCoreLib.CoreFrame.Init
{
    /// <summary>
    /// 程序运行后的插件管理
    /// </summary>
    public class AppPluginManage
    {
        public static Dictionary<string, ModulePlugin> PluginDic;

        /// <summary>
        /// 加载所有插件
        /// </summary>
        public static void LoadAllPlugin()
        {
            PluginDic = null;
            List<string> pflist = PluginSysManage.GetAllPluginFile();
            for (int i = 0; i < pflist.Count; i++)
            {
                AddPlugin(AppGlobal.AppRootPath + pflist[i]);
            }
        }

        public static void AddPlugin(string plugfile)
        {
            if (PluginDic == null)
                PluginDic = new Dictionary<string, ModulePlugin>();

            ModulePlugin mp = new ModulePlugin();
            mp.LoadPlugin(plugfile);

            if (PluginDic.ContainsKey(mp.plugin.name) == false)
            {
                PluginDic.Add(mp.plugin.name, mp);
                List<string> dllList = new List<string>();
                switch (AppGlobal.appType)
                {
                    case AppType.Web:
                        foreach (businessinfoDll dll in mp.plugin.businessinfoDllList)
                        {
                            dllList.Add(AppGlobal.AppRootPath + "bin\\" + dll.name);
                        }
                        if (dllList.Count > 0)
                        {
                            EntityManager.LoadAttribute(dllList, mp.cache, mp.plugin.name);
                            WebControllerManager.LoadAttribute(dllList, mp);
                            WebServicesManager.LoadAttribute(dllList, mp.cache, mp.plugin.name);
                        }
                        break;
                    case AppType.Winform:
                        foreach (businessinfoDll dll in mp.plugin.businessinfoDllList)
                        {
                            dllList.Add(AppGlobal.AppRootPath + dll.name);
                        }
                        if (dllList.Count > 0)
                        {
                            EntityManager.LoadAttribute(dllList, mp.cache, mp.plugin.name);
                            WinformControllerManager.LoadAttribute(dllList, mp);
                        }
                        break;
                    case AppType.WCF:
                        foreach (businessinfoDll dll in mp.plugin.businessinfoDllList)
                        {
                            dllList.Add(AppGlobal.AppRootPath + dll.name);
                        }
                        if (dllList.Count > 0)
                        {
                            EntityManager.LoadAttribute(dllList, mp.cache, mp.plugin.name);
                            WcfControllerManager.LoadAttribute(dllList, mp);
                        }
                        break;
                    case AppType.WCFClient:
                        foreach (businessinfoDll dll in mp.plugin.businessinfoDllList)
                        {
                            dllList.Add(AppGlobal.AppRootPath + dll.name);
                        }
                        if (dllList.Count > 0)
                        {
                            WinformControllerManager.LoadAttribute(dllList, mp);
                        }
                        break;
                }
            }
        }

        public static void RemovePlugin(string plugname)
        {
            if (PluginDic.ContainsKey(plugname) == true)
            {
                ICacheManager _cache = PluginDic[plugname].cache;

                switch (AppGlobal.appType)
                {
                    case AppType.Web:
                        EntityManager.ClearAttributeData(_cache, plugname);
                        WebControllerManager.ClearAttributeData(_cache, plugname);
                        WebServicesManager.ClearAttributeData(_cache, plugname);
                        break;
                    case AppType.Winform:
                        EntityManager.ClearAttributeData(_cache, plugname);
                        WinformControllerManager.ClearAttributeData(_cache, plugname);
                        break;
                    case AppType.WCF:
                        EntityManager.ClearAttributeData(_cache, plugname);
                        WcfControllerManager.ClearAttributeData(_cache, plugname);
                        break;
                }

                PluginDic.Remove(plugname);
            }
        }

        public static WebControllerAttributeInfo GetPluginWebControllerAttributeInfo(string pluginname, string name, out ModulePlugin mp)
        {
            mp = PluginDic[pluginname];
            if (mp != null)
            {
                List<WebControllerAttributeInfo> list = (List<WebControllerAttributeInfo>)WebControllerManager.GetAttributeInfo(mp.cache, mp.plugin.name);
                if (list.FindIndex(x => x.controllerName == name) > -1)
                {
                    return list.Find(x => x.controllerName == name);
                }
            }
            return null;
        }

        public static WinformControllerAttributeInfo GetPluginWinformControllerAttributeInfo(string pluginname, string name, out ModulePlugin mp)
        {
            mp = PluginDic[pluginname];
            if (mp != null)
            {
                List<WinformControllerAttributeInfo> list = (List<WinformControllerAttributeInfo>)WinformControllerManager.GetAttributeInfo(mp.cache, mp.plugin.name);
                if (list!=null && list.FindIndex(x => x.controllerName == name) > -1)
                {
                    return list.Find(x => x.controllerName == name);
                }
            }
            return null;

            //foreach (KeyValuePair<string, ModulePlugin> val in PluginDic)
            //{
            //    List<WinformControllerAttributeInfo> list = (List<WinformControllerAttributeInfo>)WinformControllerManager.GetAttributeInfo(val.Value.cache, val.Value.plugin.name);
            //    if (list.FindIndex(x => x.controllerName == name) > -1)
            //    {
            //        mp = val.Value;
            //        return list.Find(x => x.controllerName == name);
            //    }
            //}
            //mp = null;
            //return null;
        }

        public static WcfControllerAttributeInfo GetPluginWcfControllerAttributeInfo(string pluginname, string name, out ModulePlugin mp)
        {
            mp = PluginDic[pluginname];
            if (mp != null)
            {
                List<WcfControllerAttributeInfo> list = (List<WcfControllerAttributeInfo>)WcfControllerManager.GetAttributeInfo(mp.cache, mp.plugin.name);
                if (list.FindIndex(x => x.controllerName == name) > -1)
                {
                    return list.Find(x => x.controllerName == name);
                }
            }
            return null;


            //foreach (KeyValuePair<string, ModulePlugin> val in PluginDic)
            //{
            //    List<WcfControllerAttributeInfo> list = (List<WcfControllerAttributeInfo>)WcfControllerManager.GetAttributeInfo(val.Value.cache, val.Value.plugin.name);
            //    if (list.FindIndex(x => x.controllerName == name) > -1)
            //    {
            //        mp = val.Value;
            //        return list.Find(x => x.controllerName == name);
            //    }
            //}
            //mp = null;
            //return null;
        }

        public static WebServicesAttributeInfo GetPluginWebServicesAttributeInfo(string name, out ModulePlugin mp)
        {
            foreach (KeyValuePair<string, ModulePlugin> val in PluginDic)
            {
                List<WebServicesAttributeInfo> list = (List<WebServicesAttributeInfo>)WebServicesManager.GetAttributeInfo(val.Value.cache, val.Value.plugin.name);
                if (list.FindIndex(x => x.ServiceName == name) > -1)
                {
                    mp = val.Value;
                    return list.Find(x => x.ServiceName == name);
                }
            }
            mp = null;
            return null;
        }

        public static string getbaseinfoDataValue(string _pluginName,string key)
        {
            if (AppPluginManage.PluginDic[_pluginName].plugin.baseinfoDataList.FindIndex(x => x.key == key) != -1)
            {
                return AppPluginManage.PluginDic[_pluginName].plugin.baseinfoDataList.Find(x => x.key == key).value;
            }
            return null;
        }
    }
}

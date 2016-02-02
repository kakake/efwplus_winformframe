using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Init;
using System.Xml;

namespace EFWCoreLib.CoreFrame.Plugin
{
    public class PluginSysManage
    {
        private static System.Xml.XmlDocument xmlDoc = null;
        public static string pluginsysFile = AppGlobal.AppRootPath + "Config/pluginsys.xml";

        private static void InitConfig()
        {
            xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(pluginsysFile);
        }

        public static List<string> GetAllPluginFile()
        {
            List<string> pflist = new List<string>();
            if (xmlDoc == null) InitConfig();
            XmlNodeList nl = null;
            string path = AppGlobal.AppRootPath;
            switch (AppGlobal.appType)
            {
                case AppType.Web:
                     nl = xmlDoc.DocumentElement.SelectNodes("WebModulePlugin/Plugin");
                    break;
                
                case AppType.Winform:
                    nl = xmlDoc.DocumentElement.SelectNodes("WinformModulePlugin/Plugin");
                    break;
                case AppType.WCF:
                case AppType.WCFClient:
                    nl = xmlDoc.DocumentElement.SelectNodes("WcfModulePlugin/Plugin");
                    break;
            }
            foreach (XmlNode n in nl)
            {
                pflist.Add(n.Attributes["path"].Value);
            }
            return pflist;
        }

        public static void GetWinformEntry(out string entryplugin, out string entrycontroller)
        {
            if (xmlDoc == null) InitConfig();

            entryplugin = xmlDoc.DocumentElement.SelectNodes("WinformModulePlugin")[0].Attributes["EntryPlugin"].Value.ToString();
            entrycontroller = xmlDoc.DocumentElement.SelectNodes("WinformModulePlugin")[0].Attributes["EntryController"].Value.ToString();
        }

        public static void GetWcfClientEntry(out string entryplugin, out string entrycontroller)
        {
            if (xmlDoc == null) InitConfig();

            entryplugin = xmlDoc.DocumentElement.SelectNodes("WcfModulePlugin")[0].Attributes["EntryPlugin"].Value.ToString();
            entrycontroller = xmlDoc.DocumentElement.SelectNodes("WcfModulePlugin")[0].Attributes["EntryController"].Value.ToString();
        }
    }
}

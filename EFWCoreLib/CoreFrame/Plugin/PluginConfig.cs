using EFWCoreLib.CoreFrame.Init;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.Plugin
{
    /// <summary>
    /// 插件配置文件数据
    /// </summary>
    public class PluginConfig
    {
        public string name { get; set; }
        public string version { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string plugintype { get; set; }
        public string defaultdbkey { get; set; }
        public string defaultcachekey { get; set; }

        public List<baseinfoData> baseinfoDataList { get; set; }
        public List<businessinfoDll> businessinfoDllList { get; set; }

        public void Add(PluginSectionHandler plugin)
        {
            if (plugin != null)
            {
                if (baseinfoDataList == null) baseinfoDataList = new List<baseinfoData>();
                if (businessinfoDllList == null) businessinfoDllList = new List<businessinfoDll>();

                name = plugin.name;
                version = plugin.version;
                title = plugin.title;
                author = plugin.author;
                plugintype = plugin.plugintype;
                defaultdbkey = plugin.defaultdbkey;
                defaultcachekey = plugin.defaultcachekey;

                foreach (baseinfoData data in plugin.baseinfo)
                {
                    if (baseinfoDataList.FindIndex(x => x.key == data.key) == -1)
                        baseinfoDataList.Add(data);
                }

                foreach (businessinfoDll dll in plugin.businessinfo)
                {
                    if (businessinfoDllList.FindIndex(x => x.name == dll.name) == -1)
                    {
                        bool exists = false;
                        if (plugintype.ToLower() == "winform" || plugintype.ToLower() == "wcf")
                        {
                            exists = new FileInfo(AppGlobal.AppRootPath + dll.name).Exists;
                        }
                        else if (plugintype.ToLower() == "web")
                        {
                            exists = new FileInfo(AppGlobal.AppRootPath +"bin\\" + dll.name).Exists;
                        }

                        if (exists)
                            businessinfoDllList.Add(dll);
                        else//没有编译的dll记录并提示
                            AppGlobal.missingDll.Add(dll.name);
                    }
                }
            }
        }
    }
}

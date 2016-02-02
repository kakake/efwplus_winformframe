using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.EntLib;
using System.Configuration;
using Microsoft.Practices.Unity.Configuration;

namespace EFWCoreLib.CoreFrame.Plugin
{
    /// <summary>
    /// 模块插件
    /// </summary>
    public class ModulePlugin
    {
        /// <summary>
        /// 插件配置
        /// </summary>
        public PluginConfig plugin { get; set; }
        /// <summary>
        /// 数据库对象
        /// </summary>
        public AbstractDatabase database{get;set;}
        /// <summary>
        /// Unity对象容器
        /// </summary>
        public IUnityContainer container{get;set;}
        /// <summary>
        /// 企业库缓存
        /// </summary>
        public ICacheManager cache{get;set;}

        public ModulePlugin()
        {
            container = ZhyContainer.CreateUnity();
            plugin = new PluginConfig();
        }

        /// <summary>
        /// 导入插件配置文件
        /// </summary>
        /// <param name="plugfile">插件配置文件路径</param>
        public void LoadPlugin(string plugfile)
        {
            var fileMap = new ExeConfigurationFileMap { ExeConfigFilename = plugfile };
            System.Configuration.Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            var unitySection = (UnityConfigurationSection)configuration.GetSection("unity");
            if (unitySection != null)
                container.LoadConfiguration(unitySection);//判断EntLib的路径对不对

            var plugininfo = (PluginSectionHandler)configuration.GetSection("plugin");
            if (plugininfo != null)
                plugin.Add(plugininfo);

            if (plugin.defaultdbkey != "")
                database = FactoryDatabase.GetDatabase(plugin.defaultdbkey);
            else
                database = FactoryDatabase.GetDatabase();

            database.PluginName = plugin.name;

            if (plugin.defaultcachekey != "")
                cache = ZhyContainer.CreateCache(plugin.defaultcachekey);
            else
                cache = ZhyContainer.CreateCache();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.Interface;
using EFWCoreLib.CoreFrame.DbProvider;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.Plugin;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Business;

namespace EFWCoreLib.WebFrame.WebForm
{
    public class WebformPage : System.Web.UI.Page, INewObject, INewDao
    {
        private AbstractDatabase _oleDb = null;
        protected IUnityContainer _container = null;
        protected ICacheManager _cache = null;
        protected string _pluginName = null;

        public AbstractDatabase oleDb
        {
            get
            {
                return _oleDb;
            }
        }

        #region INewObject 成员
        public T NewObject<T>()
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container, _cache, _pluginName, null);
            return t;
        }

        public T NewObject<T>(string unityname)
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container, _cache, _pluginName, unityname);
            return t;
        }

        #endregion

        #region INewDao 成员

        public T NewDao<T>()
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container, _cache, _pluginName, null);
            return t;
        }

        public T NewDao<T>(string unityname)
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container, _cache, _pluginName, unityname);
            return t;
        }

        #endregion

        protected void InitPlugin(string pluginname)
        {
            //Page_Load 进行初始化
            _pluginName = pluginname;

            ModulePlugin mp;
            mp = AppPluginManage.PluginDic[_pluginName];
            if (mp == null)
                throw new Exception("插件名：" + pluginname + "不存在！");
            _oleDb = mp.database;
            _container = mp.container;
            _cache = mp.cache;
        }
    }
}

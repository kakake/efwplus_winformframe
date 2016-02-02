
using System.Web.Services;
using EFWCoreLib.CoreFrame.Business.Interface;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Plugin;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.Unity;
using EFWCoreLib.CoreFrame.Init;
 


namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// WebService基类
    /// </summary>
    public class AbstractService : WebService, INewObject, INewDao
    {

        private AbstractDatabase _oleDb = null;
        protected IUnityContainer _container = null;
        protected ICacheManager _cache = null;
        protected string _pluginName = null;

        public AbstractDatabase oleDb
        {
            get
            {
                if (_oleDb == null)
                {
                    ModulePlugin mp;
                    AppPluginManage.GetPluginWebServicesAttributeInfo(this.GetType().Name, out mp);
                    _oleDb = mp.database;
                    _container = mp.container;
                    _cache = mp.cache;
                    _pluginName = mp.plugin.name;
                }
                return _oleDb;
            }
        }

        //声明Soap头实例
        public GlobalParam param = new GlobalParam();

  

        #region INewObject 成员
        public T NewObject<T>()
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container,_cache,_pluginName, null);
            return t;
        }

        public T NewObject<T>(string unityname)
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container,_cache,_pluginName, unityname);
            return t;
        }

        #endregion

        #region INewDao 成员

        public T NewDao<T>()
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container,_cache,_pluginName, null);
            return t;
        }

        public T NewDao<T>(string unityname)
        {
            T t = FactoryModel.GetObject<T>(_oleDb, _container,_cache,_pluginName, unityname);
            return t;
        }

        #endregion

    }

    public class GlobalParam : System.Web.Services.Protocols.SoapHeader
    {
        public SysLoginRight GetSysLoginRight { get; set; }
    }

}
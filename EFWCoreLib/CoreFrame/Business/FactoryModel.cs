
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using EFWCoreLib.CoreFrame.DbProvider;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.Business.Interface;


namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// 创建实体工厂
    /// </summary>
    public class FactoryModel
    {

        public static T GetObject<T>(AbstractDatabase Db, IUnityContainer _container, ICacheManager _cache, string _pluginName, string unityname)
        {
            //if (Db == null)
            //{
            //    EFWCoreLib.CoreFrame.DbProvider.AbstractDatabase Rdb = EFWCoreLib.CoreFrame.DbProvider.FactoryDatabase.GetDatabase();
            //    //SysLoginRight currLoginUser = (SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser");
            //    //Rdb.WorkId = currLoginUser.WorkId;
            //    Db = Rdb;
            //    _container = EFWCoreLib.CoreFrame.Init.AppGlobal.container;
            //}

            //读unity配置文件把类注入到接口得到对象实例
            IUnityContainer container = _container;

            T t = default(T);
            if (unityname == null)
                t = container.Resolve<T>();
            else
                t = container.Resolve<T>(unityname);

            IbindDb ibind = (IbindDb)t;
            ibind.BindDb(Db,container,_cache,_pluginName);

            //给对象加上代理
            t = (T)PolicyInjection.Wrap(t.GetType(), t);
            return t;
        }

        public static object GetObject(Type type, AbstractDatabase Db, IUnityContainer _container, ICacheManager _cache, string _pluginName, string unityname)
        {
            //if (Db == null)
            //{
            //    EFWCoreLib.CoreFrame.DbProvider.AbstractDatabase Rdb = EFWCoreLib.CoreFrame.DbProvider.FactoryDatabase.GetDatabase();
            //    //SysLoginRight currLoginUser = (SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser");
            //    //Rdb.WorkId = currLoginUser.WorkId;
            //    Db = Rdb;
            //    _container = EFWCoreLib.CoreFrame.Init.AppGlobal.container;
            //}

            //读unity配置文件把类注入到接口得到对象实例
            IUnityContainer container = _container;

            Object t = null;
            if (unityname == null)
                t = container.Resolve(type);
            else
                t = container.Resolve(type, unityname);

            IbindDb ibind = (IbindDb)t;
            ibind.BindDb(Db,container,_cache,_pluginName);

            //给对象加上代理
            t = PolicyInjection.Wrap(t.GetType(), t);

            return t;
        }
    }
}

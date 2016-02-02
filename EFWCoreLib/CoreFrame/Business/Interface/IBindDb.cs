//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System.Collections.Generic;
using EFWCoreLib.CoreFrame.DbProvider;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.Unity;

namespace EFWCoreLib.CoreFrame.Business.Interface
{
    /// <summary>
    /// 清除数据库对象的接口，使业务对象能在Session里保存
    /// </summary>
    interface IbindDb
    {
        /// <summary>
        /// 绑定默认数据库操作对象
        /// </summary>
        /// <param name="Db">默认数据库对象</param>
        /// <param name="container"></param>
        /// <param name="cache"></param>
        void BindDb(AbstractDatabase Db, IUnityContainer container, ICacheManager cache, string pluginName);
        /// <summary>
        /// 绑定除默认数据库对象外更多数据库对象
        /// </summary>
        /// <param name="Db"></param>
        /// <param name="dbkey"></param>
        void BindMoreDb(AbstractDatabase Db, string dbkey);
        /// <summary>
        /// 给实体绑定数据库对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        T BindDb<T>(T model);

        /// <summary>
        /// 给List实体绑定数据库对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        List<T> ListBindDb<T>(List<T> list);

        /// <summary>
        /// 获取默认数据库对象
        /// </summary>
        /// <returns></returns>
        AbstractDatabase GetDb();
        /// <summary>
        /// 获取除默认外配置的更多数据库对象
        /// </summary>
        /// <returns></returns>
        List<AbstractDatabase> GetMoreDb();
        /// <summary>
        /// 获取企业库中的UnityContainer
        /// </summary>
        /// <returns></returns>
        IUnityContainer GetUnityContainer();
        /// <summary>
        /// 切换到默认数据库
        /// </summary>
        void UseDb();
        /// <summary>
        /// 切换数据库
        /// </summary>
        /// <param name="dbkey">数据库dbkey</param>
        void UseDb(string dbkey);
    }
}

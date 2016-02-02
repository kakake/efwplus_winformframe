//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;



namespace EFWCoreLib.CoreFrame.EntLib
{
    /// <summary>
    /// 封装企业库容器
    /// </summary>
    public class ZhyContainer
    {
        //public static IUnityContainer container = null;
        /// <summary>
        /// 获取依赖注入容器
        /// </summary>
        /// <returns></returns>
        public static IUnityContainer CreateUnity()
        {
            IUnityContainer container = new UnityContainer();
            return container;
        }

        //public static void AddUnity(UnityConfigurationSection section)
        //{
        //    if (container == null) CreateUnity();
        //    container.LoadConfiguration(section);
        //}

        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <returns>数据库对象</returns>
        public static Database CreateDataBase()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<Database>();
        }
        /// <summary>
        /// 获取数据库对象
        /// </summary>
        /// <param name="name">数据库实例名(默认name为空,调用默认数据库实例)</param>
        /// <returns>数据库对象</returns>
        public static Database CreateDataBase(string name)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<Database>(name);
        }
        /// <summary>
        /// 获取日志写入对象
        /// </summary>
        /// <returns></returns>
        public static LogWriter CreateLog()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
        }

        public static LogWriter CreateLog(string name)
        {
            return EnterpriseLibraryContainer.Current.GetInstance<LogWriter>(name);
        }
        /// <summary>
        /// 获取日志跟踪对象
        /// </summary>
        /// <returns></returns>
        public static TraceManager CreateTrace()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<TraceManager>();
        }
        /// <summary>
        /// 获取异常处理对象
        /// </summary>
        /// <returns></returns>
        public static ExceptionManager CreateException()
        {
            return EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
        }

        //public static ICacheManager cacheManager = null;
        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <returns></returns>
        public static ICacheManager CreateCache()
        {

            ICacheManager cacheManager = CacheFactory.GetCacheManager();
            return cacheManager;
        }

        public static ICacheManager CreateCache(string name)
        {
            ICacheManager cacheManager = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>(name);
            return cacheManager;
        }
    }
}

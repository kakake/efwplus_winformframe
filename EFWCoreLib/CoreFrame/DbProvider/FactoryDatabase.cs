//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFWCoreLib.CoreFrame.DbProvider
{
    /// <summary>
    /// 创建数据库操作对象
    /// </summary>
    public class FactoryDatabase
    {
        /// <summary>
        /// 默认数据库
        /// </summary>
        /// <returns></returns>
        public static AbstractDatabase GetDatabase()
        {
            AbstractDatabase _oleDb = null;
            _oleDb = new EntLibDb();
            //string dbtype = System.Configuration.ConfigurationManager.AppSettings["DbType"].ToString();//获取默认数据库连接
            //switch (dbtype)
            //{
            //    case "SqlServer":
            //        _oleDb.DbType = DatabaseType.SqlServer2005;
            //        break;
            //    case "SqlServer2000":
            //        _oleDb.DbType = DatabaseType.SqlServer2000;
            //        break;
            //    case "Oracle":
            //        _oleDb.DbType = DatabaseType.Oracle;
            //        break;
            //    case "MySQL":
            //        _oleDb.DbType = DatabaseType.MySQL;
            //        break;
            //    case "IbmDb2":
            //        _oleDb.DbType = DatabaseType.IbmDb2;
            //        break;
            //    case "MsAccess":
            //        _oleDb.DbType = DatabaseType.MsAccess;
            //        break;
            //    default:
            //        _oleDb.DbType = DatabaseType.UnKnown;
            //        break;
            //}
            return _oleDb;
        }

        /// <summary>
        /// 不同数据库之间切换
        /// </summary>
        /// <param name="dbkey"></param>
        /// <returns></returns>
        public static AbstractDatabase GetDatabase(string dbkey)
        {
            if (string.IsNullOrEmpty(dbkey))
                throw new Exception("没有数据库Key！");

            AbstractDatabase _oleDb = new EntLibDb(dbkey);

            //string dbtype = System.Configuration.ConfigurationManager.AppSettings["DbType"].ToString();//获取默认数据库连接
            //switch (dbtype)
            //{
            //    case "SqlServer":
            //        _oleDb.DbType = DatabaseType.SqlServer2005;
            //        break;
            //    case "SqlServer2000":
            //        _oleDb.DbType = DatabaseType.SqlServer2000;
            //        break;
            //    case "Oracle":
            //        _oleDb.DbType = DatabaseType.Oracle;
            //        break;
            //    case "MySQL":
            //        _oleDb.DbType = DatabaseType.MySQL;
            //        break;
            //    case "IbmDb2":
            //        _oleDb.DbType = DatabaseType.IbmDb2;
            //        break;
            //    case "MsAccess":
            //        _oleDb.DbType = DatabaseType.MsAccess;
            //        break;
            //    default:
            //        _oleDb.DbType = DatabaseType.UnKnown;
            //        break;
            //}
            return _oleDb;
        }

        public static void TestDbConnection()
        {
            GetDatabase().TestDbConnection();
        }
    }

    /// <summary>
    /// 数据库类别
    /// </summary>
    public enum DatabaseType
    {
        /// <summary>未指定数据库</summary>
        UnKnown,
        /// <summary>IBMDB2数据库</summary>
        IbmDb2,
        /// <summary>SqlServer2000数据库</summary>
        SqlServer2000,
        /// <summary>SqlServer2005数据库</summary>
        SqlServer2005,
        /// <summary>Access数据库</summary>
        MsAccess,
        /// <summary>MySQL数据库</summary>
        MySQL,
        /// <summary>Oracle数据库</summary>
        Oracle,
        /// <summary>
        /// 中间件
        /// </summary>
        MidLinkDB
    }
}

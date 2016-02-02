
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using EFWCoreLib.CoreFrame.DbProvider;

namespace EFWCoreLib.CoreFrame.Orm
{
    /// <summary>
    /// ORM解析类创建工厂
    /// </summary>
    class OrmAnalysisFactory
    {
        /// <summary>
        /// 根据数据库类型，创建ORM解析对象
        /// </summary>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        public static OrmAnalysis CreateOrmAnalysisObject(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.Oracle:
                    return new OracleOrmAnalysis();

                case DatabaseType.IbmDb2:
                    return new DB2OrmAnalysis();

                case DatabaseType.SqlServer2005:
                case DatabaseType.SqlServer2000:
                    return new SqlServerOrmAnalysis();

                case DatabaseType.MySQL:
                    return new MySqlOrmAnalysis();
                case DatabaseType.MsAccess:
                    return new MsAccessOrmAnalysis();
            }

            throw new Exception("没有实现该数据库");
        }
    }
}

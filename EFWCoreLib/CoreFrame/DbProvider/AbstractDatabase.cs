//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Practices.EnterpriseLibrary.Data;
using EFWCoreLib.CoreFrame.Orm;

namespace EFWCoreLib.CoreFrame.DbProvider
{
    [Serializable]
    public abstract class AbstractDatabase 
	{
        /// <summary>
        /// 数据库连接
        /// </summary>
        protected DbConnection connection = null;			//数据库连接

        /// <summary>
        /// 数据库对象执行命令
        /// </summary>
        protected DbCommand command = null;

        /// <summary>
        /// 企业库数据库访问对象
        /// </summary>
        public Database database = null;

        #region 属性

        /// <summary>
        /// 数据库事务
        /// </summary>
        protected DbTransaction transaction = null;			//数据库事务

        protected string _connString;
        /// <summary>
        /// 返回数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return _connString; }
        }

        protected bool isInTransaction = false;				//是否在事务中
        /// <summary>
        /// 返回是否处于事务中
        /// </summary>
        protected bool IsInTransaction
        {
            get { return this.isInTransaction; }
        }

        public int WorkId { get; set; }
        //Entlib.config配置文件中配置的连接数据库的key
        public string DbKey { get; set; }
        //插件名称
        public string PluginName { get; set; }
        //数据库类型
        public DatabaseType DbType { get; set; }

        public abstract void TestDbConnection();

        #endregion
		
		/// <summary>
		/// 启动一个事务
		/// </summary>
        public abstract void BeginTransaction();
		/// <summary>
		/// 提交一个事务
		/// </summary>
        public abstract void CommitTransaction();
		/// <summary>
		/// 回滚一个事务
		/// </summary>
        public abstract void RollbackTransaction();

		#region 执行插入一条记录 适用于有 自动生成标识的列
        public abstract int InsertRecord(string commandtext);
		#endregion

		#region 返回一个DataTable
		public abstract DataTable GetDataTable(string commandtext);
        public abstract DataTable GetDataTable(string storeProcedureName, params object[] parameters);
		#endregion

		#region 返回一个DataReader
		public abstract IDataReader GetDataReader(string commandtext);
		#endregion

		#region 执行一个语句，返回执行情况
		public abstract int DoCommand(string commandtext);
        public abstract int DoCommand(string storeProcedureName, params object[] parameters);
		#endregion

		#region 执行一个命令返回一个数据结果
		public abstract object GetDataResult(string commandtext);
        public abstract object GetDataResult(string storeProcedureName, params object[] parameters);
		#endregion

        public abstract DataSet GetDataSet(string storeProcedureName, params object[] parameters);

#if !CSHARP30
        public IEnumerable<dynamic> Query(string sql, object param)
        {
            connection = database.CreateConnection();
            return connection.Query(PluginName,sql, param);
        }
#else
        public IEnumerable<IDictionary<string, object>> Query(string sql, object param)
        {
            connection = database.CreateConnection();
            return connection.Query(sql, param);
        }
#endif
        public IEnumerable<T> Query<T>(string sql, object param)
        {
            connection = database.CreateConnection();
            return connection.Query<T>(PluginName,sql, param, null, true, null, null);
        }


        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object param)
        {
            connection = database.CreateConnection();
            return connection.Query<TFirst, TSecond, TReturn>(PluginName,sql, map, param, null, true, "Id", null, null);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TReturn>(string sql, Func<TFirst, TSecond, TThird, TReturn> map, object param)
        {
            connection = database.CreateConnection();
            return connection.Query<TFirst, TSecond, TThird, TReturn>(PluginName,sql, map, param, null, true, "Id", null, null);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TThird, TFourth, TReturn>(string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, object param)
        {
            connection = database.CreateConnection();
            return connection.Query<TFirst, TSecond, TThird, TFourth, TReturn>(PluginName,sql, map, param, null, true, "Id", null, null);
        }

        public EFWCoreLib.CoreFrame.Orm.SqlMapper.GridReader QueryMultiple(string sql, object param)
        {
            connection = database.CreateConnection();
            return connection.QueryMultiple(sql, param, null, null, null);
        }
       
    }



}

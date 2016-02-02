using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

using Microsoft.Practices.EnterpriseLibrary.Data;
using EFWCoreLib.CoreFrame.EntLib;

namespace EFWCoreLib.CoreFrame.DbProvider
{
    /// <summary>
    /// Oracle
    /// </summary>
    public class SqlServerDb : AbstractDatabase
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
        protected  Database database = null;

        public SqlServerDb()
            : base()
        {
            database = ZhyContainer.CreateDataBase();
            _connString = database.ConnectionString;
        }

        public SqlServerDb(string key)
            : base()
        {

            database = ZhyContainer.CreateDataBase(key);
            _connString = database.ConnectionString;
        }

        public override void TestDbConnection()
        {
            database.CreateConnection().Open();
        }

        public override void BeginTransaction()
        {
            try
            {
                if (isInTransaction == false)
                {
                    connection = database.CreateConnection();
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    isInTransaction = true;
                }
                else
                {
                    throw new Exception("事务正在进行，一个对象不能同时开启多个事务！");
                }
            }
            catch (Exception e)
            {
                connection.Close();
                isInTransaction = false;
                throw new Exception("事务启动失败，请再试一次！\n" + e.Message);
            }
        }
        public override void CommitTransaction()
        {
            if (transaction != null)
            {
                transaction.Commit();
                isInTransaction = false;
                connection.Close();
            }else
            
            throw new Exception("无可用事务！");
        }
        public override void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                isInTransaction = false;
                connection.Close();

            }else

           
            throw new Exception("无可用事务！");

        }
 

        public override int InsertRecord(string commandtext)
        {
            //string strsql = "SELECT Test_SQL.nextval FROM dual";SELECT  @@IDENTITY
            if (isInTransaction)
            {
                command = database.GetSqlStringCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                command.CommandText = command.CommandText + ";SELECT  @@IDENTITY";
                return Convert.ToInt32(database.ExecuteScalar(command, transaction));
            }
            else
            {
                command = database.GetSqlStringCommand(commandtext);
                command.CommandText = command.CommandText + ";SELECT  @@IDENTITY";
                return Convert.ToInt32(database.ExecuteScalar(command));
            }
            //command.CommandText = "SELECT  @@IDENTITY";

            //return Convert.ToInt32(database.ExecuteScalar(command));
        }
      
        public override DataTable GetDataTable(string commandtext)
        {
            DataSet ds = null;

            if (isInTransaction)
            {
                command = new SqlCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;

                ds = database.ExecuteDataSet(command,transaction);
            }
            else
            {
                ds = database.ExecuteDataSet(CommandType.Text, commandtext);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            throw new Exception("没有数据");
        }

        public override DataTable GetDataTable(string storeProcedureName, params object[] parameters)
        {
            DataSet ds = null;
            //List<object> param = new List<object>();

            //foreach (IDbDataParameter val in parameters)
            //{
            //    param.Add(val.Value);
            //}

            if (isInTransaction)
            {
                command = database.GetStoredProcCommand(storeProcedureName, parameters);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;

                ds = database.ExecuteDataSet(command, transaction);
            }
            else
            {
                ds = database.ExecuteDataSet(storeProcedureName, parameters);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            throw new Exception("没有数据");
        }

        public override IDataReader GetDataReader(string commandtext)
        {
            if (isInTransaction)
            {
                command = database.GetSqlStringCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                return database.ExecuteReader(command,transaction);
            }
            else
            {
                return database.ExecuteReader(CommandType.Text, commandtext);
            }
        }


        public override int DoCommand(string commandtext)
        {
            if (isInTransaction)
            {
                command = database.GetSqlStringCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                return database.ExecuteNonQuery(command,transaction);
            }
            else
            {
                return database.ExecuteNonQuery(CommandType.Text, commandtext);
            }
        }
        public override int DoCommand(string storeProcedureName, params object[] parameters)
        {
            if (isInTransaction)
            {
                command = database.GetStoredProcCommand(storeProcedureName, parameters);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                return database.ExecuteNonQuery(command, transaction);
            }
            else
            {
                return database.ExecuteNonQuery(storeProcedureName, parameters);
            }
        }

        public override object GetDataResult(string commandtext)
        {
            if (isInTransaction)
            {
                command = database.GetSqlStringCommand(commandtext);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                return database.ExecuteScalar(command, transaction);
            }
            else
            {
                return database.ExecuteScalar(CommandType.Text, commandtext);
            }
        }
        public override object GetDataResult(string storeProcedureName, params object[] parameters)
        {
            if (isInTransaction)
            {
                command = database.GetStoredProcCommand(storeProcedureName, parameters);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;
                return database.ExecuteScalar(command, transaction);
            }
            else
            {
                return database.ExecuteScalar(storeProcedureName, parameters);
            }
        }

        public override DataSet GetDataSet(string storeProcedureName, params object[] parameters)
        {
            DataSet ds = null;

            if (isInTransaction)
            {
                command = database.GetStoredProcCommand(storeProcedureName, parameters);
                command.Connection = connection;
                command.Transaction = transaction;
                command.CommandType = CommandType.Text;

                ds = database.ExecuteDataSet(command, transaction);
            }
            else
            {
                ds = database.ExecuteDataSet(storeProcedureName, parameters);
            }

            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }
            throw new Exception("没有数据");
        }

        
    }
}

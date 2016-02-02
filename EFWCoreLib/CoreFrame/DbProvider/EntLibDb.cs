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
    /// EntLibDb
    /// </summary>
    public class EntLibDb : AbstractDatabase
    {
       

        public EntLibDb()
            : base()
        {
            database = ZhyContainer.CreateDataBase();
            _connString = database.ConnectionString;

            switch (database.GetType().Name)
            {
                case "SqlDatabase":
                    DbType = DatabaseType.SqlServer2005;
                    break;
                case "OracleDatabase":
                    DbType = DatabaseType.Oracle;
                    break;
                default:
                    DbType = DatabaseType.UnKnown;
                    break;
            }
        }

        public EntLibDb(string key)
            : base()
        {

            database = ZhyContainer.CreateDataBase(key);
            _connString = database.ConnectionString;

            switch (database.GetType().Name)
            {
                case "SqlDatabase":
                    DbType = DatabaseType.SqlServer2005;
                    break;
                case "OracleDatabase":
                    DbType = DatabaseType.Oracle;
                    break;
                default:
                    DbType = DatabaseType.UnKnown;
                    break;
            }
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
            }
            else

                throw new Exception("无可用事务！");
        }
        public override void RollbackTransaction()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                isInTransaction = false;
                connection.Close();
            }
            else
                throw new Exception("无可用事务！");
        }


        public override int InsertRecord(string commandtext)
        {
            switch (DbType)
            {
                case DatabaseType.Oracle:
                    //string strsql = "SELECT Test_SQL.nextval FROM dual";SELECT  @@IDENTITY
                    if (isInTransaction)
                    {
                        command = database.GetSqlStringCommand(commandtext);
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.CommandType = CommandType.Text;
                        //command.CommandText = command.CommandText + ";SELECT  @@IDENTITY";
                        return Convert.ToInt32(database.ExecuteScalar(command, transaction));
                    }
                    else
                    {
                        command = database.GetSqlStringCommand(commandtext);
                        //command.CommandText = command.CommandText + ";SELECT  @@IDENTITY";
                        return Convert.ToInt32(database.ExecuteScalar(command));
                    }
                case DatabaseType.SqlServer2005:
                case DatabaseType.SqlServer2000:
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
                case DatabaseType.IbmDb2:
                    throw new Exception("未实现IbmDb2的数据库操作！");
                    break;
                case DatabaseType.MySQL:
                    throw new Exception("未实现MySQL的数据库操作！");
                    break;
                default:
                    throw new Exception("未实现的数据库操作！");
                    break;
            }
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

                ds = database.ExecuteDataSet(command, transaction);
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
                return database.ExecuteReader(command, transaction);
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
                return database.ExecuteNonQuery(command, transaction);
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using EFWCoreLib.CoreFrame.Init.AttributeManager;

namespace EFWCoreLib.CoreFrame.Orm
{
    /// <summary>
    /// 基于ORM实现sqlserver数据库的ORM
    /// </summary>
    public class SqlServerOrmAnalysis:OrmAnalysis
    {
        
        public override string GetInsertSQL(object model)
        {
            string strsql = "";
            try
            {
                Dictionary<string, object> dicsql = new Dictionary<string, object>();

                TableAttributeInfo tableAttribute = GetTableAttributeInfo(model);
                List<ColumnAttributeInfo> columnAttributeCollection = tableAttribute.ColumnAttributeInfoList;

                for (int i = 0; i < columnAttributeCollection.Count; i++)
                {

                    ColumnAttributeInfo columnAttributeInfo = columnAttributeCollection[i];

                    if (columnAttributeInfo.DataKey == true && columnAttributeInfo.Match == "Custom:Guid")//赋值给自增长ID
                    {
                        object obj = GetEntityValue(columnAttributeInfo.PropertyName, model);
                        obj = obj == null ? Guid.NewGuid().ToString() : obj;

                        SetEntityValue(columnAttributeInfo.PropertyName, model, obj);

                        dicsql.Add(columnAttributeInfo.FieldName, obj);
                    }
                    else
                    {

                        if (columnAttributeInfo.IsInsert == true)
                        {
                            object obj = GetEntityValue(columnAttributeInfo.PropertyName, model);
                            obj = obj == null ? "" : obj;
                            dicsql.Add(columnAttributeInfo.FieldName, obj);
                        }
                    }
                }

                string fields = "";
                string values = "";
                strsql = "insert into {0} ({1}) values({2})";

                if (IsJoinWorkId(tableAttribute.IsGB))
                {
                    dicsql.Add("WorkId", Db.WorkId);
                }

                foreach (KeyValuePair<string, object> val in dicsql)
                {
                    fields += (fields == "" ? "" : ",") + val.Key;
                    values += (values == "" ? "" : ",") + ConvertDBValue(val.Value);
                }

                return string.Format(strsql, tableAttribute.TableName, fields, values);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message + "SQL:" + strsql);
            }
        }

        public override string GetUpdateSQL(object model)
        {
            string strsql = "";
            string where = "";
            try
            {
                Dictionary<string, object> dicsql = new Dictionary<string, object>();


                TableAttributeInfo tableAttribute = GetTableAttributeInfo(model);
                List<ColumnAttributeInfo> columnAttributeCollection = tableAttribute.ColumnAttributeInfoList;

                for (int i = 0; i < columnAttributeCollection.Count; i++)
                {

                    ColumnAttributeInfo columnAttributeInfo = columnAttributeCollection[i];

                    if (columnAttributeInfo.DataKey == false)
                    {
                        object obj = GetEntityValue(columnAttributeInfo.PropertyName, model);
                        dicsql.Add(columnAttributeInfo.FieldName, obj);
                    }

                    if (columnAttributeInfo.DataKey == true)
                    {
                        object obj = GetEntityValue(columnAttributeInfo.PropertyName, model);
                        obj = obj == null ? "" : obj;
                        where = columnAttributeInfo.FieldName + "=" + ConvertDBValue(obj);
                    }
                }

                string field_values = "";

                strsql = "update  {0} set {1} where {2}";

                foreach (KeyValuePair<string, object> val in dicsql)
                {
                    field_values += (field_values == "" ? "" : ",") + val.Key + "=" + ConvertDBValue(val.Value);
                }

                return string.Format(strsql, tableAttribute.TableName, field_values, where);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message + "SQL:" + strsql);
            }
        }

        public override string GetDeleteSQL<T>(object key)
        {
            return GetDeleteSQL(typeof(T), key);
        }

        public override string GetDeleteSQL(Type type, object key)
        {
            string strsql = "";
            string where = "";
            try
            {
                
                TableAttributeInfo tableAttribute = GetTableAttributeInfo(type);
                List<ColumnAttributeInfo> columnAttributeCollection = tableAttribute.ColumnAttributeInfoList;

                for (int i = 0; i < columnAttributeCollection.Count; i++)
                {

                    ColumnAttributeInfo columnAttributeInfo = columnAttributeCollection[i];
                    if (columnAttributeInfo.DataKey == true)
                    {
                        object obj = key;
                        where = columnAttributeInfo.FieldName + "=" + ConvertDBValue(obj);
                    }
                }

                strsql = "delete from  {0}  where {1}";

                return string.Format(strsql, tableAttribute.TableName, where);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message + "SQL:" + strsql);
            }
        }

        public override string GetEntitySQL<T>(object key)
        {
            return GetEntitySQL(typeof(T), key);
        }

        public override string GetEntitySQL(Type type, object key)
        {
            string strsql = "";
            string fields = "";
            string where = "";
            try
            {

                TableAttributeInfo tableAttribute = GetTableAttributeInfo(type);
                List<ColumnAttributeInfo> columnAttributeCollection = tableAttribute.ColumnAttributeInfoList;

                for (int i = 0; i < columnAttributeCollection.Count; i++)
                {

                    ColumnAttributeInfo columnAttributeInfo = columnAttributeCollection[i];

                    fields += (fields == "" ? "" : ",") + columnAttributeInfo.FieldName + " as " + columnAttributeInfo.PropertyName;

                    if (columnAttributeInfo.DataKey == true)
                    {
                        object obj = key;
                        where = columnAttributeInfo.FieldName + "=" + ConvertDBValue(obj);
                    }
                }


                strsql = "select {0} from {1} where {2}";

                return string.Format(strsql, fields, tableAttribute.TableName, where);
            }
            catch (Exception err)
            {
                throw new Exception(err.Message + "SQL:" + strsql);
            }
        }

        public override string GetListSQL<T>()
        {
            return GetListSQL(typeof(T), null, null);
        }

        public override string GetListSQL<T>(string strWhere)
        {
            return GetListSQL(typeof(T), strWhere, null);
        }

        public override string GetListSQL<T>(string strWhere, EFWCoreLib.CoreFrame.DbProvider.SqlPagination.PageInfo pageInfo)
        {
            return GetListSQL(typeof(T), strWhere, pageInfo);
        }

        public override string GetListSQL(Type type, string strWhere)
        {
            return GetListSQL(type, strWhere, null);
        }

        public override string GetListSQL(Type type, string strWhere, EFWCoreLib.CoreFrame.DbProvider.SqlPagination.PageInfo pageInfo)
        {
            string strsql = "";
            string fields = "";
            string where = "";
            try
            {

                TableAttributeInfo tableAttribute = GetTableAttributeInfo(type);
                List<ColumnAttributeInfo> columnAttributeCollection = tableAttribute.ColumnAttributeInfoList;

                for (int i = 0; i < columnAttributeCollection.Count; i++)
                {
                    ColumnAttributeInfo columnAttributeInfo = columnAttributeCollection[i];
                    fields += (fields == "" ? "" : ",") + columnAttributeInfo.FieldName + " as " + columnAttributeInfo.PropertyName;
                }

                where = JoinWhere(tableAttribute.IsGB, strWhere);
                strsql = "select {0} from {1} {2}";
                strsql = string.Format(strsql, fields, tableAttribute.TableName + " as T1", where);

                if (pageInfo != null)
                {
                    if (pageInfo.KeyName == null)
                        pageInfo.KeyName = tableAttribute.DataKeyFieldName;
                    strsql = EFWCoreLib.CoreFrame.DbProvider.SqlPagination.SqlPage.FormatSql(strsql, pageInfo, Db);
                }
                return strsql;
            }
            catch (Exception err)
            {
                throw new Exception(err.Message + "SQL:" + strsql);
            }
        }

       
    }
}

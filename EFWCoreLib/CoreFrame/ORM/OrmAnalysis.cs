
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using EFWCoreLib.CoreFrame.DbProvider;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.CoreFrame.Init.AttributeManager;

namespace EFWCoreLib.CoreFrame.Orm
{
    /// <summary>
    /// ORM映射关系解析基类
    /// </summary>
    abstract public class OrmAnalysis
    {

        private AbstractDatabase _Db;
        /// <summary>
        /// 数据库对象
        /// </summary>
        public AbstractDatabase Db
        {
            get
            {
                return _Db;
            }
            set
            {
                _Db = value;
            }
        }

        public List<EntityAttributeInfo> entityAttrList { get; set; }

        private string _alias;
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        protected string JoinWhere(bool isgb, string strWhere)
        {
            if (IsJoinWorkId(isgb))
            {
                strWhere = "WorkId = " + Db.WorkId + (string.IsNullOrEmpty(strWhere) ? " " : " and " + strWhere);
            }

            string where = string.IsNullOrEmpty(strWhere) ? "" : ("where " + strWhere);
            return where;
        }

        protected bool IsJoinWorkId(bool isgb)
        {
            if (AppGlobal.IsSaas == true && isgb == false && Db.WorkId > 0)
            {
                return true;
            }

            return false;
        }


        #region 解析实体属性
        protected TableAttributeInfo GetTableAttributeInfo(Type type)
        {
            EntityAttributeInfo EAttr = entityAttrList.Find(x => x.ObjType.Equals(type));
            if (EAttr == null) throw new Exception("此对象没有配置实体自定义属性");
            TableAttributeInfo tableAttrInfo = EAttr.TableAttributeInfoList.Find(x => x.Alias == Alias);
            //if (tableAttrInfo) throw new Exception("找不到相同别名的表自定义属性");
            return tableAttrInfo;
        }

        protected TableAttributeInfo GetTableAttributeInfo(object model)
        {
            return GetTableAttributeInfo(model.GetType());
        }

        protected object GetEntityValue(string propertyName, object model)
        {
            object data = model.GetType().GetProperty(propertyName).GetValue(model, null);
            if (model.GetType().GetProperty(propertyName).PropertyType.FullName == "System.DateTime" && Convert.ToDateTime(data) == default(DateTime))
            {
                data = Convert.ToDateTime("1900/01/01 00:00:00");
            }

            return data;
        }

        public void SetEntityValue(string propertyName, object model, object value)
        {
            PropertyInfo property = model.GetType().GetProperty(propertyName);
            property.SetValue(model, ConvertValue(property.PropertyType.FullName, value), null);
        }

        protected object ConvertValue(string PropertyType, object value)
        {
            if (value.GetType().FullName == "System.Guid")
            {
                return value.ToString();
            }

            switch (PropertyType)
            {
                case "System.DBNull":
                    return null;
                case "System.Int32":
                    value = value == DBNull.Value ? 0 : value;
                    value = value == null ? 0 : value;
                    value = value.ToString().Trim() == "" ? 0 : value;
                    return Convert.ToInt32(value);
                case "System.Int64":
                    value = value == DBNull.Value ? 0 : value;
                    value = value == null ? 0 : value;
                    value = value.ToString().Trim() == "" ? 0 : value;
                    return Convert.ToInt64(value);
                case "System.Decimal":
                    value = value == DBNull.Value ? 0 : value;
                    value = value == null ? 0 : value;
                    value = value.ToString().Trim() == "" ? 0 : value;
                    return Convert.ToDecimal(value);
                case "System.DateTime":
                    value = value == DBNull.Value ? new DateTime() : value;
                    value = value == null ? new DateTime() : value;
                    value = value.ToString().Trim() == "" ? new DateTime() : value;
                    return Convert.ToDateTime(value);
            }


            value = value == DBNull.Value ? null : value;
            return value;
        }

        protected string ConvertDBValue(object value)
        {
            if(value==null) return "''";
            
            string PropertyType = value.GetType().FullName;
            switch (PropertyType)
            {
                case "System.String":
                    return "'" + value.ToString() + "'";
                case "System.DateTime":
                    return "'" + value.ToString() + "'";
                case  "System.Guid":
                    return "'" + value.ToString() + "'";
                case "System.Boolean":
                    return "'" + value.ToString() + "'";
            }

            return value.ToString();
        }

        public object GetEntityDataKeyValue(object model)
        {
            TableAttributeInfo tableAttribute = GetTableAttributeInfo(model);
            return GetEntityValue(tableAttribute.DataKeyPropertyName, model);
        }

        public string GetEntityDataKeyPropertyName(object model)
        {
            TableAttributeInfo tableAttribute = GetTableAttributeInfo(model);
            return tableAttribute.DataKeyPropertyName;
        }
        #endregion

        public virtual string GetInsertSQL(object model)
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

        public virtual string GetUpdateSQL(object model)
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

        public virtual string GetDeleteSQL<T>(object key)
        {
            return GetDeleteSQL(typeof(T), key);
        }

        public virtual string GetDeleteSQL(Type type, object key)
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

        public virtual string GetEntitySQL<T>(object key)
        {
            return GetEntitySQL(typeof(T), key);
        }

        public virtual string GetEntitySQL(Type type, object key)
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

        public virtual string GetListSQL<T>()
        {
            return GetListSQL(typeof(T), null, null);
        }

        public virtual string GetListSQL<T>(string strWhere)
        {
            return GetListSQL(typeof(T), strWhere, null);
        }

        public virtual string GetListSQL<T>(string strWhere, EFWCoreLib.CoreFrame.DbProvider.SqlPagination.PageInfo pageInfo)
        {
            return GetListSQL(typeof(T), strWhere, pageInfo);
        }

        public virtual string GetListSQL(Type type, string strWhere)
        {
            return GetListSQL(type, strWhere, null);
        }

        public virtual string GetListSQL(Type type, string strWhere, EFWCoreLib.CoreFrame.DbProvider.SqlPagination.PageInfo pageInfo)
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

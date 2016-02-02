using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using EFWCoreLib.CoreFrame.Business.Interface;
using EFWCoreLib.CoreFrame.DbProvider.SqlPagination;
using EFWCoreLib.CoreFrame.Orm;
using EFWCoreLib.CoreFrame.Orm.Interface;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using EFWCoreLib.CoreFrame.Init.AttributeManager;

namespace EFWCoreLib.CoreFrame.Business
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public abstract class AbstractEntity:AbstractBusines,IORM
    {
        #region IORM 成员
       
        protected object ConvertValue(string PropertyType, object value)
        {
            if (value == null) return null;

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
 		protected void SetEntityValue(string propertyName, object model, object value)
        {
            PropertyInfo property = model.GetType().GetProperty(propertyName);
            property.SetValue(model, ConvertValue(property.PropertyType.FullName, value), null);
        }
        private object ToObject(System.Data.IDataReader dataReader, object _obj, string alias)
        {
            Type type = _obj.GetType();
            object obj = ((ICloneable)_obj).Clone();
            System.Collections.Hashtable filedValue = new System.Collections.Hashtable();
            for (int index = 0; index < dataReader.FieldCount; index++)
            {
                filedValue.Add(dataReader.GetName(index), dataReader[index]);
            }
            foreach (System.Reflection.PropertyInfo property in type.GetProperties())
            {
                SetEntityValue(property.Name, obj, filedValue[property.Name]);
            }
            ((IbindDb)obj).BindDb(_oleDb, _container,_cache,_pluginName);
            return obj;
        }

        public int save() { return save(null); }
        public int save(string alias)
        {
            OrmAnalysis ormAnalysis;

            ormAnalysis = OrmAnalysisFactory.CreateOrmAnalysisObject(_oleDb.DbType);
            ormAnalysis.Alias = alias;
            ormAnalysis.Db = _oleDb;
            ormAnalysis.entityAttrList = (List<EntityAttributeInfo>)EntityManager.GetAttributeInfo(_cache,_pluginName);
            object keyVal = ormAnalysis.GetEntityDataKeyValue(this);
			//?Match="Custom:Guid"
            if (keyVal == null || (keyVal.GetType().Equals(typeof(int)) && Convert.ToInt32(keyVal) == 0))
            {

                string strsql = ormAnalysis.GetInsertSQL(this);
                int ret = 0;
                ret = _oleDb.InsertRecord(strsql);
                ormAnalysis.SetEntityValue(ormAnalysis.GetEntityDataKeyPropertyName(this), this, ret);

                return ret;
            }
            else
            {
                string strsql = ormAnalysis.GetUpdateSQL(this);
                return _oleDb.DoCommand(strsql);
            }
        }

        public int delete() { return delete(null, null); }
        public int delete(object key) { return delete(key, null); }
        public int delete(object key, string alias)
        {
            OrmAnalysis ormAnalysis;

            ormAnalysis = OrmAnalysisFactory.CreateOrmAnalysisObject(_oleDb.DbType);
            ormAnalysis.Alias = alias;
            ormAnalysis.Db = _oleDb;
            ormAnalysis.entityAttrList = (List<EntityAttributeInfo>)EntityManager.GetAttributeInfo(_cache, _pluginName);
            object keyVal = key == null ? ormAnalysis.GetEntityDataKeyValue(this) : key;
            string strsql = ormAnalysis.GetDeleteSQL(this.GetType(), keyVal);
            return _oleDb.DoCommand(strsql);
        }

        public object getmodel() { return getmodel(null, null); }
        public object getmodel(object key) { return getmodel(key, null); }
        public object getmodel(object key, string alias)
        {
            OrmAnalysis ormAnalysis;

            ormAnalysis = OrmAnalysisFactory.CreateOrmAnalysisObject(_oleDb.DbType);
            ormAnalysis.Alias = alias;
            ormAnalysis.Db = _oleDb;
            ormAnalysis.entityAttrList = (List<EntityAttributeInfo>)EntityManager.GetAttributeInfo(_cache, _pluginName);
            object value = null;
            object keyVal = key == null ? ormAnalysis.GetEntityDataKeyValue(this) : key;

            string strsql = ormAnalysis.GetEntitySQL(this.GetType(), keyVal);
            System.Data.IDataReader result = _oleDb.GetDataReader(strsql);

            if (result.Read())
            {
                value = ToObject(result, this, alias);

            }
            result.Close();
            result.Dispose();

            return value;
        }
        public List<T> getlist<T>()
        {
            return getlist<T>(null, null, null);
        }
        public List<T> getlist<T>(string where) { return getlist<T>(null, where, null); }
        public List<T> getlist<T>(PageInfo pageInfo, string where) { return getlist<T>(pageInfo, where, null); }
        public List<T> getlist<T>(PageInfo pageInfo, string where, string alias)
        {
            OrmAnalysis ormAnalysis;
            ormAnalysis = OrmAnalysisFactory.CreateOrmAnalysisObject(_oleDb.DbType);
            ormAnalysis.Alias = alias;
            ormAnalysis.Db = _oleDb;
            ormAnalysis.entityAttrList = (List<EntityAttributeInfo>)EntityManager.GetAttributeInfo(_cache, _pluginName);

            string strsql = ormAnalysis.GetListSQL(this.GetType(), where, pageInfo);

            IDataReader result = _oleDb.GetDataReader(strsql);
            List<T> resultList = new List<T>();
            while (result.Read())
            {
                resultList.Add((T)ToObject(result, this, alias));
            }
            result.Close();
            result.Dispose();
            return resultList;
        }
        public DataTable gettable()
        {
            return gettable(null, null, null);
        }
        public DataTable gettable(string where) { return gettable(null, where, null); }
        public DataTable gettable(PageInfo pageInfo, string where) { return gettable(pageInfo, where,null); }
        public DataTable gettable(PageInfo pageInfo, string where, string alias)
        {
            OrmAnalysis ormAnalysis;
            ormAnalysis = OrmAnalysisFactory.CreateOrmAnalysisObject(_oleDb.DbType);
            ormAnalysis.Alias = alias;
            ormAnalysis.Db = _oleDb;
            ormAnalysis.entityAttrList = (List<EntityAttributeInfo>)EntityManager.GetAttributeInfo(_cache, _pluginName);
            string strsql = ormAnalysis.GetListSQL(this.GetType(), where, pageInfo);

            return _oleDb.GetDataTable(strsql);
        }

        #endregion
    }


}

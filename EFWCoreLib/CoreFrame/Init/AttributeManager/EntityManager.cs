using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.Reflection;
using EFWCoreLib.CoreFrame.Orm;

namespace EFWCoreLib.CoreFrame.Init.AttributeManager
{

    /// <summary>
    /// 实体自定配置管理类
    /// </summary>
    public class EntityManager 
    {
        private static string GetCacheKey()
        {
            return "entityAttributeList";
        }

        /// <summary>
        /// 加载自定义标签
        /// </summary>
        /// <param name="BusinessDll">Dll路径</param>
        /// <param name="cache">存入缓存</param>
        public static void LoadAttribute(List<string> BusinessDll, ICacheManager cache,string pluginName)
        {
            string cacheKey = pluginName+"@"+GetCacheKey();

            List<EntityAttributeInfo> entityAttributeList = new List<EntityAttributeInfo>();

            for (int k = 0; k < BusinessDll.Count; k++)
            {

                System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(BusinessDll[k]);
                Type[] types = assembly.GetTypes();
                for (int i = 0; i < types.Length; i++)
                {
                    TableAttribute[] tableAttrs = ((TableAttribute[])types[i].GetCustomAttributes(typeof(TableAttribute), true));

                    if (tableAttrs.Length > 0)
                    {
                        EntityAttributeInfo eattr = new EntityAttributeInfo();
                        eattr.ObjType = types[i];
                        eattr.TableAttributeInfoList = new List<TableAttributeInfo>();
                        foreach (TableAttribute val in tableAttrs)
                        {
                            TableAttributeInfo tableattr = new TableAttributeInfo();
                            tableattr.Alias = val.Alias;
                            tableattr.EType = val.EntityType;
                            tableattr.TableName = val.TableName;
                            tableattr.IsGB = val.IsGB;
                            tableattr.ColumnAttributeInfoList = new List<ColumnAttributeInfo>();

                            PropertyInfo[] property = eattr.ObjType.GetProperties();
                            for (int n = 0; n < property.Length; n++)
                            {
                                ColumnAttribute[] columnAttributes = (ColumnAttribute[])property[n].GetCustomAttributes(typeof(ColumnAttribute), true);
                                if (columnAttributes.Length > 0)
                                {
                                    ColumnAttributeInfo colattr = new ColumnAttributeInfo();
                                    ColumnAttribute colval = columnAttributes.ToList().Find(x => x.Alias == tableattr.Alias);
                                    if (colval == null) throw new Exception("输入的Alias别名不正确");
                                    colattr.Alias = colval.Alias;
                                    colattr.DataKey = colval.DataKey;
                                    colattr.FieldName = colval.FieldName;
                                    colattr.IsInsert = colval.IsInsert;
                                    //colattr.IsSingleQuote=colval.is
                                    colattr.Match = colval.Match;
                                    colattr.PropertyName = property[n].Name;

                                    if (colattr.DataKey)
                                    {
                                        tableattr.DataKeyFieldName = colattr.FieldName;//设置TableAttributeInfo的主键字段名
                                        tableattr.DataKeyPropertyName = colattr.PropertyName;
                                    }
                                    tableattr.ColumnAttributeInfoList.Add(colattr);
                                }
                            }

                            eattr.TableAttributeInfoList.Add(tableattr);
                        }


                        entityAttributeList.Add(eattr);
                    }
                }
            }

            cache.Add(cacheKey, entityAttributeList);
        }

        public static Object GetAttributeInfo(ICacheManager cache, string pluginName)
        {
            return cache.GetData(pluginName+"@"+GetCacheKey());
        }

        public static void ClearAttributeData(ICacheManager cache, string pluginName)
        {
            cache.Remove(pluginName+"@"+GetCacheKey());
        }
    }

    public class EntityAttributeInfo
    {
        public Type ObjType { get; set; }
        public List<TableAttributeInfo> TableAttributeInfoList { get; set; }
    }

    public class TableAttributeInfo
    {
        public string Alias;
        public string TableName;
        public EntityType EType = EntityType.Table;
        public bool IsGB;
        public string DataKeyPropertyName;
        public string DataKeyFieldName;
        public List<ColumnAttributeInfo> ColumnAttributeInfoList;
    }
    /// <summary>
    /// 列属性信息
    /// </summary>
    public class ColumnAttributeInfo
    {
        /// <summary>
        /// 别名
        /// </summary>
        public string Alias;
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName;
        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName;
        /// <summary>
        /// 是否为主键
        /// </summary>
        public bool DataKey;
        /// <summary>
        /// 字段类型带不带单引号
        /// </summary>
        public bool IsSingleQuote;
        /// <summary>
        /// 值的匹配条件
        /// </summary>
        public string Match;
        /// <summary>
        /// 是否Add到库
        /// </summary>
        public bool IsInsert;
    }
}

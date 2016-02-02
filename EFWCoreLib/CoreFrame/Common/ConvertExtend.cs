//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Data;
using EFWCoreLib.CoreFrame.DbProvider;
using Microsoft.Practices.Unity;
using EFWCoreLib.CoreFrame.Business;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace EFWCoreLib.CoreFrame.Common
{
    /// <summary>
    /// Convert 的摘要说明。
    /// </summary>
    public class ConvertExtend
    {
        /// <summary>
        /// 将Null值转换为指定值
        /// </summary>
        /// <param name="obj">待判断的值</param>
        /// <param name="nValue">指定值</param>
        /// <returns></returns>
        public static string IsNull(object obj, string nValue)
        {
            try
            {
                return Convert.ToString(obj).Trim() == "" ? nValue : obj.ToString().Trim();
            }
            catch (System.InvalidCastException err)
            {
                err.ToString();
                return "";
            }
            catch (System.Exception err)
            {
                err.ToString();
                return "";
            }
        }

        /// <summary>
        ///判断输入字符串是否为数字
        /// </summary>
        /// <param name="nValue">字符串</param>
        /// <returns></returns>
        public static bool IsNumeric(string nValue)
        {
            int i, iAsc, idecimal = 0;
            if (nValue.Trim() == "") return false;
            for (i = 0; i <= nValue.Length - 1; i++)
            {
                iAsc = (int)Convert.ToChar(nValue.Substring(i, 1));
                //'-'45 '.'46 '''0-9' 48-57
                if (iAsc == 45)
                {
                    if (nValue.Length == 1)//不能只有一个负号
                    {
                        return false;
                    }
                    if (i != 0)			//'-'不能在字符串中间
                    {
                        return false;
                    }
                }
                else if (iAsc == 46)
                {
                    idecimal++;
                    if (idecimal > 1)		//如果有两个以上的小数点
                        return false;
                }
                else if (iAsc < 48 || iAsc > 57)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///判断输入字符串是否为整数
        /// </summary>
        /// <param name="nValue">字符串</param>
        /// <returns></returns>
        public static bool IsInteger(string nValue)
        {
            int i, iAsc;
            if (nValue.Trim() == "") return false;
            for (i = 0; i <= nValue.Length - 1; i++)
            {
                iAsc = (int)Convert.ToChar(nValue.Substring(i, 1));
                //'-' 45 '0-9' 48-57
                if (iAsc == 45)
                {
                    if (nValue.Length == 1)//不能只有一个负号
                    {
                        return false;
                    }
                    if (i != 0)			//'-'不能在字符串中间
                    {
                        return false;
                    }
                }
                else if (iAsc < 48 || iAsc > 57)
                {
                    return false;
                }
            }
            return true;
        }

        public static string ToPassWord(string text)
        {
            DESEncryptor des = new DESEncryptor();
            des.InputString = text;
            des.DesEncrypt();
            return des.OutString;
        }


        public static List<T> ToList<T>(DataTable table, AbstractDatabase Db, IUnityContainer container, ICacheManager cache, string pluginName, string unityname)
        {
            Type type = typeof(T);
            List<T> objects = new List<T>();
            T obj = FactoryModel.GetObject<T>(Db, container,cache,pluginName, unityname);

            if (table != null && table.Rows.Count > 0)
            {
                while (objects.Count < table.Rows.Count)
                {
                    objects.Add((T)((ICloneable)obj).Clone());
                }

                foreach (PropertyInfo property in type.GetProperties())
                {

                    if (table.Columns.IndexOf(property.Name) >= 0)
                    {
                        for (int index = 0; index < table.Rows.Count; index++)
                        {
                            object val = table.Rows[index][property.Name];
                            if (val == System.DBNull.Value) val = null;
                            property.SetValue((object)objects[index], val, null);
                        }
                    }
                }
            }
            return objects;
        }

        public static List<T> ToList<T>(DataTable table)
        {
            List<T> list = new List<T>();
            T obj = (T)System.Activator.CreateInstance(typeof(T));

            //列名
            string columnName;
            //属性名
            string propertyName;
            //列数量
            int column = table.Columns.Count;
            //属性数量
            int propertyNum = obj.GetType().GetProperties().Length;

            for (int m = 0; m < table.Rows.Count; m++)
            {
                T newobj = (T)((ICloneable)obj).Clone();

                //遍历所有列
                for (int i = 0; i < column; i++)
                {
                    //遍历所有属性
                    for (int j = 0; j < propertyNum; j++)
                    {
                        columnName = table.Columns[i].ColumnName.ToUpper();
                        propertyName = newobj.GetType().GetProperties()[j].Name.ToUpper();
                        if (columnName == propertyName)
                        {
                            string fullName = table.Rows[m][columnName].GetType().FullName;
                            object objectValue = table.Rows[m][i];
                            //如果datatable中的对应项是空类型
                            if (fullName == "System.DBNull")
                            {
                                newobj.GetType().GetProperties()[j].SetValue(newobj, null, null);
                            }
                            else
                            {
                                newobj.GetType().GetProperties()[j].SetValue(newobj, objectValue, null);
                            }
                        }
                    }
                }

                list.Add(newobj);

            }

            return list;
        }


        //public static DataTable ToDataTable<T>(IList<T> ilist)
        //{
        //    Type type = typeof(T);
        //    DataTable result = new DataTable();
        //    PropertyInfo[] propertys = type.GetProperties();
        //    foreach (PropertyInfo property in propertys)
        //    {
        //        result.Columns.Add(property.Name, property.PropertyType);
        //    }

        //    for (int i = 0; i < ilist.Count; i++)
        //    {
        //        ArrayList tempList = new ArrayList();
        //        foreach (PropertyInfo property in propertys)
        //        {
        //            tempList.Add(property.GetValue(ilist[i], null));
        //        }
        //        object[] array = tempList.ToArray();
        //        result.LoadDataRow(array, true);
        //    }
        //    return result;
        //}

        public static DataTable ToDataTable(IList list)
        {
            DataTable result = new DataTable();

            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        public static T ToObject<T>(DataTable dt, int Rowindex, AbstractDatabase Db, IUnityContainer container, ICacheManager cache, string pluginName, string unityname)
        {
            T obj = FactoryModel.GetObject<T>(Db, container, cache, pluginName, unityname);

            if (Rowindex >= dt.Rows.Count)
            {
                return obj;
            }
            //列名
            string columnName;
            //属性名
            string propertyName;
            //列数量
            int column = dt.Columns.Count;
            //属性数量
            int propertyNum = obj.GetType().GetProperties().Length;
            //遍历所有列
            for (int i = 0; i < column; i++)
            {
                //遍历所有属性
                for (int j = 0; j < propertyNum; j++)
                {
                    columnName = dt.Columns[i].ColumnName.ToUpper();
                    propertyName = obj.GetType().GetProperties()[j].Name.ToUpper();
                    if (columnName == propertyName)
                    {
                        string fullName = dt.Rows[Rowindex][columnName].GetType().FullName;
                        object objectValue = dt.Rows[Rowindex][i];
                        //如果datatable中的对应项是空类型
                        if (fullName == "System.DBNull")
                        {
                            obj.GetType().GetProperties()[j].SetValue(obj, null, null);
                        }
                        else
                        {
                            obj.GetType().GetProperties()[j].SetValue(obj, objectValue, null);
                        }
                    }
                }
            }
            return obj;
        }
        public static T ToObject<T>(DataTable dt, int Rowindex)
        {
            T obj = (T)System.Activator.CreateInstance(typeof(T));

            if (Rowindex >= dt.Rows.Count)
            {
                return obj;
            }
            //列名
            string columnName;
            //属性名
            string propertyName;
            //列数量
            int column = dt.Columns.Count;
            //属性数量
            int propertyNum = obj.GetType().GetProperties().Length;
            //遍历所有列
            for (int i = 0; i < column; i++)
            {
                //遍历所有属性
                for (int j = 0; j < propertyNum; j++)
                {
                    columnName = dt.Columns[i].ColumnName.ToUpper();
                    propertyName = obj.GetType().GetProperties()[j].Name.ToUpper();
                    if (columnName == propertyName)
                    {
                        string fullName = dt.Rows[Rowindex][columnName].GetType().FullName;
                        object objectValue = dt.Rows[Rowindex][i];
                        //如果datatable中的对应项是空类型
                        if (fullName == "System.DBNull")
                        {
                            obj.GetType().GetProperties()[j].SetValue(obj, null, null);
                        }
                        else
                        {
                            obj.GetType().GetProperties()[j].SetValue(obj, objectValue, null);
                        }
                    }
                }
            }
            return obj;
        }

        public static T ToObject<T>(Object in_obj)
        {
            T out_obj = (T)System.Activator.CreateInstance(typeof(T));

            int in_propertyNum = in_obj.GetType().GetProperties().Length;
            int out_propertyNum = out_obj.GetType().GetProperties().Length;
            string in_propertyName, out_propertyName;
            for (int i = 0; i < in_propertyNum; i++)
            {
                for (int j = 0; j < out_propertyNum; j++)
                {
                    in_propertyName = in_obj.GetType().GetProperties()[i].Name;
                    out_propertyName = out_obj.GetType().GetProperties()[j].Name;

                    if (in_propertyName == out_propertyName)
                    {
                        object obj = in_obj.GetType().GetProperties()[i].GetValue(in_obj, null);
                        out_obj.GetType().GetProperties()[j].SetValue(out_obj, obj, null);
                    }
                }
            }

            return out_obj;
        }

        public static string UrlAddParams(string httpurl, string paramName, string paramValue)
        {
            if (httpurl.IndexOf('?') == -1)
            {
                httpurl += "?" + paramName + "=" + paramValue;
            }
            else
            {
                httpurl += "&" + paramName + "=" + paramValue;
            }
            return httpurl;
        }

    }
}

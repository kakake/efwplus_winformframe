using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace EFWCoreLib.WcfFrame.ServerController
{
    /// <summary>
    /// 基于Json格式的WCF服务基类
    /// </summary>
    public class JsonWcfServerController : WcfServerController
    {
        #region  值转换
        private object convertVal(Type t, object data)
        {
            object val = null;
            if (t == typeof(Int32))
                val = Convert.ToInt32(data);
            else if (t == typeof(DateTime))
                val = Convert.ToDateTime(data);
            else if (t == typeof(Decimal))
                val = Convert.ToDecimal(data);
            else if (t == typeof(Boolean))
                val = Convert.ToBoolean(data);
            else if (t == typeof(String))
                val = Convert.ToString(data).Trim();
            else if (t == typeof(Guid))
                val = new Guid(data.ToString());
            else if (t == typeof(byte[]))
                if (data != null && data.ToString().Length > 0)
                {
                    val = Convert.FromBase64String(data.ToString());
                }
                else
                {
                    val = null;
                }
            else
                val = data;
            return val;
        }
        #endregion

        #region IToJson 成员
        public string ToJson(object model)
        {
            string value = JavaScriptConvert.SerializeObject(model, new AspNetDateTimeConverter(), new AspNetBytesConverter());
            return value;
        }
        public string ToJson(System.Data.DataTable dt)
        {
            string value = JavaScriptConvert.SerializeObject(dt);
            return value;
        }
        public string ToJson(string data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(int data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(decimal data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(bool data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(DateTime data)
        {
            object[] objs = new object[] { data };
            return ToJson(objs);
        }
        public string ToJson(params object[] data)
        {
            string value = JavaScriptConvert.SerializeObject(data, new AspNetDateTimeConverter(),new AspNetBytesConverter());
            return value;
        }

        #endregion

        #region IJsonToObject成员
        public T ToObject<T>(string json)
        {
            return JavaScriptConvert.DeserializeObject<T>(json,new AspNetBytesConverter());
        }
        public object ToObject(string json)
        {
            return JavaScriptConvert.DeserializeObject(json);
        }
        public object[] ToArray(string json)
        {
            return (ToObject(json) as Newtonsoft.Json.JavaScriptArray).ToArray();
        }
        public string ToString(string json)
        {
            return Convert.ToString(ToArray(json)[0]);
        }
        public bool ToBoolean(string json)
        {
            return Convert.ToBoolean(ToArray(json)[0]);
        }
        public int ToInt32(string json)
        {
            return Convert.ToInt32(ToArray(json)[0]);
        }
        public decimal ToDecimal(string json)
        {
            return Convert.ToDecimal(ToArray(json)[0]);
        }
        public DateTime ToDateTime(string json)
        {
            return Convert.ToDateTime(ToArray(json)[0]);
        }

        public T ToObject<T>(object data)
        {
            if (typeof(T).Equals(typeof(int[])))
            {
                List<int> intvals = new List<int>();
                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    intvals.Add(Convert.ToInt32((data as JavaScriptArray)[i]));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(string[])))
            {
                List<string> intvals = new List<string>();
                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    intvals.Add(Convert.ToString((data as JavaScriptArray)[i]));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(decimal[])))
            {
                List<decimal> intvals = new List<decimal>();
                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    intvals.Add(Convert.ToDecimal((data as JavaScriptArray)[i]));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(Boolean[])))
            {
                List<Boolean> intvals = new List<Boolean>();
                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    intvals.Add(Convert.ToBoolean((data as JavaScriptArray)[i]));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(DateTime[])))
            {
                List<DateTime> intvals = new List<DateTime>();
                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    intvals.Add(Convert.ToDateTime((data as JavaScriptArray)[i]));
                }
                return (T)(intvals.ToArray() as object);
            }
            else if (typeof(T).Equals(typeof(object[])))
            {
                List<object> intvals = new List<object>();
                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    intvals.Add((data as JavaScriptArray)[i]);
                }
                return (T)(intvals.ToArray() as object);
            }
            else
            {
                T obj = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] pros = typeof(T).GetProperties();
                for (int k = 0; k < pros.Length; k++)
                {
                    object val = convertVal(pros[k].PropertyType, (data as JavaScriptObject)[pros[k].Name]);
                    pros[k].SetValue(obj, val, null);
                }
                return obj;
            }
        }
        public object[] ToArray(object data)
        {
            return (data as Newtonsoft.Json.JavaScriptArray).ToArray();
        }
        public List<T> ToListObj<T>(object data)
        {
            if (data is JavaScriptArray)
            {
                PropertyInfo[] pros = typeof(T).GetProperties();
                List<T> list = new List<T>();
                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    T obj = (T)Activator.CreateInstance(typeof(T));
                    object _data = (data as JavaScriptArray)[i];
                    for (int k = 0; k < pros.Length; k++)
                    {
                        object val = convertVal(pros[k].PropertyType, (_data as JavaScriptObject)[pros[k].Name]);
                        pros[k].SetValue(obj, val, null);
                    }
                    list.Add(obj);
                }
                return list;
            }

            return null;
        }
        public DataTable ToDataTable(object data)
        {
            if (data is JavaScriptArray && (data as JavaScriptArray).Count > 0)
            {
                JavaScriptObject _data = (data as JavaScriptArray)[0] as JavaScriptObject;
                DataTable dt = new DataTable();
                foreach (var name in _data.Keys)
                {
                    dt.Columns.Add(name, _data[name].GetType());
                }

                for (int i = 0; i < (data as JavaScriptArray).Count; i++)
                {
                    object _jsarray = (data as JavaScriptArray)[i];
                    DataRow dr = dt.NewRow();
                    for (int k = 0; k < dt.Columns.Count; k++)
                    {
                        dr[k] = convertVal(dt.Columns[k].DataType, (_jsarray as JavaScriptObject)[dt.Columns[k].ColumnName]);
                    }
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            return null;
        }
        public DataTable ToDataTable(string data)
        {
            return ToDataTable(JavaScriptConvert.DeserializeObject(data));
        }
        #endregion
    }

    public class AspNetDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            DateTime datetime = Convert.ToDateTime(value);
            writer.WriteValue(datetime.ToString());
        }
    }
    public class AspNetBytesConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(byte[]).IsAssignableFrom(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value)
        {
            string sBytes = "null";
            if (value != null)
            {
                sBytes = Convert.ToBase64String((byte[])value);
            }
            writer.WriteValue(sBytes);
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            byte[] sBytes =null;
            if (reader.Value!=null && reader.Value.ToString().Length>= 10)
            {
                sBytes = Convert.FromBase64String(reader.Value.ToString());
            }  
            return sBytes;
        }
    }
}

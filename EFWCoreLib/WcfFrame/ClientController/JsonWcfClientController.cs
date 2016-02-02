using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Reflection;
using System.Data;

namespace EFWCoreLib.WcfFrame.ClientController
{
    public class JsonWcfClientController : WcfClientController
    {
        #region ToJson

        public string ToJson(params object[] data)
        {
            string value = JavaScriptConvert.SerializeObject(data, new AspNetDateTimeConverter(), new AspNetBytesConverter());
            return value;
        }
        public string ToJson(object model)
        {
            string value = JavaScriptConvert.SerializeObject(model, new AspNetDateTimeConverter(),new AspNetBytesConverter());
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
        #endregion

        #region ToObject
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
        public T ToObject<T>(object data)
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
        public T ToObject<T>(string data)
        {
            return JavaScriptConvert.DeserializeObject<T>(data);
        }
        public string ToString(object data)
        {
            return Convert.ToString(ToArray(data)[0]);
        }
        public bool ToBoolean(object data)
        {
            return Convert.ToBoolean(ToArray(data)[0]);
        }
        public int ToInt32(object data)
        {
            return Convert.ToInt32(ToArray(data)[0]);
        }
        public decimal ToDecimal(object data)
        {
            return Convert.ToDecimal(ToArray(data)[0]);
        }
        public DateTime ToDateTime(object data)
        {
            return Convert.ToDateTime(ToArray(data)[0]);
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
            byte[] sBytes = null;
            if (reader.Value != null && reader.Value.ToString().Length >= 10)
            {
                sBytes = Convert.FromBase64String(reader.Value.ToString());
            }

            return sBytes;
        }
    }
}

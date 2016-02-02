using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;

using Newtonsoft.Json;

namespace EFWCoreLib.WebFrame.HttpHandler.Controller
{
    /// <summary>
    /// 基于JqueryEasyUI框架的Web控制器基类
    /// </summary>
    public abstract class JEasyUIController : WebHttpController, IToJqueryEasyUIJson
    {

        #region IController2 成员

        public string FilterJson(string json)
        {
            throw new NotImplementedException();
        }

        public string ToJson(object model)
        {
            string value = JavaScriptConvert.SerializeObject(model, new AspNetDateTimeConverter());
            return value;
        }

        public string ToJson(System.Data.DataTable dt)
        {
            string value = JavaScriptConvert.SerializeObject(dt);
            return value;
        }

        public string ToJson(System.Collections.Hashtable hash)
        {
            string value = JavaScriptConvert.SerializeObject(hash, new AspNetDateTimeConverter());
            return value;
        }

        public string ToGridJson(string rowsjson, int totalCount)
        {
            return ToGridJson(rowsjson, null, totalCount);
        }

        public string ToGridJson(string rowsjson, string footjson, int totalCount)
        {
            if (footjson == null)
                return "{\"total\":" + totalCount + ",\"rows\":" + rowsjson + "}";
            else
                return "{\"total\":" + totalCount + ",\"rows\":" + rowsjson + ",\"footer\":" + footjson + "}";
        }

        public string ToGridJson(System.Data.DataTable dt)
        {
            return ToGridJson(dt, -1, null);
        }

        public string ToGridJson(System.Data.DataTable dt, int totalCount)
        {
            return ToGridJson(dt, totalCount, null);
        }

        public string ToGridJson(System.Data.DataTable dt, int totalCount, System.Collections.Hashtable[] footers)
        {
            totalCount = totalCount == -1 ? dt.Rows.Count : totalCount;
            string rowsjson = ToJson(dt);
            string footjson = footers == null ? null : ToJson(footers);
            return ToGridJson(rowsjson, footjson, totalCount);
        }

        public string ToGridJson(System.Collections.IList list)
        {
            return ToGridJson(list, -1, null);
        }

        public string ToGridJson(System.Collections.IList list, int totalCount)
        {
            return ToGridJson(list, totalCount, null);
        }

        public string ToGridJson(System.Collections.IList list, int totalCount, System.Collections.Hashtable[] footers)
        {
            totalCount = totalCount == -1 ? list.Count : totalCount;
            string rowsjson = ToJson(list);
            string footjson = footers == null ? null : ToJson(footers);
            return ToGridJson(rowsjson, footjson, totalCount);

        }

        
        public string ToFloorJson(List<floorclass> floor)
        {
            return ToFloorJson(floor, floor.Count);
        }

        public string ToFloorJson(List<floorclass> floor, int totalCount)
        {
            string Json = "{\"total\":" + totalCount + ",\"rows\":[";
            string str = "";
            for (int i = 0; i < floor.Count; i++)
            {
                if (str == "")
                {
                    str += "{\"floorid\":" + floor[i].floorid + ",\"floortext\":\"" + floor[i].floortext + "\",\"room\":";
                }
                else
                {
                    str += ",{\"floorid\":" + floor[i].floorid + ",\"floortext\":\"" + floor[i].floortext + "\",\"room\":";
                }
                str += JavaScriptConvert.SerializeObject(floor[i].room);
                str += "}";
            }
            Json += str;
            Json += "]}";

            return Json;
        }

        public string ToTreeJson(List<treeNode> list)
        {
            JsonConverter converter = new AspNetDateTimeConverter();
            string value = JavaScriptConvert.SerializeObject(list, converter);
            value = value.Replace("check", "checked");
            return value;
        }

        public string ToTreeGridJson(List<treeNodeGrid> list)
        {
            return ToTreeGridJson(list, null);
        }

        public string ToTreeGridJson(List<treeNodeGrid> list, System.Collections.Hashtable[] footers)
        {
            List<Hashtable> hashlist = new List<Hashtable>();
            for (int i = 0; i < list.Count; i++)
            {
                Hashtable hash = new Hashtable();
                hash.Add("id", list[i].id);
                if (list[i]._parentId > 0)
                    hash.Add("_parentId", list[i]._parentId);
                if (!string.IsNullOrEmpty(list[i].state))
                    hash.Add("state", list[i].state);
                if (!string.IsNullOrEmpty(list[i].iconCls))
                    hash.Add("iconCls", list[i].iconCls);
                if (list[i].check)
                    hash.Add("check", list[i].check);
                if (list[i].model != null)
                {
                    PropertyInfo[] propertys = list[i].model.GetType().GetProperties();
                    for (int j = 0; j < propertys.Length; j++)
                    {
                        if (!hash.ContainsKey(propertys[j].Name))
                            hash.Add(propertys[j].Name, propertys[j].GetValue(list[i].model, null));
                    }
                }

                hashlist.Add(hash);
            }

            int totalCount = hashlist.Count;
            string rowsjson = ToJson(hashlist);
            string footjson = footers == null ? null : ToJson(footers);
            return ToTreeGridJson(rowsjson, footjson, totalCount);
        }

        public string ToTreeGridJson(System.Data.DataTable dt, string IdName, string _parentIdName)
        {
            return ToTreeGridJson(dt, IdName, _parentIdName, null);
        }

        public string ToTreeGridJson(System.Data.DataTable dt, string IdName, string _parentIdName, System.Collections.Hashtable[] footers)
        {
            List<Hashtable> hashlist = new List<Hashtable>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Hashtable hash = new Hashtable();
                hash.Add("id", dt.Rows[i][IdName]);
                if (Convert.ToInt32(dt.Rows[i][_parentIdName]) > 0)
                    hash.Add("_parentId", dt.Rows[i][_parentIdName]);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Columns[j].ColumnName.ToLower() == IdName.ToLower()) continue;
                    if (dt.Columns[j].ColumnName.ToLower() == _parentIdName.ToLower()) continue;

                    hash.Add(dt.Columns[j].ColumnName, dt.Rows[i][j]);
                }
                hashlist.Add(hash);
            }

            int totalCount = hashlist.Count;
            string rowsjson = ToJson(hashlist);
            string footjson = footers == null ? null : ToJson(footers);
            return ToTreeGridJson(rowsjson, footjson, totalCount);
        }

        public string ToTreeGridJson(string rowsjson, string footjson, int totalCount)
        {
            if (footjson == null)
                return "{\"total\":" + totalCount + ",\"rows\":" + rowsjson + "}";
            else
                return "{\"total\":" + totalCount + ",\"rows\":" + rowsjson + ",\"footer\":" + footjson + "}";
        }

        public T GetModel<T>()
        {
            T model = NewObject<T>();
            return GetModel<T>(model);
        }

        public T GetModel<T>(T model)
        {
            if (model == null)
            {
                model = GetModel<T>();
                return model;
            }
            System.Reflection.PropertyInfo[] propertys = model.GetType().GetProperties();
            for (int j = 0; j < propertys.Length; j++)
            {
                if (propertys[j].Name == "WorkId") break;
                if (FormData.ContainsKey(propertys[j].Name) == true)
                {
                    if (propertys[j].PropertyType.Equals(typeof(Int32)))
                        propertys[j].SetValue(model, Convert.ToInt32(FormData[propertys[j].Name].Trim() == "" ? "0" : FormData[propertys[j].Name]), null);
                    else if (propertys[j].PropertyType.Equals(typeof(Int64)))
                        propertys[j].SetValue(model, Convert.ToInt64(FormData[propertys[j].Name].Trim() == "" ? "0" : FormData[propertys[j].Name]), null);
                    else if (propertys[j].PropertyType.Equals(typeof(decimal)))
                        propertys[j].SetValue(model, Convert.ToDecimal(FormData[propertys[j].Name].Trim() == "" ? "0" : FormData[propertys[j].Name]), null);
                    else if (propertys[j].PropertyType.Equals(typeof(DateTime)))
                        propertys[j].SetValue(model, Convert.ToDateTime(FormData[propertys[j].Name].Trim() == "" ? DateTime.Now.ToString() : FormData[propertys[j].Name]), null);
                    else
                        propertys[j].SetValue(model, FormData[propertys[j].Name], null);
                }
            }

            return model;
        }

        public System.Data.DataTable ToDataTable(string json)
        {
            throw new NotImplementedException();
        }

        public List<T> ToList<T>(string json)
        {
            Newtonsoft.Json.JavaScriptArray jsonArray = (Newtonsoft.Json.JavaScriptArray)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(json);

            List<T> list = new List<T>();
            T model = NewObject<T>();

            System.Reflection.PropertyInfo[] propertys = model.GetType().GetProperties();

            for (int i = 0; i < jsonArray.Count; i++)
            {
                T _model = (T)((ICloneable)model).Clone();

                Newtonsoft.Json.JavaScriptObject Jobject = ((Newtonsoft.Json.JavaScriptObject)((Newtonsoft.Json.JavaScriptArray)jsonArray)[i]);
                for (int n = 0; n < Jobject.Count; n++)
                {
                    for (int j = 0; j < propertys.Length; j++)
                    {
                        if (propertys[j].Name == "WorkId") break;
                        if (Jobject.ToList()[n].Key.Trim().ToUpper() == propertys[j].Name.ToUpper())
                        {

                            if (propertys[j].PropertyType.Equals(typeof(Int32)))
                                propertys[j].SetValue(_model, Convert.ToInt32(Jobject.ToList()[n].Value.ToString().Trim() == "" ? 0 : Jobject.ToList()[n].Value), null);
                            else if (propertys[j].PropertyType.Equals(typeof(Int64)))
                                propertys[j].SetValue(_model, Convert.ToInt64(Jobject.ToList()[n].Value.ToString().Trim() == "" ? 0 : Jobject.ToList()[n].Value), null);
                            else if (propertys[j].PropertyType.Equals(typeof(decimal)))
                                propertys[j].SetValue(_model, Convert.ToDecimal(Jobject.ToList()[n].Value.ToString().Trim() == "" ? 0 : Jobject.ToList()[n].Value), null);
                            else if (propertys[j].PropertyType.Equals(typeof(DateTime)))
                                propertys[j].SetValue(_model, Convert.ToDateTime(Jobject.ToList()[n].Value.ToString().Trim() == "" ? DateTime.Now.ToString() : Jobject.ToList()[n].Value), null);
                            else
                                propertys[j].SetValue(_model, Jobject.ToList()[n].Value.ToString(), null);
                            break;
                        }
                    }
                }
                list.Add(_model);
            }

            return list;
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
            string sBytes = Convert.ToBase64String((byte[])value);
            writer.WriteValue(sBytes);
        }

        public override object ReadJson(JsonReader reader, Type objectType)
        {
            byte[] sBytes = Convert.FromBase64String(reader.Value.ToString ());
            return sBytes;
        }
    }
}

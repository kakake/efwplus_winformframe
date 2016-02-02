using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Collections;

namespace EFWCoreLib.WebFrame.HttpHandler.Utility
{
    /// <summary>
    /// 根据主从表的信息输出XML
    /// </summary>
    public class RAXmlDataSource
    {
        public RAXmlDataSource()
        {
            masterDt = null;
            detaildtTable = new Hashtable();
            encoding = "";
        }

        /// <summary>
        /// 设置主表信息
        /// </summary>
        /// <param name="dt">指定主表对象。</param>
        /// <param name="fields">指定输出的字段列表，例子："field1,field2"，空串代表全部字段。</param>
        public void SetMaster(DataTable dt, String fields)
        {
            masterDt = dt;
            masterFields = fields;
        }

        /// <summary>
        /// 增加明细表信息
        /// </summary>
        /// <param name="detailID">指定标识明细表对象的唯一ID值。</param>
        /// <param name="dt">指定明细表对象。</param>
        /// <param name="relation">指定明细表与主记录集的关系；主表字段在等号左侧，明细记录集在等号右侧。</param>
        /// <param name="fields">指定输出的字段列表，例子："field1,field2"，空串代表全部字段。</param>
        public void AddDetail(String detailID, DataTable dt, String relation, String fields)
        {
            DetailInfo info = new DetailInfo();
            info.dt = dt;
            info.relation = relation;
            info.fields = fields;
            info.masterTable = new Hashtable();
            info.relationList = new ArrayList();
            String[] sr = relation.Split(',');
            for (int i = 0; i <= sr.Length - 1; i++)
                info.relationList.Add(sr[i]);
            detaildtTable.Add(detailID, info);
        }

        /// <summary>
        /// 删除明细表信息
        /// </summary>
        /// <param name="detailID">指定标识明细表的唯一ID值。</param>
        public void deleteDetail(String detailID)
        {
            detaildtTable.Remove(detailID);
        }

        /// <summary>
        /// 清空明细表信息
        /// </summary>
        public void clearDetail()
        {
            detaildtTable.Clear();
        }

        /// <summary>
        /// 输出XML字符串
        /// </summary>
        /// <param name="write">指定接收XML字符串的写入对象</param>
        public void ExportToXML(TextWriter write)
        {
            ArrayList fields = new ArrayList();

            if (encoding != "")
            {
                write.Write("<?xml version=\"1.0\" encoding=\"");
                write.Write(encoding);
                write.Write("\"?>");
            }
            write.Write("<DocumentData>");

            //如果没有主表数据,将所有明细表数据与默认主记录行对应
            if (masterDt == null)
            {
                IDictionaryEnumerator e = (IDictionaryEnumerator)detaildtTable.Keys.GetEnumerator();
                while (e.MoveNext())
                {
                    String id = (String)e.Key;
                    DetailInfo info = (DetailInfo)e.Value;

                    String[] sr = info.fields.Split(','); ;
                    for (int i = 0; i <= sr.Length - 1; i++)
                        if (sr[i] != "") fields.Add(sr[i].Trim());

                    write.Write("<");
                    write.Write(id);
                    write.Write(">");
                    foreach (DataRow dr in info.dt.Rows)
                    {
                        write.Write("<Row");
                        foreach (DataColumn c in info.dt.Columns)
                        {
                            String fn = c.ColumnName;
                            if (fields.Count == 0 || fields.IndexOf(fn) != -1)
                            {
                                write.Write(" ");
                                write.Write(fn);
                                write.Write("=\"");
                                WriteTransferred(write, dr[c].ToString());
                                write.Write("\"");
                            }
                        }
                        write.Write("/>");
                    }
                    write.Write("</");
                    write.Write(id);
                    write.Write(">");
                }
            }
            else
            {//如果有主表数据,将主表外键进行Hashed运算,以便能和索引值快速对应
                //清除Hash数据
                IDictionaryEnumerator e = (IDictionaryEnumerator)detaildtTable.Keys.GetEnumerator();
                while (e.MoveNext())
                {
                    DetailInfo info = (DetailInfo)e.Value;
                    info.masterTable.Clear();
                }

                String[] sr = masterFields.Split(',');
                for (int i = 0; i <= sr.Length - 1; i++)
                    if (!sr[i].Equals("")) fields.Add(sr[i].Trim());

                write.Write("<Master>");

                int midx = 0;
                foreach (DataRow dr in masterDt.Rows)
                {
                    write.Write("<Row");
                    foreach (DataColumn c in masterDt.Columns)
                    {
                        String fn = c.ColumnName;
                        if (fields.Count == 0 || fields.IndexOf(fn) != -1)
                        {
                            write.Write(" ");
                            write.Write(fn);
                            write.Write("=\"");
                            WriteTransferred(write, dr[c].ToString());
                            write.Write("\"");
                        }
                    }
                    write.Write("/>");

                    //将外键存入Hash表中
                    IDictionaryEnumerator e2 = (IDictionaryEnumerator)detaildtTable.Keys.GetEnumerator();
                    while (e2.MoveNext())
                    {
                        DetailInfo info = (DetailInfo)e2.Value;
                        String key = "";

                        for (int j = 0; j <= info.relationList.Count - 1; j++)
                        {
                            String r = (String)info.relationList[j];
                            String v = dr[(r.Split('='))[0]].ToString();

                            if (j == 0)
                                key = key + v;
                            else
                                key = key + "\t" + v;
                        }

                        info.masterTable.Add(key, midx);
                    }
                    midx = midx + 1;
                }

                write.Write("</Master>");

                IDictionaryEnumerator e3 = (IDictionaryEnumerator)detaildtTable.Keys.GetEnumerator();
                while (e3.MoveNext())
                {
                    String id = (String)e3.Key;
                    DetailInfo info = (DetailInfo)e3.Value;

                    sr = info.fields.Split(',');
                    for (int i = 0; i <= sr.Length - 1; i++)
                        if (!sr[i].Equals("")) fields.Add(sr[i].Trim());

                    write.Write("<");
                    write.Write(id);
                    write.Write(">");

                    foreach (DataRow dr in info.dt.Rows)
                    {
                        //从Hash表里查找主索引
                        String key = "";

                        for (int j = 0; j <= info.relationList.Count - 1; j++)
                        {
                            String r = (String)info.relationList[j];
                            String[] t = r.Split('=');
                            String v = dr[t[1]].ToString();

                            if (j == 0)
                                key = key + v;
                            else
                                key = key + "\t" + v;
                        }

                        midx = (int)info.masterTable[key];

                        if (midx != -1)
                        {
                            write.Write("<Row");
                            write.Write(" ");
                            write.Write("__MasterIdx");
                            write.Write("=\"");
                            write.Write(midx);
                            write.Write("\"");
                            foreach (DataColumn c in info.dt.Columns)
                            {
                                String fn = c.ColumnName;
                                if (fields.Count == 0 || fields.IndexOf(fn) != -1)
                                {
                                    write.Write(" ");
                                    write.Write(fn);
                                    write.Write("=\"");
                                    WriteTransferred(write, dr[c].ToString());
                                    write.Write("\"");
                                }
                            }
                            write.Write("/>");
                        }
                    }
                    write.Write("</");
                    write.Write(id);
                    write.Write(">");
                }
            }
            write.Write("</DocumentData>");
        }

        /// <summary>
        /// 输出XML字符串
        /// </summary>
        /// <param name="write">指定接收XML字符串的写入对象</param>
        public string ExportToJson()
        {
            StringBuilder sb = new StringBuilder();
            ArrayList fields = new ArrayList();

            //if (encoding != "")
            //{
            //    sb.Append("<?xml version=\"1.0\" encoding=\"");
            //    sb.Append(encoding);
            //    sb.Append("\"?>");
            //}
            sb.Append("<DocumentData>");

            //如果没有主表数据,将所有明细表数据与默认主记录行对应
            if (masterDt == null)
            {
                IDictionaryEnumerator e = (IDictionaryEnumerator)detaildtTable.Keys.GetEnumerator();
                while (e.MoveNext())
                {
                    String id = (String)e.Key;
                    DetailInfo info = (DetailInfo)e.Value;

                    String[] sr = info.fields.Split(','); ;
                    for (int i = 0; i <= sr.Length - 1; i++)
                        if (sr[i] != "") fields.Add(sr[i].Trim());

                    sb.Append("<");
                    sb.Append(id);
                    sb.Append(">");
                    foreach (DataRow dr in info.dt.Rows)
                    {
                        sb.Append("<Row");
                        foreach (DataColumn c in info.dt.Columns)
                        {
                            String fn = c.ColumnName;
                            if (fields.Count == 0 || fields.IndexOf(fn) != -1)
                            {
                                sb.Append(" ");
                                sb.Append(fn);
                                sb.Append("=\"");
                                WriteTransferred(sb, dr[c].ToString().Trim());
                                sb.Append("\"");
                            }
                        }
                        sb.Append("/>");
                    }
                    sb.Append("</");
                    sb.Append(id);
                    sb.Append(">");
                }
            }
            else
            {//如果有主表数据,将主表外键进行Hashed运算,以便能和索引值快速对应
                //清除Hash数据
                IDictionaryEnumerator e = (IDictionaryEnumerator)detaildtTable.Keys.GetEnumerator();
                while (e.MoveNext())
                {
                    DetailInfo info = (DetailInfo)e.Value;
                    info.masterTable.Clear();
                }

                String[] sr = masterFields.Split(',');
                for (int i = 0; i <= sr.Length - 1; i++)
                    if (!sr[i].Equals("")) fields.Add(sr[i].Trim());

                sb.Append("<Master>");

                int midx = 0;
                foreach (DataRow dr in masterDt.Rows)
                {
                    sb.Append("<Row");
                    foreach (DataColumn c in masterDt.Columns)
                    {
                        String fn = c.ColumnName;
                        if (fields.Count == 0 || fields.IndexOf(fn) != -1)
                        {
                            sb.Append(" ");
                            sb.Append(fn);
                            sb.Append("=\"");
                            WriteTransferred(sb, dr[c].ToString().Trim());
                            sb.Append("\"");
                        }
                    }
                    sb.Append("/>");

                    //将外键存入Hash表中
                    //IDictionaryEnumerator e2 = (IDictionaryEnumerator)detaildtTable.Keys.GetEnumerator();
                    //while (e2.MoveNext())
                    //{
                    //    DetailInfo info = (DetailInfo)e2.Value;
                    //    String key = "";

                    //    for (int j = 0; j <= info.relationList.Count - 1; j++)
                    //    {
                    //        String r = (String)info.relationList[j];
                    //        String v = dr[(r.Split('='))[0]].ToString();

                    //        if (j == 0)
                    //            key = key + v;
                    //        else
                    //            key = key + "\t" + v;
                    //    }

                    //    info.masterTable.Add(key, midx);
                    //}
                    //midx = midx + 1;
                }

                sb.Append("</Master>");

                IDictionaryEnumerator e3 = (IDictionaryEnumerator)detaildtTable.Keys.GetEnumerator();
                while (e3.MoveNext())
                {
                    String id = (String)e3.Key;
                    DetailInfo info = (DetailInfo)e3.Value;

                    sr = info.fields.Split(',');
                    for (int i = 0; i <= sr.Length - 1; i++)
                        if (!sr[i].Equals("")) fields.Add(sr[i].Trim());

                    sb.Append("<");
                    sb.Append(id);
                    sb.Append(">");

                    foreach (DataRow dr in info.dt.Rows)
                    {
                        //从Hash表里查找主索引
                        String key = "";

                        //for (int j = 0; j <= info.relationList.Count - 1; j++)
                        //{
                        //    String r = (String)info.relationList[j];
                        //    String[] t = r.Split('=');
                        //    String v = dr[t[1]].ToString();

                        //    if (j == 0)
                        //        key = key + v;
                        //    else
                        //        key = key + "\t" + v;
                        //}

                        //midx = (int)info.masterTable[key];

                        if (midx != -1)
                        {
                            sb.Append("<Row");
                            //sb.Append(" ");
                            //sb.Append("__MasterIdx");
                            //sb.Append("=\"");
                            //sb.Append(midx);
                            //sb.Append("\"");
                            foreach (DataColumn c in info.dt.Columns)
                            {
                                String fn = c.ColumnName;
                                if (fields.Count == 0 || fields.IndexOf(fn) != -1)
                                {
                                    sb.Append(" ");
                                    sb.Append(fn);
                                    sb.Append("=\"");
                                    WriteTransferred(sb, dr[c].ToString().Trim());
                                    sb.Append("\"");
                                }
                            }
                            sb.Append("/>");
                        }
                    }
                    sb.Append("</");
                    sb.Append(id);
                    sb.Append(">");
                }
            }
            sb.Append("</DocumentData>");

            string xmlstr = sb.ToString();
            //xmlstr = xmlstr.Replace("\r\n", "").Replace("\"", "'").Replace("\r", "");
            //return "{\"data\":\"" + xmlstr + "\"}";

            xmlstr = xmlstr.Replace("\r\n", "").Replace("\r", "");
            return xmlstr;
        }

        /// <summary>
        /// 指定XML的字符编码格式 
        /// </summary>
        public string Encoding
        {
            set { encoding = value; }
            get { return encoding; }
        }

        private DataTable masterDt;
        private String masterFields;
        private Hashtable detaildtTable;
        private String encoding;

        /// <summary>
        /// 转义输出字符
        /// </summary>
        /// <param name="write">字符输出对象</param>
        /// <param name="str">待输出的字符</param>
        private void WriteTransferred(TextWriter write, String str)
        {
            if (str == null)
                return;
            for (int i = 0; i <= str.Length - 1; i++)
            {
                if (str[i] == '<') write.Write("&lt;");
                else if (str[i] == '>') write.Write("&gt;");
                else if (str[i] == '&') write.Write("&amp;");
                else if (str[i] == '"') write.Write("&quot;");
                else write.Write(str[i]);
            }
        }

        /// <summary>
        /// 转义输出字符
        /// </summary>
        /// <param name="write">字符输出对象</param>
        /// <param name="str">待输出的字符</param>
        private void WriteTransferred(StringBuilder sb, String str)
        {
            if (str == null)
                return;
            for (int i = 0; i <= str.Length - 1; i++)
            {
                if (str[i] == '<') sb.Append("&lt;");
                else if (str[i] == '>') sb.Append("&gt;");
                else if (str[i] == '&') sb.Append("&amp;");
                else if (str[i] == '"') sb.Append("&quot;");
                else sb.Append(str[i]);
            }
        }

    }

    public class DetailInfo
    {
        public DataTable dt;
        public String relation;
        public String fields;
        public ArrayList relationList;
        public Hashtable masterTable;
    }

    /// <summary>
    /// Web报表封装帮助类
    /// </summary>
    public static class WebReportAll
    {
        /// <summary>
        /// 转换为报表数据
        /// </summary>
        /// <param name="dtDetail">明细数据内容</param>
        /// <returns></returns>
        public static string ToReportData(DataTable dtDetail)
        {
            RAXmlDataSource xmlData = new RAXmlDataSource();
            //xmlData.SetMaster(dtMaster, "");
            xmlData.AddDetail("Detail1", dtDetail, "", "");
            return xmlData.ExportToJson();
        }
        /// <summary>
        /// 转换为报表数据
        /// </summary>
        /// <param name="dtMaster">报表页眉数据内容</param>
        /// <param name="dtDetail">明细数据内容</param>
        /// <returns></returns>
        public static string ToReportData(DataTable dtMaster, DataTable dtDetail)
        {
            RAXmlDataSource xmlData = new RAXmlDataSource();
            xmlData.SetMaster(dtMaster, "");
            xmlData.AddDetail("Detail1", dtDetail, "", "");
            return xmlData.ExportToJson();
        }
        /// <summary>
        /// 转换为报表数据
        /// </summary>
        /// <param name="dtMaster">报表页眉数据内容</param>
        /// <param name="dtDetails">多个明细数据内容</param>
        /// <returns></returns>
        public static string ToReportData(DataTable dtMaster, DataTable[] dtDetails)
        {
            RAXmlDataSource xmlData = new RAXmlDataSource();
            xmlData.SetMaster(dtMaster, "");
            for (int i = 0; i < dtDetails.Length; i++)
            {
                xmlData.AddDetail("Detail" + (i + 1).ToString(), dtDetails[i], "", "");
            }
            return xmlData.ExportToJson();
        }

        /// <summary>
        /// 转换为报表数据
        /// </summary>
        /// <param name="dtDetail">明细数据内容</param>
        /// <returns></returns>
        public static void ToReportData(DataTable dtDetail, TextWriter write)
        {
            RAXmlDataSource xmlData = new RAXmlDataSource();
            xmlData.Encoding = "UTF-8";
            //xmlData.SetMaster(dtMaster, "");
            xmlData.AddDetail("Detail1", dtDetail, "", "");
            xmlData.ExportToXML(write);
        }
        /// <summary>
        /// 转换为报表数据
        /// </summary>
        /// <param name="dtMaster">报表页眉数据内容</param>
        /// <param name="dtDetail">明细数据内容</param>
        /// <returns></returns>
        public static void ToReportData(DataTable dtMaster, DataTable dtDetail, TextWriter write)
        {
            RAXmlDataSource xmlData = new RAXmlDataSource();
            xmlData.SetMaster(dtMaster, "");
            xmlData.AddDetail("Detail1", dtDetail, "", "");
            xmlData.ExportToXML(write);
        }
        /// <summary>
        /// 转换为报表数据
        /// </summary>
        /// <param name="dtMaster">报表页眉数据内容</param>
        /// <param name="dtDetails">多个明细数据内容</param>
        /// <returns></returns>
        public static void ToReportData(DataTable dtMaster, DataTable[] dtDetails, TextWriter write)
        {
            RAXmlDataSource xmlData = new RAXmlDataSource();
            xmlData.SetMaster(dtMaster, "");
            for (int i = 0; i < dtDetails.Length; i++)
            {
                xmlData.AddDetail("Detail" + (i + 1).ToString(), dtDetails[i], "", "");
            }
            xmlData.ExportToXML(write);
        }
    }
}

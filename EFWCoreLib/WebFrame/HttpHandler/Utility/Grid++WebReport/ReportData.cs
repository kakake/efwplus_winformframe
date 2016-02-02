using System;
using System.Text;
using System.Data;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EFWCoreLib.WebFrame.HttpHandler.Utility
{

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //以下枚举指定报表数据的格式类型
    public enum ResponseDataType
    {
        PlainText, //报表数据为XML或JSON文本，在调试时可以查看报表数据。数据未经压缩，大数据量报表采用此种方式不合适
        ZipBinary, //报表数据为XML或JSON文本经过压缩得到的二进制数据。此种方式数据量最小(约为原始数据的1/10)，但用Ajax方式加载报表数据时不能为此种方式
        ZipBase64, //报表数据为将 ZipBinary 方式得到的数据再进行 BASE64 编码的数据。此种方式适合用Ajax方式加载报表数据
    };

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //class ReportDataBase
    public class ReportDataBase
    {
        //指定报表的默认数据类型，便于统一定义整个报表系统的数据类型
        //在报表开发调试阶段，通常指定为 ResponseDataType.PlainText, 以便在浏览器中查看响应的源文件时能看到可读的文本数据
        //在项目部署时，通常指定为 ResponseDataType.ZipBinary 或 ResponseDataType.ZipBase64，这样可以极大减少数据量，提供报表响应速度
        public const ResponseDataType DefaultDataType = ResponseDataType.ZipBase64; //PlainText ZipBinary ZipBase64 

        //将报表XML数据文本输出到HTTP请求
        public static void ResponseData(HttpContext DataPage, ref string DataText, ResponseDataType DataType)
        {
            //报表XML数据的前后不能附加任何其它数据，否则XML数据将不能成功解析，所以调用ClearContent方法清理网页中前面多余的数据
            DataPage.Response.ClearContent();

            if (ResponseDataType.PlainText == DataType)
            {
                // 把xml对象发送给客户端
                //DataPage.Response.ContentType = "text/xml";
                DataPage.Response.Write(DataText);
            }
            else
            {
                //将string数据转换为byte[]，以便进行压缩
                System.Text.UTF8Encoding converter = new System.Text.UTF8Encoding();
                byte[] XmlBytes = converter.GetBytes(DataText);

                //在 HTTP 头信息中写入报表数据压缩信息
                DataPage.Response.AppendHeader("gr_zip_type", "deflate");                  //指定压缩方法
                DataPage.Response.AppendHeader("gr_zip_size", XmlBytes.Length.ToString()); //指定数据的原始长度
                DataPage.Response.AppendHeader("gr_zip_encode", converter.HeaderName);     //指定数据的编码方式 utf-8 utf-16 ...

                // 把压缩后的xml数据发送给客户端
                if (ResponseDataType.ZipBinary == DataType)
                {
                    DeflateStream compressedzipStream = new DeflateStream(DataPage.Response.OutputStream, CompressionMode.Compress, true);
                    compressedzipStream.Write(XmlBytes, 0, XmlBytes.Length);
                    compressedzipStream.Close();
                }
                else //ResponseDataType.ZipBase64
                {
                    MemoryStream memStream = new MemoryStream();
                    DeflateStream compressedzipStream = new DeflateStream(memStream, CompressionMode.Compress, true);
                    compressedzipStream.Write(XmlBytes, 0, XmlBytes.Length);
                    compressedzipStream.Close(); //这句很重要，这样数据才能全部写入 MemoryStream

                    // Read bytes from the stream.
                    memStream.Seek(0, SeekOrigin.Begin); // Set the position to the beginning of the stream.
                    int count = (int)memStream.Length;
                    byte[] byteArray = new byte[count];
                    count = memStream.Read(byteArray, 0, count);

                    string Base64Text = Convert.ToBase64String(byteArray);
                    DataPage.Response.Write(Base64Text);
                }
            }

            //报表XML数据的前后不能附加任何其它数据，否则XML数据将不能成功解析，所以调用End方法放弃网页中后面不必要的数据
            //DataPage.Response.End();
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //class XMLReportData 产生报表需要的xml数据
    public class Grid_XMLReportData
    {
        //根据 DataSet 产生提供给报表需要的XML数据，参数DataType指定压缩编码数据的形式
        public static void GenDetailData(HttpContext DataPage, DataSet myds, ResponseDataType DataType)
        {
            string XMLText = myds.GetXml();
            ReportDataBase.ResponseData(DataPage, ref XMLText, DataType);
        }

        //根据 DataTable 产生提供给报表需要的XML数据，参数DataType指定压缩编码数据的形式
        public static void GenDetailData(HttpContext DataPage, DataTable mydt, ResponseDataType DataType)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(mydt);
            GenDetailData(DataPage, ds, DataType);
        }

        //根据 DataSet 产生提供给报表需要的XML数据，并同时将ParamterPart中的报表参数数据一起打包，参数DataType指定压缩编码数据的形式
        public static void GenEntireData(HttpContext DataPage, DataSet myds, ref string ParameterPart, ResponseDataType DataType)
        {
            string RecordsetPart = myds.GetXml();
            string XMLText = "<report>\r\n" + RecordsetPart + ParameterPart + "</report>";
            ReportDataBase.ResponseData(DataPage, ref XMLText, DataType);
        }

        //根据IDataReader, 产生提供给报表需要的XML数据，其中的空值字段也会产生XML节点，参数DataType指定压缩编码数据的形式
        public static void GenNodeXmlDataFromReader(HttpContext DataPage, IDataReader dr, ResponseDataType DataType)
        {
            string XMLText = "<xml>\n";
            while (dr.Read())
            {
                XMLText += "<row>";
                for (int i = 0; i < dr.FieldCount; ++i)
                {
                    string FldName = dr.GetName(i);
                    if (FldName == "")
                        FldName = "Fld" + i;
                    XMLText += String.Format("<{0}>{1}</{0}>", FldName, HttpUtility.HtmlEncode(dr.GetValue(i).ToString()));
                }
                XMLText += "</row>\n";
            }
            XMLText += "</xml>\n";

            ReportDataBase.ResponseData(DataPage, ref XMLText, DataType);
        }

        //根据 IDataReader 产生提供给报表需要的XML参数数据包
        public static void GenParameterData(HttpContext DataPage, IDataReader drParamer)
        {
            string XMLText = GenParameterText(drParamer);
            XMLText = "<report>" + XMLText + "</report>";
            ReportDataBase.ResponseData(DataPage, ref XMLText, ResponseDataType.PlainText);
        }

        //将 DataReader 中的数据打包为报表需要的参数数据包形式
        public static string GenParameterText(IDataReader drParamer)
        {
            string ParameterPart = "\r\n<_grparam>\r\n";
            if (drParamer.Read())
            {
                for (int i = 0; i < drParamer.FieldCount; ++i)
                {
                    if (drParamer.IsDBNull(i))
                        continue;

                    if (drParamer.GetFieldType(i).IsArray)
                    {
                        long DataSize = drParamer.GetBytes(i, 0, null, 0, int.MaxValue);
                        byte[] buffer = new byte[DataSize];
                        drParamer.GetBytes(i, 0, buffer, 0, (int)DataSize);
                        ParameterPart += String.Format("<{0}>{1}</{0}>\r\n", drParamer.GetName(i), Convert.ToBase64String(buffer));
                    }
                    else
                    {
                        ParameterPart += String.Format("<{0}>{1}</{0}>\r\n", drParamer.GetName(i), HttpUtility.HtmlEncode(drParamer.GetValue(i).ToString()));
                    }
                }
            }
            ParameterPart += "</_grparam>\r\n";
            return ParameterPart;
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //class JSONReportData 产生报表需要的 JSON 格式数据
    public class Grid_JSONReportData
    {
        //根据 DataSet 产生提供给报表需要的JSON数据，参数DataType指定压缩编码数据的形式
        public static void GenDetailData(HttpContext DataPage, DataSet myds, ResponseDataType DataType)
        {
            GenDetailData(DataPage, myds.Tables[0], DataType);
        }

        //根据 DataTable 产生提供给报表需要的JSON数据，参数DataType指定压缩编码数据的形式
        public static void GenDetailData(HttpContext DataPage, DataTable dt, ResponseDataType DataType)
        {
            string Out = GenDetailText(dt);
            ReportDataBase.ResponseData(DataPage, ref Out, DataType);
        }

        //根据 DataSet 产生提供给报表需要的JSON数据，并同时将ParamterPart中的报表参数数据一起打包，参数DataType指定压缩编码数据的形式
        public static void GenEntireData(HttpContext DataPage, DataSet myds, ref string ParameterPart, ResponseDataType DataType)
        {
            string JSONText = GenDetailText(myds.Tables[0]);

            StringBuilder sb = new StringBuilder(JSONText, 0, JSONText.Length - 1, JSONText.Length + ParameterPart.Length + 2); //去掉最后一个“}”
            sb.Append(',');
            sb.Append(ParameterPart);
            sb.Append('}');
            string Out = sb.ToString();
            ReportDataBase.ResponseData(DataPage, ref Out, DataType);
        }

        //根据 IDataReader 产生提供给报表需要的JSON参数数据包
        public static void GenParameterData(HttpContext DataPage, IDataReader drParamer)
        {
            string sbJSONText = GenParameterText(drParamer);
            sbJSONText = "{" + sbJSONText + "}";
            ReportDataBase.ResponseData(DataPage, ref sbJSONText, ResponseDataType.PlainText);
        }

        //根据 DataTable 产生提供给报表需要的JSON文本数据
        public static string GenDetailText(DataTable dt)
        {
            StringBuilder sbJSONText = new StringBuilder("{\"recordset\":[\n");
            foreach (DataRow dr in dt.Rows)
            {
                sbJSONText.Append('{');
                //bool IsFirst = true;
                for (int i = 0; i < dt.Columns.Count; ++i)
                {
                    if (!dr.IsNull(i))
                    {
                        string Value;
                        if (dt.Columns[i].DataType.IsArray)
                        {
                            Value = Convert.ToBase64String((byte[])dr[i]);
                        }
                        else
                        {
                            Value = dr[i].ToString();
                            PrepareValueText(ref Value);
                        }
                        sbJSONText.AppendFormat("\"{0}\":\"{1}\",", dt.Columns[i].ColumnName, Value);
                    }
                }
                sbJSONText.Remove(sbJSONText.Length - 1, 1); //去掉每笔记录最后一个字段后面的","
                sbJSONText.Append("},\n");
            }
            sbJSONText.Remove(sbJSONText.Length - 2, 1); //去掉最后一条记录后面的","
            sbJSONText.Append("]}");

            return sbJSONText.ToString();
        }

        //将 DataReader 中的数据打包为报表需要的参数数据包形式
        public static string GenParameterText(IDataReader drParamer)
        {
            StringBuilder sbJSONText = new StringBuilder();
            if (drParamer.Read())
            {
                for (int i = 0; i < drParamer.FieldCount; ++i)
                {
                    if (drParamer.IsDBNull(i))
                        continue;

                    string Value;
                    if (drParamer.GetFieldType(i).IsArray)
                    {
                        long DataSize = drParamer.GetBytes(i, 0, null, 0, int.MaxValue);
                        byte[] buffer = new byte[DataSize];
                        drParamer.GetBytes(i, 0, buffer, 0, (int)DataSize);
                        Value = Convert.ToBase64String(buffer);
                    }
                    else
                    {
                        Value = drParamer.GetValue(i).ToString();
                        PrepareValueText(ref Value);
                    }

                    sbJSONText.AppendFormat("\"{0}\":\"{1}\",", drParamer.GetName(i), Value);
                }
            }
            sbJSONText.Remove(sbJSONText.Length - 1, 1); //去掉最后一个字段后面的","
            return sbJSONText.ToString();
        }

        //如果数据中包含有JSON规范中的特殊字符(" \ \r \n \t)，多特殊字符加 \ 编码
        public static void PrepareValueText(ref string ValueText)
        {
            bool HasSpecialChar = false;
            foreach (char ch in ValueText)
            {
                if (ch == '"' || ch == '\\' || ch == '\r' || ch == '\n' || ch == '\t')
                {
                    HasSpecialChar = true;
                    break;
                }
            }
            if (HasSpecialChar)
            {
                StringBuilder NewValueText = new StringBuilder();
                foreach (char ch in ValueText)
                {
                    if (ch == '"' || ch == '\\' || ch == '\r' || ch == '\n' || ch == '\t')
                    {
                        NewValueText.Append('\\');
                        if (ch == '"' || ch == '\\')
                            NewValueText.Append(ch);
                        else if (ch == '\r')
                            NewValueText.Append('r');
                        else if (ch == '\n')
                            NewValueText.Append('n');
                        else if (ch == '\t')
                            NewValueText.Append('t');
                    }
                    else
                    {
                        NewValueText.Append(ch);
                    }
                }
                ValueText = NewValueText.ToString();
            }
        }
    }


}
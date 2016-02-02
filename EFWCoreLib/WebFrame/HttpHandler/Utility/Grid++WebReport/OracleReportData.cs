using System;
using System.Data;
using System.Data.OracleClient;
using System.Configuration;
using System.IO;
using System.IO.Compression;

/////////////////////////////////////////////////////////////////////////////////////////////////////////
//class  OracleReportData 产生提供给报表生成需要的 XML 或 JSON 数据，采用 Oracle 数据引擎
public class OracleReportData
{
    //★特别提示★：
    //连接Grid++Report Access例子数据库的连接串，应该修改为与实际一致，如果安装目录不在C:\Grid++Report 5.0，应进行修改。
    public const string OracleConnStr = @"Provider=Microsoft.Jet.Oracle.4.0;User ID=Admin;Data Source=C:\Grid++Report 5.0\Samples\Data\Northwind.mdb";

    //定义在SQL中表示日期值的包围符号，Access用“#”, 而MS SQl Server用“'”，为了生成两者都可用的查询SQL语句，将其参数化定义出来。这样处理只是为了演示例子方便
    public const char DateSqlBracketChar = '#';

    //根据查询SQL,产生提供给报表生成需要的 XML 数据，采用 Oracle 数据引擎，字段值为空也产生数据
    public static void FullGenNodeXmlData(System.Web.UI.Page DataPage, string QuerySQL, bool ToCompress)
    {
        OracleConnection myConn = new OracleConnection(OracleConnStr);
        OracleCommand myCommand = new OracleCommand(QuerySQL, myConn);
        myConn.Open();
        OracleDataReader myReader = myCommand.ExecuteReader();
        XMLReportData.GenNodeXmlDataFromReader(DataPage, myReader, ToCompress ? ResponseDataType.ZipBinary : ResponseDataType.PlainText);
        myReader.Close();
        myConn.Close();
    }

    //获取 Count(*) SQL 查询到的数据行数。参数 QuerySQL 指定获取报表数据的查询SQL
    public static int BatchGetDataCount(string QuerySQL)
    {
        int Total = 0;

        OracleConnection myConn = new OracleConnection(OracleConnStr);
        OracleCommand myCommand = new OracleCommand(QuerySQL, myConn);
        myConn.Open();
        OracleDataReader myReader = myCommand.ExecuteReader();
        if (myReader.Read())
            Total = myReader.GetInt32(0);
        myReader.Close();
        myConn.Close();

        return Total;
    }

    //<<protected function
    //根据查询SQL,产生提供给报表生成需要的 XML 或 JSON 数据，采用 Oracle 数据引擎
    protected static void DoGenDetailData(System.Web.UI.Page DataPage, string QuerySQL, ResponseDataType DataType, bool IsJSON)
    {
        OracleConnection myConn = new OracleConnection(OracleConnStr);
        OracleDataAdapter myda = new OracleDataAdapter(QuerySQL, myConn);
        DataSet myds = new DataSet();
        myConn.Open();
        myda.Fill(myds);
        myConn.Close();

        if (IsJSON)
            JSONReportData.GenDetailData(DataPage, myds, DataType);
        else
            XMLReportData.GenDetailData(DataPage, myds, DataType);
    }

    //根据查询 SQL,产生提供给报表生成需要的 XML 或 JSON 数据，采用 Oracle 数据引擎, 这里只产生报表参数数据
    //当报表没有明细时，调用本方法生成数据，查询 SQL 应该只能查询出一条记录
    protected static void DoGenParameterData(System.Web.UI.Page DataPage, string ParameterQuerySQL, bool IsJSON)
    {
        OracleConnection myConn = new OracleConnection(OracleConnStr);
        OracleCommand myCommand = new OracleCommand(ParameterQuerySQL, myConn);
        myConn.Open();
        OracleDataReader myReader = myCommand.ExecuteReader();

        if (IsJSON)
            JSONReportData.GenParameterData(DataPage, myReader);
        else
            XMLReportData.GenParameterData(DataPage, myReader);
        myReader.Close();
        myConn.Close();
    }

    //根据查询SQL,产生提供给报表生成需要的 或 JSON 数据，采用 Oracle 数据引擎, 根据RecordsetQuerySQL获取报表明细数据，根据ParameterQuerySQL获取报表参数数据
    protected static void DoGenEntireData(System.Web.UI.Page DataPage, string RecordsetQuerySQL, string ParameterQuerySQL, ResponseDataType DataType, bool IsJSON)
    {
        OracleConnection myConn = new OracleConnection(OracleConnStr);
        myConn.Open();

        OracleDataAdapter myda = new OracleDataAdapter(RecordsetQuerySQL, myConn);
        DataSet myds = new DataSet();
        myda.Fill(myds);

        OracleCommand mycmd = new OracleCommand(ParameterQuerySQL, myConn);
        OracleDataReader mydr = mycmd.ExecuteReader(CommandBehavior.CloseConnection);

        if (IsJSON)
        {
            string ParameterPart = JSONReportData.GenParameterText(mydr);
            JSONReportData.GenEntireData(DataPage, myds, ref ParameterPart, DataType);
        }
        else
        {
            string ParameterPart = XMLReportData.GenParameterText(mydr);
            XMLReportData.GenEntireData(DataPage, myds, ref ParameterPart, DataType);
        }

        myConn.Close();
    }
    //>>protected function

    //<<保留前面版本的函数，兼容以前版本例子
    //根据查询SQL,产生提供给报表生成需要的 XML 数据，采用 Oracle 数据引擎
    public static void GenNodeXmlData(System.Web.UI.Page DataPage, string QuerySQL, bool ToCompress)
    {
        DoGenDetailData(DataPage, QuerySQL, ToCompress ? ResponseDataType.ZipBinary : ResponseDataType.PlainText, false);
    }

    //根据查询SQL,产生提供给报表生成需要的 XML 数据，采用 Oracle 数据引擎, 这里只产生报表参数数据
    //当报表没有明细时，调用本方法生成数据，查询SQL应该只能查询出一条记录
    public static void GenParameterReportData(System.Web.UI.Page DataPage, string ParameterQuerySQL)
    {
        DoGenParameterData(DataPage, ParameterQuerySQL, false);
    }

    //根据查询SQL,产生提供给报表生成需要的 XML 数据，采用 Oracle 数据引擎, 根据RecordsetQuerySQL获取报表明细数据，根据ParameterQuerySQL获取报表参数数据
    public static void GenEntireReportData(System.Web.UI.Page DataPage, string RecordsetQuerySQL, string ParameterQuerySQL, bool ToCompress)
    {
        DoGenEntireData(DataPage, RecordsetQuerySQL, ParameterQuerySQL, ToCompress ? ResponseDataType.ZipBinary : ResponseDataType.PlainText, false);
    }
    //>>保留前面版本的函数，兼容以前版本例子
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////
//class  OracleXMLReportData 根据SQL产生报表需要的 XML 数据
public class OracleXMLReportData : OracleReportData
{
    //产生报表明细记录数据，数据将被加载到明细网格的记录集中
    public static void GenDetailData(System.Web.UI.Page DataPage, string QuerySQL, ResponseDataType DataType)
    {
        OracleReportData.DoGenDetailData(DataPage, QuerySQL, DataType, false);
    }
    public static void GenDetailData(System.Web.UI.Page DataPage, string QuerySQL)
    {
        OracleReportData.DoGenDetailData(DataPage, QuerySQL, ReportDataBase.DefaultDataType, false);
    }

    //这里只产生报表参数数据，数据加载到报表参数、非明细网格中的部件框中
    //当报表没有明细时，调用本方法生成数据，查询SQL应该只能查询出一条记录
    public static void GenParameterData(System.Web.UI.Page DataPage, string ParameterQuerySQL)
    {
        OracleReportData.DoGenParameterData(DataPage, ParameterQuerySQL, false);
    }

    //根据RecordsetQuerySQL获取报表明细数据，对应数据加载到报表的明细网格的记录集中
    //根据ParameterQuerySQL获取报表参数数据，对应数据加载到报表参数、非明细网格中的部件框中
    public static void GenEntireData(System.Web.UI.Page DataPage, string RecordsetQuerySQL, string ParameterQuerySQL, ResponseDataType DataType)
    {
        OracleReportData.DoGenEntireData(DataPage, RecordsetQuerySQL, ParameterQuerySQL, DataType, false);
    }
    public static void GenEntireData(System.Web.UI.Page DataPage, string RecordsetQuerySQL, string ParameterQuerySQL)
    {
        OracleReportData.DoGenEntireData(DataPage, RecordsetQuerySQL, ParameterQuerySQL, ReportDataBase.DefaultDataType, false);
    }
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////
//class  OracleJsonReportData 根据SQL产生报表需要的 JSON 数据
public class OracleJsonReportData : OracleReportData
{
    //产生报表明细记录数据，数据将被加载到明细网格的记录集中
    public static void GenDetailData(System.Web.UI.Page DataPage, string QuerySQL, ResponseDataType DataType)
    {
        OracleReportData.DoGenDetailData(DataPage, QuerySQL, DataType, true);
    }
    public static void GenDetailData(System.Web.UI.Page DataPage, string QuerySQL)
    {
        OracleReportData.DoGenDetailData(DataPage, QuerySQL, ReportDataBase.DefaultDataType, true);
    }

    //这里只产生报表参数数据，数据加载到报表参数、非明细网格中的部件框中
    //当报表没有明细时，调用本方法生成数据，查询SQL应该只能查询出一条记录
    public static void GenParameterData(System.Web.UI.Page DataPage, string ParameterQuerySQL)
    {
        OracleReportData.DoGenParameterData(DataPage, ParameterQuerySQL, true);
    }

    //根据RecordsetQuerySQL获取报表明细数据，对应数据加载到报表的明细网格的记录集中
    //根据ParameterQuerySQL获取报表参数数据，对应数据加载到报表参数、非明细网格中的部件框中
    public static void GenEntireData(System.Web.UI.Page DataPage, string RecordsetQuerySQL, string ParameterQuerySQL, ResponseDataType DataType)
    {
        OracleReportData.DoGenEntireData(DataPage, RecordsetQuerySQL, ParameterQuerySQL, DataType, true);
    }
    public static void GenEntireData(System.Web.UI.Page DataPage, string RecordsetQuerySQL, string ParameterQuerySQL)
    {
        OracleReportData.DoGenEntireData(DataPage, RecordsetQuerySQL, ParameterQuerySQL, ReportDataBase.DefaultDataType, true);
    }
}

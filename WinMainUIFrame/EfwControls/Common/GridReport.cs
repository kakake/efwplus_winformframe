using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using grproLib;
using System.Data;
using EFWCoreLib.CoreFrame.Init;

namespace EfwControls.Common
{
    public struct MatchFieldPairType
    {
        public IGRField grField;
        public int MatchColumnIndex;
    }

    /// <summary>
    /// Grid++Report 报表封装类
    /// </summary>
    public class GridReport
    {
        private static grproLib.GridppReport _Report;
        private static DataTable _dataS;

        public static void Preview(string reportpath, Dictionary<string, string> parameters, DataTable dtGrid)
        {
            _Report = new GridppReport();
            _Report.LoadFromFile(AppGlobal.AppRootPath + reportpath);
            FillParameterToReport(_Report, parameters);
            _dataS = dtGrid;
            _Report.FetchRecord += new _IGridppReportEvents_FetchRecordEventHandler(_Report_FetchRecord);
            _Report.PrintPreview(false);
        }

        public static void Print(string reportpath, Dictionary<string, string> parameters, DataTable dtGrid)
        {
            _Report = new GridppReport();
            _Report.LoadFromFile(AppGlobal.AppRootPath + reportpath);
            FillParameterToReport(_Report, parameters);
            _dataS = dtGrid;
            _Report.FetchRecord += new _IGridppReportEvents_FetchRecordEventHandler(_Report_FetchRecord);
            _Report.Print(false);
        }

        public static GridppReport GetReport(string reportpath, Dictionary<string, string> parameters, DataTable dtGrid)
        {
            _Report = new GridppReport();
            _Report.LoadFromFile(AppGlobal.AppRootPath + reportpath);
            FillParameterToReport(_Report, parameters);
            _dataS = dtGrid;
            _Report.FetchRecord += new _IGridppReportEvents_FetchRecordEventHandler(_Report_FetchRecord);
            return _Report;
        }


        static void _Report_FetchRecord()
        {
            FillRecordToReport(_Report, _dataS);
        }

        public static void FillParameterToReport(IGridppReport Report, Dictionary<string, string> parameters)
        {
            if (parameters == null) return;
            foreach (KeyValuePair<string, string> val in parameters)
            {
                if (Report.Parameters.IndexByName(val.Key) != -1)
                {
                    Report.ParameterByName(val.Key).AsString = val.Value;
                }
            }
        }

        public static void FillRecordToReport(IGridppReport Report, DataTable dt)
        {
            if (dt == null) return;
            MatchFieldPairType[] MatchFieldPairs = new MatchFieldPairType[Math.Min(Report.DetailGrid.Recordset.Fields.Count, dt.Columns.Count)];

            //根据字段名称与列名称进行匹配，建立DataReader字段与Grid++Report记录集的字段之间的对应关系
            int MatchFieldCount = 0;
            for (int i = 0; i < dt.Columns.Count; ++i)
            {
                foreach (IGRField fld in Report.DetailGrid.Recordset.Fields)
                {
                    if (String.Compare(fld.Name, dt.Columns[i].ColumnName, true) == 0)
                    {
                        MatchFieldPairs[MatchFieldCount].grField = fld;
                        MatchFieldPairs[MatchFieldCount].MatchColumnIndex = i;
                        ++MatchFieldCount;
                        break;
                    }
                }
            }


            // 将 DataTable 中的每一条记录转储到 Grid++Report 的数据集中去
            foreach (DataRow dr in dt.Rows)
            {
                Report.DetailGrid.Recordset.Append();

                for (int i = 0; i < MatchFieldCount; ++i)
                {
                    if (!dr.IsNull(MatchFieldPairs[i].MatchColumnIndex))
                        MatchFieldPairs[i].grField.Value = dr[MatchFieldPairs[i].MatchColumnIndex];
                }

                Report.DetailGrid.Recordset.Post();
            }
        }
    }
}

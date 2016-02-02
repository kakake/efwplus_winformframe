using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;

namespace EFWCoreLib.WebFrame.HttpHandler.Utility
{
    public delegate object DelegateChart(XmlDocument xmldoc);

    public static class WebFusionChart
    {
        public static XmlDocument CreateChartXML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<graph>");
            sb.Append("</graph>");
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.LoadXml(sb.ToString());

            return _xmlDoc;
        }

        /// <summary>
        /// 根据XML数据文件创建对象
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static XmlDocument CreateChartXML(string filename)
        {
            XmlDocument _xmlDoc = new XmlDocument();
            _xmlDoc.Load(filename);
            return _xmlDoc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartXML"></param>
        /// <param name="AttributeName"></param>
        /// <param name="AttributeValue"></param>
        public static void AddgraphAttribute(XmlDocument chartXML, string AttributeName, string AttributeValue)
        {
            chartXML.DocumentElement.SetAttribute(AttributeName, AttributeValue);
        }

        public static void Addcategories(XmlDocument chartXML)
        {
            XmlElement xmlcategories = chartXML.CreateElement("categories");
            chartXML.DocumentElement.AppendChild(xmlcategories);
        }

        public static void AddcategoriesAttribute(XmlDocument chartXML, int index, string AttributeName, string AttributeValue)
        {
            ((XmlElement)chartXML.DocumentElement.SelectNodes("categories")[index]).SetAttribute(AttributeName, AttributeValue);
        }

        public static void Addcategory(XmlDocument chartXML, int index)
        {
            XmlElement xmlcategory = chartXML.CreateElement("category");
            chartXML.DocumentElement.SelectNodes("categories")[index].AppendChild(xmlcategory);
        }

        public static void AddcategoryAttribute(XmlDocument chartXML, int index, int index2, string AttributeName, string AttributeValue)
        {
            ((XmlElement)chartXML.DocumentElement.SelectNodes("categories")[index].SelectNodes("category")[index2]).SetAttribute(AttributeName, AttributeValue);
        }

        public static void Adddataset(XmlDocument chartXML)
        {
            XmlElement xmldataset = chartXML.CreateElement("dataset");
            chartXML.DocumentElement.AppendChild(xmldataset);
        }

        public static void AdddatasetAttribute(XmlDocument chartXML, int index, string AttributeName, string AttributeValue)
        {
            ((XmlElement)chartXML.DocumentElement.SelectNodes("dataset")[index]).SetAttribute(AttributeName, AttributeValue);
        }

        public static void Addset(XmlDocument chartXML, int index)
        {
            XmlElement xmlset = chartXML.CreateElement("set");
            chartXML.DocumentElement.SelectNodes("dataset")[index].AppendChild(xmlset);
        }

        public static void AddsetAttribute(XmlDocument chartXML, int index, int index2, string AttributeName, string AttributeValue)
        {
            ((XmlElement)chartXML.DocumentElement.SelectNodes("dataset")[index].SelectNodes("set")[index2]).SetAttribute(AttributeName, AttributeValue);
        }

        public static string chartXMLtoJson(XmlDocument chartXML)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            chartXML.Save(sw);
            string xmlstr = sw.ToString();
            xmlstr = xmlstr.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("\r\n", "").Replace("\"", "'");
            return "{\"data\":\"" + xmlstr + "\"}";
        }

        public static string chartXMLtoJson(XmlDocument chartXML, int rowNum)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            chartXML.Save(sw);
            string xmlstr = sw.ToString();
            xmlstr = xmlstr.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("\r\n", "").Replace("\"", "'");
            return "{\"data\":\"" + xmlstr + "\",\"rowNum\":\"" + rowNum.ToString() + "\"}";
        }

        public static string chartXML(XmlDocument chartXML)
        {
            System.IO.StringWriter sw = new System.IO.StringWriter();
            chartXML.Save(sw);
            string xmlstr = sw.ToString();
            xmlstr = xmlstr.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "").Replace("\r\n", "").Replace("\"", "'");
            return xmlstr;
        }

        public static string GetLineChartXmlDataDemo1()
        {
            XmlDocument xmlchart = CreateChartXML();

            AddgraphAttribute(xmlchart, "numdivlines", "4");
            AddgraphAttribute(xmlchart, "lineThickness", "3");
            AddgraphAttribute(xmlchart, "showValues", "0");
            AddgraphAttribute(xmlchart, "numVDivLines", "10");
            AddgraphAttribute(xmlchart, "formatNumberScale", "1");
            AddgraphAttribute(xmlchart, "rotateNames", "1");
            AddgraphAttribute(xmlchart, "decimalPrecision", "1");
            AddgraphAttribute(xmlchart, "anchorRadius", "2");
            AddgraphAttribute(xmlchart, "anchorBgAlpha", "0");
            AddgraphAttribute(xmlchart, "numberPrefix", "$");
            AddgraphAttribute(xmlchart, "divLineAlpha", "30");
            AddgraphAttribute(xmlchart, "showAlternateHGridColor", "1");
            AddgraphAttribute(xmlchart, "yAxisMinValue", "800000");
            AddgraphAttribute(xmlchart, "shadowAlpha", "50");

            Addcategories(xmlchart);
            Addcategory(xmlchart, 0);
            AddcategoryAttribute(xmlchart, 0, 0, "Name", "Jan");
            Addcategory(xmlchart, 0);
            AddcategoryAttribute(xmlchart, 0, 1, "Name", "Feb");
            Addcategory(xmlchart, 0);
            AddcategoryAttribute(xmlchart, 0, 2, "Name", "Mar");

            Adddataset(xmlchart);
            AdddatasetAttribute(xmlchart, 0, "seriesName", "Current Year");
            AdddatasetAttribute(xmlchart, 0, "color", "A66EDD");
            AdddatasetAttribute(xmlchart, 0, "anchorBorderColor", "A66EDD");
            AdddatasetAttribute(xmlchart, 0, "anchorRadius", "4");
            Addset(xmlchart, 0);
            AddsetAttribute(xmlchart, 0, 0, "value", "927654");
            Addset(xmlchart, 0);
            AddsetAttribute(xmlchart, 0, 1, "value", "917654");
            Addset(xmlchart, 0);
            AddsetAttribute(xmlchart, 0, 2, "value", "987654");

            Adddataset(xmlchart);
            AdddatasetAttribute(xmlchart, 1, "seriesName", "Current Year");
            AdddatasetAttribute(xmlchart, 1, "color", "A66EDD");
            AdddatasetAttribute(xmlchart, 1, "anchorBorderColor", "A66EDD");
            AdddatasetAttribute(xmlchart, 1, "anchorRadius", "4");
            Addset(xmlchart, 1);
            AddsetAttribute(xmlchart, 1, 0, "value", "827654");
            Addset(xmlchart, 1);
            AddsetAttribute(xmlchart, 1, 1, "value", "817654");
            Addset(xmlchart, 1);
            AddsetAttribute(xmlchart, 1, 2, "value", "887654");




            return chartXMLtoJson(xmlchart);
        }

        public static string GetLineChartXmlDataDemo2()
        {
            DataTable dt = new DataTable();
            DelegateChart chart = delegate(XmlDocument xmlchart)
            {
                AddgraphAttribute(xmlchart, "numdivlines", "4");
                AddgraphAttribute(xmlchart, "lineThickness", "3");
                AddgraphAttribute(xmlchart, "showValues", "0");
                AddgraphAttribute(xmlchart, "numVDivLines", "10");
                AddgraphAttribute(xmlchart, "formatNumberScale", "1");
                AddgraphAttribute(xmlchart, "rotateNames", "1");
                AddgraphAttribute(xmlchart, "decimalPrecision", "1");
                AddgraphAttribute(xmlchart, "anchorRadius", "2");
                AddgraphAttribute(xmlchart, "anchorBgAlpha", "0");
                AddgraphAttribute(xmlchart, "numberPrefix", "$");
                AddgraphAttribute(xmlchart, "divLineAlpha", "30");
                AddgraphAttribute(xmlchart, "showAlternateHGridColor", "1");
                AddgraphAttribute(xmlchart, "yAxisMinValue", "800000");
                AddgraphAttribute(xmlchart, "shadowAlpha", "50");
                return xmlchart;//?
            };
            return CreateLineXMLData(dt, chart);
        }

        public static string CreateLineXMLData(DataTable dt, DelegateChart chart)
        {
            XmlDocument xmlchart = CreateChartXML();

            AddgraphAttribute(xmlchart, "numdivlines", "4");
            AddgraphAttribute(xmlchart, "lineThickness", "3");
            AddgraphAttribute(xmlchart, "showValues", "0");
            AddgraphAttribute(xmlchart, "numVDivLines", "10");
            AddgraphAttribute(xmlchart, "formatNumberScale", "1");
            AddgraphAttribute(xmlchart, "rotateNames", "1");
            AddgraphAttribute(xmlchart, "decimalPrecision", "1");
            AddgraphAttribute(xmlchart, "anchorRadius", "2");
            AddgraphAttribute(xmlchart, "anchorBgAlpha", "0");
            AddgraphAttribute(xmlchart, "numberPrefix", "$");
            AddgraphAttribute(xmlchart, "divLineAlpha", "30");
            AddgraphAttribute(xmlchart, "showAlternateHGridColor", "1");
            AddgraphAttribute(xmlchart, "yAxisMinValue", "800000");
            AddgraphAttribute(xmlchart, "shadowAlpha", "50");

            Addcategories(xmlchart);
            //创建X轴
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Addcategory(xmlchart, 0);
                AddcategoryAttribute(xmlchart, 0, i, "Name", dt.Rows[i][0].ToString());
            }

            //画线
            for (int i = 1; i < dt.Columns.Count; i++)//多少条线
            {
                Adddataset(xmlchart);
                AdddatasetAttribute(xmlchart, i - 1, "seriesName", "Current Year");
                AdddatasetAttribute(xmlchart, i - 1, "color", "A66EDD");
                AdddatasetAttribute(xmlchart, i - 1, "anchorBorderColor", "A66EDD");
                AdddatasetAttribute(xmlchart, i - 1, "anchorRadius", "4");

                for (int j = 0; j < dt.Rows.Count; j++)//画每条线的点的位置
                {
                    Addset(xmlchart, i-1);
                    AddsetAttribute(xmlchart, i - 1, j, "value", dt.Rows[j][i].ToString());
                }
            }

            if (chart != null)
                chart(xmlchart);
            return  chartXML(xmlchart);
        }

        public static string CreateLineXMLData(DataTable dt, Hashtable chartAttribute, string categoriesColumn, string[] valueColumns, Hashtable[] valueAttributes)
        {
            DelegateChart chart = delegate(XmlDocument xmlchart)
            {
                AddgraphAttribute(xmlchart, "numdivlines", "4");
                AddgraphAttribute(xmlchart, "lineThickness", "3");
                AddgraphAttribute(xmlchart, "showValues", "0");
                AddgraphAttribute(xmlchart, "numVDivLines", "10");
                AddgraphAttribute(xmlchart, "formatNumberScale", "1");
                AddgraphAttribute(xmlchart, "rotateNames", "1");
                AddgraphAttribute(xmlchart, "decimalPrecision", "1");
                AddgraphAttribute(xmlchart, "anchorRadius", "2");
                AddgraphAttribute(xmlchart, "anchorBgAlpha", "0");
                AddgraphAttribute(xmlchart, "numberPrefix", "$");
                AddgraphAttribute(xmlchart, "divLineAlpha", "30");
                AddgraphAttribute(xmlchart, "showAlternateHGridColor", "1");
                AddgraphAttribute(xmlchart, "yAxisMinValue", "800000");
                AddgraphAttribute(xmlchart, "shadowAlpha", "50");
                return xmlchart;//?
            };
            return CreateLineXMLData(dt, chart, chartAttribute, categoriesColumn, valueColumns, valueAttributes);
        }
        /// <summary>
        /// 创建图表线数据源的方法
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="chart">回调函数用来数据源属性</param>
        /// <param name="chartAttribute">数据源根节点属性</param>
        /// <param name="categoriesColumn">指定dt数据集中列名一致的列值为X轴内容</param>
        /// <param name="valueColumns">指定dt数据集中列名一致的数组列值为线的值，可以有多条线</param>
        /// <param name="valueAttributes">给每条线赋相关属性值</param>
        /// <returns></returns>
        public static string CreateLineXMLData(DataTable dt, DelegateChart chart, Hashtable chartAttribute, string categoriesColumn, string[] valueColumns, Hashtable[] valueAttributes)
        {
            XmlDocument xmlchart = CreateChartXML();
            AddgraphAttribute(xmlchart, "caption", "XXXXXX统计");//主标题
            AddgraphAttribute(xmlchart, "subcaption", "2009年");//子标题
            AddgraphAttribute(xmlchart, "xAxisName", "月份");//x轴标题
            AddgraphAttribute(xmlchart, "yAxisName", "销售额");//y轴标题
            AddgraphAttribute(xmlchart, "yAxisMinValue", "800000");//y轴最小值
            AddgraphAttribute(xmlchart, "numVDivLines", "10");//y抽分隔线条数
            AddgraphAttribute(xmlchart, "numdivlines", "4");
            AddgraphAttribute(xmlchart, "numberPrefix", "$");//y轴值得单位
            AddgraphAttribute(xmlchart, "lineThickness", "3");//折线的粗细
            AddgraphAttribute(xmlchart, "showValues", "0");
            AddgraphAttribute(xmlchart, "formatNumberScale", "1");
            AddgraphAttribute(xmlchart, "rotateNames", "1");
            AddgraphAttribute(xmlchart, "decimalPrecision", "1");
            AddgraphAttribute(xmlchart, "anchorRadius", "2");
            AddgraphAttribute(xmlchart, "anchorBgAlpha", "0");
            AddgraphAttribute(xmlchart, "divLineAlpha", "30");
            AddgraphAttribute(xmlchart, "showAlternateHGridColor", "1");
            AddgraphAttribute(xmlchart, "shadowAlpha", "50");


            if (chartAttribute != null)
            {
                foreach (DictionaryEntry de in chartAttribute)
                {
                    AddgraphAttribute(xmlchart, de.Key.ToString(), de.Value.ToString());
                }
            }

            Addcategories(xmlchart);
            //创建X轴
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Addcategory(xmlchart, 0);
                AddcategoryAttribute(xmlchart, 0, i, "Name", dt.Rows[i][categoriesColumn].ToString());
            }

            //画线
            for (int i = 0; i < valueColumns.Length; i++)//多少条线
            {
                Adddataset(xmlchart);
                AdddatasetAttribute(xmlchart, i, "seriesName", "Current Year");
                AdddatasetAttribute(xmlchart, i, "color", "A66EDD");
                AdddatasetAttribute(xmlchart, i, "anchorBorderColor", "A66EDD");
                AdddatasetAttribute(xmlchart, i, "anchorRadius", "4");

                if (valueAttributes != null)
                {
                    foreach (DictionaryEntry de in valueAttributes[i])
                    {
                        AdddatasetAttribute(xmlchart, i, de.Key.ToString(), de.Value.ToString());
                    }
                }

                for (int j = 0; j < dt.Rows.Count; j++)//画每条线的点的位置
                {
                    Addset(xmlchart, i);
                    AddsetAttribute(xmlchart, i, j, "value", dt.Rows[j][valueColumns[i]].ToString());
                }
            }

            if (chart != null)
                chart(xmlchart);

            return chartXML(xmlchart);
        }

        /// <summary>
        /// 创建图表具状图数据源的方法
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="chart">回调函数用来数据源属性</param>
        /// <param name="chartAttribute">数据源根节点属性</param>
        /// <param name="categoriesColumn">指定dt数据集中列名一致的列值为X轴内容</param>
        /// <param name="valueColumns">指定dt数据集中列名一致的数组列值为线的值，可以有多条线</param>
        /// <param name="valueAttributes">给每条线赋相关属性值</param>
        /// <returns></returns>
        public static string CreateColumnXMLData(DataTable dt, DelegateChart chart, Hashtable chartAttribute, string categoriesColumn, string[] valueColumns, Hashtable[] valueAttributes)
        {
            XmlDocument xmlchart = CreateChartXML();
            AddgraphAttribute(xmlchart, "caption", "XXXXXX统计");//主标题
            AddgraphAttribute(xmlchart, "subcaption", "2009年");//子标题
            AddgraphAttribute(xmlchart, "xAxisName", "月份");//x轴标题
            AddgraphAttribute(xmlchart, "yAxisName", "销售额");//y轴标题
            AddgraphAttribute(xmlchart, "yAxisMinValue", "800000");//y轴最小值
            AddgraphAttribute(xmlchart, "numVDivLines", "10");//y抽分隔线条数
            AddgraphAttribute(xmlchart, "numdivlines", "4");
            AddgraphAttribute(xmlchart, "numberPrefix", "$");//y轴值得单位
            AddgraphAttribute(xmlchart, "lineThickness", "3");//折线的粗细
            AddgraphAttribute(xmlchart, "showValues", "0");
            AddgraphAttribute(xmlchart, "formatNumberScale", "1");
            AddgraphAttribute(xmlchart, "rotateNames", "1");
            AddgraphAttribute(xmlchart, "decimalPrecision", "1");
            AddgraphAttribute(xmlchart, "anchorRadius", "2");
            AddgraphAttribute(xmlchart, "anchorBgAlpha", "0");
            AddgraphAttribute(xmlchart, "divLineAlpha", "30");
            AddgraphAttribute(xmlchart, "showAlternateHGridColor", "1");
            AddgraphAttribute(xmlchart, "shadowAlpha", "50");


            if (chartAttribute != null)
            {
                foreach (DictionaryEntry de in chartAttribute)
                {
                    AddgraphAttribute(xmlchart, de.Key.ToString(), de.Value.ToString());
                }
            }

            Addcategories(xmlchart);
            //创建X轴
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Addcategory(xmlchart, 0);
                AddcategoryAttribute(xmlchart, 0, i, "Name", dt.Rows[i][categoriesColumn].ToString());
            }

            //画线
            for (int i = 0; i < valueColumns.Length; i++)//多少条线
            {
                Adddataset(xmlchart);
                AdddatasetAttribute(xmlchart, i, "seriesName", "Current Year");
                AdddatasetAttribute(xmlchart, i, "color", "A66EDD");
                AdddatasetAttribute(xmlchart, i, "anchorBorderColor", "A66EDD");
                AdddatasetAttribute(xmlchart, i, "anchorRadius", "4");

                if (valueAttributes != null)
                {
                    foreach (DictionaryEntry de in valueAttributes[i])
                    {
                        AdddatasetAttribute(xmlchart, i, de.Key.ToString(), de.Value.ToString());
                    }
                }

                for (int j = 0; j < dt.Rows.Count; j++)//画每条线的点的位置
                {
                    Addset(xmlchart, i);
                    AddsetAttribute(xmlchart, i, j, "value", dt.Rows[j][valueColumns[i]].ToString());
                }
            }

            if (chart != null)
                chart(xmlchart);

            return chartXML(xmlchart);
        }

        /// <summary>
        /// 创建图表饼图数据源的方法
        /// </summary>
        /// <param name="dt">数据集</param>
        /// <param name="chart">回调函数用来数据源属性</param>
        /// <param name="chartAttribute">数据源根节点属性</param>
        /// <param name="categoriesColumn">指定dt数据集中列名一致的列值为X轴内容</param>
        /// <param name="valueColumns">指定dt数据集中列名一致的数组列值为线的值，可以有多条线</param>
        /// <param name="valueAttributes">给每条线赋相关属性值</param>
        /// <returns></returns>
        public static string CreatePieXMLData(DataTable dt, DelegateChart chart, Hashtable chartAttribute, string categoriesColumn, string[] valueColumns, Hashtable[] valueAttributes)
        {
            XmlDocument xmlchart = CreateChartXML();
            AddgraphAttribute(xmlchart, "caption", "XXXXXX统计");//主标题
            AddgraphAttribute(xmlchart, "subcaption", "2009年");//子标题
            AddgraphAttribute(xmlchart, "xAxisName", "月份");//x轴标题
            AddgraphAttribute(xmlchart, "yAxisName", "销售额");//y轴标题
            AddgraphAttribute(xmlchart, "yAxisMinValue", "800000");//y轴最小值
            AddgraphAttribute(xmlchart, "numVDivLines", "10");//y抽分隔线条数
            AddgraphAttribute(xmlchart, "numdivlines", "4");
            AddgraphAttribute(xmlchart, "numberPrefix", "$");//y轴值得单位
            AddgraphAttribute(xmlchart, "lineThickness", "3");//折线的粗细
            AddgraphAttribute(xmlchart, "showValues", "0");
            AddgraphAttribute(xmlchart, "formatNumberScale", "1");
            AddgraphAttribute(xmlchart, "rotateNames", "1");
            AddgraphAttribute(xmlchart, "decimalPrecision", "1");
            AddgraphAttribute(xmlchart, "anchorRadius", "2");
            AddgraphAttribute(xmlchart, "anchorBgAlpha", "0");
            AddgraphAttribute(xmlchart, "divLineAlpha", "30");
            AddgraphAttribute(xmlchart, "showAlternateHGridColor", "1");
            AddgraphAttribute(xmlchart, "shadowAlpha", "50");


            if (chartAttribute != null)
            {
                foreach (DictionaryEntry de in chartAttribute)
                {
                    AddgraphAttribute(xmlchart, de.Key.ToString(), de.Value.ToString());
                }
            }

            Addcategories(xmlchart);
            //创建X轴
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Addcategory(xmlchart, 0);
                AddcategoryAttribute(xmlchart, 0, i, "Name", dt.Rows[i][categoriesColumn].ToString());
            }

            //画线
            for (int i = 0; i < valueColumns.Length; i++)//多少条线
            {
                Adddataset(xmlchart);
                AdddatasetAttribute(xmlchart, i, "seriesName", "Current Year");
                AdddatasetAttribute(xmlchart, i, "color", "A66EDD");
                AdddatasetAttribute(xmlchart, i, "anchorBorderColor", "A66EDD");
                AdddatasetAttribute(xmlchart, i, "anchorRadius", "4");

                if (valueAttributes != null)
                {
                    foreach (DictionaryEntry de in valueAttributes[i])
                    {
                        AdddatasetAttribute(xmlchart, i, de.Key.ToString(), de.Value.ToString());
                    }
                }

                for (int j = 0; j < dt.Rows.Count; j++)//画每条线的点的位置
                {
                    Addset(xmlchart, i);
                    AddsetAttribute(xmlchart, i, j, "value", dt.Rows[j][valueColumns[i]].ToString());
                }
            }

            if (chart != null)
                chart(xmlchart);

            return chartXML(xmlchart);
        }
    }
}

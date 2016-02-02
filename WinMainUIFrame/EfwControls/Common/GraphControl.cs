using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ZedGraph;

namespace EfwControls.Common
{
    public enum DataTableStruct
    {
        /// <summary>
        /// 以数据表的字段为X轴刻度参照,此时每个刻度内的元素为数据表内的纪录
        /// </summary>
        Columns,
        /// <summary>
        /// 以数据表的纪录数为X轴刻度参照,此时每个刻度内的元素为数据表的字段
        /// </summary>
        Rows
    }
    public struct TableColumn
    {
        public string ColumnName;
        public string ColumnField;
    }
    /// <summary>
    /// 图像控制类
    /// </summary>
    public abstract class GraphControl
    {
        /// <summary>
        /// 指定的图像显示容器
        /// </summary>
        private System.Windows.Forms.Control container;
        /// <summary>
        /// 指定X轴的刻度参照类型(以纪录数为刻度参照或以字段为参照)
        /// </summary>
        private DataTableStruct xAxisScaleRefrence;
        /// <summary>
        /// 要显示的数据
        /// </summary>
        private System.Data.DataTable dataSource;
        /// <summary>
        /// 中文名称列名
        /// </summary>
        private string cnNameColumn;
        /// <summary>
        /// 图形标题
        /// </summary>
        private string graphTitle = "";
        /// <summary>
        /// X轴标题
        /// </summary>
        private string xAxisTitle = "";
        /// <summary>
        /// Y轴标题
        /// </summary>
        private string yAxisTitle = "";
        /// <summary>
        /// 数据表列信息
        /// </summary>
        private TableColumn[] showValueColumns;
        private Color[] colors;
        /// <summary>
        /// 指定的图像显示容器
        /// </summary>
        public System.Windows.Forms.Control GraphContainer
        {
            get
            {
                return container;
            }
            set
            {
                container = value;
            }
        }
        /// <summary>
        /// 指定X轴的数据来源
        /// </summary>
        public DataTableStruct XAxisScaleRefrence
        {
            get
            {
                return xAxisScaleRefrence;
            }
            set
            {
                xAxisScaleRefrence = value;
            }
        }
        /// <summary>
        /// 要显示的数据
        /// </summary>
        public System.Data.DataTable DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {
                dataSource = value;
            }
        }
        /// <summary>
        /// 中文列字段名称
        /// </summary>
        public string CNNameColumn
        {
            get
            {
                return cnNameColumn;
            }
            set
            {
                cnNameColumn = value;
            }
        }
        /// <summary>
        /// 图形标题
        /// </summary>
        public string GraphTitle
        {
            get
            {
                return graphTitle;
            }
            set
            {
                graphTitle = value;
            }
        }
        /// <summary>
        /// X轴标题
        /// </summary>
        public string XAxisTitle
        {
            get
            {
                return xAxisTitle;
            }
            set
            {
                xAxisTitle = value;
            }
        }
        /// <summary>
        /// Y轴标题
        /// </summary>
        public string YAxisTitle
        {
            get
            {
                return yAxisTitle;
            }
            set
            {
                yAxisTitle = value;
            }
        }
        /// <summary>
        /// 数据表的数据列信息(不包含名称列)
        /// </summary>
        public TableColumn[] ShowValueColumns
        {
            get
            {
                return showValueColumns;
            }
            set
            {
                showValueColumns = value;
            }
        }
        public Color[] Colors
        {
            get
            {
                return colors;
            }
            set
            {
                colors = value;
            }
        }
        public System.Drawing.Color GetColor()
        {
            Random rd = new Random();
            int red = rd.Next(0, 255);
            int green = rd.Next(0, 255);
            int blue = rd.Next(0, 255);

            return System.Drawing.Color.FromArgb(red, green, blue);

        }
        /// <summary>
        /// 画图
        /// </summary>
        public abstract void DrawGraph();
    }
    /// <summary>
    /// 柱状图
    /// </summary>
    public class HistogramGraphControl : GraphControl
    {
        /// <summary>
        /// 实例化柱状图类
        /// </summary>
        /// <param name="GraphContainer">图形显示所需的容器</param>
        /// <param name="XAxisScaleRefrence">X轴刻度参照类型</param>
        /// <param name="ShowValueColumns">需要显示的值的列,和传入的DataSource的列对应(仅数值列)</param>
        /// <param name="HistogramColors">每个刻度内的柱的颜色，数组长度取决于每个刻度内的柱数</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="CNNameColumn">数据源中的名称列的字段名</param>
        public HistogramGraphControl(System.Windows.Forms.Control GraphContainer,
            DataTableStruct XAxisScaleRefrence, TableColumn[] ShowValueColumns, Color[] HistogramColors, System.Data.DataTable DataSource,
            string CNNameColumn)
        {
            base.GraphContainer = GraphContainer;
            base.XAxisScaleRefrence = XAxisScaleRefrence;
            base.ShowValueColumns = ShowValueColumns;
            base.DataSource = DataSource;
            base.CNNameColumn = CNNameColumn;
            base.Colors = HistogramColors;
        }
        /// <summary>
        /// 实例化柱状图类
        /// </summary>
        /// <param name="GraphContainer">图形显示所需的容器</param>
        /// <param name="XAxisScaleRefrence">X轴刻度参照类型</param>
        /// <param name="ShowValueColumns">需要显示的值的列,和传入的DataSource的列对应(仅数值列)</param>
        /// <param name="HistogramColors">每个刻度内的柱的颜色，数组长度取决于每个刻度内的柱数</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="CNNameColumn">数据源中的名称列的字段名</param>
        /// <param name="GraphTitle">图形的标题，显示在图形的正上方</param>
        public HistogramGraphControl(System.Windows.Forms.Control GraphContainer,
            DataTableStruct XAxisScaleRefrence, TableColumn[] ShowValueColumns, Color[] HistogramColors, System.Data.DataTable DataSource,
            string CNNameColumn, string GraphTitle)
        {
            base.GraphContainer = GraphContainer;
            base.XAxisScaleRefrence = XAxisScaleRefrence;
            base.ShowValueColumns = ShowValueColumns;
            base.DataSource = DataSource;
            base.CNNameColumn = CNNameColumn;
            base.Colors = HistogramColors;
            base.GraphTitle = GraphTitle;
        }
        /// <summary>
        /// 实例化柱状图类
        /// </summary>
        /// <param name="GraphContainer">图形显示所需的容器</param>
        /// <param name="XAxisScaleRefrence">X轴刻度参照类型</param>
        /// <param name="ShowValueColumns">需要显示的值的列,和传入的DataSource的列对应(仅数值列)</param>
        /// <param name="HistogramColors">每个刻度内的柱的颜色，数组长度取决于每个刻度内的柱数</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="CNNameColumn">数据源中的名称列的字段名</param>
        /// <param name="GraphTitle">图形的标题，显示在图形的正上方</param>
        /// <param name="XAxisTitle">X轴标题</param>
        /// <param name="YAxisTitle">Y轴标题</param>
        public HistogramGraphControl(System.Windows.Forms.Control GraphContainer,
            DataTableStruct XAxisScaleRefrence, TableColumn[] ShowValueColumns, Color[] HistogramColors, System.Data.DataTable DataSource,
            string CNNameColumn, string GraphTitle, string XAxisTitle, string YAxisTitle)
        {
            base.GraphContainer = GraphContainer;
            base.XAxisScaleRefrence = XAxisScaleRefrence;
            base.ShowValueColumns = ShowValueColumns;
            base.DataSource = DataSource;
            base.CNNameColumn = CNNameColumn;
            base.Colors = HistogramColors;
            base.GraphTitle = GraphTitle;
            base.XAxisTitle = XAxisTitle;
            base.YAxisTitle = YAxisTitle;
        }
        /// <summary>
        /// 图形描绘
        /// </summary>
        public override void DrawGraph()
        {
            #region 计算图形所需数据
            //x轴上的项目数
            int xAxisItemCount = 0;
            //项目上的柱数
            int itemElementCount = 0;

            if (base.XAxisScaleRefrence == DataTableStruct.Rows)
            {
                xAxisItemCount = base.DataSource.Rows.Count;
                itemElementCount = base.ShowValueColumns.Length;
            }
            else
            {
                xAxisItemCount = base.ShowValueColumns.Length;
                itemElementCount = base.DataSource.Rows.Count;
            }



            //X轴元素的标签
            string[] labels = new string[xAxisItemCount];
            if (base.XAxisScaleRefrence == DataTableStruct.Rows)
            {
                //如果X轴数据取数据表的纪录行
                for (int i = 0; i < base.DataSource.Rows.Count; i++)
                    labels[i] = base.DataSource.Rows[i][base.CNNameColumn].ToString();
            }
            else
            {
                for (int i = 0; i < base.ShowValueColumns.Length; i++)
                    labels[i] = base.ShowValueColumns[i].ColumnName;
            }
            //图像显示需要的值 ,两层数组,
            //第一层数组为X轴上的项目的个数(X1,X2,X3,X4...),长度和labels长度一致
            //第二层数组为每个项目的第N个值,长度为每个项目内的元素个数如 
            //{F1.Value1,F1.Value1,F3.Value1....} 
            //{F1.Value2,F1.Value2,F3.Value2....} 
            //{F1.Value3,F1.Value3,F3.Value3....} 
            double[][] values = new double[itemElementCount][];
            //填充数组;
            for (int i = 0; i < itemElementCount; i++)
            {
                values[i] = new double[xAxisItemCount]; //长度为纪录数

                for (int j = 0; j < xAxisItemCount; j++)
                {
                    if (XAxisScaleRefrence == DataTableStruct.Rows)
                    {
                        if (Convert.IsDBNull(base.DataSource.Rows[j][base.ShowValueColumns[i].ColumnField]))
                            values[i][j] = 0;
                        else
                            values[i][j] = Convert.ToDouble(base.DataSource.Rows[j][base.ShowValueColumns[i].ColumnField]);
                    }
                    else
                    {
                        if (Convert.IsDBNull(base.DataSource.Rows[i][base.ShowValueColumns[j].ColumnField]))
                            values[i][j] = 0;
                        else
                            values[i][j] = Convert.ToDouble(base.DataSource.Rows[i][base.ShowValueColumns[j].ColumnField]);
                    }
                }
            }
            //X轴下方的标签
            string[] markText;
            if (base.XAxisScaleRefrence == DataTableStruct.Columns)
            {

                markText = new string[base.DataSource.Rows.Count];
                for (int i = 0; i < base.DataSource.Rows.Count; i++)
                    markText[i] = base.DataSource.Rows[i][CNNameColumn].ToString();
            }
            else
            {
                markText = new string[base.ShowValueColumns.Length];
                for (int i = 0; i < base.ShowValueColumns.Length; i++)
                    markText[i] = base.ShowValueColumns[i].ColumnName;
            }
            #endregion
            //Console.Beep();
            #region 显示图形
            base.GraphContainer.Controls.Clear();

            ZedGraph.ZedGraphControl grphTx = new ZedGraph.ZedGraphControl();

            grphTx.IsEnableHZoom = false;
            grphTx.IsEnableVZoom = false;
            grphTx.IsEnableWheelZoom = false;
            grphTx.Dock = System.Windows.Forms.DockStyle.Fill;


            ZedGraph.GraphPane myPane = grphTx.GraphPane;
            myPane.Title.Text = base.GraphTitle;
            myPane.XAxis.Title.Text = base.XAxisTitle;
            myPane.YAxis.Title.Text = base.YAxisTitle;

            ////添加显示条
            ZedGraph.BarItem bar;

            for (int i = 0; i < values.Length; i++)
            {
                double[] y = values[i];
                bar = myPane.AddBar(markText[i], null, y, Color.Red);
                //颜色如果没有指定,随机取
                Color color;
                if (base.Colors == null)
                    color = GetColor();
                else
                    color = base.Colors[i];

                bar.Bar.Fill = new ZedGraph.Fill(color, Color.White, color);
            }

            myPane.XAxis.MajorTic.IsBetweenLabels = true;
            myPane.XAxis.Scale.TextLabels = labels;
            myPane.XAxis.Scale.FontSpec.Angle = 90;
            myPane.XAxis.Scale.FontSpec.Size = 11F;
            myPane.XAxis.Type = ZedGraph.AxisType.Text;
            myPane.Chart.Fill = new ZedGraph.Fill(Color.White, Color.FromArgb(255, 255, 166), 90F);
            myPane.Fill = new ZedGraph.Fill(Color.FromArgb(250, 250, 255));

            grphTx.AxisChange();

            base.GraphContainer.Controls.Add(grphTx);
            #endregion
        }
    }
    /// <summary>
    /// 线形图
    /// </summary>
    public class LineGraphControl : GraphControl
    {
        /// <summary>
        /// 实例化线形图
        /// </summary>
        /// <param name="GraphContainer">图形显示所需要的容器</param>
        /// <param name="XAxisScaleRefrence">X轴刻度参照类型</param>
        /// <param name="ShowValueColumns">需要显示的值的列,和传入的DataSource的列对应(仅数值列)</param>
        /// <param name="LineColors">每条线的颜色，数组</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="CNNameColumn">数据源中的中文名称列的字段名</param>
        public LineGraphControl(System.Windows.Forms.Control GraphContainer,
            DataTableStruct XAxisScaleRefrence, TableColumn[] ShowValueColumns, Color[] LineColors, System.Data.DataTable DataSource,
            string CNNameColumn)
        {
            base.GraphContainer = GraphContainer;
            base.XAxisScaleRefrence = XAxisScaleRefrence;
            base.ShowValueColumns = ShowValueColumns;
            base.Colors = LineColors;
            base.DataSource = DataSource;
            base.CNNameColumn = CNNameColumn;
        }
        /// <summary>
        /// 实例化线形图
        /// </summary>
        /// <param name="GraphContainer">图形显示所需要的容器</param>
        /// <param name="XAxisScaleRefrence">X轴刻度参照类型</param>
        /// <param name="ShowValueColumns">需要显示的值的列,和传入的DataSource的列对应(仅数值列)</param>
        /// <param name="LineColors">每条线的颜色，数组</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="CNNameColumn">数据源中的中文名称列的字段名</param>
        /// <param name="GraphTitle">图形的标题，位于整个图形的正上方</param>
        public LineGraphControl(System.Windows.Forms.Control GraphContainer,
            DataTableStruct XAxisScaleRefrence, TableColumn[] ShowValueColumns, Color[] LineColors, System.Data.DataTable DataSource,
            string CNNameColumn, string GraphTitle)
        {
            base.GraphContainer = GraphContainer;
            base.XAxisScaleRefrence = XAxisScaleRefrence;
            base.ShowValueColumns = ShowValueColumns;
            base.Colors = LineColors;
            base.DataSource = DataSource;
            base.CNNameColumn = CNNameColumn;
            base.GraphTitle = GraphTitle;
        }
        /// <summary>
        /// 实例化线形图
        /// </summary>
        /// <param name="GraphContainer">图形显示所需要的容器</param>
        /// <param name="XAxisScaleRefrence">X轴刻度参照类型</param>
        /// <param name="ShowValueColumns">需要显示的值的列,和传入的DataSource的列对应(仅数值列)</param>
        /// <param name="LineColors">每条线的颜色，数组</param>
        /// <param name="DataSource">数据源</param>
        /// <param name="CNNameColumn">数据源中的中文名称列的字段名</param>
        /// <param name="GraphTitle">图形的标题，位于整个图形的正上方</param>
        /// <param name="XAxisTitle">x轴的标题</param>
        /// <param name="YAxisTitle">y轴的标题</param>
        public LineGraphControl(System.Windows.Forms.Control GraphContainer,
            DataTableStruct XAxisScaleRefrence, TableColumn[] ShowValueColumns, Color[] LineColors, System.Data.DataTable DataSource,
            string CNNameColumn, string GraphTitle, string XAxisTitle, string YAxisTitle)
        {
            base.GraphContainer = GraphContainer;
            base.XAxisScaleRefrence = XAxisScaleRefrence;
            base.ShowValueColumns = ShowValueColumns;
            base.Colors = LineColors;
            base.DataSource = DataSource;
            base.CNNameColumn = CNNameColumn;
            base.GraphTitle = GraphTitle;
            base.XAxisTitle = XAxisTitle;
            base.YAxisTitle = YAxisTitle;
        }
        /// <summary>
        /// 图形描绘
        /// </summary>
        public override void DrawGraph()
        {
            #region 图形所需数据
            //线条数组[线条数] [每条线的刻度值]
            double[][] lines;
            //X轴刻度
            double[] xAixsScale;
            //线条标注
            string[] labels;
            //Ｘ轴刻度标注
            string[] xAxisScaleLabels;

            if (base.XAxisScaleRefrence == DataTableStruct.Rows)
            {
                xAixsScale = new double[base.DataSource.Rows.Count];
                xAxisScaleLabels = new string[base.DataSource.Rows.Count];
                for (int i = 0; i < base.DataSource.Rows.Count; i++)
                {
                    xAixsScale[i] = i;
                    xAxisScaleLabels[i] = base.DataSource.Rows[i][base.CNNameColumn].ToString();
                }
                //要绘制的线条数为数据列数
                lines = new double[base.ShowValueColumns.Length][];
                //线条标注
                labels = new string[lines.Length];
                for (int i = 0; i < base.ShowValueColumns.Length; i++)
                    labels[i] = base.ShowValueColumns[i].ColumnName;


                //填充每条线的数据值
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = new double[xAixsScale.Length];
                    for (int j = 0; j < base.DataSource.Rows.Count; j++)
                    {
                        if (Convert.IsDBNull(base.DataSource.Rows[j][base.ShowValueColumns[i].ColumnField]))
                            lines[i][j] = 0;
                        else
                            lines[i][j] = Convert.ToDouble(base.DataSource.Rows[j][base.ShowValueColumns[i].ColumnField]);
                    }
                }
            }
            else
            {
                xAixsScale = new double[base.ShowValueColumns.Length];
                xAxisScaleLabels = new string[base.ShowValueColumns.Length];
                for (int i = 0; i < base.ShowValueColumns.Length; i++)
                {
                    xAixsScale[i] = i;
                    xAxisScaleLabels[i] = base.ShowValueColumns[i].ColumnName;
                }
                //要绘制的线条数为数据集的纪录数
                lines = new double[base.DataSource.Rows.Count][];
                //线条标注
                labels = new string[lines.Length];
                for (int i = 0; i < base.DataSource.Rows.Count; i++)
                    labels[i] = base.DataSource.Rows[i][base.CNNameColumn].ToString();
                //填充每条线的数据值
                for (int i = 0; i < lines.Length; i++)
                {
                    lines[i] = new double[xAixsScale.Length];
                    for (int j = 0; j < base.ShowValueColumns.Length; j++)
                    {
                        if (Convert.IsDBNull(base.DataSource.Rows[i][base.ShowValueColumns[j].ColumnField]))
                            lines[i][j] = 0;
                        else
                            lines[i][j] = Convert.ToDouble(base.DataSource.Rows[i][base.ShowValueColumns[j].ColumnField]);
                    }
                }
            }

            int maxXaisxScale = xAixsScale.Length;

            //Console.Beep();

            #endregion

            #region 显示图形
            base.GraphContainer.Controls.Clear();

            ZedGraph.ZedGraphControl grphTx = new ZedGraph.ZedGraphControl();

            grphTx.IsEnableHZoom = false;
            grphTx.IsEnableVZoom = false;
            grphTx.IsEnableWheelZoom = false;
            grphTx.Dock = System.Windows.Forms.DockStyle.Fill;


            ZedGraph.GraphPane myPane = grphTx.GraphPane;
            myPane.Title.Text = base.GraphTitle;
            myPane.XAxis.Title.Text = base.XAxisTitle;
            myPane.YAxis.Title.Text = base.YAxisTitle;

            myPane.Chart.Fill = new Fill(Color.FromArgb(255, 255, 245), Color.FromArgb(255, 255, 190), 90F);
            //线条显示
            for (int i = 0; i < lines.Length; i++)
            {
                Color color;
                if (base.Colors == null)
                    color = GetColor();
                else
                    color = base.Colors[i];

                ZedGraph.LineItem myCurve = myPane.AddCurve(labels[i], xAixsScale, lines[i], color);

                myCurve.Symbol.Fill = new Fill(Color.White);
            }

            //X轴属性
            myPane.XAxis.Scale.Min = 0; //X轴刻度起始值
            myPane.XAxis.Scale.Max = maxXaisxScale;//X轴刻度最大值
            myPane.XAxis.Scale.TextLabels = xAxisScaleLabels;//x轴刻度中文标注
            myPane.XAxis.Type = AxisType.Text;   //类型
            myPane.XAxis.Scale.FontSpec.Angle = 90;　//文字方向
            myPane.XAxis.Scale.FontSpec.Size = 11F;  //文字大小

            // Display the Y axis grid lines
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;


            BoxObj box = new BoxObj(0, 100, 1, 30, Color.Empty,
            Color.FromArgb(150, Color.LightGreen));
            box.Fill = new Fill(Color.White, Color.FromArgb(200, Color.LightGreen), 45.0F);

            box.ZOrder = ZOrder.E_BehindCurves;

            box.IsClippedToChartRect = true;

            box.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            myPane.GraphObjList.Add(box);

            TextObj text = new TextObj("", 0.95f, 85, CoordType.AxisXYScale,
                                    AlignH.Right, AlignV.Center);
            text.FontSpec.Fill.IsVisible = false;
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.IsBold = true;
            text.FontSpec.IsItalic = true;
            text.Location.CoordinateFrame = CoordType.XChartFractionYScale;
            text.IsClippedToChartRect = true;

            myPane.GraphObjList.Add(text);

            myPane.Fill = new Fill(Color.WhiteSmoke, Color.Lavender, 0F);

            grphTx.AxisChange();

            base.GraphContainer.Controls.Add(grphTx);
            #endregion
        }
    }
    /// <summary>
    /// 饼状图
    /// </summary>
    public class CakyGraphControl : GraphControl
    {
        /// <summary>
        /// 实例化饼状图
        /// </summary>
        /// <param name="GraphContainer">图形显示所需的容器</param>
        /// <param name="XAxisScaleRefrence">饼组成部分的数据来源．当等于Row时，整个饼状图代表一个字段,Index参数指定DataSource的列,；当等于Column时．整个饼状图代表数据表内的一行数据,具体行由参数Index指定</param>
        /// <param name="ShowValueColumns">要显示的的值的列</param>
        /// <param name="PartyColors">每部分饼状颜色，数组</param>
        /// <param name="DataSource">数据源，至少要有一列名称列和值列</param>
        /// <param name="CNNameColumn">名称列的字段名</param>
        /// <param name="Index">指定要显示的列或行的索引(相对于DataSource对象)</param>
        public CakyGraphControl(System.Windows.Forms.Control GraphContainer, DataTableStruct XAxisScaleRefrence,
            TableColumn[] ShowValueColumns, Color[] PartyColors, System.Data.DataTable DataSource,
            string CNNameColumn, int Index)
        {
            base.GraphContainer = GraphContainer;
            base.Colors = PartyColors;
            base.DataSource = DataSource;
            base.CNNameColumn = CNNameColumn;
            base.XAxisScaleRefrence = XAxisScaleRefrence;
            base.ShowValueColumns = ShowValueColumns;
            index = Index;
        }

        private int index;

        public override void DrawGraph()
        {
            string graphTitle;
            //饼组成部分标签
            string[] labels = null;
            //饼组成部分的值
            double[] values = null;
            //合计
            double total = 0;
            if (base.XAxisScaleRefrence == DataTableStruct.Rows)
            {
                labels = new string[base.DataSource.Rows.Count];
                values = new double[base.DataSource.Rows.Count];

                for (int i = 0; i < base.DataSource.Rows.Count; i++)
                {
                    labels[i] = base.DataSource.Rows[i][base.CNNameColumn].ToString();
                    if (Convert.IsDBNull(base.DataSource.Rows[i][base.ShowValueColumns[index].ColumnField]))
                    {
                        values[i] = 0;
                    }
                    else
                    {
                        values[i] = Convert.ToDouble(base.DataSource.Rows[i][base.ShowValueColumns[index].ColumnField]);
                        total += values[i];
                    }
                }

                graphTitle = base.ShowValueColumns[index].ColumnName;
            }
            else
            {
                labels = new string[base.ShowValueColumns.Length];
                values = new double[base.ShowValueColumns.Length];

                for (int i = 0; i < base.ShowValueColumns.Length; i++)
                {
                    labels[i] = base.ShowValueColumns[i].ColumnName;
                    if (Convert.IsDBNull(base.DataSource.Rows[index][base.ShowValueColumns[i].ColumnField]))
                        values[i] = 0;
                    else
                        values[i] = Convert.ToDouble(base.DataSource.Rows[index][base.ShowValueColumns[i].ColumnField]);

                    total += values[i];
                }
                graphTitle = base.DataSource.Rows[index][base.CNNameColumn].ToString();
            }

            #region 显示图形
            base.GraphContainer.Controls.Clear();

            ZedGraph.ZedGraphControl grphTx = new ZedGraph.ZedGraphControl();

            grphTx.IsEnableHZoom = false;
            grphTx.IsEnableVZoom = false;
            grphTx.IsEnableWheelZoom = false;
            grphTx.Dock = System.Windows.Forms.DockStyle.Fill;


            ZedGraph.GraphPane myPane = grphTx.GraphPane;
            myPane.Title.Text = graphTitle;
            //字体
            myPane.Fill = new Fill(Color.White, Color.Goldenrod, 45.0f);
            myPane.Chart.Fill.IsVisible = false;
            myPane.Legend.Position = LegendPos.Float;
            myPane.Legend.Location = new Location(0.95f, 0.15f, CoordType.PaneFraction, AlignH.Right, AlignV.Top);
            myPane.Legend.FontSpec.Size = 10f;
            myPane.Legend.IsHStack = false;
            myPane.Legend.IsVisible = false;
            //画饼状部分
            for (int i = 0; i < labels.Length; i++)
            {
                Color color;
                if (base.Colors == null)
                    color = GetColor();
                else
                    color = base.Colors[i];

                PieItem segment = myPane.AddPieSlice(values[i], color, color, 45f, 0, (labels[i] + ":" + values[i].ToString("0.00")));
            }

            CurveList curves = myPane.CurveList;

            TextObj text = new TextObj("合计:\n" + total.ToString(), 0.18F, 0.40F, CoordType.PaneFraction);
            text.Location.AlignH = AlignH.Right;
            text.Location.AlignV = AlignV.Bottom;
            text.FontSpec.Border.IsVisible = false;
            text.FontSpec.Fill = new Fill(Color.White, Color.FromArgb(255, 100, 100), 45F);
            text.FontSpec.StringAlignment = StringAlignment.Center;
            myPane.GraphObjList.Add(text);

            // Create a drop shadow for the total value text item
            TextObj text2 = new TextObj(text);
            text2.FontSpec.Fill = new Fill(Color.Black);
            text2.Location.X += 0.01f;
            text2.Location.Y += 0.01f;
            myPane.GraphObjList.Add(text2);


            grphTx.AxisChange();

            base.GraphContainer.Controls.Add(grphTx);
            #endregion
        }
    }
}

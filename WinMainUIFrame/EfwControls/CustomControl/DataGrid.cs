using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EfwControls.CustomControl
{
    /// <summary>
    /// 前景或背景
    /// </summary>
    public enum ForeOrBack
    {
        Fore,
        Back
    }

    /// <summary>
    /// 方向
    /// </summary>
    public enum Direction
    {
        LeftToRight,
        TopToButtom,
    }

    public partial class DataGrid : DevComponents.DotNetBar.Controls.DataGridViewX
    {
        /// <summary>
        /// 要画的线的信息列表
        /// </summary>
        private List<DataGridViewDrawLineInfo> lines;

        /// <summary>
        /// 是否允许点击ColumnHeader排序
        /// </summary>
        private bool allowSortWhenClickColumnHeader;
        [Description("获取或设置是否允许点击ColumnHeader排序")]
        public bool AllowSortWhenClickColumnHeader
        {
            get
            {
                return allowSortWhenClickColumnHeader;
            }
            set
            {
                allowSortWhenClickColumnHeader = value;
                if (this.Columns != null)
                {
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        if (value == false)
                            this.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        else
                            this.Columns[i].SortMode = DataGridViewColumnSortMode.Automatic;
                    }
                }
            }
        }

        private bool _SeqVisible = true;
        [Description("获取或设置是否显示序号")]
        public bool SeqVisible
        {
            get { return _SeqVisible; }
            set
            {
                _SeqVisible = value;
                this.RowHeadersVisible = _SeqVisible;
            }
        }


        public DataGrid()
        {
            InitializeComponent();
            this.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.BorderStyle = BorderStyle.Fixed3D;
            this.AutoGenerateColumns = false;
            this.RowHeadersWidth = 25;
            this.AllowUserToResizeRows = false;
            this.HighlightSelectedColumnHeaders = false;
            this.SelectAllSignVisible = false;
            System.Windows.Forms.DataGridViewCellStyle cellstyle = new System.Windows.Forms.DataGridViewCellStyle();
            cellstyle.BackColor = System.Drawing.Color.AliceBlue;
            this.AlternatingRowsDefaultCellStyle = cellstyle;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (lines != null)
            {
                foreach (DataGridViewDrawLineInfo line in lines)
                {
                    float lineWidth = line.Width;
                    if (lineWidth == 0)
                        lineWidth = 1F;
                    Color lineColor = line.Color;
                    if (lineColor.IsEmpty)
                        lineColor = this.DefaultCellStyle.ForeColor;

                    if (line.DrawDirection == Direction.TopToButtom)
                    {
                        #region 上到下的线
                        Rectangle startRect = new Rectangle();
                        Rectangle endRect = new Rectangle();
                        try
                        {
                            startRect = this.GetCellDisplayRectangle(line.DrawLineOfRowColumnIndex, line.StartIndexOfRowColumn, true);
                            endRect = this.GetCellDisplayRectangle(line.DrawLineOfRowColumnIndex, line.EndIndexOfRowColumn, true);
                        }
                        catch
                        {
                            throw new Exception("索引超出数组界限，原因可能：要绘制的线条的目标行或列已经被移除！");
                        }
                        Point ptStart = new Point(startRect.Left + startRect.Width / 2, startRect.Top + startRect.Height / 2);
                        Point ptEnd = new Point(endRect.Left + endRect.Width / 2, endRect.Bottom - endRect.Height / 2);

                        Point ptStart_1 = new Point(ptStart.X + this.Columns[line.DrawLineOfRowColumnIndex].Width / 2, ptStart.Y);
                        Point ptEnd_1 = new Point(ptEnd.X + this.Columns[line.DrawLineOfRowColumnIndex].Width / 2, ptEnd.Y);

                        if (ptStart.X == 0 && ptStart.Y == 0 && ptEnd.X == 0 && ptEnd.Y == 0)
                            continue;

                        if (ptStart.X == 0 && ptStart.Y == 0)
                        {
                            ptStart.X = ptEnd.X;
                            ptStart.Y = this.ColumnHeadersHeight;
                            ptStart_1.X = 0;
                            ptStart_1.Y = 0;

                        }
                        if (ptEnd.X == 0 && ptEnd.Y == 0)
                        {
                            ptEnd.X = ptStart.X;
                            ptEnd.Y = this.Height;
                            ptEnd_1.X = 0;
                            ptEnd_1.Y = 0;
                        }

                        Pen pen = new Pen(lineColor, lineWidth);
                        //开始端的横线
                        if (ptStart_1.X != 0 && ptStart_1.Y != 0)
                            e.Graphics.DrawLine(pen, ptStart, ptStart_1);
                        //竖线主体
                        e.Graphics.DrawLine(pen, ptStart, ptEnd);
                        //结束端的横线
                        if (ptEnd_1.X != 0 && ptEnd.Y != 0)
                            e.Graphics.DrawLine(pen, ptEnd, ptEnd_1);
                        #endregion
                    }
                    else
                    {
                        #region 左到右
                        Rectangle startRect = new Rectangle();
                        Rectangle endRect = new Rectangle();
                        try
                        {
                            startRect = this.GetCellDisplayRectangle(line.StartIndexOfRowColumn, line.DrawLineOfRowColumnIndex, true);
                            endRect = this.GetCellDisplayRectangle(line.EndIndexOfRowColumn, line.DrawLineOfRowColumnIndex, true);
                        }
                        catch
                        {
                            throw new Exception("索引超出数组界限，原因可能：要绘制的线条的目标行或列已经被移除！");
                        }

                        Point ptStart = new Point(startRect.Left, startRect.Top + startRect.Height / 2);
                        Point ptEnd = new Point(endRect.Left + endRect.Width, endRect.Top + endRect.Height / 2);

                        if (ptStart.X == 0 && ptStart.Y == 0 && ptEnd.X == 0 && ptEnd.Y == 0)
                            continue;

                        if (ptStart.X == 0 && ptStart.Y == 0)
                        {
                            ptStart.X = ptStart.X = this.RowHeadersWidth;
                        }
                        if (ptEnd.X == 0 && ptEnd.Y == 0)
                        {
                            ptEnd.X = this.Width;

                        }
                        Pen pen = new Pen(lineColor, lineWidth);

                        e.Graphics.DrawLine(pen, ptStart, ptEnd);

                        #endregion
                    }
                }
            }
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            if (RowHeadersVisible)
            {
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, RowHeadersWidth - 4, e.RowBounds.Height);
                System.Windows.Forms.TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                    RowHeadersDefaultCellStyle.Font, rectangle, RowHeadersDefaultCellStyle.ForeColor, System.Windows.Forms.TextFormatFlags.VerticalCenter | System.Windows.Forms.TextFormatFlags.HorizontalCenter);
            }
            base.OnRowPostPaint(e);
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            if (allowSortWhenClickColumnHeader)
                e.Column.SortMode = DataGridViewColumnSortMode.Automatic;
            else
                e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            base.OnColumnAdded(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            HitTestInfo htInfo = this.HitTest(e.X, e.Y);
            if (htInfo.Type == DataGridViewHitTestType.Cell)
            {
                if (htInfo.ColumnIndex != -1 && htInfo.RowIndex != -1)
                    this.CurrentCell = this[htInfo.ColumnIndex, htInfo.RowIndex];
            }
            else if (htInfo.Type == DataGridViewHitTestType.ColumnHeader)
            {
                if (this.Rows.Count > 0)
                {
                    int rowIndex = 0;
                    while (true)
                    {
                        if (this.Rows[rowIndex].Visible)
                            break;
                        else
                            rowIndex++;
                    }
                    this.CurrentCell = this[htInfo.ColumnIndex, rowIndex];
                }
            }
            else if (htInfo.Type == DataGridViewHitTestType.RowHeader)
            {
                int columnIndex = 0;
                while (true)
                {
                    if (this.Columns[columnIndex].Visible)
                        break;
                    else
                        columnIndex++;
                }
                this.CurrentCell = this[columnIndex, htInfo.RowIndex];
            }
        }


        /// <summary>
        /// 在指定的位置划线
        /// </summary>
        /// <param name="StartIndex">开始行或列</param>
        /// <param name="EndIndex">结束行或列</param>
        /// <param name="RowOrColumnIndex">行或列</param>
        /// <param name="direct">方向</param>
        public void DrawLines(List<DataGridViewDrawLineInfo> Lines)
        {
            lines = Lines;
            this.InvokePaint(this, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)));
        }
        /// <summary>
        /// 增加一个线条对象
        /// </summary>
        /// <param name="Line"></param>
        public void AddLine(DataGridViewDrawLineInfo Line)
        {
            if (lines == null)
                lines = new List<DataGridViewDrawLineInfo>();
            lines.Add(Line);
            this.InvokePaint(this, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)));
        }
        /// <summary>
        /// 移除一个线条对象
        /// </summary>
        /// <param name="Line"></param>
        public void RemoveLine(DataGridViewDrawLineInfo Line)
        {
            if (lines != null)
            {
                DataGridViewDrawLineInfo needRemoveLine = null;
                foreach (DataGridViewDrawLineInfo line in lines)
                {
                    if (line.StartIndexOfRowColumn == Line.StartIndexOfRowColumn && line.EndIndexOfRowColumn == Line.EndIndexOfRowColumn
                        && line.DrawLineOfRowColumnIndex == Line.DrawLineOfRowColumnIndex && line.DrawDirection == Line.DrawDirection)
                    {
                        needRemoveLine = line;
                        break;
                    }
                }
                if (needRemoveLine != null)
                {
                    lines.Remove(needRemoveLine);
                    this.InvokePaint(this, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)));
                }
            }
        }
        /// <summary>
        /// 清除所有线条
        /// </summary>
        public void ClearLines()
        {
            lines = null;
            this.InvokePaint(this, new PaintEventArgs(this.CreateGraphics(), new Rectangle(0, 0, this.Width, this.Height)));
        }
        /// <summary>
        /// 设置行颜色
        /// </summary>
        /// <param name="forceColor">前景色</param>
        /// <param name="BackColor">背景色</param>
        public void SetRowColor(int RowIndex, Color foreColor, Color backColor)
        {
            for (int ColIndex = 0; ColIndex < this.Columns.Count; ColIndex++)
            {
                //this.Rows[RowIndex].DefaultCellStyle
                this[ColIndex, RowIndex].Style.ForeColor = foreColor;
                this[ColIndex, RowIndex].Style.BackColor = backColor;

            }
        }
        /// <summary>
        /// 设置行颜色
        /// </summary>
        /// <param name="forceColor">前景色或者背景色</param>
        /// <param name="forceColor">指示是否是前景色</param>
        public void SetRowColor(int RowIndex, Color color, bool IsforceColor)
        {
            for (int ColIndex = 0; ColIndex < this.Columns.Count; ColIndex++)
            {
                if (IsforceColor)
                    this[ColIndex, RowIndex].Style.ForeColor = color;
                else
                    this[ColIndex, RowIndex].Style.BackColor = color;

            }
        }
        /// <summary>
        /// 设置行颜色
        /// </summary>
        /// <param name="forceColor">前景色</param>
        /// <param name="BackColor">背景色</param>
        public void SetRowColor(int RowIndex, Color color, ForeOrBack foreOrBack)
        {
            for (int ColIndex = 0; ColIndex < this.Columns.Count; ColIndex++)
            {
                if (foreOrBack == ForeOrBack.Fore)
                    this[ColIndex, RowIndex].Style.ForeColor = color;
                else
                    this[ColIndex, RowIndex].Style.BackColor = color;
            }
        }
    }

    /// <summary>
    /// 网格画线所需的信息
    /// </summary>
    public class DataGridViewDrawLineInfo
    {
        /// <summary>
        /// 待画线的开始行（列）的索引
        /// </summary>
        private int startIndexOfRowColumn;
        /// <summary>
        /// 带画线的结束行（列）的索引
        /// </summary>
        private int endIndexOfRowColumn;
        /// <summary>
        /// 需要画线的行（列）索引
        /// </summary>
        private int drawLineOfRowColumnIndex;
        /// <summary>
        /// 画线的方向（自左到右 或者 自上到下）
        /// </summary>
        private Direction drawDirection;
        /// <summary>
        /// 线条编号
        /// </summary>
        private int index;
        /// <summary>
        /// 线条颜色
        /// </summary>
        private Color color;
        /// <summary>
        /// 线条宽度
        /// </summary>
        private float width;


        /// <summary>
        /// 待画线的开始行（列）的索引
        /// </summary>
        public int StartIndexOfRowColumn
        {
            get
            {
                return startIndexOfRowColumn;
            }
            set
            {
                startIndexOfRowColumn = value;
            }
        }
        /// <summary>
        /// 带画线的结束行（列）的索引
        /// </summary>
        public int EndIndexOfRowColumn
        {
            get
            {
                return endIndexOfRowColumn;
            }
            set
            {
                endIndexOfRowColumn = value;
            }
        }
        /// <summary>
        /// 需要画线的行（列）索引
        /// </summary>
        public int DrawLineOfRowColumnIndex
        {
            get
            {
                return drawLineOfRowColumnIndex;
            }
            set
            {
                drawLineOfRowColumnIndex = value;
            }
        }
        /// <summary>
        /// 画线的方向（自左到右 或者 自上到下）
        /// </summary>
        public Direction DrawDirection
        {
            get
            {
                return drawDirection;
            }
            set
            {
                drawDirection = value;
            }
        }
        /// <summary>
        /// 线条编号
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
        }
        /// <summary>
        /// 线条颜色
        /// </summary>
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        /// <summary>
        /// 线条宽度
        /// </summary>
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }
    }
}

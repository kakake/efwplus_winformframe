using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EfwControls.CustomControl
{
    public partial class CardDataGrid : UserControl
    {
        /// <summary>
        /// 列表头缺省默认样式
        /// </summary>
        private DataGridViewCellStyle HeadDefaultCellStyle;
        /// <summary>
        /// 列表行缺省默认样式
        /// </summary>
        private DataGridViewCellStyle RowDefaultCellStyle;

        public CardDataGrid()
        {
            InitializeComponent();
            DrawLetter();

            textdataGrid.ReadOnly = true;
            textdataGrid.MultiSelect = false;
            textdataGrid.AllowUserToAddRows = false;
            textdataGrid.AllowUserToDeleteRows = false;
            textdataGrid.AllowUserToResizeRows = false;
            textdataGrid.AutoGenerateColumns = false;
            textdataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            textdataGrid.ShowCellToolTips = false;
            textdataGrid.EnableHeadersVisualStyles = false;
            textdataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

            HeadDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            HeadDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            HeadDefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
            HeadDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            HeadDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            HeadDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            HeadDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            HeadDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            textdataGrid.ColumnHeadersDefaultCellStyle = HeadDefaultCellStyle;

            RowDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
            RowDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            RowDefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            RowDefaultCellStyle.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            RowDefaultCellStyle.ForeColor = System.Drawing.SystemColors.ControlText;
            RowDefaultCellStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            RowDefaultCellStyle.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            RowDefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            textdataGrid.DefaultCellStyle = RowDefaultCellStyle;
        }

        /// <summary>
        /// 设置表头样式
        /// </summary>
        /// <param name="dgvCellStyle"></param>
        public void SetColumnHeadDefaultCellStyle(DataGridViewCellStyle dgvCellStyle)
        {
            if (dgvCellStyle == null)
                return;

            textdataGrid.ColumnHeadersDefaultCellStyle = dgvCellStyle;
        }

        /// <summary>
        /// 设置行样式
        /// </summary>
        /// <param name="dgvCellStyle"></param>
        public void SetRowDefaultCellStyle(DataGridViewCellStyle dgvCellStyle)
        {
            if (dgvCellStyle == null)
                return;

            textdataGrid.DefaultCellStyle = dgvCellStyle;
        }

        //画字母
        public void DrawLetter()
        {

            letterpanel.Height = 20;
            letterpanel.Controls.Clear();
            Label lbl = null;
            int yCount = 0;

            #region 字母标签
            for (int i = 65, xCount = 0; i <= 93; i++, xCount++)
            {
                lbl = new Label();
                if (i < 91)
                {
                    lbl.Text = ((char)i).ToString();
                    lbl.Size = new Size(20, 20);
                }
                else if (i == 91)
                {
                    lbl.Text = "删除";
                    lbl.Size = new Size(42, 20);
                }
                else if (i == 92)
                {
                    lbl.Text = "确定";
                    lbl.Size = new Size(42, 20);
                }
                else if (i == 93)
                {
                    lbl.Text = "关闭";
                    lbl.Size = new Size(42, 20);
                }
                lbl.Font = new Font("宋体", 12F, FontStyle.Regular, GraphicsUnit.Point, ((Byte)(134)));
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.MouseHover += new EventHandler(lbl_MouseHover);
                lbl.MouseLeave += new EventHandler(lbl_MouseLeave);
                lbl.Click += new EventHandler(lblDrug_Click);
                if (i <= 91)
                    lbl.Location = new Point(xCount * 20, yCount * 20);
                else if (i == 92)
                    lbl.Location = new Point(xCount * 20 + 22, yCount * 20);
                else if (i == 93)
                    lbl.Location = new Point(xCount * 20 + 22 * 2, yCount * 20);
                if (lbl.Left + lbl.Width > this.letterpanel.Width)	//如果超出
                {
                    xCount = 0;
                    yCount++;
                    letterpanel.Height += 20;
                    if (i <= 91)
                        lbl.Location = new Point(xCount * 20, yCount * 20);
                    else if (i == 92)
                        lbl.Location = new Point(xCount * 20 + 22, yCount * 20);
                    else if (i == 93)
                        lbl.Location = new Point(xCount * 20 + 22 * 2, yCount * 20);
                }


                letterpanel.Controls.Add(lbl);
            }
            #endregion
        }

        /// <summary>
        /// 鼠标在字母上悬停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_MouseHover(object sender, System.EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.ForeColor = Color.Blue;
            lbl.Cursor = Cursors.Hand;
        }
        /// <summary>
        /// 鼠标在字母上离开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_MouseLeave(object sender, System.EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BorderStyle = BorderStyle.None;
            lbl.ForeColor = Color.Black;
            lbl.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 鼠标单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblDrug_Click(object sender, System.EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.ForeColor = Color.Red;
            if (lbl.Text == "删除")
            {
                //if (this.Text.Length > 0)
                    //this.Text = this.Text.Substring(0, this.Text.Length - 1);
                if (DeleteLetter != null)
                    DeleteLetter(sender, e);
            }
            else if (lbl.Text == "确定")
            {
                //dgvSelectionCard_DoubleClick(null, null);
                if (ConfirmLetter != null)
                    ConfirmLetter(sender, e);
            }
            else if (lbl.Text == "关闭")
            {
                //this.Hide(); //选择后隐藏选项卡
                if (CloseLetter != null)
                    CloseLetter(sender, e);
            }
            else
            {
                if (ClickLetter != null)
                    ClickLetter(sender, e);
            }
                

            //this.SelectionStart = this.Text.Length;
        }


        [Description("点击字母触发事件")]
        public event EventHandler ClickLetter;

        [Description("点击删除触发事件")]
        public event EventHandler DeleteLetter;

        [Description("点击确定触发事件")]
        public event EventHandler ConfirmLetter;

        [Description("点击关闭触发事件")]
        public event EventHandler CloseLetter;

        /// <summary>
        /// 是否本控件获取焦点
        /// </summary>
        /// <returns></returns>
        public bool GetIsFocused()
        {
            if (this.textdataGrid.Focused == true || this.textpager.comboBoxEx1.Focused == true || this.textpager.slider1.Focused == true)
            {
                return true;
            }

            return false;
        }

        private string GetGridColumnName(string DataName)
        {
            for (int i = 0; i < textdataGrid.Columns.Count; i++)
            {
                if (DataName.Trim().ToUpper() == textdataGrid.Columns[i].DataPropertyName.ToUpper())
                    return textdataGrid.Columns[i].Name;
            }
            return null;
        }

        public object GetGridValue(string DataName, int rowIndex)
        {
            //return textdataGrid[GetGridColumnName(DataName), rowIndex].Value;
            DataTable dt = (DataTable)textdataGrid.DataSource;
            return dt.Rows[rowIndex][DataName];
        }

        public void SetGridSelectRow(string DataName, object DataVal)
        {
            DataTable dt = (DataTable)textdataGrid.DataSource;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (DataVal.ToString() == GetGridValue(DataName, i).ToString())
                {
                    textdataGrid.CurrentCell = textdataGrid[0, i];
                    break;
                }
            }
        }

        public object GetGridRow(int rowIndex)
        {
            DataTable dt = (DataTable)textdataGrid.DataSource;
            return dt.Rows[rowIndex];
        }
    }
}

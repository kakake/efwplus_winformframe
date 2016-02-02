using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace EfwControls.CustomControl
{
    public partial class MultiSelectText : UserControl
    {
        private bool isLeave = true;

        public MultiSelectText()
        {
            InitializeComponent();

            textContext.ButtonCustomClick += new EventHandler(textContext_ButtonCustomClick);
            textContext.Enter += new EventHandler(textContext_Enter);
            textContext.Leave += new EventHandler(textContext_Leave);
            textContext.GotFocus += new EventHandler(textContext_GotFocus);
            btnClear.Click += new EventHandler(btnClear_Click);

            panelSelect.Visible = false;
            panelGrid.Visible = false;
            this.Height = 26;
            this.Width = 200;

        }

      

        void textContext_GotFocus(object sender, EventArgs e)
        {
            txtChar.Focus();
        }
        void textContext_ButtonCustomClick(object sender, EventArgs e)
        {
            if (multiSelectTextType == MultiSelectTextType.CheckBox)
            {
                popup.Show(textContext, selectWidth, selectHeight);
            }
            else if (multiSelectTextType == MultiSelectTextType.Grid)
            {
                popup.Show(textContext, this.Width, selectHeight);
            }
            txtChar.Focus();
        }
        void textContext_Enter(object sender, EventArgs e)
        {
            if (multiSelectTextType == MultiSelectTextType.CheckBox)
            {
                popup.Show(textContext, selectWidth, selectHeight);
            }
            else if (multiSelectTextType == MultiSelectTextType.Grid)
            {
                popup.Show(textContext, this.Width, selectHeight);
            }
        }
        void textContext_Leave(object sender, EventArgs e)
        {
            //if (multiSelectTextType == MultiSelectTextType.Grid)
            //{
            //    if (txtChar.Focused || dataGridSelect.Focused)
            //    {
            //        return;
            //    }
            //}
            //else if (multiSelectTextType == MultiSelectTextType.CheckBox)
            //{

            //}
            //if (isLeave==true)
            //    popup.Hide();

            //isLeave = true;
        }



        [Description("多选文本")]
        public string SelectText
        {
            get
            {
                return textContext.Text;
            }
        }

        private object[] _selectValue;
        [Description("多选的值")]
        public object[] SelectValue
        {
            get { return _selectValue; }
            set
            {
                _selectValue = value;
                if (SelectValueChanged != null)
                    SelectValueChanged(this, null);

                if (_datasource != null)
                {
                    //CheckBox
                    if (multiSelectTextType == MultiSelectTextType.CheckBox)
                    {
                        string text = "";
                        for (int i = 0; i < tableLayoutPanel.Controls.Count; i++)
                        {
                            CheckBox ck = (CheckBox)tableLayoutPanel.Controls[i];
                            ck.CheckedChanged -= new EventHandler(ck_CheckedChanged);
                            ck.Checked = false;
                            for (int k = 0; k < _selectValue.Length; k++)
                            {
                                if (_selectValue[k].Equals(ck.Tag))
                                {
                                    ck.Checked = true;
                                    if (text == "")
                                        text = ck.Text;
                                    else
                                        text += "," + ck.Text;
                                }
                            }
                            ck.CheckedChanged += new EventHandler(ck_CheckedChanged);
                        }
                        textContext.Text = text;
                        //ck_CheckedChanged(null, null);
                    }
                    //Grid
                    else if (multiSelectTextType == MultiSelectTextType.Grid)
                    {
                        DataView dv = (DataView)dataGridSelect.DataSource;
                        for (int i = 0; i < dv.Table.Rows.Count; i++)
                        {
                            dv.Table.Rows[i]["ck"] = false;
                            for (int k = 0; k < _selectValue.Length; k++)
                            {
                                if (dv.Table.Rows[i][valueField].Equals(_selectValue[k]))
                                {
                                    dv.Table.Rows[i]["ck"] = true;
                                }
                            }
                        }

                        getvalue(dv);
                        dataGridSelect.Refresh();
                    }
                }
            }
        }

        TableLayoutPanel tableLayoutPanel;
        private object _datasource;
        [Description("下拉数据源")]
        public object DataSource
        {
            get
            {
                return _datasource;
            }
            set
            {
                _datasource = value;
                if (_datasource is DataTable)
                {
                    //CheckBox
                    if (multiSelectTextType == MultiSelectTextType.CheckBox)
                    {
                        panelSelect.Controls.Clear();
                        tableLayoutPanel = new TableLayoutPanel();
                        tableLayoutPanel.Dock = DockStyle.Fill;
                        tableLayoutPanel.AutoScroll = true;
                        tableLayoutPanel.AutoSize = true;
                        tableLayoutPanel.AutoSizeMode = AutoSizeMode.GrowOnly;

                        panelSelect.Controls.Add(tableLayoutPanel);
                        DataTable dt = _datasource as DataTable;
                        if (dt.Rows.Count > 0)
                        {
                            tableLayoutPanel.Refresh();
                            tableLayoutPanel.ColumnCount = selectColumnCount;
                            tableLayoutPanel.RowCount = (int)(dt.Rows.Count - (dt.Rows.Count % selectColumnCount) / selectColumnCount) + 1;

                            for (int i = 0; i < selectColumnCount; i++)
                            {
                                tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, (float)100 / selectColumnCount));
                            }
                            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
                            {
                                tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (string.IsNullOrEmpty(valueField) || string.IsNullOrEmpty(displayField)) break;
                                CheckBox ck = new CheckBox();
                                ck.Name = "ck" + i.ToString();
                                ck.Text = dt.Rows[i][displayField].ToString();
                                ck.Tag = dt.Rows[i][valueField];
                                ck.CheckedChanged += new EventHandler(ck_CheckedChanged);
                                tableLayoutPanel.Controls.Add(ck, i % selectColumnCount, (int)(i - (i % selectColumnCount)) / selectColumnCount);
                            }

                        }
                        tableLayoutPanel.Show();
                        panelSelect.Refresh();
                    }
                    else if (multiSelectTextType == MultiSelectTextType.Grid)//Grid
                    {
                        if (string.IsNullOrEmpty(_CardColumn) == false)
                        {
                            string[] Columns = _CardColumn.Split(new char[] { ',' });
                            DataGridViewColumn[] columns = new DataGridViewColumn[Columns.Length + 1];
                            columns[0] = new DataGridViewCheckBoxColumn();
                            columns[0].Name = "ck";
                            columns[0].HeaderText = "";
                            columns[0].DataPropertyName = "ck";
                            columns[0].Width = 20;
                            columns[0].ReadOnly = true;
                            columns[0].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                            (columns[0] as DataGridViewCheckBoxColumn).TrueValue = true;
                            (columns[0] as DataGridViewCheckBoxColumn).FalseValue = false;

                            for (int i = 1; i < columns.Length; i++)
                            {
                                string[] pms = Columns[i - 1].Split(new char[] { '|' });
                                columns[i] = new DataGridViewTextBoxColumn();
                                columns[i].Name = "col" + pms[0];
                                columns[i].HeaderText = pms[1];
                                columns[i].DataPropertyName = pms[0];
                                if (pms[2] == "auto")
                                    columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                                else
                                    columns[i].Width = Convert.ToInt32(pms[2]);
                                columns[i].ReadOnly = true;
                                columns[i].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                                //columns[i].DefaultCellStyle = new DataGridViewCellStyle();
                            }

                            dataGridSelect.Columns.Clear();
                            dataGridSelect.Columns.AddRange(columns);
                        }

                        DataTable dt = (_datasource as DataTable).Clone();
                        dt.Columns.Add("ck", typeof(System.Boolean));
                        for (int i = 0; i < (_datasource as DataTable).Rows.Count; i++)
                        {
                            dt.Rows.Add((_datasource as DataTable).Rows[i].ItemArray);
                            dt.Rows[i]["ck"] = false;
                        }
                        DataView dv = new DataView(dt);
                        dataGridSelect.AutoGenerateColumns = false;
                        dataGridSelect.DataSource = dv;
                        //dataGridSelect.Tag = dt;
                    }
                }
                else if (_datasource is System.Collections.IList)
                {
                }
            }
        }

        private string _displayField;
        [Description("显示字段")]
        public string displayField
        {
            get { return _displayField; }
            set { _displayField = value; }
        }
        private string _valueField;
        [Description("值字段")]
        public string valueField
        {
            get { return _valueField; }
            set { _valueField = value; }
        }

        [Description("SelectValue值改变事件")]
        public event EventHandler SelectValueChanged;

        private MultiSelectTextType _multiSelectTextType = MultiSelectTextType.CheckBox;
        [Description("多选类型")]
        public MultiSelectTextType multiSelectTextType
        {
            get { return _multiSelectTextType; }
            set
            {
                _multiSelectTextType = value;
                if (_multiSelectTextType == MultiSelectTextType.CheckBox)
                    popup.AddPopupPanel(textContext, panelSelect, PopupEvent.Custom, selectWidth, selectHeight);
                else if (_multiSelectTextType == MultiSelectTextType.Grid)
                {
                    popup.AddPopupPanel(textContext, panelGrid, PopupEvent.Custom, selectWidth, selectHeight);
                }
            }
        }

        public void AddValue(object val)
        {
            if ((_datasource as DataTable).Select("" + valueField + "=" + val).Length > 0)
            {
                List<object> values;
                if (_selectValue == null)
                    values = new List<object>();
                else
                    values = ((object[])_selectValue).ToList();

                values.Add(val);

                SelectValue = values.ToArray();
            }
        }

        public void ClearValue()
        {
            if (_selectValue != null)
            {
                List<object> values = ((object[])_selectValue).ToList();
                values.Clear();
                SelectValue = values.ToArray();
            }
        }

        #region CheckBox
        private int _selectWidth;
        [Description("下拉框的宽度")]
        public int selectWidth
        {
            get
            {
                if (_selectWidth == 0)
                    return textContext.Width;
                else
                    return _selectWidth;
            }
            set { _selectWidth = value; }
        }

        private int _selectHeight;
        [Description("下拉框的高度")]
        public int selectHeight
        {
            get
            {
                if (_selectHeight == 0)
                    return 200;
                else
                    return _selectHeight;
            }
            set { _selectHeight = value; }
        }

        private int _selectColumnCount = 2;
        [Description("下拉框的列数")]
        public int selectColumnCount
        {
            get { return _selectColumnCount; }
            set { _selectColumnCount = value; }
        }
        #endregion

        #region Grid
        private string _queryFieldsString;
        [Description("设置查询字段,通过“,”分割")]
        public string QueryFieldsString
        {
            get { return _queryFieldsString; }
            set { _queryFieldsString = value; }
        }

        private string _CardColumn;
        [Description("选项卡列信息,如：Code|编码|80,Name|名称|120")]
        public string CardColumn
        {
            get { return _CardColumn; }
            set { _CardColumn = value; }
        }
        #endregion

        protected override void OnSizeChanged(EventArgs e)
        {
            textContext.Height = Height;
            base.OnSizeChanged(e);
        }

        #region CheckBox
        void ck_CheckedChanged(object sender, EventArgs e)
        {
            List<object> values = new List<object>();
            for (int i = 0; i < tableLayoutPanel.Controls.Count; i++)
            {
                CheckBox ck = (CheckBox)tableLayoutPanel.Controls[i];
                if (ck.Checked)
                {
                    values.Add(ck.Tag);
                }
            }
            SelectValue = values.ToArray();
        }
        #endregion

        #region Grid
        private string FormatKeyword(string str)
        {
            string strKey = str;
            strKey = strKey.Replace("'", "''");
            strKey = strKey.Replace("[", "");
            strKey = strKey.Replace("%", "[%]");
            strKey = strKey.Replace("*", "[*]");
            strKey = strKey.Replace("(", "[(]");

            return strKey;
        }

        private void txtChar_TextChanged(object sender, EventArgs e)
        {
            string strKey = FormatKeyword(this.txtChar.Text);
            string[] queryFields = _queryFieldsString.Split(',');
            string filterString = "";

            if (_queryFieldsString != "")
            {
                for (int i = 0; i < queryFields.Length - 1; i++)
                {
                    filterString += queryFields[i] + " like '%" + strKey + "%' or ";
                }
                filterString += queryFields[queryFields.Length - 1] + " like '%" + strKey + "%'";
            }

            if (_datasource != null)
            {
                DataView dv = (DataView)dataGridSelect.DataSource;
                dv.RowFilter = filterString;
                dataGridSelect.DataSource = dv;
            }
        }

        List<Control> lablist = new List<Control>();
        private void getvalue(DataView dv)
        {
            string text = "";
            List<object> values = new List<object>();
            for (int i = 0; i < dv.Table.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dv.Table.Rows[i]["ck"]) == true)
                {
                    if (text == "")
                        text = dv.Table.Rows[i][displayField].ToString();
                    else
                        text += "," + dv.Table.Rows[i][displayField].ToString();
                    values.Add(dv.Table.Rows[i][valueField]);
                }
            }

            int colLenth = 70;
            int rowLenth = 25;
            int pwidth = this.Width;
            int pcount = values.Count + 1;//加上清空和输入框
            int colnum = (int)(pwidth - pwidth % colLenth) / colLenth;//列数
            int rownum = (int)(pcount - pcount % colnum) / colnum + (pcount % colnum == 0 ? 0 : 1);//行数
            rownum = rownum == 0 ? 1 : rownum;
            panelValue.Height = rowLenth * rownum;
            //panelGrid.Height += rowLenth * rownum;
            //清空
            for (int i = 0; i < lablist.Count; i++)
            {
                panelValue.Controls.Remove(lablist[i]);
            }
            lablist.Clear();
            //显示已选择的
            for (int i = 0; i < rownum; i++)
            {
                for (int k = 0; k < colnum; k++)
                {
                    int index = i * colnum + k;
                    if (index < values.Count)
                    {
                        Label labtext = new Label();
                        labtext.Location = new System.Drawing.Point(k * colLenth+2, i * rowLenth+2);
                        labtext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        labtext.Size = new System.Drawing.Size(colLenth-3, 20);
                        labtext.Text = text.Split(new char[] { ',' })[index];
                        labtext.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                        labtext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

                        labtext.Tag = values[index];
                        labtext.Click += new EventHandler(labdel_Click);
                        labtext.MouseHover += new EventHandler(labdel_MouseHover);
                        labtext.MouseLeave += new EventHandler(labdel_MouseLeave);

                        //Label labdel = new Label();
                        //labdel.AutoSize = true;
                        //labdel.Location = new System.Drawing.Point(k * colLenth + colLenth-20 + 2, i * rowLenth + 2);
                        ////labdel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                        //labdel.ForeColor = System.Drawing.Color.Blue;
                        //labdel.Text = "X";
                        //labdel.Tag = values[index];
                        //labdel.Click += new EventHandler(labdel_Click);
                        //labdel.MouseHover += new EventHandler(labdel_MouseHover);
                        //labdel.MouseLeave += new EventHandler(labdel_MouseLeave);

                        panelValue.Controls.Add(labtext);
                        //panelValue.Controls.Add(labdel);

                        lablist.Add(labtext);
                        //lablist.Add(labdel);
                    }
                }
            }

            textContext.Text = text;
            _selectValue = values.ToArray();
        }

        void labdel_MouseLeave(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lbl.ForeColor = Color.Black;
            lbl.Cursor = Cursors.Default;
        }

        void labdel_MouseHover(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lbl.ForeColor = Color.Red;
            lbl.Cursor = Cursors.Hand;
        }

        void labdel_Click(object sender, EventArgs e)
        {
            List<object> values = SelectValue.ToList();
            object delval = (sender as Label).Tag;

            values.Remove(delval);
            SelectValue = values.ToArray();
        }

        private void dataGridSelect_Click(object sender, EventArgs e)
        {
            if (dataGridSelect.CurrentCell != null)
            {
                txtChar.Focus();
                int rowindex = dataGridSelect.CurrentCell.RowIndex;
                DataView dv = (DataView)dataGridSelect.DataSource;

                if (Convert.ToBoolean(dv[rowindex]["ck"]) == true)
                {
                    dv[rowindex]["ck"] = false;
                }
                else
                {
                    dv[rowindex]["ck"] = true;
                }

                getvalue(dv);
            }

            dataGridSelect.Refresh();
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            SelectValue = new object[] { };
        }
        #endregion

    }

    public enum MultiSelectTextType
    {
        CheckBox,Grid
    }
}

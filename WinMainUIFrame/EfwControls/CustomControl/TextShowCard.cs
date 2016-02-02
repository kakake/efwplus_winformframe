using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace EFWCoreLib.WinformFrame.CustomControl
{
    /// <summary>
    /// 匹配模式
    /// </summary>
    public enum MatchModes
    {
        /// <summary>
        /// 任意字符
        /// </summary>
        ByAnyString = 0,
        /// <summary>
        /// 按首字符
        /// </summary>
        ByFirstChar = 1
    }

    public delegate void AfterSelectedRowHandler(object sender, object SelectedValue);

    /// <summary>
    /// 翻页事件
    /// </summary>
    /// <param name="sender">对象</param>
    /// <param name="pageNo">当前页</param>
    /// <param name="pageSize">页大小</param>
    public delegate void PagerTextEventHandler(object sender, int pageNo, int pageSize, string fiterChar);

    public partial class TextShowCard : DevComponents.DotNetBar.Controls.TextBoxX
    {
        bool enterhide = true;

        public TextShowCard()
        {
            CreateDropDownButtonImage();

            InitializeComponent();

            textdataGrid.ReadOnly = true;
            textdataGrid.MultiSelect = false;
            textdataGrid.AllowUserToAddRows = false;
            textdataGrid.AllowUserToDeleteRows = false;
            textdataGrid.AllowUserToResizeRows = false;
            textdataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            textdataGrid.ShowCellToolTips = false;
            textdataGrid.EnableHeadersVisualStyles = false;

            textdataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            //textdataGrid.Leave += new EventHandler(dgvSelectionCard_Leave);
            //textdataGrid.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dgvSelectionCard_RowPostPaint);
            textdataGrid.DoubleClick += new EventHandler(dgvSelectionCard_DoubleClick);
            textdataGrid.VisibleChanged += new EventHandler(dgvSelectionCard_VisibleChanged);

            //点击下拉按钮
            this.ButtonCustomClick += new EventHandler(TextBoxEr_ButtonCustomClick);

            this.textpanel.Width = this.Width;
            textpager.IsPage = true;
        }

        private void CreateDropDownButtonImage()
        {
            //创建下拉箭头按钮图像
            Bitmap img = new Bitmap(9, 5);
            Graphics grp = Graphics.FromImage(img);
            grp.SmoothingMode = SmoothingMode.HighQuality;
            grp.PixelOffsetMode = PixelOffsetMode.None;
            Pen pen = new Pen(Color.FromArgb(21, 66, 139), 1);
            grp.DrawLine(pen, 2, 1, 6, 1);
            grp.DrawLine(pen, 3, 2, 5, 2);
            grp.Dispose();
            img.SetPixel(4, 3, Color.FromArgb(21, 66, 139));
            this.ButtonCustom.Image = img;
            this.ButtonCustom.Visible = true;
            this.ButtonCustom.Enabled = true;
        }

        private void TextBoxEr_ButtonCustomClick(object sender, EventArgs e)
        {

            if (textpanel.Visible)
            {
                if (enterhide)
                {
                    textpanel.Hide();
                }
            }
            else
            {
                textpanel.Show();
            }

            enterhide = true;
            
        }

        private string GettextdataGridColumnName(string bindName)
        {
            for (int i = 0; i < textdataGrid.Columns.Count; i++)
            {
                if (bindName.Trim().ToUpper() == textdataGrid.Columns[i].DataPropertyName.ToUpper())
                    return textdataGrid.Columns[i].Name;
            }
            return null;
        }
        /// <summary>
        /// 选中事件
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private bool OnSelectedEvent(int rowIndex)
        {
            if (this.Focused && _source != null && rowIndex > -1 && rowIndex < textdataGrid.Rows.Count)
            {
                if (memberField != null)
                {
                    //设定MemberField指定的字段值
                    memberValue = textdataGrid[GettextdataGridColumnName(memberField), rowIndex].Value;
                    if (memberValue != null)
                    {
                        //取出DisplayField指定的字段值并显示在文本框内
                        isSetValue = true;
                        if (displayField.Trim() != "")
                        {
                            if (textdataGrid[GettextdataGridColumnName(displayField), rowIndex].Value != null)
                                this.Text = textdataGrid[GettextdataGridColumnName(displayField), rowIndex].Value.ToString().Trim();
                            else
                                this.Text = "";
                        }
                        this.SelectionStart = this.Text.Length;
                        isSetValue = false;
                    }
                }
                selectedValue = ((DataRowView)textdataGrid.Rows[rowIndex].DataBoundItem).Row.ItemArray;
                if (AfterSelectedRow != null)
                    AfterSelectedRow(this, selectedValue);
                textpanel.Hide(); //选择后隐藏选项卡
                return true;
            }
            else
                return false;
        }

        void dgvSelectionCard_DoubleClick(object sender, EventArgs e)
        {
            //选项卡双击选中事件
            if (this.Focused == true && this.textpanel.Visible == true)
            {
                if (textdataGrid.CurrentCell != null)
                    OnSelectedEvent(textdataGrid.CurrentCell.RowIndex);
            }
        }

        void dgvSelectionCard_VisibleChanged(object sender, EventArgs e)
        {
            if (((DataGridView)sender).Visible == true)
                SetSelectionCardLocation();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        //protected override void OnSizeChanged(EventArgs e)
        //{
        //    if (this.Width > 250)
        //        this.textpanel.Width = this.Width;
        //    else
        //        this.textpanel.Width = 350;
        //    this.textpanel.Height = 276;
        //    base.OnSizeChanged(e);
        //}

        /// <summary>
        /// 处理查询关键字中的特殊字符
        /// </summary>
        /// <param name="str">查询关键字</param>
        /// <returns></returns>
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
        /// <summary>
        /// 处理空白键,全字匹配
        /// </summary>
        private bool pressSpacekey;

        //5.检索数据
        protected override void OnTextChanged(EventArgs e)
        {
            if (!isSetValue && IsPage == true)
            {
                if (queryFields != null)
                {
                    //throw new Exception("没有为选项卡指定查询字段QueryFields属性");
                    //return;

                    string filterString = "";
                    string s1 = "%";
                    string s2 = "%";
                    if (matchMode == MatchModes.ByFirstChar)
                        s1 = "";

                    string strKey = FormatKeyword(this.Text);

                    for (int i = 0; i < queryFields.Length - 1; i++)
                    {
                        filterString += queryFields[i] + " like '" + s1 + strKey + s2 + "' or ";
                    }
                    filterString += queryFields[queryFields.Length - 1] + " like '" + s1 + strKey + s2 + "'";

                    if (pressSpacekey)
                    {
                        filterString = "";
                        for (int i = 0; i < queryFields.Length - 1; i++)
                        {
                            filterString += queryFields[i] + " = '" + strKey + "' or ";
                        }
                        filterString += queryFields[queryFields.Length - 1] + " = '" + strKey + "'";
                    }

                    if (_source != null)
                    {
                        DataRow[] drs = _source.Select(filterString);
                        DataTable dt = _source.Clone();
                        for (int i = 0; i < drs.Length; i++)
                        {
                            dt.Rows.Add(drs[i].ItemArray);
                        }
                        this.textpager.DataSource = dt;
                        if (dt.Rows.Count > 0)
                            if (textdataGrid.CurrentCell != null)
                                textdataGrid.CurrentCell = textdataGrid[GetGridVisibleColumnIndex(), 0];
                    }
                }

            }

            if (IsPage == false)
            {
                textpager.pageNo = 1;
                //PageNoChanged(this, 1, textpager.pageSize);
            }
            if (this.Focused)
                textpanel.Show();
            base.OnTextChanged(e);
            if (this.Text.Trim() == "")
            {
                memberValue = null;
            }
        }

        /// <summary>
        /// 该控件所在的窗体
        /// </summary>
        private System.Windows.Forms.Form containerForm;
        /// <summary>
        /// 添加到顶级窗口
        /// </summary>
        private void AddSelectCardToTopForm()
        {

            #region 控件所在窗体
            System.Windows.Forms.Control pctrl = this.Parent;
            while (true)
            {
                if (pctrl is System.Windows.Forms.Form)
                    break;
                else
                {
                    pctrl = pctrl.Parent;
                    if (pctrl is System.Windows.Forms.Form)
                    {
                        break;
                    }
                }
            }
            #endregion
            //窗体
            containerForm = (System.Windows.Forms.Form)pctrl;
            if (!containerForm.Controls.Contains(this.textpanel))
                containerForm.Controls.Add(textpanel);

            SetSelectionCardLocation();
        }
        /// <summary>
        /// 设置位置
        /// </summary>
        private void SetSelectionCardLocation()
        {
            /*以控件所在的窗体为参照对象定位选项卡位置
             */
            int x = this.Left;
            int y = this.Top + this.Height;

            System.Windows.Forms.Control ctrl = this.Parent;
            if (this.Parent == null)
                return;

            if (ctrl is Form)
            {
            }
            else
            {
                x = x + ctrl.Left;
                y = y + ctrl.Top;
                ctrl = ctrl.Parent;
                while (true)
                {
                    if (ctrl is System.Windows.Forms.Form)
                    {
                        break;
                    }
                    else
                    {
                        x = x + ctrl.Left;
                        y = y + ctrl.Top;
                        ctrl = ctrl.Parent;
                    }
                }
            }


            int buttomLeft = containerForm.Height - y - this.Height;//选项卡当前位置的底部与窗体下边界的距离(留1个行高的距离)
            //如果超出下边界
            if (textpanel.Height > buttomLeft)
            {
                //在上面显示选择卡
                int _y = y - this.Height - textpanel.Height;
                if (_y > 0)
                    y = _y - 0;
            }
            else
            {
                y = y + 0;
            }

            int rightLeft = containerForm.Width - x; //选项卡当前位置的右部与窗体右边界的距离
            int tmpx = x;//暂存
            //如果超出右边界,选项卡的右部与窗体右边界对齐
            if (textpanel.Width > rightLeft)
            {
                x = x - textpanel.Width + rightLeft - 10;
                if (x < 0)
                    x = tmpx;
                else
                    x = x - 0;
            }
            else
            {
                x = x + 0;
            }
            Point pt = new Point(x, y);
            textpanel.Location = pt;
            textpanel.BringToFront();
        }
        //1.获取焦点显示ShowCard
        protected override void OnEnter(EventArgs e)
        {
            if (PageNoChanged != null)
            {
                textpager.PageNoChanged -= new PagerEventHandler(textpager_PageNoChanged);
                textpager.PageNoChanged += new PagerEventHandler(textpager_PageNoChanged);
            }
            AddSelectCardToTopForm();

            //this.ButtonCustomClick -= new EventHandler(TextBoxEr_ButtonCustomClick);
            this.textdataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.textpanel.Show();
            //this.ButtonCustomClick += new EventHandler(TextBoxEr_ButtonCustomClick);

            base.OnEnter(e);

            enterhide = false;
            
        }

        void textpager_PageNoChanged(object sender, int pageNo, int pageSize)
        {
            PageNoChanged(this, pageNo, pageSize, FormatKeyword(this.Text));
        }
        //2.离开焦点隐藏ShowCard
        protected override void OnLeave(EventArgs e)
        {
            if (this.textdataGrid.Focused == true || this.textpager.comboBoxEx1.Focused == true || this.textpager.slider1.Focused == true)
            {
                this.Focus();
                return;
            }
            else
            {
                textpanel.Hide();
            }
            base.OnLeave(e);
        }

        /// <summary>
        /// 获取选项卡的可见列
        /// </summary>
        /// <returns></returns>
        private int GetGridVisibleColumnIndex()
        {
            for (int i = 0; i < textdataGrid.Columns.Count; i++)
            {
                if (textdataGrid.Columns[i].Visible)
                    return i;
            }
            return -1;
        }
        //3.上下移动和翻页
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.Focused == true && textpanel.Visible==true && textdataGrid.CurrentCell != null)
            {

                #region 上下键移动
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    int row = textdataGrid.CurrentCell.RowIndex;
                    if (e.KeyCode == Keys.Up)
                    {
                        if (row > 0)
                            textdataGrid.CurrentCell = textdataGrid[GetGridVisibleColumnIndex(), row - 1];
                    }
                    else
                    {
                        if (row < textdataGrid.Rows.Count - 1)
                            textdataGrid.CurrentCell = textdataGrid[GetGridVisibleColumnIndex(), row + 1];
                    }
                    e.Handled = true;
                    return;
                }
                #endregion

                #region page翻页
                if (e.KeyCode == Keys.PageUp || e.KeyCode == Keys.PageDown)
                {
                    if (e.KeyCode == System.Windows.Forms.Keys.PageDown)
                    {
                        textpager.pageNo += 1;
                    }
                    if (e.KeyCode == System.Windows.Forms.Keys.PageUp)
                    {
                        textpager.pageNo -= 1;
                    }
                }
                #endregion
            }

            base.OnKeyDown(e);
        }
        //4.Esc隐藏ShowCard，回车选择行记录，数字键选择记录
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            int keyAsc = (int)e.KeyChar;

            if (keyAsc == 13 || keyAsc == 27 )
            {
                //Esc取消
                if (keyAsc == 27 && this.Focused)
                {
                    textpanel.Hide();
                    this.Focus();
                }
                //回车选择数据
                if (keyAsc == 13 && this.Focused && this.textpanel.Visible == true)
                {
                    //回车键选中事件
                    if (textdataGrid.CurrentRow != null)
                        OnSelectedEvent(textdataGrid.CurrentRow.Index);
                }
                //选好数据回车跳转到下一个控件
                else if (keyAsc == 13 && this.Focused && this.textpanel.Visible == false)
                {
                    //SendKeys.Send("{Tab}");
                    e.Handled = true;
                }

                else if (keyAsc == 32 && this.Focused && textpanel.Visible == true)
                {
                    pressSpacekey = true;
                    this.OnTextChanged(null);
                    pressSpacekey = false;
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
                base.OnKeyPress(e);
                return;
            }
            //按数字键选择
            if (this.Focused == true && this.textpanel.Visible == true)
            {
                if (keyAsc >= 48 && keyAsc <= 57)
                {
                    //定位行
                    int row = 0;
                    if (keyAsc >= 48 && keyAsc <= 57)
                    {
                        row = keyAsc - 48;
                    }
                    if (keyAsc >= 96 && keyAsc <= 105)
                    {
                        row = keyAsc - 96;
                    }
                    if (row == 0)
                        row = 9;
                    else
                        row = row - 1;

                    //数字键选择事件
                    OnSelectedEvent(row);
                    e.Handled = true;
                    return;
                }
            }

            base.OnKeyPress(e);
        }

        private DataTable _source;
        [RefreshProperties(RefreshProperties.Repaint)]
        [DefaultValue("")]
        [AttributeProvider(typeof(IListSource))]
        public DataTable ShowCardDataSource
        {
            get { return _source; }
            set
            {
                _source = value;
                this.textpager.DataSource = _source;
            }
        }

        private DataGridViewTextBoxColumn[] _showCardColumns;
        [Description("选项卡列信息,如果不设置,控件将以普通文本框模式运行")]
        public DataGridViewTextBoxColumn[] ShowCardColumns
        {
            get
            {
                return _showCardColumns;
            }
            set
            {
                _showCardColumns = value;
                if (_showCardColumns != null && _showCardColumns.Length > 0)
                {
                    DataGridViewColumn[] columns = new DataGridViewColumn[_showCardColumns.Length];
                    for (int i = 0; i < _showCardColumns.Length; i++)
                    {
                        columns[i] = _showCardColumns[i];
                    }
                    this.textdataGrid.Columns.Clear();
                    this.textdataGrid.Columns.AddRange(columns);
                }
            }
        }

        /// <summary>
        /// 选项卡的过滤匹配模式
        /// </summary>
        private MatchModes matchMode;
        /// <summary>
        /// 获取或设置选项卡的过滤匹配模式
        /// </summary>
        [Description("获取或设置选项卡的过滤匹配模式")]
        public MatchModes MatchMode
        {
            get
            {
                return matchMode;
            }
            set
            {
                matchMode = value;
            }
        }
        /// <summary>
        /// 获取或设置查询字段
        /// </summary>
        private string[] queryFields;
        [Description("获取或设置查询字段")]
        public string[] QueryFields
        {
            get
            {
                return queryFields;
            }
            set
            {
                queryFields = value;
            }
        }

        /// <summary>
        /// 要显示的字段或属性名
        /// </summary>
        private string displayField = "";
        /// <summary>
        /// 获取或设置要显示在控件TextBox内的字段
        /// </summary>
        [Description("获取或设置要显示在控件TextBox内的字段")]
        public string DisplayField
        {
            get
            {
                return displayField;
            }
            set
            {
                displayField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string memberField = "";
        [Description("获取或设置绑定字段名")]
        public string MemberField
        {
            get
            {
                return memberField;
            }
            set
            {
                memberField = value;
            }
        }
        /// <summary>
        /// 是否注入值标识,防止赋值的时候循环触发TextChange事件
        /// </summary>
        private bool isSetValue;
        /// <summary>
        /// 成员值
        /// </summary>
        private object memberValue;
        [Description("获取或设置绑定值")]
        public object MemberValue
        {
            get
            {
                return memberValue;
            }
            set
            {
                memberValue = value;
                try
                {
                    isSetValue = true;
                    if (value != null && value.GetType() == typeof(string))
                    {
                        if (value.ToString().Trim() == "")
                        {
                            this.Text = "";
                            isSetValue = false;
                            return;
                        }
                    }
                    //设置display字段
                    if (_source!=null && value != null)
                    {
                        if (_source.Columns[memberField].DataType == typeof(System.Int16) ||
                            _source.Columns[memberField].DataType == typeof(System.Int32) ||
                            _source.Columns[memberField].DataType == typeof(System.Int64) ||
                            _source.Columns[memberField].DataType == typeof(System.Decimal) ||
                            _source.Columns[memberField].DataType == typeof(System.Double))
                        {
                            DataRow[] dr = _source.Select(memberField + "=" + value.ToString());
                            if (dr.Length > 0)
                            {

                                this.Text = dr[0][displayField].ToString().Trim();
                            }
                            else
                            {
                                this.Text = "";
                                memberValue = null;
                            }
                        }
                        else
                        {
                            DataRow[] dr = _source.Select(memberField + "='" + value.ToString() + "'");
                            if (dr.Length > 0)
                            {
                                this.Text = dr[0][displayField].ToString().Trim();
                            }
                            else
                            {
                                this.Text = "";
                                memberValue = null;
                            }
                        }
                    }
                }
                catch
                {
                    this.isSetValue = true;
                    this.Text = "<Error>";
                    this.isSetValue = false;
                }
                finally
                {
                    isSetValue = false;
                }
            }
        }

        /// <summary>
        /// 选中的值
        /// </summary>
        private object selectedValue;
        /// <summary>
        /// 选择的值
        /// </summary>
        [Description("选择的值")]
        public object SelectedValue
        {
            get
            {
                return selectedValue;
            }
            set
            {
                selectedValue = value;
            }
        }

        [Description("设置是否内部分页，IsPage=false时结合PagerEventHandler事件使用")]
        public bool IsPage
        {
            get { return textpager.IsPage; }
            set { textpager.IsPage = value; }
        }

        [Description("设置选项卡的宽")]
        public int ShowCardWidth
        {
            get
            {
                return this.textpanel.Width;
            }
            set
            {
                this.textpanel.Width = value;
            }
        }
        [Description("设置选项卡的高")]
        public int ShowCardHeight
        {
            get
            {
                return this.textpanel.Height;
            }
            set
            {
                this.textpanel.Height = value;
            }
        }



        /// <summary>
        /// 总记录数
        /// </summary>
        public int PageTotalRecord
        {
            get { return textpager.totalRecord; }
            set { textpager.totalRecord = value; }
        }

        [Description("在选项卡选定记录后引发的事件")]
        public event AfterSelectedRowHandler AfterSelectedRow;

        [Description("翻页的事件，结合IsPage=false属性使用")]
        public event PagerTextEventHandler PageNoChanged;
    }
}

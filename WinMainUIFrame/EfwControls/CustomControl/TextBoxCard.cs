using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;

namespace EfwControls.CustomControl
{

    public class TextBoxCard : DevComponents.DotNetBar.Controls.TextBoxX
    {
        //处理空白键,全字匹配
        private bool pressSpacekey;
        //该控件所在的窗体
        private Form containerForm;
        // 选项卡
        private CardDataGrid cardDataGrid;

        public TextBoxCard()
        {
            cardDataGrid = new CardDataGrid();
           
            CreateDropDownButtonImage();
            //点击下拉按钮
            this.Enter += new EventHandler(TextBoxCard_Enter);
            this.ButtonCustomClick += new EventHandler(TextBoxEr_ButtonCustomClick);
            
        }

        public TextBoxCard(IContainer container)
        {
            container.Add(this);
            cardDataGrid = new CardDataGrid();

            CreateDropDownButtonImage();
            //点击下拉按钮
            this.Enter += new EventHandler(TextBoxCard_Enter);
            this.ButtonCustomClick += new EventHandler(TextBoxEr_ButtonCustomClick);

        }

        //隐藏下拉网格
        public void HideCard()
        {
            cardDataGrid.Hide();
        }

        //下拉按钮
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
        // 选中事件
        private bool OnSelectedEvent(int rowIndex)
        {
            //this.Focused &&
            if ( _source != null && rowIndex > -1 && rowIndex < cardDataGrid.textdataGrid.Rows.Count)
            {
                if (memberField != null)
                {
                    //设定MemberField指定的字段值
                    memberValue = cardDataGrid.GetGridValue(memberField, rowIndex);
                    if (memberValue != null)
                    {
                        //取出DisplayField指定的字段值并显示在文本框内
                        isSetValue = true;
                        if (displayField.Trim() != "")
                        {
                            if (cardDataGrid.GetGridValue(displayField, rowIndex) != null)
                                this.Text = cardDataGrid.GetGridValue(displayField, rowIndex).ToString().Trim();
                            else
                                this.Text = "";
                        }
                        this.SelectionStart = this.Text.Length;
                        isSetValue = false;
                    }
                }
                selectedValue = cardDataGrid.GetGridRow(rowIndex);
                this.Focus();//newadd
                if (AfterSelectedRow != null)
                    AfterSelectedRow(this, selectedValue);
                cardDataGrid.Hide(); //选择后隐藏选项卡
                return true;
            }
            else
                return false;
        }
        ///处理查询关键字中的特殊字符
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
        // 添加到顶级窗口
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
            //if (!containerForm.Controls.Contains(this.cardDataGrid))
            //    containerForm.Controls.Add(cardDataGrid);

            //if (this.Parent != null && this.Parent.Controls.Contains(this.cardDataGrid) == false)
            //{
            //    this.Parent.Controls.Add(cardDataGrid);
            //    SetSelectionCardLocation();
            //    cardDataGrid.TabStop = false;
            //    cardDataGrid.BringToFront();
            //}

           

        }
        //设置位置
        private void SetSelectionCardLocation()
        {
            /*以控件所在的窗体为参照对象定位选项卡位置
             */
            int x = this.Left;
            int y = this.Top + this.Height;

            System.Windows.Forms.Control ctrl = this.Parent;
            if (this.Parent == null)
                return;

            this.FindForm().AutoValidate = AutoValidate.Disable;
            if (!this.FindForm().Controls.Contains(this.cardDataGrid))
                this.FindForm().Controls.Add(cardDataGrid);

            cardDataGrid.Width = ShowCardWidth < this.Width ? this.Width : ShowCardWidth;
            cardDataGrid.Height = ShowCardHeight < 200 ? 200 : ShowCardHeight;

            Point location = new Point();
            location = this.Parent.PointToScreen(this.Location);
            location = this.FindForm().PointToClient(location);
            if (location.Y + cardDataGrid.Height < this.FindForm().Height-40)
            {
                location.Y = location.Y + this.Height;
            }
            else
            {
                location.Y = location.Y - cardDataGrid.Height;
                if (location.Y < 0)
                {
                    cardDataGrid.Height = cardDataGrid.Height + location.Y;
                    location.Y = 0;
                }
            }
            cardDataGrid.Top = location.Y;
            Rectangle scrRect = Screen.GetBounds(this);
            if (location.X + cardDataGrid.Width < scrRect.Width)
            {
                cardDataGrid.Left = location.X;
            }
            else
            {
                cardDataGrid.Left = location.X + (scrRect.Width - (location.X + cardDataGrid.Width));
            }
            cardDataGrid.BringToFront();

            //if (ctrl is Form)
            //{
            //}
            //else
            //{
            //    x = x + ctrl.Left;
            //    y = y + ctrl.Top;
            //    ctrl = ctrl.Parent;
            //    while (true)
            //    {
            //        if (ctrl is System.Windows.Forms.Form)
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            x = x + ctrl.Left;
            //            y = y + ctrl.Top;
            //            ctrl = ctrl.Parent;
            //        }
            //    }
            //}


            //int buttomLeft = containerForm.Height - y - this.Height;//选项卡当前位置的底部与窗体下边界的距离(留1个行高的距离)
            ////如果超出下边界
            //if (cardDataGrid.Height > buttomLeft)
            //{
            //    //在上面显示选择卡
            //    int _y = y - this.Height - cardDataGrid.Height;
            //    if (_y > 0)
            //        y = _y - 0;
            //}
            //else
            //{
            //    y = y + 0;
            //}

            //int rightLeft = containerForm.Width - x; //选项卡当前位置的右部与窗体右边界的距离
            //int tmpx = x;//暂存
            ////如果超出右边界,选项卡的右部与窗体右边界对齐
            //if (cardDataGrid.Width > rightLeft)
            //{
            //    x = x - cardDataGrid.Width + rightLeft - 10;
            //    if (x < 0)
            //        x = tmpx;
            //    else
            //        x = x - 0;
            //}
            //else
            //{
            //    x = x + 0;
            //}
            //Point pt = new Point(x, y);
            //cardDataGrid.Location = pt;
            //cardDataGrid.BringToFront();
        }
        //给ShowCardDataSource赋值的时候调用此函数
        private void InitShowCard()
        {
            //先创建Card显示列
            if (string.IsNullOrEmpty(_CardColumn))
            {
                if (_showCardColumns != null && _showCardColumns.Length > 0)
                {
                    DataGridViewColumn[] columns = new DataGridViewColumn[_showCardColumns.Length];
                    for (int i = 0; i < _showCardColumns.Length; i++)
                    {
                        columns[i] = _showCardColumns[i];
                    }

                    cardDataGrid.textdataGrid.Columns.Clear();
                    cardDataGrid.textdataGrid.Columns.AddRange(columns);
                }
            }
            else
            {
                string[] Columns = _CardColumn.Split(new char[] { ',' });
                DataGridViewColumn[] columns = new DataGridViewColumn[Columns.Length];
                for (int i = 0; i < columns.Length; i++)
                {
                    string[] pms = Columns[i].Split(new char[] { '|' });
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

                cardDataGrid.textdataGrid.Columns.Clear();
                cardDataGrid.textdataGrid.Columns.AddRange(columns);
            }


            cardDataGrid.textdataGrid.SeqVisible = _isShowSeq;


            if (PageNoChanged != null)
            {
                cardDataGrid.textpager.PageNoChanged += new PagerEventHandler(textpager_PageNoChanged);
            }

            cardDataGrid.textdataGrid.Click += new EventHandler(textdataGrid_Click);
            cardDataGrid.textdataGrid.DoubleClick += new EventHandler(dgvSelectCard_DoubleClick);
            cardDataGrid.ClickLetter += new EventHandler(cardDataGrid_ClickLetter);
            cardDataGrid.DeleteLetter += new EventHandler(cardDataGrid_DeleteLetter);
            cardDataGrid.ConfirmLetter += new EventHandler(cardDataGrid_ConfirmLetter);
            cardDataGrid.CloseLetter += new EventHandler(cardDataGrid_CloseLetter);



            cardDataGrid.letterpanel.Visible = IsShowLetter;
            if (IsShowPage == false)
            {
                cardDataGrid.textpager.pageSize = 200;
            }
            cardDataGrid.textpager.Visible = IsShowPage;
            cardDataGrid.Width = ShowCardWidth < this.Width ? this.Width : ShowCardWidth;
            cardDataGrid.Height = ShowCardHeight < 200 ? 200 : ShowCardHeight;
            cardDataGrid.textpager.IsPage = _isPage;
            cardDataGrid.textpager.DataSource = _source;
            cardDataGrid.DrawLetter();
            AddSelectCardToTopForm();
            cardDataGrid.Hide();
        }


        #region 控件事件
        bool isEnter = true;
        //点击下拉小图标
        private void TextBoxEr_ButtonCustomClick(object sender, EventArgs e)
        {
            if (cardDataGrid.Visible && isEnter)
            {
                cardDataGrid.Hide();
            }
            else
            {
                if (_source != null)
                {
                    SetSelectionCardLocation();
                    this.cardDataGrid.Show();
                }
            }

            isEnter = true;
        }
        void TextBoxCard_Enter(object sender, EventArgs e)
        {
            if (_source != null && IsEnterShowCard == true)
            {
                isEnter = false;
                SetSelectionCardLocation();
                this.cardDataGrid.Show();
                //OnTextChanged(null);
            }
        }
        void textpager_PageNoChanged(object sender, int pageNo, int pageSize)
        {
            PageNoChanged(this, pageNo, pageSize, FormatKeyword(this.Text));
        }
        void dgvSelectCard_DoubleClick(object sender, EventArgs e)
        {
            //选项卡双击选中事件
            if (cardDataGrid.Visible == true)
            {
                if (cardDataGrid.textdataGrid.CurrentCell != null)
                    OnSelectedEvent(cardDataGrid.textdataGrid.CurrentCell.RowIndex);
            }
        }
        void cardDataGrid_CloseLetter(object sender, EventArgs e)
        {
            cardDataGrid.Hide();
        }
        void cardDataGrid_ConfirmLetter(object sender, EventArgs e)
        {
            dgvSelectCard_DoubleClick(null, null);
        }
        void cardDataGrid_DeleteLetter(object sender, EventArgs e)
        {
            if (this.Text.Length > 0)
                this.Text = this.Text.Substring(0, this.Text.Length - 1);
            this.SelectionStart = this.Text.Length;
        }
        void cardDataGrid_ClickLetter(object sender, EventArgs e)
        {
            this.Text += (sender as Label).Text.ToLower();
            this.SelectionStart = this.Text.Length;
        }
        void textdataGrid_Click(object sender, EventArgs e)
        {
            //选项卡双击选中事件
            if (cardDataGrid.Visible == true)
            {
                if (cardDataGrid.textdataGrid.CurrentCell != null)
                    OnSelectedEvent(cardDataGrid.textdataGrid.CurrentCell.RowIndex);
            }
            this.Focus();
        }

        //1.获取焦点显示ShowCard
        protected override void OnEnter(EventArgs e)
        {
            //this.cardDataGrid.Show();
            base.OnEnter(e);
        }
        //2.离开焦点隐藏ShowCard
        protected override void OnLeave(EventArgs e)
        {
            if (cardDataGrid.GetIsFocused())
            {
                //this.Focus();
                return;
            }
            else
            {
                cardDataGrid.Hide();
            }
            base.OnLeave(e);
        }
        //3.上下移动和翻页
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (this.Focused == true && cardDataGrid.Visible == true && cardDataGrid.textdataGrid.CurrentCell != null)
            {

                #region 上下键移动
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    int row = cardDataGrid.textdataGrid.CurrentCell.RowIndex;
                    int col = cardDataGrid.textdataGrid.CurrentCell.ColumnIndex;
                    if (e.KeyCode == Keys.Up)
                    {
                        if (row > 0)
                            cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[col, row - 1];
                    }
                    else
                    {
                        if (row < cardDataGrid.textdataGrid.Rows.Count - 1)
                            cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[col, row + 1];
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
                        cardDataGrid.textpager.pageNo += 1;
                    }
                    if (e.KeyCode == System.Windows.Forms.Keys.PageUp)
                    {
                        cardDataGrid.textpager.pageNo -= 1;
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

            if (keyAsc == 13 || keyAsc == 27 || keyAsc==32)
            {
                //Esc取消
                if (keyAsc == 27 && this.Focused)
                {
                    cardDataGrid.Hide();
                    this.Focus();
                }
                //回车选择数据
                if (keyAsc == 13 && this.Focused && this.cardDataGrid.Visible == true)
                {
                    //回车键选中事件
                    if (cardDataGrid.textdataGrid.CurrentRow != null)
                        OnSelectedEvent(cardDataGrid.textdataGrid.CurrentRow.Index);
                }
                //选好数据回车跳转到下一个控件
                else if (keyAsc == 13 && this.Focused && this.cardDataGrid.Visible == false)
                {
                    SendKeys.Send("{Tab}");
                    e.Handled = true;
                }

                else if (keyAsc == 32 && this.Focused && cardDataGrid.Visible == true)
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
            if (_isNumSelected == true)
            {
                if (this.Focused == true && this.cardDataGrid.Visible == true)
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
            }

            base.OnKeyPress(e);
        }
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
                        DataView dv = new DataView(_source);
                        dv.RowFilter = filterString;
                        cardDataGrid.textpager.DataSource = dv.ToTable();

                        //DataRow[] drs = _source.Select(filterString);
                        //DataTable dt = _source.Clone();
                        //for (int i = 0; i < drs.Length; i++)
                        //{
                        //    dt.Rows.Add(drs[i].ItemArray);
                        //}
                        //if (dt.Columns.Contains("OrderBy"))
                        //{
                        //    DataView dv = dt.DefaultView;
                        //    dv.Sort = "OrderBy Asc";
                        //    DataTable dt2 = dv.ToTable();
                        //    cardDataGrid.textpager.DataSource = dt2;
                        //}
                        //else
                        //    cardDataGrid.textpager.DataSource = dt;

                        //if (dt.Rows.Count > 0)
                        //    if (cardDataGrid.textdataGrid.CurrentCell != null)
                        //        cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[0, 0];
                    }
                }

            }

            if (IsPage == false)
            {
                cardDataGrid.textpager.pageNo = 1;
                //PageNoChanged(this, 1, textpager.pageSize);
            }
            if (this.Focused)
            {
                SetSelectionCardLocation();
                cardDataGrid.Show();
            }
            base.OnTextChanged(e);
            if (this.Text.Trim() == "")
            {
                memberValue = null;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (this.Focused == true && cardDataGrid.Visible == true && cardDataGrid.textdataGrid.CurrentCell != null)
            {
                int row = cardDataGrid.textdataGrid.CurrentCell.RowIndex;
                int col = cardDataGrid.textdataGrid.CurrentCell.ColumnIndex;
                int val = e.Delta;
                if (val > 0)//上滚
                {
                    if (row > 0)
                        cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[col, row - 1];
                }
                else//下滚
                {
                    if (row < cardDataGrid.textdataGrid.Rows.Count - 1)
                        cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[col, row + 1];
                }
            }
            base.OnMouseWheel(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible == false)
                cardDataGrid.Hide();
            base.OnVisibleChanged(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
        #endregion

        #region 自定义属性
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
                if (_source != null)
                InitShowCard();
            }
        }

        private string _CardColumn;
        [Description("选项卡列信息,如：Code|编码|80,Name|名称|120")]
        public string CardColumn
        {
            get { return _CardColumn; }
            set { _CardColumn = value; }
        }

        private DataGridViewTextBoxColumn[] _showCardColumns;
        [Description("选项卡列信息(优先使用CardColumn这个属性),如果不设置,控件将以普通文本框模式运行")]
        public DataGridViewTextBoxColumn[] ShowCardColumns
        {
            get
            {
                return _showCardColumns;
            }
            set
            {
                _showCardColumns = value;
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
        /// 设置查询字段,通过“,”分割
        /// </summary>
        [Description("设置查询字段,通过“,”分割")]
        public string QueryFieldsString
        {
            get
            {
                string strValue = "";
                if (queryFields != null)
                {
                    for (int i = 0; i < queryFields.Length; i++)
                    {
                        if (strValue == "")
                            strValue = queryFields[i];
                        else
                            strValue += "," + queryFields[i];
                    }
                }
                return strValue;
            }
            set
            {
                String strQueryField = value;
                queryFields = strQueryField.Split(',');
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
        /// 网格控件列样式
        /// </summary>
        [Description("设置网格控件列样式")]
        public DataGridViewCellStyle HeadDefaultCellStyle
        {
            set
            {
                if (cardDataGrid == null)
                    return;
                cardDataGrid.SetColumnHeadDefaultCellStyle(value);
            }
        }

        /// <summary>
        /// 网格控件行样式
        /// </summary>
        [Description("设置网格控件行样式")]
        public DataGridViewCellStyle RowDefaultCellStyle
        {
            set
            {
                if (cardDataGrid == null)
                    return;
                cardDataGrid.SetRowDefaultCellStyle(value);
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
                    if (_source != null && value != null)
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
                                cardDataGrid.SetGridSelectRow(memberField, value);
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
                                cardDataGrid.SetGridSelectRow(memberField, value);
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
        [Description("选择选项卡的值")]
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

        private int _showCardWidth;
        [Description("设置选项卡的宽")]
        public int ShowCardWidth
        {
            get
            {
                return _showCardWidth;
            }
            set
            {
                _showCardWidth = value;
            }
        }

        private int _showCardHeight;
        [Description("设置选项卡的高")]
        public int ShowCardHeight
        {
            get
            {
                return _showCardHeight;
            }
            set
            {
                _showCardHeight = value;
            }
        }

        private bool _isShowLetter = false;
        [Description("是否显示过滤字母")]
        public bool IsShowLetter
        {
            get { return _isShowLetter; }
            set { _isShowLetter = value; }
        }

        private bool _isShowPage = false;
        [Description("是否显示分页条")]
        public bool IsShowPage
        {
            get { return _isShowPage; }
            set { _isShowPage = value; }
        }

        private bool _isPage = true;
        [Description("设置是否内部分页，IsPage=false时结合PagerEventHandler事件使用")]
        public bool IsPage
        {
            get { return _isPage; }
            set { _isPage = value; }
        }

        private bool _isShowSeq = true;
        [Description("弹出网格是否显示序号")]
        public bool IsShowSeq
        {
            get { return _isShowSeq; }
            set { _isShowSeq = value; }
        }

        private bool _isNumSelected;
        [Description("是否数字选定")]
        public bool IsNumSelected
        {
            get { return _isNumSelected; }
            set { _isNumSelected = value; }
        }

        private bool _isEnterShowCard = true;
        [Description("是否焦点进入就显示内容")]
        public bool IsEnterShowCard
        {
            get { return _isEnterShowCard; }
            set { _isEnterShowCard = value; }
        }
        #endregion

        #region 自定义事件
        [Description("在选项卡选定记录后引发的事件")]
        public event AfterSelectedRowHandler AfterSelectedRow;

        [Description("翻页的事件，结合IsPage=false属性使用")]
        public event PagerTextEventHandler PageNoChanged;

        #endregion
    }
}

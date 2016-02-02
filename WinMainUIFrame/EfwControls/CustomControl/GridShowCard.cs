using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EFWCoreLib.WinformFrame.CustomControl
{
    public partial class GridShowCard : DataGrid
    {
        public GridShowCard()
        {
            InitializeComponent();

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
            textdataGrid.DoubleClick += new EventHandler(dgvSelectCard_DoubleClick);

            textpanel.Visible = false;

            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
        }

        void dgvSelectCard_DoubleClick(object sender, EventArgs e)
        {
            DataGridView grd = (DataGridView)sender;
            if (grd != null)
            {
                System.Windows.Forms.Message msg = new System.Windows.Forms.Message();
                ProcessCmdKey(ref  msg, System.Windows.Forms.Keys.Enter);
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        /// <summary>
        /// 网格编辑框对象
        /// </summary>
        private TextBox editTextBox;

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
        private bool pressSpaceKey;

        /// <summary>
        /// 获取选择卡第一个可见列的索引
        /// </summary>
        /// <returns></returns>
        private int GetSelectCardVisableColumnIndex(DataGridView grdSelectCard)
        {
            foreach (System.Windows.Forms.DataGridViewColumn col in grdSelectCard.Columns)
            {
                if (col.Visible && col.Width > 0)
                    return col.Index;
            }
            return -1;
        }
        //7.文本过滤
        void editTextBox_TextChanged(object sender, EventArgs e)
        {
            if (CurrentCell != null)
            {
                if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex))
                {
                    if (textpanel.Visible == false)
                        return;
                }
                else
                    return;
            }

            DataGridViewSelectionCard selectionCardInfo = (DataGridViewSelectionCard)textpanel.Tag;
            //textpager.IsPage = selectionCardInfo.IsPage;
            if (textpager.IsPage == false)
                textpager.totalRecord = selectionCardInfo.PageTotalRecord;

            if (textpanel.Visible == true && selectionCardInfo.IsPage == true)
            {
                //过滤选项卡数据
                string filterString = "";
                string s1 = "%";
                string s2 = "%";
                string keyWord = FormatKeyword(editTextBox.Text);



                if (!pressSpaceKey)
                {
                    if (selectionCardInfo.SelectCardFilterType == MatchModes.ByFirstChar)
                        s1 = "";
                    filterString = "";
                    if (selectionCardInfo.QueryFields != null)
                    {
                        for (int i = 0; i < selectionCardInfo.QueryFields.Length - 1; i++)
                        {
                            filterString += selectionCardInfo.QueryFields[i] + " like '" + s1 + keyWord + s2 + "' or ";
                        }
                        filterString += selectionCardInfo.QueryFields[selectionCardInfo.QueryFields.Length - 1] + " like '" + s1 + keyWord + s2 + "'";
                    }
                }
                else
                {
                    filterString = "";
                    if (selectionCardInfo.QueryFields != null)
                    {
                        for (int i = 0; i < selectionCardInfo.QueryFields.Length - 1; i++)
                        {
                            filterString += selectionCardInfo.QueryFields[i] + " = '" + keyWord + "' or ";
                        }
                        filterString += selectionCardInfo.QueryFields[selectionCardInfo.QueryFields.Length - 1] + " = '" + keyWord + "'";
                    }
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
                        textdataGrid.CurrentCell = textdataGrid[GetSelectCardVisableColumnIndex(textdataGrid), 0];
                }
            }

            if (selectionCardInfo.IsPage == false)
            {
                textpager.pageNo = 1;
            }
        }

        void editTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CurrentCell.IsInEditMode)
            {
                if (textpanel.Visible ==true && ColumnIsBindSelectionCard(CurrentCell.ColumnIndex))
                {
                    bool showCardVisable = true;

                    DataGridViewSelectionCard selectCardInfo = ((DataGridViewSelectionCard)textpanel.Tag);
                  
                    if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) && showCardVisable == true)
                    {
                        bool isEmptyRow = true;
                        for (int i = 0; i < textdataGrid.Columns.Count; i++)
                        {
                            int keyRow = Convert.ToInt32(e.KeyChar.ToString()) - 1;
                            if (keyRow < 0)
                                keyRow = 9;
                            if (keyRow > textdataGrid.Rows.Count - 1)
                            {
                                isEmptyRow = true;
                                break;
                            }
                            if (textdataGrid[i, keyRow].Value != null && textdataGrid[i, keyRow].Value.ToString() != "")
                            {
                                isEmptyRow = false;
                                break;
                            }
                        }
                        if (!isEmptyRow)
                        {
                            int rowIndex = Convert.ToInt32(e.KeyChar.ToString()) - 1;
                            if (rowIndex == -1)
                                rowIndex = 9;
                            if (rowIndex > textdataGrid.Rows.Count - 1)
                            {
                                e.Handled = true;
                                return;
                            }
                            textdataGrid.CurrentCell = textdataGrid[GetSelectCardVisableColumnIndex(textdataGrid), rowIndex];

                            this.Focus();
                            e.Handled = true;
                            this.EndEdit();
                            System.Windows.Forms.Message msg = new System.Windows.Forms.Message();
                            ProcessCmdKey(ref  msg, System.Windows.Forms.Keys.Enter);
                        }
                        else
                        {
                            e.Handled = true;
                            return;
                        }
                    }

                }
                else
                {
                    int keyAsc = (int)e.KeyChar;
                    if (Columns[CurrentCell.ColumnIndex].GetType() != typeof(DataGridViewTextBoxColumn))
                    {
                        e.Handled = false;
                        return;
                    }
                 
                    if (keyAsc == 8 || keyAsc == 13)
                        e.Handled = false;
                }

            }
            else
            {
                if (textpanel.Visible)
                    textpanel.Hide();
            }
        }
        /// <summary>
        /// 网格编辑文本框按键事件
        /// </summary>
        private KeyPressEventHandler editTextBoxKeyPressEventHandler;
        /// <summary>
        /// 网格编辑文本框值改变事件
        /// </summary>
        private EventHandler editTextBoxTextChangeEventHandler;

        private DataTable _source;
        /// <summary>
        /// 判断指定的列是否绑定了选项卡
        /// </summary>
        /// <returns></returns>
        private bool ColumnIsBindSelectionCard(int ColumnIndex)
        {
            if (selectionCards != null)
            {
                for (int i = 0; i < selectionCards.Length; i++)
                {
                    if (selectionCards[i].BindColumnIndex == ColumnIndex)
                    {

                        DataGridViewColumn[] columns = new DataGridViewColumn[selectionCards[i].ShowCardColumns.Length];
                        for (int j = 0; j < selectionCards[i].ShowCardColumns.Length; j++)
                        {
                            columns[j] = selectionCards[i].ShowCardColumns[j];
                        }
                        this.textdataGrid.Columns.Clear();
                        this.textdataGrid.Columns.AddRange(columns);

                        textpanel.Tag = selectionCards[i];

                        _source = selectionCards[i].DataSource;
                        
                        //this.textpager.DataSource = _source;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 绑定了选项卡的索引
        /// </summary>
        /// <returns></returns>
        private int GetBindSelectionCardIndex(int ColumnIndex)
        {
            for (int i = 0; i < selectionCards.Length; i++)
            {
                if (selectionCards[i].BindColumnIndex == ColumnIndex)
                {
                    return i;
                }
            }
            return 0;
        }
        /// <summary>
        /// 设置选择卡位置
        /// </summary>
        private void SetSelectCardLocation(int ColumnIndex)
        {
            int grdX = 0;
            int grdY = 0;
            System.Windows.Forms.Control pctrl = this.Parent;
            while (true)
            {
                if (pctrl is System.Windows.Forms.Form)
                    break;
                else
                {
                    grdX += pctrl.Left;
                    grdY += pctrl.Top;

                    pctrl = pctrl.Parent;
                    if (pctrl is System.Windows.Forms.Form)
                    {
                        break;
                    }
                }
            }
            //窗体
            System.Windows.Forms.Form parentForm = (System.Windows.Forms.Form)pctrl;
            if (!parentForm.Controls.Contains(textpanel))
            {
                parentForm.Controls.Add(textpanel);
            }

            //正常情况下的坐标
            grdX = grdX + this.Left + this.RowHeadersWidth;
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Visible)
                {
                    if (i == CurrentCell.ColumnIndex)
                        break;
                    else
                        grdX = grdX + Columns[i].Width;
                }
            }

            //顶部Y坐标
            grdY = grdY + this.Top + this.ColumnHeadersHeight;
            //计算相对可见行是第几行
            int visiableRowIndex = this.CurrentRow.Index - this.FirstDisplayedCell.RowIndex + 1;
            grdY = grdY + this.RowTemplate.Height * visiableRowIndex;


            int buttomLeft = parentForm.Height - grdY - this.RowTemplate.Height;//选项卡当前位置的底部与窗体下边界的距离(留1个行高的距离)
            //如果超出下边界
            if (textpanel.Height > buttomLeft)
            {
                //在上面显示选择卡
                grdY = grdY - this.RowTemplate.Height - textpanel.Height;
                if (grdY < 0)
                    grdY = 0;
                else
                    grdY = grdY + 0;
            }
            else
            {
                grdY = grdY + 0;
            }

            int rightLeft = parentForm.Width - grdX; //选项卡当前位置的右部与窗体右边界的距离
            int tmpx = grdX;//暂存
            //如果超出右边界,选项卡的右部与窗体右边界对齐
            if (textpanel.Width > rightLeft)
            {
                grdX = grdX - textpanel.Width + rightLeft - 10;
                if (grdX < 0)
                    grdX = tmpx;
                else
                    grdX = grdX - 0;
            }
            else
            {
                grdX = grdX + 0;
            }

            textpanel.Location = new System.Drawing.Point(grdX, grdY);
            textpanel.BringToFront();

        }
        /// <summary>
        /// 网格单元格编辑
        /// </summary>
        /// <param name="e"></param>
        //1.显示ShowCard
        protected override void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
        {
            
            int columnIndex = this.CurrentCell.ColumnIndex;

            if (e.Control.GetType() == typeof(System.Windows.Forms.DataGridViewTextBoxEditingControl))
            {

                editTextBox = (TextBox)e.Control;

                if (editTextBoxKeyPressEventHandler == null)
                {
                    editTextBoxKeyPressEventHandler = new KeyPressEventHandler(editTextBox_KeyPress);
                    editTextBox.KeyPress += editTextBoxKeyPressEventHandler;
                }
                if (editTextBoxTextChangeEventHandler == null)
                {
                    editTextBoxTextChangeEventHandler = new EventHandler(editTextBox_TextChanged);
                    editTextBox.TextChanged += editTextBoxTextChangeEventHandler;
                }
                if (Columns[CurrentCell.ColumnIndex].GetType() == typeof(DataGridViewTextBoxColumn))
                {
                    editTextBox.MaxLength = ((DataGridViewTextBoxColumn)Columns[CurrentCell.ColumnIndex]).MaxInputLength;
                }
                //设定绑定的对应的选项卡的位置
                if (ColumnIsBindSelectionCard(columnIndex))
                {
                    DataGridViewSelectionCard selectionCardInfo = (DataGridViewSelectionCard)textpanel.Tag;
                    textpager.IsPage = selectionCardInfo.IsPage;
                    this.textpager.DataSource = _source;
                    if (textpager.IsPage == false)
                        textpager.totalRecord = selectionCardInfo.PageTotalRecord;

                    if (PageNoChanged != null)
                    {
                        textpager.PageNoChanged -= new PagerEventHandler(textpager_PageNoChanged);
                        textpager.PageNoChanged += new PagerEventHandler(textpager_PageNoChanged);
                        textpager.pageNo = 1;
                    }

                    SetSelectCardLocation(columnIndex);

                    string oldString = editTextBox.Text;
                    editTextBox.Text = oldString;


                    if (hideSelectionCardWhenCustomInput == false)
                        textpanel.Show();
                    else
                        textpanel.Hide();
                }
                else
                {
                    textpanel.Hide();
                }
            }
        }

        void textpager_PageNoChanged(object sender, int pageNo, int pageSize)
        {
            PageNoChanged(this, GetBindSelectionCardIndex(this.CurrentCell.ColumnIndex), pageNo, pageSize, FormatKeyword(editTextBox.Text));
        }
        //2.进入编辑状态
        protected override void OnCellEnter(DataGridViewCellEventArgs e)
        {
            this.BeginEdit(true);           
            base.OnCellEnter(e);
        }
        //2.点击编辑状态
        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            this.BeginEdit(true);
            base.OnCellClick(e);
        }
        //3.结束编辑状态隐藏ShowCard
        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            if (textpanel.Visible == true)
            {
                textpanel.Hide();
            }
            base.OnCellEndEdit(e);
        }
        //3.离开隐藏ShowCard
        protected override void OnLeave(EventArgs e)
        {
            if (textpanel.Visible == true)
            {

                if (this.textdataGrid.Focused == true || this.textpager.comboBoxEx1.Focused == true || this.textpager.slider1.Focused == true)
                {
                    editTextBox.Focus();
                    return;
                }
                else
                {
                    textpanel.Hide();
                }
            }
            //this.EndEdit();
            base.OnLeave(e);
        }
        //3.离开隐藏ShowCard
        protected override void OnCellLeave(DataGridViewCellEventArgs e)
        {
            if (textpanel.Visible == true)
            {
                textpanel.Hide();
            }
            //this.EndEdit();
            base.OnCellLeave(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (textpanel.Visible == true)
            {
                return;
            }
            else
            {
                base.OnMouseWheel(e);
            }
        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            if (textpanel.Visible == true)
            {
                e.NewValue = e.OldValue;
            }
            base.OnScroll(e);
        }

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
            //base.OnDataError(displayErrorDialogIfNoHandler, e);
            return;
        }

        /// <summary>
        /// 获取返回值
        /// </summary>
        /// <param name="grdSelectCard"></param>
        /// <param name="dataSourceIsDataTable"></param>
        /// <returns></returns>
        private object GetReturnValue()
        {
            try
            {
                int grdRowIndex = textdataGrid.CurrentCell.RowIndex;
                return ((DataRowView)textdataGrid.Rows[grdRowIndex].DataBoundItem).Row;
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
        /// <summary>
        /// 获取指定行的下一个可见的，可编辑列
        /// </summary>
        /// <param name="currentColumnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <remarks>如果是最后一行，则返回-1，调用程序需要自己处理是否增加行</remarks>
        private int GetNextVisableEditColumnIndex(int currentColumnIndex, int rowIndex)
        {
            if (currentColumnIndex == Columns.Count - 1)
            {
                return -1;
            }
            else
            {
                while (true)
                {
                    if (currentColumnIndex == Columns.Count)
                    {
                        return -1;
                    }
                    else
                    {
                        currentColumnIndex++;
                        if (currentColumnIndex == Columns.Count)
                        {
                            return -1;
                        }
                        if (Columns[currentColumnIndex].Visible && Columns[currentColumnIndex].ReadOnly == false)
                        {
                            return currentColumnIndex;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取网格第一个可见列的索引
        /// </summary>
        /// <returns></returns>
        private int GetGridVisibleFirstColumnIndex()
        {
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i].Visible)
                    return i;
            }
            return -1;
        }
        /// <summary>
        /// 跳转控制
        /// </summary>
        /// <param name="columnIndex">当前列索引</param>
        /// <param name="rowIndex">当前行索引</param>
        private void ColumnJumpControl(int columnIndex, int rowIndex)
        {
           
            int nextColumn = GetNextVisableEditColumnIndex(columnIndex, rowIndex);
            if (nextColumn == -1)
            {
                if (rowIndex == this.Rows.Count - 1)
                {
                    if (this[columnIndex, rowIndex].ReadOnly == false)
                    {
                        if (this.DataSource != null)
                        {
                            try
                            {
                                //新增一行新纪录,回调一个新增一行的事件给这一行赋值
                                DataTable dtS= (DataTable)this.DataSource;
                                DataRow dr=dtS.NewRow();
                                //加入事件
                                if (UserAddGirdRow != null)
                                    UserAddGirdRow(dr);
                                dtS.Rows.Add(dr);
                            }
                            catch
                            {
                                throw new Exception("你给控件绑定的数据源不是DataTable！");
                            }
                        }
                        else
                        {
                            this.Rows.Add();
                        }
                        rowIndex = this.Rows.Count - 1;
                        nextColumn = GetNextVisableEditColumnIndex(-1, rowIndex);
                        this.CurrentCell = this[nextColumn, rowIndex];
                    }
                    else
                    {
                        this.EndEdit();
                    }
                }
                else
                {
                    rowIndex = rowIndex + 1;
                    nextColumn = GetNextVisableEditColumnIndex(-1, rowIndex);
                    if (nextColumn == -1)
                        nextColumn = GetGridVisibleFirstColumnIndex();
                    this.CurrentCell = this[nextColumn, rowIndex];
                }
            }
            else
            {
                this.CurrentCell = this[nextColumn, rowIndex];
            }

        }
        /// <summary>
        /// 给Gird新增一行
        /// </summary>
        public void AddRow()
        {
            if (this.DataSource != null)
            {
                try
                {
                    //新增一行新纪录,回调一个新增一行的事件给这一行赋值
                    DataTable dtS = (DataTable)this.DataSource;
                    DataRow dr = dtS.NewRow();
                    //加入事件
                    if (UserAddGirdRow != null)
                        UserAddGirdRow(dr);
                    dtS.Rows.Add(dr);
                }
                catch
                {
                    throw new Exception("你给控件绑定的数据源不是DataTable！");
                }
            }
            else
            {
                this.Rows.Add();
            }
            int rowIndex = this.Rows.Count - 1;
            int nextColumn = GetNextVisableEditColumnIndex(-1, rowIndex);
            this.CurrentCell = this[nextColumn, rowIndex];
        }
        //4.按键控制
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            if (textpanel.Visible == true)
            {
                DataGridViewSelectionCard selectCardInfo = ((DataGridViewSelectionCard)textpanel.Tag);
                DataGridViewColumn currentColumn = (DataGridViewColumn)Columns[CurrentCell.ColumnIndex];
                #region up and down key
                if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
                {
                    if (keyData == Keys.Left || keyData == Keys.Right)
                        return true;

                    if (textdataGrid.Rows.Count == 0)
                        return true;
                    int selectionCardColIndex = textdataGrid.CurrentCell.ColumnIndex;
                    int selectionCardRowIndex = textdataGrid.CurrentCell.RowIndex;

                    if (keyData == Keys.Up)
                    {
                        if (selectionCardRowIndex > 0)
                            selectionCardRowIndex = selectionCardRowIndex - 1;
                        else
                            selectionCardRowIndex = 0;
                        textdataGrid.CurrentCell = textdataGrid[selectionCardColIndex, selectionCardRowIndex];
                    }
                    else
                    {
                        if (selectionCardRowIndex == textdataGrid.Rows.Count - 1)
                            selectionCardRowIndex = textdataGrid.Rows.Count - 1;
                        else
                            selectionCardRowIndex = selectionCardRowIndex + 1;
                        textdataGrid.CurrentCell = textdataGrid[selectionCardColIndex, selectionCardRowIndex];
                    }
                    return true;//返回true，控件不响应键盘事件
                }
                #endregion
                #region enter key
                if (keyData == Keys.Enter)
                {
                    //触发选择事件并跳转
                    if (textdataGrid.Rows.Count == 0)
                        return true;
                    else
                    {
                        //判断是否是空行
                        bool isEmptyRow = true;
                        for (int i = 0; i < textdataGrid.Columns.Count; i++)
                        {
                            int keyRow = textdataGrid.CurrentRow.Index;
                            if (textdataGrid[i, keyRow].Value != null && textdataGrid[i, keyRow].Value.ToString().Trim() != "")
                            {
                                isEmptyRow = false;
                                break;
                            }
                        }
                        if (isEmptyRow)
                            return true;
                    }
                    bool stopJump = false; //辅助参数，用户返回用户是否决定跳转
                    int customNextColumnIndex = CurrentCell.ColumnIndex; //设置用户可能定义的下一个跳转列

                    if (SelectCardRowSelected != null)
                        SelectCardRowSelected(GetReturnValue(), ref stopJump, ref customNextColumnIndex);

                    textpanel.Hide();

                    this.EndEdit();
                    if (!stopJump)
                    {
                        if (customNextColumnIndex == CurrentCell.ColumnIndex)
                        {
                            //如果用户没有指定自定义的列，按可编辑列顺序跳转
                            ColumnJumpControl(CurrentCell.ColumnIndex, CurrentCell.RowIndex);
                        }
                        else
                        {
                            //跳转到指定的列
                            this.CurrentCell = this[customNextColumnIndex, CurrentCell.RowIndex];
                        }
                    }
                    return true;
                }
                #endregion
                #region page key
                if (keyData == Keys.PageUp || keyData == Keys.PageDown)
                {
                    if (keyData == Keys.PageDown)
                    {
                        textpager.pageNo += 1;
                    }
                    if (keyData == Keys.PageUp)
                    {
                        textpager.pageNo -= 1;
                    }
                    return true;
                }
                #endregion
                #region space
                if (keyData == Keys.Space)
                {
                    pressSpaceKey = true;
                    editTextBox_TextChanged(null, null);
                    pressSpaceKey = false;
                    return true;
                }
                #endregion
            }
            else
            {
                if (keyData == Keys.Enter && CurrentCell != null)
                {
                    ColumnJumpControl(CurrentCell.ColumnIndex, CurrentCell.RowIndex);
                    return true;
                }
                if ((int)keyData >= 48 && (int)keyData <= 57 && CurrentCell != null)
                {
                    if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex))
                    {
                        if (textpanel.Visible == false)
                        {
                            return true;
                        }
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// 当自定输入的时候隐藏选项卡
        /// </summary>
        private bool hideSelectionCardWhenCustomInput = false;
        [Description("当自定输入的时候是否隐藏选项卡")]
        public bool HideSelectionCardWhenCustomInput
        {
            get
            {
                return hideSelectionCardWhenCustomInput;
            }
            set
            {
                hideSelectionCardWhenCustomInput = value;
            }
        }

        /// <summary>
        /// 绑定的选项卡数组
        /// </summary>
        private DataGridViewSelectionCard[] selectionCards;
        [Description("获取或设置选项卡")]
        public DataGridViewSelectionCard[] SelectionCards
        {
            get
            {
                return selectionCards;
            }
            set
            {
                selectionCards = value;
            }
        }

        //6.翻页的事件
        [Description("翻页的事件，结合IsPage=false属性使用")]
        public event PagerGridEventHandler PageNoChanged;
        
        //5.选择后的事件
        [Description("用户选定选择卡记录后发生")]
        public event OnSelectCardRowSelectedHandle SelectCardRowSelected;

        //6.新增一行的事件
        [Description("用户新增一行记录时触发")]
        public event UserAddGirdRowHandler UserAddGirdRow;
    }

    /// <summary>
    /// DataGridViewEx中的选项卡选中行后的事件委托
    /// </summary>
    /// <param name="SelectedValue"></param>
    /// <param name="stop"></param>
    /// <param name="customNextColumnIndex"></param>
    public delegate void OnSelectCardRowSelectedHandle(object SelectedValue, ref bool stop, ref int customNextColumnIndex);
    /// <summary>
    /// 翻页事件
    /// </summary>
    /// <param name="sender">对象</param>
    /// <param name="index">ShowCard数据源索引</param>
    /// <param name="pageNo">当前页</param>
    /// <param name="pageSize">页大小</param>
    public delegate void PagerGridEventHandler(object sender,int index, int pageNo, int pageSize,string fiterChar);

    /// <summary>
    /// 自动新增一行时给dataRow赋值
    /// </summary>
    /// <param name="dataRow">新增一行赋值</param>
    public delegate void UserAddGirdRowHandler(DataRow dataRow);



    /// <summary>
    /// 网格选项卡类
    /// </summary>
    public class DataGridViewSelectionCard
    {
        private DataGridViewTextBoxColumn[] columns;
        private System.Data.DataTable dataSource;
        private System.Drawing.Size cardSize = new Size(350, 276);
        private MatchModes selectCardFilterType;
        private int bindColumnIndex;
        private DataRow[] filterResult;
        public DataGridViewSelectionCard()
        {

        }

        /// <summary>
        /// 选择卡列信息
        /// </summary>
        public DataGridViewTextBoxColumn[] ShowCardColumns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }
        /// <summary>
        /// 选择卡数据源
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
        /// 选择卡大小
        /// </summary>
        public System.Drawing.Size CardSize
        {
            get
            {
                return cardSize;
            }
            set
            {
                cardSize = value;
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
        /// 选择卡记录过滤方式
        /// </summary>
        public MatchModes SelectCardFilterType
        {
            get
            {
                return selectCardFilterType;
            }
            set
            {
                selectCardFilterType = value;
            }
        }
       
        /// <summary>
        /// 获取或设置选项卡要绑定到的列的索引
        /// </summary>
        public int BindColumnIndex
        {
            get
            {
                return bindColumnIndex;
            }
            set
            {
                bindColumnIndex = value;
            }
        }
     
        public DataRow[] FilterResult
        {
            get
            {
                return filterResult;
            }
            set
            {
                filterResult = value;
            }
        }

        private bool _isPage;
        [Description("设置是否内部分页，IsPage=false时结合PagerEventHandler事件使用")]
        public bool IsPage
        {
            get { return _isPage; }
            set { _isPage = value; }
        }

        private int _pageTotalRecord;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int PageTotalRecord
        {
            get { return _pageTotalRecord; }
            set { _pageTotalRecord = value; }
        }
       
    }
}

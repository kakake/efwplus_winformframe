using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.ComponentModel;
using DevComponents.DotNetBar;

namespace EfwControls.CustomControl
{

    public partial class GridBoxCard : DataGrid
    {
        /// <summary>
        /// 网格编辑框对象
        /// </summary>
        private TextBox editTextBox;
        /// <summary>
        /// 处理空白键,全字匹配
        /// </summary>
        private bool pressSpaceKey;
        /// <summary>
        /// 网格编辑文本框按键事件
        /// </summary>
        private KeyPressEventHandler editTextBoxKeyPressEventHandler;
        /// <summary>
        /// 网格编辑文本框值改变事件
        /// </summary>
        private EventHandler editTextBoxTextChangeEventHandler;
        /// <summary>
        /// 当前选项卡的数据源
        /// </summary>
        //private DataTable _source;

        /// <summary>
        /// 选项卡
        /// </summary>
        private CardDataGrid[] cardDataGrids;
        /// <summary>
        /// 数字键盘
        /// </summary>
        private NumericKeyBoard numKeyBoard;

        public GridBoxCard()
        {
            //cardDataGrid = new CardDataGrid();
            numKeyBoard = new NumericKeyBoard();
        }



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
        /// 获取返回值
        /// </summary>
        /// <param name="grdSelectCard"></param>
        /// <param name="dataSourceIsDataTable"></param>
        /// <returns></returns>
        private object GetReturnValue(CardDataGrid cardDataGrid)
        {
            try
            {
                int grdRowIndex = cardDataGrid.textdataGrid.CurrentCell.RowIndex;
                return ((DataRowView)cardDataGrid.textdataGrid.Rows[grdRowIndex].DataBoundItem).Row;
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
                        //if (this.DataSource != null)
                        //{
                        //    try
                        //    {
                        //        //新增一行新纪录,回调一个新增一行的事件给这一行赋值
                        //        DataTable dtS = (DataTable)this.DataSource;
                        //        DataRow dr = dtS.NewRow();

                        //        //加入事件
                        //        if (UserAddGirdRow != null)
                        //            UserAddGirdRow(dr);
                        //        else
                        //        {
                                    
                        //            dtS.Rows.Add(dr);
                        //        }
                        //    }
                        //    catch
                        //    {
                        //        throw new Exception("你给控件绑定的数据源不是DataTable！");
                        //    }
                        //}
                        //else
                        //{
                        //    this.Rows.Add();
                        //}
                        //rowIndex = this.Rows.Count - 1;
                        //nextColumn = GetNextVisableEditColumnIndex(-1, rowIndex);
                        //this.CurrentCell = this[nextColumn, rowIndex];

                        rowIndex = AddRow(delegate()
                        {
                            if (this[columnIndex, rowIndex].Value == DBNull.Value)
                                return false;
                            else
                                return true;
                        });
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
        /// 设置选择卡位置
        /// </summary>
        private void SetSelectCardLocation(int ColumnIndex, CardDataGrid cardDataGrid)
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
            if (!parentForm.Controls.Contains(cardDataGrid))
            {
                parentForm.Controls.Add(cardDataGrid);
            }

            //正常情况下的坐标
            grdX = grdX + this.Left + (this.RowHeadersVisible == true ? this.RowHeadersWidth : 0);
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
            if (cardDataGrid.Height > buttomLeft)
            {
                //在上面显示选择卡
                grdY = grdY - this.RowTemplate.Height - cardDataGrid.Height;
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
            if (cardDataGrid.Width > rightLeft)
            {
                grdX = grdX - cardDataGrid.Width + rightLeft - 10;
                if (grdX < 0)
                    grdX = tmpx;
                else
                    grdX = grdX - 0;
            }
            else
            {
                grdX = grdX + 0;
            }

            cardDataGrid.Location = new System.Drawing.Point(grdX, grdY);

            cardDataGrid.BringToFront();

        }

        private void SetSelectNumKeyBoardLocation(int columnIndex)
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
            if (!parentForm.Controls.Contains(numKeyBoard))
            {
                parentForm.Controls.Add(numKeyBoard);
            }

            //正常情况下的坐标
            grdX = grdX + this.Left + (this.RowHeadersVisible == true ? this.RowHeadersWidth : 0);
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
            if (numKeyBoard.Height > buttomLeft)
            {
                //在上面显示选择卡
                grdY = grdY - this.RowTemplate.Height - numKeyBoard.Height;
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
            if (numKeyBoard.Width > rightLeft)
            {
                grdX = grdX - numKeyBoard.Width + rightLeft - 10;
                if (grdX < 0)
                    grdX = tmpx;
                else
                    grdX = grdX - 0;
            }
            else
            {
                grdX = grdX + 0;
            }

            numKeyBoard.Location = new System.Drawing.Point(grdX, grdY);

            numKeyBoard.BringToFront();
        }
        /// <summary>
        /// 判断指定的列是否绑定了选项卡
        /// </summary>
        /// <returns></returns>
        private bool ColumnIsBindSelectionCard(int ColumnIndex, out int cardIndex)
        {
            if (selectionCards != null && cardDataGrids != null)
            {
                for (int i = 0; i < selectionCards.Length; i++)
                {
                    if (selectionCards[i].BindColumnIndex == ColumnIndex)
                    {
                        cardIndex = i;

                        if (cardDataGrids[cardIndex] != null)
                            return true;
                    }
                }
            }
            cardIndex = -1;
            return false;
        }

        private bool ColumnIsBindSelectionNumKeyBoard(int ColumnIndex)
        {
            if (selectionNumKeyBoards != null)
            {
                for (int i = 0; i < selectionNumKeyBoards.Length; i++)
                {
                    if (selectionNumKeyBoards[i].BindColumnIndex == ColumnIndex)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void InitShowCard(int ColumnIndex,CardDataGrid cardDataGrid)
        {
            //if (PageNoChanged != null)
            //{
            //    cardDataGrid.textpager.PageNoChanged += new PagerEventHandler(textpager_PageNoChanged);
            //    cardDataGrid.textpager.pageNo = 1;
            //}

            cardDataGrid.textdataGrid.DoubleClick += new EventHandler(dgvSelectCard_DoubleClick);
            cardDataGrid.ClickLetter += new EventHandler(cardDataGrid_ClickLetter);
            cardDataGrid.DeleteLetter += new EventHandler(cardDataGrid_DeleteLetter);
            cardDataGrid.ConfirmLetter += new EventHandler(cardDataGrid_ConfirmLetter);
            cardDataGrid.CloseLetter += new EventHandler(cardDataGrid_CloseLetter);


            int cardIndex;
            if (ColumnIsBindSelectionCard(ColumnIndex,out cardIndex))
            {
                DataGridViewColumn[] columns = null;
                if (string.IsNullOrEmpty(selectionCards[cardIndex].CardColumn))
                {
                    columns = new DataGridViewColumn[selectionCards[cardIndex].ShowCardColumns.Length];
                    for (int j = 0; j < selectionCards[cardIndex].ShowCardColumns.Length; j++)
                    {
                        columns[j] = selectionCards[cardIndex].ShowCardColumns[j];
                    }
                }
                else
                {
                    string[] Columns = selectionCards[cardIndex].CardColumn.Split(new char[] { ',' });
                    columns = new DataGridViewColumn[Columns.Length];
                    for (int k = 0; k < columns.Length; k++)
                    {
                        string[] pms = Columns[k].Split(new char[] { '|' });
                        columns[k] = new DataGridViewTextBoxColumn();
                        columns[k].Name = "col" + pms[0];
                        columns[k].HeaderText = pms[1];
                        columns[k].DataPropertyName = pms[0];
                        if (pms[2] == "auto")
                            columns[k].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        else
                            columns[k].Width = Convert.ToInt32(pms[2]);
                        columns[k].ReadOnly = true;
                        columns[k].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
                        //columns[i].DefaultCellStyle = new DataGridViewCellStyle();
                    }
                }
                cardDataGrid.textdataGrid.Columns.Clear();
                cardDataGrid.textdataGrid.Columns.AddRange(columns);
                cardDataGrid.Tag = selectionCards[cardIndex];
                DataGridViewSelectionCard selectionCardInfo = selectionCards[cardIndex];
                cardDataGrid.Width = selectionCardInfo.CardSize.Width;
                cardDataGrid.Height = selectionCardInfo.CardSize.Height;
                cardDataGrid.textpager.IsPage = selectionCardInfo.IsPage;
                cardDataGrid.textpager.DataSource = selectionCardInfo.DataSource;
            }
            cardDataGrid.letterpanel.Visible = IsShowLetter;
            cardDataGrid.textpager.Visible = IsShowPage;
            cardDataGrid.DrawLetter();
            
            cardDataGrid.Hide();

            if (PageNoChanged != null)
            {
                cardDataGrid.textpager.PageNoChanged += new PagerEventHandler(textpager_PageNoChanged);
                cardDataGrid.textpager.pageNo = 1;
            }
        }

        private void InitShowNumKeyBoard(int ColumnIndex, NumericKeyBoard numKeyBoard)
        {

        }

        #region 事件
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
                    //editTextBox.MouseWheel += new MouseEventHandler(editTextBox_MouseWheel);
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

                int cardIndex = -1;

                if (ColumnIsBindSelectionCard(columnIndex, out cardIndex))
                {
                    SetSelectCardLocation(columnIndex, cardDataGrids[cardIndex]);
                    if (hideSelectionCardWhenCustomInput == false)
                    {
                        cardDataGrids[cardIndex].Show();
                        //editTextBox_TextChanged(null, null);
                        cardGridBindDataSource();
                    }
                    else
                        cardDataGrids[cardIndex].Hide();
                }
                else if (ColumnIsBindSelectionNumKeyBoard(columnIndex))
                {
                    SetSelectNumKeyBoardLocation(columnIndex);
                    numKeyBoard.Show();
                    //numKeyBoard.InitClickEvent();
                }
            }
        }

        private void cardGridBindDataSource()
        {
            int cardIndex = -1;
            CardDataGrid cardDataGrid;
            if (CurrentCell != null)
            {
                if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex))
                {
                    if (cardDataGrids[cardIndex].Visible == false)
                        return;
                }
                else if (ColumnIsBindSelectionNumKeyBoard(CurrentCell.ColumnIndex))
                {
                    return;
                }
                else
                    return;
            }

            cardDataGrid = cardDataGrids[cardIndex];

            DataGridViewSelectionCard selectionCardInfo = (DataGridViewSelectionCard)cardDataGrid.Tag;

            if (cardDataGrid.Visible == true && selectionCardInfo.IsPage == true)
            {
                if (selectionCardInfo.DataSource != null && selectionCardInfo.DataSource.Rows.Count > 0)
                {
                    DataView dv = new DataView(selectionCardInfo.DataSource);
                    dv.RowFilter = "";
                    DataTable dt = dv.ToTable();
                    cardDataGrid.textpager.DataSource = dt;
                    if (dt.Rows.Count > 0)
                        cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[GetSelectCardVisableColumnIndex(cardDataGrid.textdataGrid), 0];
                }
            }
            if (selectionCardInfo.IsPage == false)
            {
                if (selectionCardInfo.DataSource != null)
                {
                    cardDataGrid.textpager.DataSource = selectionCardInfo.DataSource;
                    //cardDataGrid.textpager.pageNo = 1;
                }
            }
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
            if (cardDataGrids != null)
                for (int i = 0; i < cardDataGrids.Length; i++)
                {
                    if (cardDataGrids[i] != null)
                        cardDataGrids[i].Hide();
                }
            if (numKeyBoard != null)
                numKeyBoard.Hide();
            base.OnCellEndEdit(e);
        }
        //3.离开隐藏ShowCard
        protected override void OnLeave(EventArgs e)
        {
            if (cardDataGrids != null)
                for (int i = 0; i < cardDataGrids.Length; i++)
                {
                    if (cardDataGrids[i] != null && cardDataGrids[i].Visible && cardDataGrids[i].GetIsFocused())
                    {
                        editTextBox.Focus();
                        return;
                    }
                    else
                        if (cardDataGrids[i] != null)
                            cardDataGrids[i].Hide();
                }

            if (numKeyBoard != null)
                if (numKeyBoard.GetIsFocused() && numKeyBoard.Visible)
                {
                    editTextBox.Focus();
                    return;
                }
                else
                    numKeyBoard.Hide();
            //this.EndEdit();
            base.OnLeave(e);
        }
        //3.离开隐藏ShowCard
        protected override void OnCellLeave(DataGridViewCellEventArgs e)
        {
            if (cardDataGrids != null)
                for (int i = 0; i < cardDataGrids.Length; i++)
                {
                    if (cardDataGrids[i] != null && cardDataGrids[i].Visible && cardDataGrids[i].GetIsFocused())
                    {
                        editTextBox.Focus();
                        return;
                    }
                    else
                        if (cardDataGrids[i] != null)
                            cardDataGrids[i].Hide();
                }

            if (numKeyBoard != null)
                if (numKeyBoard.GetIsFocused() && numKeyBoard.Visible)
                {
                    editTextBox.Focus();
                    return;
                }
                else
                    numKeyBoard.Hide();
            //this.EndEdit();
            base.OnCellLeave(e);
        }
        //4.按键控制
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            int cardIndex = -1;
            if (CurrentCell != null &&  ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex))
            {
                CardDataGrid cardDataGrid = cardDataGrids[cardIndex];
                if (cardDataGrid.Visible == true)
                {
                    DataGridViewSelectionCard selectCardInfo = ((DataGridViewSelectionCard)cardDataGrid.Tag);
                    DataGridViewColumn currentColumn = (DataGridViewColumn)Columns[CurrentCell.ColumnIndex];
                    #region up and down key
                    if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
                    {
                        if (keyData == Keys.Left || keyData == Keys.Right)
                            return true;

                        if (cardDataGrid.textdataGrid.Rows.Count == 0)
                            return true;
                        int selectionCardColIndex = cardDataGrid.textdataGrid.CurrentCell.ColumnIndex;
                        int selectionCardRowIndex = cardDataGrid.textdataGrid.CurrentCell.RowIndex;

                        if (keyData == Keys.Up)
                        {
                            if (selectionCardRowIndex > 0)
                                selectionCardRowIndex = selectionCardRowIndex - 1;
                            else
                                selectionCardRowIndex = 0;
                            cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[selectionCardColIndex, selectionCardRowIndex];
                        }
                        else
                        {
                            if (selectionCardRowIndex == cardDataGrid.textdataGrid.Rows.Count - 1)
                                selectionCardRowIndex = cardDataGrid.textdataGrid.Rows.Count - 1;
                            else
                                selectionCardRowIndex = selectionCardRowIndex + 1;
                            cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[selectionCardColIndex, selectionCardRowIndex];
                        }
                        return true;//返回true，控件不响应键盘事件
                    }
                    #endregion
                    #region enter key
                    if (keyData == Keys.Enter)
                    {
                        //触发选择事件并跳转
                        if (cardDataGrid.textdataGrid.Rows.Count == 0)
                            return true;
                        else
                        {
                            //判断是否是空行
                            bool isEmptyRow = true;
                            for (int i = 0; i < cardDataGrid.textdataGrid.Columns.Count; i++)
                            {
                                int keyRow = cardDataGrid.textdataGrid.CurrentRow.Index;
                                if (cardDataGrid.textdataGrid[i, keyRow].Value != null && cardDataGrid.textdataGrid[i, keyRow].Value.ToString().Trim() != "")
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

                        this.EndEdit();
                        if (SelectCardRowSelected != null)
                            SelectCardRowSelected(GetReturnValue(cardDataGrid), ref stopJump, ref customNextColumnIndex);

                        cardDataGrid.Hide();

                        
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
                            cardDataGrid.textpager.pageNo += 1;
                        }
                        if (keyData == Keys.PageUp)
                        {
                            cardDataGrid.textpager.pageNo -= 1;
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
                        bool jumpStop = false;
                        if (this.DataGridViewCellPressEnterKey != null)
                        {
                            this.DataGridViewCellPressEnterKey(this, base.CurrentCell.ColumnIndex, base.CurrentCell.RowIndex, ref jumpStop);
                        }
                        if (!jumpStop)
                        {
                            this.ColumnJumpControl(base.CurrentCell.ColumnIndex, base.CurrentCell.RowIndex);
                        }

                        return true;
                    }
                    if ((int)keyData >= 48 && (int)keyData <= 57 && CurrentCell != null)
                    {
                        if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex))
                        {
                            if (cardDataGrid.Visible == false)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            else if (CurrentCell != null && ColumnIsBindSelectionNumKeyBoard(CurrentCell.ColumnIndex))
            {
                if (keyData == Keys.Enter && CurrentCell != null)
                {
                    bool jumpStop = false;
                    if (this.DataGridViewCellPressEnterKey != null)
                    {
                        this.DataGridViewCellPressEnterKey(this, base.CurrentCell.ColumnIndex, base.CurrentCell.RowIndex, ref jumpStop);
                    }
                    if (!jumpStop)
                    {
                        this.ColumnJumpControl(base.CurrentCell.ColumnIndex, base.CurrentCell.RowIndex);
                    }
                    //ColumnJumpControl(CurrentCell.ColumnIndex, CurrentCell.RowIndex);
                    return true;
                }
            }

            if (keyData == Keys.Enter && CurrentCell != null)
            {
                bool jumpStop = false;
                if (this.DataGridViewCellPressEnterKey != null)
                {
                    this.DataGridViewCellPressEnterKey(this, base.CurrentCell.ColumnIndex, base.CurrentCell.RowIndex, ref jumpStop);
                }
                if (!jumpStop)
                {
                    this.ColumnJumpControl(base.CurrentCell.ColumnIndex, base.CurrentCell.RowIndex);
                }

                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int cardIndex = -1;
            CardDataGrid cardDataGrid = null;
            if (CurrentCell == null) return;
            if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex))
            {
                cardDataGrid = cardDataGrids[cardIndex];
                if (cardDataGrid.Visible == true)
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
            }
            else
            {
                if (cardDataGrids != null)
                    for (int i = 0; i < cardDataGrids.Length; i++)
                    {
                        if (cardDataGrids[i] != null && cardDataGrids[i].Visible == true)
                        {
                            return;
                        }
                    }
                if (numKeyBoard != null)
                    numKeyBoard.Hide();
                base.OnMouseWheel(e);
            }

        }

        protected override void OnScroll(ScrollEventArgs e)
        {
            if (cardDataGrids != null)
            for (int i = 0; i < cardDataGrids.Length; i++)
            {
                if (cardDataGrids[i] != null && cardDataGrids[i].Visible == true)
                {
                    e.NewValue = e.OldValue;
                }
            }
            if (numKeyBoard != null)
                numKeyBoard.Hide();
            base.OnScroll(e);
        }

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = false;
            //base.OnDataError(displayErrorDialogIfNoHandler, e);
            return;
        }

        //7.文本过滤
        void editTextBox_TextChanged(object sender, EventArgs e)
        {
            int cardIndex = -1;
            CardDataGrid cardDataGrid;
            if (CurrentCell != null)
            {
                if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex))
                {
                    if (cardDataGrids[cardIndex].Visible == false)
                        return;
                }
                else if (ColumnIsBindSelectionNumKeyBoard(CurrentCell.ColumnIndex))
                {
                    return;
                }
                else
                    return;
            }

            cardDataGrid = cardDataGrids[cardIndex];

            DataGridViewSelectionCard selectionCardInfo = (DataGridViewSelectionCard)cardDataGrid.Tag;

            if (cardDataGrid.Visible == true && selectionCardInfo.IsPage == true)
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


                if (selectionCardInfo.DataSource != null && selectionCardInfo.DataSource.Rows.Count>0)
                {
                    DataView dv = new DataView(selectionCardInfo.DataSource);
                    dv.RowFilter = filterString;
                    DataTable dt= dv.ToTable();
                    cardDataGrid.textpager.DataSource = dt;

                    //DataRow[] drs = selectionCardInfo.DataSource.Select(filterString);
                    //DataTable dt = selectionCardInfo.DataSource.Clone();
                    //for (int i = 0; i < drs.Length; i++)
                    //{
                    //    dt.Rows.Add(drs[i].ItemArray);
                    //}
                    //cardDataGrid.textpager.DataSource = dt;
                    if (dt.Rows.Count > 0)
                        cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[GetSelectCardVisableColumnIndex(cardDataGrid.textdataGrid), 0];
                }
            }

            if (selectionCardInfo.IsPage == false)
            {
                if (selectionCardInfo.DataSource != null)
                {
                    cardDataGrid.textpager.DataSource = selectionCardInfo.DataSource;
                    cardDataGrid.textpager.pageNo = 1;
                }
            }
        }

        void editTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CurrentCell.IsInEditMode)
            {
                int cardIndex = -1;
                CardDataGrid cardDataGrid;
                if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex) && cardDataGrids[cardIndex].Visible == true)
                {
                    cardDataGrid = cardDataGrids[cardIndex];
                    bool showCardVisable = true;

                    DataGridViewSelectionCard selectCardInfo = ((DataGridViewSelectionCard)cardDataGrid.Tag);

                    if (((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57) && showCardVisable == true && IsInputNumSelectedCard==true)
                    {
                        bool isEmptyRow = true;
                        for (int i = 0; i < cardDataGrid.textdataGrid.Columns.Count; i++)
                        {
                            int keyRow = Convert.ToInt32(e.KeyChar.ToString()) - 1;
                            if (keyRow < 0)
                                keyRow = 9;
                            if (keyRow > cardDataGrid.textdataGrid.Rows.Count - 1)
                            {
                                isEmptyRow = true;
                                break;
                            }
                            if (cardDataGrid.textdataGrid[i, keyRow].Value != null && cardDataGrid.textdataGrid[i, keyRow].Value.ToString() != "")
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
                            if (rowIndex > cardDataGrid.textdataGrid.Rows.Count - 1)
                            {
                                e.Handled = true;
                                return;
                            }
                            cardDataGrid.textdataGrid.CurrentCell = cardDataGrid.textdataGrid[GetSelectCardVisableColumnIndex(cardDataGrid.textdataGrid), rowIndex];

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
                int cardIndex = -1;
                CardDataGrid cardDataGrid=null;
                if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex) && cardDataGrids[cardIndex].Visible == true)
                {
                    cardDataGrid = cardDataGrids[cardIndex];
                    cardDataGrid.Hide();
                }
            }
        }

        void editTextBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (CurrentCell.IsInEditMode)
            {
                 int cardIndex = -1;
                CardDataGrid cardDataGrid=null;
                if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex) && cardDataGrids[cardIndex].Visible == true)
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
            }
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

        void textpager_PageNoChanged(object sender, int pageNo, int pageSize)
        {
            if (this.CurrentCell != null)
                PageNoChanged(this, GetBindSelectionCardIndex(this.CurrentCell.ColumnIndex), pageNo, pageSize, FormatKeyword(editTextBox.Text));
        }

        void cardDataGrid_CloseLetter(object sender, EventArgs e)
        {
            int cardIndex = -1;
            if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex) && cardDataGrids[cardIndex].Visible == true)
            {
                cardDataGrids[cardIndex].Hide();
            }
        }
        void cardDataGrid_ConfirmLetter(object sender, EventArgs e)
        {
            int cardIndex = -1;
            if (ColumnIsBindSelectionCard(CurrentCell.ColumnIndex, out cardIndex) && cardDataGrids[cardIndex].Visible == true)
            {
                dgvSelectCard_DoubleClick(cardDataGrids[cardIndex].textdataGrid, null);
            }
        }
        void cardDataGrid_DeleteLetter(object sender, EventArgs e)
        {
            if (editTextBox.Text.Length > 0)
                editTextBox.Text = editTextBox.Text.Substring(0, editTextBox.Text.Length - 1);
            editTextBox.SelectionStart = editTextBox.Text.Length;
        }
        void cardDataGrid_ClickLetter(object sender, EventArgs e)
        {
            editTextBox.Text += (sender as Label).Text.ToLower();
            editTextBox.SelectionStart = editTextBox.Text.Length;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        #endregion

        #region 自定义属性
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
                if (selectionCards != null)
                    cardDataGrids = new CardDataGrid[selectionCards.Length];
            }
        }

        private DataGridViewSelectionNumericKeyBoard[] selectionNumKeyBoards;
        [Description("获取或设置数字键盘")]
        public DataGridViewSelectionNumericKeyBoard[] SelectionNumKeyBoards
        {
            get
            {
                return selectionNumKeyBoards;
            }
            set
            {
                selectionNumKeyBoards = value;
                if (selectionNumKeyBoards != null)
                {
                    numKeyBoard = new NumericKeyBoard();
                    numKeyBoard.ClickNum += new EventHandler(numKeyBoard_ClickNum);
                    numKeyBoard.ConfirmNum += new EventHandler(numKeyBoard_ConfirmNum);
                    numKeyBoard.DeleteNum += new EventHandler(numKeyBoard_DeleteNum);
                    numKeyBoard.InitClickEvent();
                    numKeyBoard.Hide();
                }
            }
        }

        void numKeyBoard_DeleteNum(object sender, EventArgs e)
        {
            if (editTextBox.Text.Length > 0)
                editTextBox.Text = editTextBox.Text.Substring(0, editTextBox.Text.Length - 1);
            editTextBox.SelectionStart = editTextBox.Text.Length;
        }

        void numKeyBoard_ConfirmNum(object sender, EventArgs e)
        {
            System.Windows.Forms.Message msg = new System.Windows.Forms.Message();
            ProcessCmdKey(ref  msg, System.Windows.Forms.Keys.Enter);
        }

        void numKeyBoard_ClickNum(object sender, EventArgs e)
        {
            editTextBox.Text += (sender as LabelX).Tag.ToString().ToLower();
            editTextBox.SelectionStart = editTextBox.Text.Length;
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

        private bool _IsInputNumSelectedCard = true;
        [Description("是否输入数字选定")]
        public bool IsInputNumSelectedCard
        {
            get { return _IsInputNumSelectedCard; }
            set { _IsInputNumSelectedCard = value; }
        }
        #endregion

        #region 自定义事件
        //6.翻页的事件
        [Description("翻页的事件，结合DataGridViewSelectionCard属性的IsPage=false属性使用")]
        public event PagerGridEventHandler PageNoChanged;

        //5.选择后的事件
        [Description("用户选定选择卡记录后触发")]
        public event OnSelectCardRowSelectedHandle SelectCardRowSelected;

        //6.新增一行的事件
        [Description("用户新增一行记录时触发")]
        public event UserAddGirdRowHandler UserAddGirdRow;

        [Description("用户在单元格回车键")]
        public event OnDataGridViewCellPressEnterKeyHandle DataGridViewCellPressEnterKey;


        #endregion

        #region 控件开放的方法
        /// <summary>
        /// 给Gird新增一行
        /// </summary>
        public int AddRow()
        {
            if (this.DataSource != null)
            {
                try
                {
                    //新增一行新纪录,回调一个新增一行的事件给这一行赋值
                    DataTable dtS = (DataTable)this.DataSource;
                    DataRow dr = dtS.NewRow();
                    //加入事件
                    //if (UserAddGirdRow != null)
                    //    UserAddGirdRow(dr);
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

            return rowIndex;
        }

        /// <summary>
        /// 给Gird新增一行
        /// </summary>
        public int AddRow(UserAddGridRowCustom userAdd)
        {
            if (this.DataSource != null)
            {
                try
                {
                    if (userAdd != null && userAdd() == true)
                    {
                        //新增一行新纪录,回调一个新增一行的事件给这一行赋值
                        DataTable dtS = (DataTable)this.DataSource;
                        DataRow dr = dtS.NewRow();
                        //加入事件
                        //if (UserAddGirdRow != null)
                        //    UserAddGirdRow(dr);
                        dtS.Rows.Add(dr);
                    }
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

            return rowIndex;
        }

        public void BindSelectionCardDataSource(int index, DataTable DataSource)
        {
            if (selectionCards.Length > 0 && selectionCards.Length>index)
            {
                selectionCards[index].DataSource = DataSource;

                cardDataGrids[index]=new CardDataGrid();
                InitShowCard(selectionCards[index].BindColumnIndex, cardDataGrids[index]);
            }
        }

        #endregion
    }

   
}

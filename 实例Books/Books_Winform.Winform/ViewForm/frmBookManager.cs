using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFWCoreLib.WinformFrame.Controller;
using Books_Winform.Winform.IView;
using Books_Winform.Entity;

namespace Books_Winform.Winform.Viewform
{
    public partial class frmBookManager : BaseForm, IfrmBookManager
    {
        public frmBookManager()
        {
            InitializeComponent();

            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            InvokeController("Exit");
        }

        //新增
        private void button1_Click(object sender, EventArgs e)
        {
            Books book= new Books();
            book.BuyPrice = 10;
            book.BuyDate = DateTime.Now;
            currBook = book;
            button2.Enabled = true;
        }
        //保存
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("书籍名称不能为空！");
                textBox1.Focus();
                return;
            }

            InvokeController("bookSave");
        }

        //点击网格
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null) return;

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            DataTable dt = (DataTable)dataGridView1.DataSource;

            Books book = new Books();
            book.Id = Convert.ToInt32(dt.Rows[rowindex]["Id"]);
            book.BookName = dt.Rows[rowindex]["BookName"].ToString();
            book.BuyPrice = Convert.ToDecimal(dt.Rows[rowindex]["BuyPrice"]);
            book.BuyDate = Convert.ToDateTime(dt.Rows[rowindex]["BuyDate"]);
            book.Flag = Convert.ToInt32(dt.Rows[rowindex]["Flag"]);


            currBook = book;

        }

        #region IfrmBookManager

        public void loadbooks(DataTable dt)
        {
            dataGridView1.DataSource = dt;
        }

        private Books _book;
        public Books currBook
        {
            get
            {
                _book.BookName = textBox1.Text;
                _book.BuyPrice = Convert.ToDecimal(textBox2.Text);
                _book.BuyDate = dateTimePicker1.Value;
                _book.Flag = checkBox1.Checked ? 1 : 0;
                return _book;
            }
            set
            {
                _book = value;
                textBox1.Text = _book.BookName;
                textBox2.Text = _book.BuyPrice.ToString();
                dateTimePicker1.Value = _book.BuyDate;
                checkBox1.Checked = _book.Flag == 0 ? false : true;
            }
        }
        #endregion

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EfwControls.CustomControl;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Winform.IView;

namespace WinMainUIFrame.Winform.ViewForm
{
    public partial class FrmPassWord : BaseFormEx, IfrmPassWord
    {
        public FrmPassWord()
        {
            InitializeComponent();

            this.frmForm1.AddItem(this.textBoxX1, null, "请输入原始密码！");
            this.frmForm1.AddItem(this.textBoxX2, null, "请输入新密码！");
            this.frmForm1.AddItem(this.textBoxX3, null, "请再次输入新密码！");
        }

        #region IfrmPassWord 成员

        public string oldpass
        {
            get { return textBoxX1.Text; }
        }

        public string newpass
        {
            get { return textBoxX2.Text; }
        }

        public string newpass2
        {
            get { return textBoxX3.Text; }
        }

        public void clearPass()
        {
            frmForm1.Clear();
        }

        #endregion

        private void buttonX1_Click(object sender, EventArgs e)
        {
            if (newpass != newpass2)
            {
                MessageBoxEx.Show("两次输入的密码不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                if (frmForm1.Validate())
                {
                    InvokeController("AlterPass");
                    this.Close();
                }
            }
            catch (Exception err)
            {
                MessageBoxEx.Show(err.InnerException.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.textBoxX1.Focus();
            }
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

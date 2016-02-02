using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Winform.IView;
using EfwControls.CustomControl;
using EFWCoreLib.CoreFrame.Init;

namespace WinMainUIFrame.Winform.ViewForm
{
    public partial class FrmLogin : BaseFormEx, IfrmLogin
    {
        
        //public static string currentPath = AppDomain.CurrentDomain.BaseDirectory;

        public FrmLogin()
        {
            InitializeComponent();

            
            this.frmForm1.AddItem(txtUser, null, "请输入用户名！");
            this.frmForm1.AddItem(txtPassWord, null, "请输入密码！");
        }

        public void btLogin_Click(object sender, EventArgs e)
        {
            M_SetButtonOkImage("button_chick.png");
            if (this.frmForm1.Validate())
            {
                try
                {
                    InvokeController("UserLogin");
                    isReLogin = true;
                    this.Close();
                }
                catch (Exception err)
                {
                    MessageBoxEx.Show(err.InnerException.Message, "登录提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            M_SetButtonCancelImage("button_chick.png");
            if (isReLogin)
                this.Close();
            else
                InvokeController("Exit");
        }


        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btLogin_Click(null, null);
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            this.txtUser.Focus();
            this.labsystemName.Text = InvokeController("GetSysName").ToString();
            this.pcb_background.ImageLocation = InvokeController("GetLoginBackgroundImage").ToString();
        }



        #region IfrmLogin 成员

        public string usercode
        {
            get
            {
                return this.txtUser.Text;
            }
            set
            {
                this.txtUser.Text = value;
            }
        }

        public string password
        {
            get
            {
                return this.txtPassWord.Text;
            }
            set
            {
                this.txtPassWord.Text = value;
            }
        }

        private bool _isReLogin = false;

        public bool isReLogin
        {
            get
            {
                return _isReLogin;
            }
            set
            {
                _isReLogin = value;
            }
        }

        #endregion

        private void FrmLogin_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                if (isReLogin == false)
                    InvokeController("Exit");
            }
        }


        #region 确定和退出按钮的图片变换
        private void M_SetButtonOkImage(string str_FileName)
        {
            string str_ImageFile = Application.StartupPath.ToString() + "\\images\\main\\" + str_FileName;
            if (System.IO.File.Exists(str_ImageFile))
            {
                this.pb_Ok.Image = Image.FromFile(str_ImageFile);
                lab_OK.Image = Image.FromFile(str_ImageFile);
            }
        }

        private void M_SetButtonCancelImage(string str_FileName)
        {
            string str_ImageFile = Application.StartupPath.ToString() + "\\images\\main\\" + str_FileName;
            if (System.IO.File.Exists(str_ImageFile))
            {
                this.pb_Cancel.Image = Image.FromFile(str_ImageFile);
                lab_Cancel.Image = Image.FromFile(str_ImageFile);
            }
        }
 
        private void pcb_OK_MouseHover(object sender, EventArgs e)
        {
            M_SetButtonOkImage("button_houver.png");
        }

        private void pcb_OK_MouseLeave(object sender, EventArgs e)
        {
            M_SetButtonOkImage("button.png");
        }
  
        private void pcb_Cancel_MouseHover(object sender, EventArgs e)
        {
            M_SetButtonCancelImage("button_houver.png");
        }

        private void pcb_Cancel_MouseLeave(object sender, EventArgs e)
        {
            M_SetButtonCancelImage("button.png");
        }
        #endregion

        private void pb_Ok_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btLogin_Click(null, null);
            }
        }
    }
}

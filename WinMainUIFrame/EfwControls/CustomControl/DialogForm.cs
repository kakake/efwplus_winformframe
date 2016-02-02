using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFWCoreLib.CoreFrame.Business;

namespace EfwControls.CustomControl
{
    public partial class DialogForm : BaseFormBusiness
    {
        public DialogForm()
        {
            InitializeComponent();
        }

        private bool _buttons = true;

        public bool Buttons
        {
            get { return _buttons; }
            set
            {
                _buttons = value;
                if (_buttons == false)
                {
                    panelBottom.Visible = false;
                }
                else
                {
                    panelBottom.Visible = true;
                }
            }
        }

        private string _saveFunName;
        [Description("设置保存按钮执行控制器内的方法名")]
        public string SaveFunName
        {
            get { return _saveFunName; }
            set { _saveFunName = value; }
        }

        public event EventHandler SaveEventHandler;

        public bool IsOk = false;

 
        private void DialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (frmForm1.Validate())
            {
                this.Close();
                if (SaveEventHandler != null)
                    SaveEventHandler(sender, e);
                IsOk = true;
            }
        }

        private void DialogForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
                IsOk = false;
        }
    }
}

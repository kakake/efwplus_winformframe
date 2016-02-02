using EfwControls.CustomControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.IView.RightManager;

namespace WinMainUIFrame.Winform.ViewForm.RightManager
{
    public partial class frmAddModule : DialogForm, IfrmAddmodule
    {
        public frmAddModule()
        {
            InitializeComponent();
        }

        private void frmAddModule_SaveEventHandler(object sender, EventArgs e)
        {
            InvokeController("SaveModule");
        }

        private BaseModule _currModule = new BaseModule();
        public Entity.BaseModule currModule
        {
            get
            {
                _currModule.Name = txtName.Text;
                _currModule.Memo = txtMemo.Text;
                return _currModule;
            }
            set
            {
                _currModule = value;
                txtName.Text = _currModule.Name;
                txtMemo.Text = _currModule.Memo;
            }
        }


    }
}

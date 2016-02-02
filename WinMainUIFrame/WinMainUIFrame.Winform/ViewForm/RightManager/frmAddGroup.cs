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
    public partial class frmAddGroup : DialogForm, IfrmAddGroup
    {
        public frmAddGroup()
        {
            InitializeComponent();
        }

        private void frmAddGroup_SaveEventHandler(object sender, EventArgs e)
        {
            InvokeController("SaveGroup");
        }

        private BaseGroup _currGroup=new BaseGroup();
        public Entity.BaseGroup currGroup
        {
            get
            {
                _currGroup.Name = txtName.Text;
                _currGroup.Memo = txtMemo.Text;
                return _currGroup;
            }
            set
            {
                _currGroup = value;
                txtName.Text = _currGroup.Name;
                txtMemo.Text = _currGroup.Memo;
            }
        }
    }
}

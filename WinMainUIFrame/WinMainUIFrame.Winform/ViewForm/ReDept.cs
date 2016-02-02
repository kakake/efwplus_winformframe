using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinMainUIFrame.Winform.IView;
using EFWCoreLib.WinformFrame.Controller;
using EfwControls.CustomControl;
using WinMainUIFrame.Entity;

namespace WinMainUIFrame.Winform.ViewForm
{
    public partial class ReDept : BaseFormEx, IfrmReSetDept
    {


        public ReDept()
        {
            InitializeComponent();
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            InvokeController("SaveReDept");
            this.Close();
        }

        private void ReDept_Load(object sender, EventArgs e)
        {

        }

        #region IfrmReSetDept 成员

        public string UserName
        {
            set { this.labelUserName.Text = value; }
        }

        public string WorkName
        {
            set { this.labelWorkName.Text = value; }
        }

        public void loadDepts(List<BaseDept> list,int selectDeptId)
        {
            this.comboBoxDepts.DataSource = list;
            this.comboBoxDepts.SelectedValue = selectDeptId;
        }

        public BaseDept getDept()
        {
            return (BaseDept)this.comboBoxDepts.SelectedItem;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFWCoreLib.WinformFrame.Controller;
using EFWCoreLib.CoreFrame.Business;

namespace EFWCoreLib.WinformFrame.Controller
{
    public partial class BaseForm : BaseFormBusiness, IBaseView
    {
        

        public BaseForm()
        {
            InitializeComponent();
        }


        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


        public virtual void doBarCode(string barCode)
        {

        }
    }
}

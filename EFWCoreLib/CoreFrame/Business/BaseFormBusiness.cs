using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EFWCoreLib.CoreFrame.Business
{
    public partial class BaseFormBusiness : Form, IBaseViewBusiness
    {


        public BaseFormBusiness()
        {
            InitializeComponent();
        }


        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        #region IBaseView 成员

        //public event ControllerEventHandler ControllerEvent;

        private ControllerEventHandler _InvokeController;
        public ControllerEventHandler InvokeController
        {
            get
            {
                return _InvokeController;
            }
            set
            {
                _InvokeController = value;
            }
        }

        #endregion


        public virtual void doBarCode(string barCode)
        {

        }

        [Description("打开窗体之前")]
        public event EventHandler OpenWindowBefore;

        [Description("关闭窗体之后")]
        public event EventHandler CloseWindowAfter;

        public void ExecOpenWindowBefore(object sender,EventArgs e)
        {
            if (OpenWindowBefore != null)
                OpenWindowBefore(sender, e);
        }

        public void ExecCloseWindowAfter(object sender, EventArgs e)
        {
            if (CloseWindowAfter != null)
                CloseWindowAfter(sender, e);
        }
    }
}

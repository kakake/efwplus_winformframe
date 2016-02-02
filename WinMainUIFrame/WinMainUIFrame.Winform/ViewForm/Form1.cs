using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Winform.IView;

namespace WinMainUIFrame.Winform.ViewForm
{
    public partial class Form1 : BaseForm, IForm1
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(InvokeController("GetHello").ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InvokeController("Exit");
        }
    }
}

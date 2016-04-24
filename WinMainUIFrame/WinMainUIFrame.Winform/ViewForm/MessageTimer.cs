using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFWCoreLib.WinformFrame.Controller;

namespace WinMainUIFrame.Winform.ViewForm
{
    public partial class MessageTimer : Timer
    {
        private Form _frmMain;

        public Form FrmMain
        {
            get { return _frmMain; }
            set
            {
                _frmMain = value;
                Messages.InvokeController = (_frmMain as IBaseView).InvokeController;
            }
        }

        public MessageTimer(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public MessageTimer()
        {
            InitializeComponent();
            //manager = new MessageManager();
            this.Interval = 5000;
            //this.Enabled = true;
            this.Tick += new EventHandler(MessageTimer_Tick);
        }

        void MessageTimer_Tick(object sender, EventArgs e)
        {
            this.Enabled = false;
            try { GetMessages(); }
            catch { }
            finally
            {
                this.Enabled = true;
            }
        }

        private bool GetMessages()
        {
            List<Messages> showMs = Messages.GetMessages();

            if (showMs.Count > 0)
            {
                TaskbarForm.ShowForm(FrmMain, showMs);
                return true;
            }

            return false;
        }

    }
}

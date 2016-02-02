using System;
using System.Windows.Forms;

namespace EfwControls.WebBrowser
{
    public partial class FrmWebBrowser : Form, IfrmWebBrowserView
    {
        private System.Windows.Forms.WebBrowser webBrowser1;
        //private Skybound.Gecko.GeckoWebBrowser geckoWebBrowser1;
        //private WebKit.WebKitBrowser webKitBrowser1;

        public FrmWebBrowser()
        {
            InitializeComponent();

            webBrowser1 = new System.Windows.Forms.WebBrowser();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(746, 511);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.panel2.Controls.Add(this.webBrowser1);

        }

        
        #region IfrmWebBrowserView 成员
        public delegate void ShowHandler();
        public void NavigateUrl()
        {
            if (string.IsNullOrEmpty(Url) == false)
            {
                if (this.webBrowser1.InvokeRequired)
                {
                    ShowHandler handler = new ShowHandler(NavigateUrl);
                    this.Invoke(handler);
                }
                else
                {
                    this.webBrowser1.Navigate(Url);
                }
            }
        }

       
        public string Url
        {
            get
            {
                return this.txtUrl.Text;
            }
            set
            {
                this.txtUrl.Text = value;
            }
        }

        #endregion


        private void FrmWebBrowser_Load(object sender, EventArgs e)
        {
            NavigateUrl();
        }

        private void FrmWebBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}

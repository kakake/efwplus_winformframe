using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EfwControls.CustomControl
{
    public partial class frmHotkeys : Component
    {
        private Dictionary<Keys, EventHandler> keyslist;

        public Form MainFrm{
            set
            {
                Form f1 = value;
                keyslist = new Dictionary<Keys, EventHandler>();
                f1.KeyPreview = true;
                f1.KeyUp += new KeyEventHandler(MainFrm_KeyUp);
            }
        }

        public frmHotkeys()
        {
            
        }

        public void AddKeys(Keys keyCode, EventHandler hander)
        {
            keyslist.Add(keyCode, hander);
        }

        void MainFrm_KeyUp(object sender, KeyEventArgs e)
        {

            foreach (KeyValuePair<Keys, EventHandler> val in keyslist)
            {
                if (e.KeyCode == val.Key)
                {
                    val.Value(null, null);
                }
            }
        }
        public frmHotkeys(IContainer container)
            : this()
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}

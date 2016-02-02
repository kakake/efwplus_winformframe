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
    public partial class BaseFormEx : BaseFormBusiness
    {

        public BaseFormEx()
        {
            InitializeComponent();
        }

        public virtual void doBarCode(string barCode)
        {

        }
    }
}

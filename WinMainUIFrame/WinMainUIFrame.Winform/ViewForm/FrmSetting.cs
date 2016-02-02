using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinMainUIFrame.Winform.IView;
using System.Management;
using EFWCoreLib.WinformFrame.Controller;
using EfwControls.CustomControl;

namespace WinMainUIFrame.Winform.ViewForm
{
    public partial class FrmSetting : BaseFormEx, IfrmSetting
    {
        public FrmSetting()
        {
            InitializeComponent();
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            InvokeController("SaveSetting");
        }


        #region IfrmSetting 成员

        public List<InputLanguage> languageList
        {
            set
            {
                comboBoxEx1.Items.Clear();
                comboBoxEx2.Items.Clear();
                foreach (InputLanguage val in value)
                {
                    comboBoxEx1.Items.Add(val.LayoutName);
                    comboBoxEx2.Items.Add(val.LayoutName);
                }
            }
        }

        public int inputMethod_CH
        {
            get { return comboBoxEx2.SelectedIndex; }
            set { comboBoxEx2.SelectedIndex = value; }
        }

        public int inputMethod_EN
        {
            get { return comboBoxEx1.SelectedIndex; }
            set { comboBoxEx1.SelectedIndex = value; }
        }

        public void loadPrinter(System.Management.ManagementObjectCollection printers, int first, int second, int three)
        {
            print1.Items.Clear();
            print2.Items.Clear();
            print3.Items.Clear();
            foreach (ManagementObject mo in printers)
            {
                print1.Items.Add(mo["Name"].ToString());
                
                print2.Items.Add(mo["Name"].ToString());
                
                print3.Items.Add(mo["Name"].ToString());
                
            }
            print1.SelectedIndex = first;
            print2.SelectedIndex = second;
            print3.SelectedIndex = three;
        }

        public int printfirst
        {
            get { return print1.SelectedIndex; }
        }

        public int printsecond
        {
            get { return print2.SelectedIndex; }
        }

        public int printthree
        {
            get { return print3.SelectedIndex; }
        }

        public bool runacceptMessage
        {
            get
            {
                return checkBoxX1.Checked;
            }
            set
            {
                this.checkBoxX1.Checked = value;
            }
        }

        public bool displayWay
        {
            get
            {
                return checkBoxX2.Checked;
            }
            set
            {
                this.checkBoxX2.Checked = value;
            }
        }


        public string setbackgroundImage
        {
            get
            {
                return this.pictureBox1.Tag.ToString();
            }
            set
            {
                this.pictureBox1.Tag = value;
                this.pictureBox1.Image = Image.FromFile(value);
            }
        }

        #endregion

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                setbackgroundImage = openFileDialog1.FileName;
            }
        }

        #region IfrmSetting 成员


        public int mainStyle
        {
            get
            {
                return cbmainStyle.SelectedIndex;
            }
            set
            {
                cbmainStyle.SelectedIndex = value;
            }
        }

        #endregion

        private void btnBar_Click(object sender, EventArgs e)
        {
            if (FrmMainRibbon.BarCode != null)
            {
                FrmMainRibbon.BarCode.Stop();
                FrmMainRibbon.BarCode.Start();
            }
        }
    }
}

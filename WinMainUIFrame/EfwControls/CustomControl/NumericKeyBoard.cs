using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace EfwControls.CustomControl
{
    public partial class NumericKeyBoard : UserControl
    {
        public NumericKeyBoard()
        {
            InitializeComponent();

            
        }



        void NumericKeyBoard_Click(object sender, EventArgs e)
        {
            if (ConfirmNum != null && (sender as LabelX).Text == "确定")
                ConfirmNum(sender, e);
            else if (DeleteNum != null && (sender as LabelX).Text == "删除")
                DeleteNum(sender, e);
            else
                if (ClickNum != null)
                    ClickNum(sender, e);
        }

        public void InitClickEvent()
        {
            for (int i = 0; i < panelEx1.Controls.Count; i++)
            {
                panelEx1.Controls[i].Click += new EventHandler(NumericKeyBoard_Click);
                (panelEx1.Controls[i] as LabelX).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
                (panelEx1.Controls[i] as LabelX).MouseHover += new EventHandler(NumericKeyBoard_MouseHover);
                (panelEx1.Controls[i] as LabelX).MouseLeave += new EventHandler(NumericKeyBoard_MouseLeave);
            }
        }

        void NumericKeyBoard_MouseLeave(object sender, EventArgs e)
        {
            (sender as LabelX).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
        }

        void NumericKeyBoard_MouseHover(object sender, EventArgs e)
        {
            (sender as LabelX).BackColor = Color.Yellow;
        }

        public bool GetIsFocused()
        {
            for (int i = 0; i < panelEx1.Controls.Count; i++)
            {
                if (panelEx1.Controls[i].Focused)
                    return true;
            }

            return false;
        }

        [Description("点击数字触发事件")]
        public event EventHandler ClickNum;

        [Description("点击删除触发事件")]
        public event EventHandler DeleteNum;

        [Description("点击确定触发事件")]
        public event EventHandler ConfirmNum;
    }
}

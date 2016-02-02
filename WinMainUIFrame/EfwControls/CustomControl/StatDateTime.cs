using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.Editors.DateTimeAdv;
using DevComponents.DotNetBar;

namespace EfwControls.CustomControl
{
    public partial class StatDateTime : UserControl
    {
        [Description("设置时间格式")]
        public string DateFormat
        {
            get { return this.bdate.CustomFormat; }
            set
            {
                this.bdate.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
                this.bdate.CustomFormat = value;
                this.edate.Format = DevComponents.Editors.eDateTimePickerFormat.Custom;
                this.edate.CustomFormat = value;
            }
        }

        [Description("开始时间")]
        public DateTimeInput Bdate
        {
            get { return this.bdate; }
        }
        [Description("结束时间")]
        public DateTimeInput Edate
        {
            get { return this.edate; }
        }
        [Description("时间控件的宽")]
        public int DateWidth
        {
            get { return this.bdate.Width; }
            set
            {
                this.bdate.Width = value;
                this.edate.Width = value + 15;
            }
        }
        private showStyle _showStyle = showStyle.horizontal;
        [Description("显示方式")]
        public showStyle ShowStyle
        {
            get { return _showStyle; }
            set
            {
                _showStyle = value;
                if (_showStyle == showStyle.horizontal)
                {
                    labelX1.Visible = true;
                    bdate.Dock = DockStyle.Left;
                    labelX1.Dock = DockStyle.Left;
                    edate.Dock = DockStyle.Left;

                    DateWidth = 150;
                    this.Width = 340;
                    this.Height = bdate.Height;
                }
                else
                {
                    labelX1.Visible = false;
                    bdate.Dock = DockStyle.Top;
                    edate.Dock = DockStyle.Top;

                    DateWidth = 150;
                    this.Width = 150;
                    this.Height = 52;
                }
            }
        }


        public StatDateTime()
        {
            InitializeComponent();
            InitControl();
            popup1.AddPopupPanel(bdate, panelEx1, PopupEvent.Custom, this.Width, 100);


            this.Font = new Font("微软雅黑", (float)10.5);
            DateWidth = 150;
            this.Width = 340;
            this.Height = bdate.Height;
        }

        private dateData CalculationDate(DateTime now, string name)
        {
            DateTime begin = now;
            DateTime end = now;
            switch (name)
            {
                case "本周":
                    begin = now.AddDays(0 - (int)now.DayOfWeek);
                    end = now.AddDays(6 - (int)now.DayOfWeek);
                    break;
                case "本月":
                    begin = new DateTime(now.Year, now.Month, 1);
                    end = new DateTime(now.Year, now.Month + 1, 1).AddDays(-1);
                    break;
                case "本季度":
                    if (now.Month >= 1 && now.Month <= 3)
                    {
                        begin = new DateTime(now.Year, 1, 1);
                        end = new DateTime(now.Year, 4, 1).AddDays(-1);
                    }
                    else if (now.Month >= 4 && now.Month <= 6)
                    {
                        begin = new DateTime(now.Year, 4, 1);
                        end = new DateTime(now.Year, 7, 1).AddDays(-1);
                    }
                    else if (now.Month >= 7 && now.Month <= 9)
                    {
                        begin = new DateTime(now.Year, 7, 1);
                        end = new DateTime(now.Year, 10, 1).AddDays(-1);
                    }
                    else if (now.Month >= 10 && now.Month <= 12)
                    {
                        begin = new DateTime(now.Year, 10, 1);
                        end = new DateTime(now.Year + 1, 1, 1).AddDays(-1);
                    }
                    break;
                case "本年度":
                    begin = new DateTime(now.Year, 1, 1);
                    end = new DateTime(now.Year + 1, 1, 1).AddDays(-1);
                    break;
                case "一月":
                    begin = new DateTime(now.Year, 1, 1);
                    end = new DateTime(now.Year, 2, 1).AddDays(-1);
                    break;
                case "二月":
                    begin = new DateTime(now.Year, 2, 1);
                    end = new DateTime(now.Year, 3, 1).AddDays(-1);
                    break;
                case "三月":
                    begin = new DateTime(now.Year, 3, 1);
                    end = new DateTime(now.Year, 4, 1).AddDays(-1);
                    break;
                case "四月":
                    begin = new DateTime(now.Year, 4, 1);
                    end = new DateTime(now.Year, 5, 1).AddDays(-1);
                    break;
                case "五月":
                    begin = new DateTime(now.Year, 5, 1);
                    end = new DateTime(now.Year, 6, 1).AddDays(-1);
                    break;
                case "六月":
                    begin = new DateTime(now.Year, 6, 1);
                    end = new DateTime(now.Year, 7, 1).AddDays(-1);
                    break;
                case "七月":
                    begin = new DateTime(now.Year, 7, 1);
                    end = new DateTime(now.Year, 8, 1).AddDays(-1);
                    break;
                case "八月":
                    begin = new DateTime(now.Year, 8, 1);
                    end = new DateTime(now.Year, 9, 1).AddDays(-1);
                    break;
                case "九月":
                    begin = new DateTime(now.Year, 9, 1);
                    end = new DateTime(now.Year, 10, 1).AddDays(-1);
                    break;
                case "十月":
                    begin = new DateTime(now.Year, 10, 1);
                    end = new DateTime(now.Year, 11, 1).AddDays(-1);
                    break;
                case "十一月":
                    begin = new DateTime(now.Year, 11, 1);
                    end = new DateTime(now.Year, 12, 1).AddDays(-1);
                    break;
                case "十二月":
                    begin = new DateTime(now.Year, 12, 1);
                    end = new DateTime(now.Year + 1, 1, 1).AddDays(-1);
                    break;
                case "一季度":
                    begin = new DateTime(now.Year, 1, 1);
                    end = new DateTime(now.Year, 4, 1).AddDays(-1);
                    break;
                case "二季度":
                    begin = new DateTime(now.Year, 4, 1);
                    end = new DateTime(now.Year, 7, 1).AddDays(-1);
                    break;
                case "三季度":
                    begin = new DateTime(now.Year, 7, 1);
                    end = new DateTime(now.Year, 10, 1).AddDays(-1);
                    break;
                case "四季度":
                    begin = new DateTime(now.Year, 10, 1);
                    end = new DateTime(now.Year + 1, 1, 1).AddDays(-1);
                    break;
            }

            return new dateData(name, begin, end);
        }
        private dateData[,] CalculationYear(DateTime now)
        {
            dateData[,] texts3 = new dateData[2, 5];
            texts3[0, 0] = new dateData(now.Year - 9 + "年", new DateTime(now.Year - 9, 1, 1), new DateTime(now.Year - 9 + 1, 1, 1).AddDays(-1));
            texts3[0, 1] = new dateData(now.Year - 8 + "年", new DateTime(now.Year - 8, 1, 1), new DateTime(now.Year - 8 + 1, 1, 1).AddDays(-1));
            texts3[0, 2] = new dateData(now.Year - 7 + "年", new DateTime(now.Year - 7, 1, 1), new DateTime(now.Year - 7 + 1, 1, 1).AddDays(-1));
            texts3[0, 3] = new dateData(now.Year - 6 + "年", new DateTime(now.Year - 6, 1, 1), new DateTime(now.Year - 6 + 1, 1, 1).AddDays(-1));
            texts3[0, 4] = new dateData(now.Year - 5 + "年", new DateTime(now.Year - 5, 1, 1), new DateTime(now.Year - 5 + 1, 1, 1).AddDays(-1));
            texts3[1, 0] = new dateData(now.Year - 4 + "年", new DateTime(now.Year - 4, 1, 1), new DateTime(now.Year - 4 + 1, 1, 1).AddDays(-1));
            texts3[1, 1] = new dateData(now.Year - 3 + "年", new DateTime(now.Year - 3, 1, 1), new DateTime(now.Year - 3 + 1, 1, 1).AddDays(-1));
            texts3[1, 2] = new dateData(now.Year - 2 + "年", new DateTime(now.Year - 2, 1, 1), new DateTime(now.Year - 2 + 1, 1, 1).AddDays(-1));
            texts3[1, 3] = new dateData(now.Year - 1 + "年", new DateTime(now.Year - 1, 1, 1), new DateTime(now.Year - 1 + 1, 1, 1).AddDays(-1));
            texts3[1, 4] = new dateData(now.Year + "年", new DateTime(now.Year, 1, 1), new DateTime(now.Year + 1, 1, 1).AddDays(-1));
            return texts3;
        }

        private void InitControl()
        {
            dateData[,] texts = new dateData[2, 4];
            texts[0, 0] = new dateData("今天", DateTime.Now, DateTime.Now);
            texts[0, 1] = new dateData("昨天", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1));
            texts[0, 2] = new dateData("近两天", DateTime.Now.AddDays(-2), DateTime.Now);
            texts[0, 3] = new dateData("近三天", DateTime.Now.AddDays(-3), DateTime.Now);
            texts[1, 0] = CalculationDate(DateTime.Now, "本周");
            texts[1, 1] = CalculationDate(DateTime.Now, "本月");
            texts[1, 2] = CalculationDate(DateTime.Now, "本季度");
            texts[1, 3] = CalculationDate(DateTime.Now, "本年度");

            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    if (texts[i, k].name != "")
                    {
                        Button btn = new Button();
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Text = texts[i, k].name;
                        btn.Tag = texts[i, k];
                        btn.Click += new EventHandler(btn_Click);
                        tableLayoutPanel1.Controls.Add(btn, k, i);
                    }
                }
            }


            dateData[,] texts1 = new dateData[2, 6];
            texts1[0, 0] = CalculationDate(DateTime.Now, "一月");
            texts1[0, 1] = CalculationDate(DateTime.Now, "二月");
            texts1[0, 2] = CalculationDate(DateTime.Now, "三月");
            texts1[0, 3] = CalculationDate(DateTime.Now, "四月");
            texts1[0, 4] = CalculationDate(DateTime.Now, "五月");
            texts1[0, 5] = CalculationDate(DateTime.Now, "六月");
            texts1[1, 0] = CalculationDate(DateTime.Now, "七月");
            texts1[1, 1] = CalculationDate(DateTime.Now, "八月");
            texts1[1, 2] = CalculationDate(DateTime.Now, "九月");
            texts1[1, 3] = CalculationDate(DateTime.Now, "十月");
            texts1[1, 4] = CalculationDate(DateTime.Now, "十一月");
            texts1[1, 5] = CalculationDate(DateTime.Now, "十二月");

            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < 6; k++)
                {
                    if (texts1[i, k].name != "")
                    {
                        Button btn = new Button();
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Text = texts1[i, k].name;
                        btn.Tag = texts1[i, k];
                        btn.Click += new EventHandler(btn_Click);
                        tableLayoutPanel2.Controls.Add(btn, k, i);
                    }
                }
            }

            dateData[,] texts2 = new dateData[2, 2];
            texts2[0, 0] = CalculationDate(DateTime.Now, "一季度");
            texts2[0, 1] = CalculationDate(DateTime.Now, "二季度");
            texts2[1, 0] = CalculationDate(DateTime.Now, "三季度");
            texts2[1, 1] = CalculationDate(DateTime.Now, "四季度");
            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    if (texts2[i, k].name != "")
                    {
                        Button btn = new Button();
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Text = texts2[i, k].name;
                        btn.Tag = texts2[i, k];
                        btn.Click += new EventHandler(btn_Click);
                        tableLayoutPanel3.Controls.Add(btn, k, i);
                    }
                }
            }

            dateData[,] texts3 = CalculationYear(DateTime.Now);
            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (texts3[i, k].name != "")
                    {
                        Button btn = new Button();
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.Text = texts3[i, k].name;
                        btn.Tag = texts3[i, k];
                        btn.Click += new EventHandler(btn_Click);
                        tableLayoutPanel4.Controls.Add(btn, k, i);
                    }
                }
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            this.bdate.Value = ((dateData)((Control)sender).Tag).begindate;
            this.edate.Value = ((dateData)((Control)sender).Tag).enddate;
            popup1.Hide();
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            
        }

        private void edate_ButtonCustomClick(object sender, EventArgs e)
        {
            int _width = this.Width < 340 ? 340 : this.Width;
            popup1.Show(bdate, _width, 100);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            bdate.Height = Height;
            edate.Height = Height;
            base.OnSizeChanged(e);
        }
    }

    public struct dateData
    {
        public string name;
        public DateTime begindate;
        public DateTime enddate;

        public dateData(string _name, DateTime _begin, DateTime _end)
        {
            name = _name;
            begindate = _begin;
            enddate = _end;
        }
    }

    public enum showStyle
    {
        horizontal, vertical
    }
}

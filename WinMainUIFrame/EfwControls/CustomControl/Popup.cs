using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Threading;

namespace EfwControls.CustomControl
{
    /// <summary>
    /// 弹出面板
    /// </summary>
    public partial class Popup : Component
    {
        List<PopupContent> popupList;
        Rectangle rect_close;//画关闭按钮
        Rectangle rect_titletext;//画标题

        public Popup()
        {
            InitializeComponent();
            popupList = new List<PopupContent>();
        }

        public Popup(IContainer container)
        {
            container.Add(this);

            InitializeComponent();

            //this.picture.SendToBack();//将背景图片放到最下面
            //this.panelContainer.BackColor = Color.Transparent;//将Panel设为透明
            //this.panelContainer.Parent = this.picture;//将panel父控件设为背景图片控件
            //this.panelContainer.BringToFront();//将panel放在前面
            popupList = new List<PopupContent>();
            picTitle.Paint += new PaintEventHandler(panelContainer_Paint);
            picTitle.MouseClick += new MouseEventHandler(picTitle_MouseClick);
            picTitle.MouseDown += new MouseEventHandler(picTitle_MouseDown);
            picTitle.MouseMove += new MouseEventHandler(picTitle_MouseMove);
        }

        [Browsable(true), Description("显示时执行此事件")]
        public event doShowEvent doShow;

        [Browsable(true), Description("隐藏时执行此事件")]
        public event doHideEvent doHide;

        public void AddPopupPanel(Control _TargetControl, Control _PopupControl,  PopupEvent _PStyle, int _width, int _height)
        {
            PopupContent popup = new PopupContent();
            popup.TargetControl = _TargetControl;
            popup.PopupControl = _PopupControl;
            popup.PType = PopupType.Panel;
            popup.PStyle = _PStyle;
            popup.WStyle = WindowStyle.Default;
            popup.Width = _width;
            popup.Height = _height;
            popup.IsMCHide = true;
            popup.WindowTitle = "";

            _PopupControl.Tag = this;

            if (popup.PStyle == PopupEvent.Click)
            {
                _TargetControl.Click += new EventHandler(_TargetControl_Click);
            }
            else if (popup.PStyle == PopupEvent.Mouse)
            {
                _TargetControl.MouseEnter += new EventHandler(_TargetControl_MouseEnter);
                _TargetControl.MouseLeave += new EventHandler(_TargetControl_MouseLeave);
            }
            else
            {

            }

            popupList.Add(popup);

            _PopupControl.Hide();
        }
        
        public void AddPopupWindow(Control _TargetControl, Control _PopupControl, PopupEvent _PStyle,WindowStyle _WStyle, int _width, int _height,bool _ismouseclickhide)
        {
            PopupContent popup = new PopupContent();
            popup.TargetControl = _TargetControl;
            popup.PopupControl = _PopupControl;
            popup.PType = PopupType.Window;
            popup.PStyle = _PStyle;
            popup.WStyle = _WStyle;
            popup.Width = _width;
            popup.Height = _height;
            popup.IsMCHide = _ismouseclickhide;
            popup.WindowTitle = "";

            _PopupControl.Tag = this;

            if (popup.PStyle == PopupEvent.Click)
            {
                _TargetControl.Click += new EventHandler(_TargetControl_Click);
            }
            else if (popup.PStyle == PopupEvent.Mouse)
            {
                _TargetControl.MouseEnter += new EventHandler(_TargetControl_MouseEnter);
                _TargetControl.MouseLeave += new EventHandler(_TargetControl_MouseLeave);
            }
            else
            {

            }

            popupList.Add(popup);

            _PopupControl.Hide();
        }
        public void Show(Control _TargetControl,int width,int height)
        {
            Hide();

            if (popupList.FindIndex(x => x.TargetControl.Equals(_TargetControl)) == -1)
            {
                throw new Exception("当前控件并没有设置弹出面板！");
            }

            PopupContent popup = popupList.Find(x => x.TargetControl.Equals(_TargetControl));
            panelContainer.Tag = popup;
            if (width != -1)
                popup.Width = width;
            if (height != -1)
                popup.Height = height;

            panelContainer.Width = popup.Width;
            panelContainer.Height = popup.Height;

            if (popup.PopupControl is Form)
            {
                (popup.PopupControl as Form).TopLevel = false;
                (popup.PopupControl as Form).FormBorderStyle = FormBorderStyle.None;
                //(popup.PopupControl as Form).VisibleChanged+=new EventHandler(Popup_VisibleChanged);
            }

            if (popup.PType == PopupType.Panel)
            {

                panelContainer.Padding = new Padding(0);
                panelContainer.Controls.Add(popup.PopupControl);
                popup.PopupControl.Dock = DockStyle.Fill;
                popup.PopupControl.Show();
                SetPopupLocation(_TargetControl);

                //panelContainer.Show();

            }
            else if (popup.PType == PopupType.Window)
            {

                panelContainer.Padding = new Padding(2);
                panelContainer.Controls.Add(popup.PopupControl);
                panelContainer.Controls.Add(picTitle);
                picTitle.Height = 20;
                picTitle.Dock = DockStyle.Top;

                popup.PopupControl.Dock = DockStyle.Fill;
                rect_close = new Rectangle(panelContainer.Width - 22, 0, 20, 20);
                popup.PopupControl.Show();

                SetPopupCenter(_TargetControl);
            }

            if (popup.WStyle == WindowStyle.Default)
            {
                panelContainer.Show();
                if (popup.PopupControl is Form)
                {
                    (popup.PopupControl as Form).VisibleChanged += new EventHandler(Popup_VisibleChanged);
                }
            }
            else if (popup.WStyle == WindowStyle.Shadow)
            {
                SetPopupCenter(_TargetControl);
                panelContainer.Width = 0;
                panelContainer.Height = 0;
                Thread ts = new Thread(new ThreadStart(ShadowPanel));
                ts.IsBackground = true;
                ts.Priority = ThreadPriority.Normal;
                ts.Start();
            }
            else if (popup.WStyle == WindowStyle.Task)
            {
                SetPopupTask(_TargetControl);
                Thread ts = new Thread(new ThreadStart(TaskPanel));
                ts.IsBackground = true;
                ts.Priority = ThreadPriority.Normal;
                ts.Start();
            }

            if (doShow != null)
                doShow();

            BindParentEvent(_TargetControl, popup);

        }
        public void Show(Control _TargetControl)
        {
            Show(_TargetControl, -1, -1);
        }
        public void ShowWindow(Control _TargetForm, Control _PopupForm, int width, int height, WindowStyle _WStyle)
        {
            ShowWindow(_TargetForm, _PopupForm, width, height, _WStyle, "");
        }
        public void ShowWindow(Control _TargetForm, Control _PopupForm, int width, int height, WindowStyle _WStyle,string title)
        {
            Hide();
            
            PopupContent popup = new PopupContent();
            popup.TargetControl = _TargetForm;
            popup.PopupControl = _PopupForm;
            popup.PType = PopupType.Window;
            popup.PStyle = PopupEvent.Custom;
            popup.WStyle = _WStyle;
            popup.Width = _PopupForm.Width;
            popup.Height = _PopupForm.Height;
            popup.IsMCHide = true;//点击隐藏
            popup.WindowTitle = title;

            panelContainer.Tag = popup;
            if (width != -1)
                popup.Width = width;
            if (height != -1)
                popup.Height = height;

            panelContainer.Width = popup.Width;
            panelContainer.Height = popup.Height;

            if (popup.PopupControl is Form)
            {
                (popup.PopupControl as Form).TopLevel = false;
                (popup.PopupControl as Form).FormBorderStyle = FormBorderStyle.None;
                //(popup.PopupControl as Form).VisibleChanged += new EventHandler(Popup_VisibleChanged);
            }

             if (popup.PType == PopupType.Window)
            {

                panelContainer.Padding = new Padding(2);
                panelContainer.Controls.Add(popup.PopupControl);
                panelContainer.Controls.Add(picTitle);
                picTitle.Height = 20;
                picTitle.Dock = DockStyle.Top;

                popup.PopupControl.Dock = DockStyle.Fill;
                rect_close = new Rectangle(panelContainer.Width - 22, 0, 20, 20);
                rect_titletext = new Rectangle(0, 0, panelContainer.Width - 50, 18);
                popup.PopupControl.Show();       
            }

            if (popup.WStyle == WindowStyle.Default)
            {
                SetPopupCenter(_TargetForm);
                panelContainer.Show();

                if (popup.PopupControl is Form)
                {
                    (popup.PopupControl as Form).VisibleChanged += new EventHandler(Popup_VisibleChanged);
                }
            }
            else if (popup.WStyle == WindowStyle.Shadow)
            {
                SetPopupCenter(_TargetForm);
                panelContainer.Width = 0;
                panelContainer.Height = 0;
                Thread ts = new Thread(new ThreadStart(ShadowPanel));
                ts.IsBackground = true;
                ts.Priority = ThreadPriority.Normal;
                ts.Start();
            }
            else if (popup.WStyle == WindowStyle.Task)
            {
                SetPopupTask(_TargetForm);
                Thread ts = new Thread(new ThreadStart(TaskPanel));
                ts.IsBackground = true;
                ts.Priority = ThreadPriority.Normal;
                ts.Start();
            }

          

            if (doShow != null)
                doShow();

            BindParentEvent(_TargetForm, popup);
        }

        public void Hide()
        {
            panelContainer.Hide();
            if (panelContainer.Tag !=null && (panelContainer.Tag as PopupContent).PopupControl is Form)
            {
                ((panelContainer.Tag as PopupContent).PopupControl as Form).VisibleChanged -= new EventHandler(Popup_VisibleChanged);
            }
            panelContainer.Controls.Clear();
            if (doHide != null)
            {
                doHide();
            }
        }

        void Popup_VisibleChanged(object sender, EventArgs e)
        {
            if ((sender as Control).Visible == false)
                Hide();
        }
        void _TargetControl_MouseLeave(object sender, EventArgs e)
        {
            Hide();
        }
        void _TargetControl_MouseEnter(object sender, EventArgs e)
        {
            Show((Control)sender);
        }
        void _TargetControl_Click(object sender, EventArgs e)
        {
            Show((Control)sender);
        }
        private void EnumControls(Control ctrl, bool _isadd)
        {
            foreach (Control c in ctrl.Controls)
            {
                if (c.Visible == false) continue;
                if (popupList.FindIndex(x => x.TargetControl.Equals(c)) == -1 && c.Equals(panelContainer) == false)
                {
                    if (_isadd == true)
                        c.MouseDown += new MouseEventHandler(Parent_LostFocus);
                    else
                        c.MouseDown -= new MouseEventHandler(Parent_LostFocus);
                }
                if (c.Equals(panelContainer) == false)
                    EnumControls(c, _isadd);
            }
        }
        private void CleanParentEvent(Control _TargetControl, PopupContent _popup)
        {
            Control ctrl = _TargetControl.FindForm();
            EnumControls(ctrl, false);
            ctrl.MouseDown -= new MouseEventHandler(Parent_LostFocus);

            for (int i = 0; i < popupList.Count; i++)
            {
                popupList[i].TargetControl.MouseDown -= new MouseEventHandler(Parent_LostFocus);
            }
        }
        private void BindParentEvent(Control _TargetControl, PopupContent _popup)
        {
            CleanParentEvent(_TargetControl, _popup);
            if (_popup.IsMCHide == true)
            {
                Control ctrl = _TargetControl.FindForm();
                EnumControls(ctrl, true);
                ctrl.MouseDown += new MouseEventHandler(Parent_LostFocus);

                for (int i = 0; i < popupList.Count; i++)
                {
                    popupList[i].TargetControl.MouseDown -= new MouseEventHandler(Parent_LostFocus);
                    popupList[i].TargetControl.MouseDown += new MouseEventHandler(Parent_LostFocus);
                }
                _TargetControl.MouseDown -= new MouseEventHandler(Parent_LostFocus);
            }
        }
        private void Parent_LostFocus(object sender, EventArgs e)
        {
            Hide();
        }
        //设置弹出位置
        private void SetPopupLocation(Control _TargetControl)
        {
            /*以控件所在的窗体为参照对象定位选项卡位置
             */
            int x = _TargetControl.Left;
            int y = _TargetControl.Top + _TargetControl.Height;

            System.Windows.Forms.Control ctrl = _TargetControl.Parent;
            if (_TargetControl.Parent == null)
                return;

            _TargetControl.FindForm().AutoValidate = AutoValidate.Disable;
            if (!_TargetControl.FindForm().Controls.Contains(this.panelContainer))
                _TargetControl.FindForm().Controls.Add(this.panelContainer);

            Point location = new Point();
            location = _TargetControl.Parent.PointToScreen(_TargetControl.Location);
            location = _TargetControl.FindForm().PointToClient(location);
            if (location.Y + panelContainer.Height < _TargetControl.FindForm().Height-40)
            {
                location.Y = location.Y + _TargetControl.Height;
            }
            else
            {
                location.Y = location.Y - panelContainer.Height;
            }
            panelContainer.Top = location.Y;
            //Rectangle scrRect = Screen.GetBounds(_TargetControl);
            int pwidth = _TargetControl.FindForm().Width;
            if (location.X + panelContainer.Width < pwidth)
            {
                panelContainer.Left = location.X;
            }
            else
            {
                panelContainer.Left = location.X + (pwidth - (location.X + panelContainer.Width));
            }
            panelContainer.BringToFront();
        }
        //设置弹出位置
        private void SetPopupCenter(Control _TargetControl)
        {

            Form main = _TargetControl.FindForm();
            if (main == null) return;

            main.AutoValidate = AutoValidate.Disable;
            if (!main.Controls.Contains(this.panelContainer))
                main.Controls.Add(this.panelContainer);

            Point location = new Point();
            location.X = (int)((main.Width - panelContainer.Width) / 2);
            location.Y = (int)((main.Height - panelContainer.Height) / 2);

            panelContainer.Left = location.X < 0 ? 0 : location.X;
            panelContainer.Top = location.Y < 0 ? 0 : location.Y;

            panelContainer.BringToFront();
        }
        private void SetPopupTask(Control _TargetControl)
        {
            Form main = _TargetControl.FindForm();
            if (main == null) return;

            main.AutoValidate = AutoValidate.Disable;
            if (!main.Controls.Contains(this.panelContainer))
                main.Controls.Add(this.panelContainer);

            panelContainer.SetBounds(main.Width - panelContainer.Width, main.Height, panelContainer.Width, 0);
            panelContainer.BringToFront();
        }
        private delegate void SetTextCallback(Panel p1, int i,int count);
        private void ShadowPanel()
        {
            int count = 20;
            for (int i = 0; i <= count+20; i++)
            {
                Shadow(this.panelContainer, i,count);
            }

            if (panelContainer.Tag != null && (panelContainer.Tag as PopupContent).PopupControl is Form)
            {
                ((panelContainer.Tag as PopupContent).PopupControl as Form).VisibleChanged += new EventHandler(Popup_VisibleChanged);
            }
        }
        private void Shadow(Panel p1, int i,int count)
        {
            try
            {
                if (p1.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Shadow);
                    p1.Invoke(d, new Object[] { p1, i, count });
                }
                else
                {

                    int w = ((PopupContent)panelContainer.Tag).Width;
                    int h = ((PopupContent)panelContainer.Tag).Height;

                    if (p1.Width >= w)
                        p1.Width = w;
                    else
                        p1.Width = i * (int)Convert.ToDouble(w / count);

                    if (p1.Height >= h)
                        p1.Height = h;
                    else
                        p1.Height = i * (int)Convert.ToDouble(h / count);

                    if (!p1.Visible)
                    {
                        p1.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ShowPanel函数执行错误");
            }
        }
        private void TaskPanel()
        {
            int count = 20;
            for (int i = 0; i <= count+1; i++)
            {
                Task(this.panelContainer, i, count);
            }

            if (panelContainer.Tag != null && (panelContainer.Tag as PopupContent).PopupControl is Form)
            {
                ((panelContainer.Tag as PopupContent).PopupControl as Form).VisibleChanged += new EventHandler(Popup_VisibleChanged);
            }
        }
        private void Task(Panel p1, int i,int count)
        {
            try
            {
                if (p1.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Task);
                    p1.Invoke(d, new Object[] { p1, i,count });
                }
                else
                {
                    int w = ((PopupContent)panelContainer.Tag).Width;
                    int h = ((PopupContent)panelContainer.Tag).Height;
                    int left = ((PopupContent)panelContainer.Tag).TargetControl.FindForm().Width - w - 20;
                    int top = ((PopupContent)panelContainer.Tag).TargetControl.FindForm().Height - 40;
                    if (p1.Height >= h)
                    {
                        p1.SetBounds(left, top - h,w, h);
                    }
                    else
                    {
                        p1.SetBounds(left, top - i * (int)Convert.ToDouble(h / count), p1.Width, i * (int)Convert.ToDouble(h / count));
                    }

                    if (!p1.Visible)
                    {
                        p1.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Task函数执行错误");
            }
        }
        private bool MouseInRect(int X, int Y, Rectangle rect)
        {
            return X > rect.X && X < (rect.X + rect.Width) &&
                   Y > rect.Y && Y < (rect.Y + rect.Height);
        }
        private void DrawCloseButton(Graphics g)
        {
            //Graphics g = panelContainer.CreateGraphics();
            Rectangle rect = rect_close;
            Image img = EfwControls.Properties.Resources.close;
            g.DrawImage(img, new Rectangle(rect.Left + (rect.Width - img.Width) / 2,
                            rect.Top + (rect.Height - img.Height) / 2, img.Width, img.Height),
                        new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);
        }
        private void DrawTitleText(Graphics g, string text)
        {
            Rectangle rect = rect_titletext;
            Font font = new Font("微软雅黑", 10.0f, FontStyle.Regular);
            Brush fontBrush = Brushes.Black;
            SizeF fontsize = g.MeasureString(text, font);

            g.DrawString(text, font, fontBrush,
                rect.Left,
                rect.Top);
        }

        void panelContainer_Paint(object sender, PaintEventArgs e)
        {
            if (((PopupContent)panelContainer.Tag).PType == PopupType.Window)
            {
                DrawCloseButton(e.Graphics);
                DrawTitleText(e.Graphics, ((PopupContent)panelContainer.Tag).WindowTitle);
            }
        }
        void picTitle_MouseClick(object sender, MouseEventArgs e)
        {
            if (MouseInRect(e.X, e.Y, rect_close))
            {
                Hide();
            }
        }
        void picTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int px = Cursor.Position.X - pt.X;
                int py = Cursor.Position.Y - pt.Y;
                panelContainer.Location = new Point(panelContainer.Location.X + px, panelContainer.Location.Y + py);

                pt = Cursor.Position;
            }
        }
        Point pt;
        void picTitle_MouseDown(object sender, MouseEventArgs e)
        {
            pt = Cursor.Position;
        }
    }

    public delegate void doShowEvent();
    public delegate void doHideEvent();

    public class PopupContent
    {
        public Control TargetControl { get; set; }
        public Control PopupControl { get; set; }
        public PopupType PType { get; set; }
        public PopupEvent PStyle { get; set; }
        public WindowStyle WStyle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsMCHide { get; set; }
        public string WindowTitle { get; set; }
    }

    public enum PopupType
    {
        Panel, Window
    }

    public enum PopupEvent
    {
        Click, Mouse,Custom
    }

    public enum WindowStyle
    {
        Shadow,Task,Default
    }
}

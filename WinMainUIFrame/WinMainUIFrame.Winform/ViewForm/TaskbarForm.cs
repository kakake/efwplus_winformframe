using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;

namespace WinMainUIFrame.Winform.ViewForm
{
    internal partial class TaskbarForm : Form
    {
        private static List<Messages> messageList = new List<Messages>(); //消息列表
        private int currentMessageIndex = -1;  //当前消息索引
        private static Form frmMain;
        public static TaskbarForm instance;



        enum CurrentState
        {
            上升 = 500,
            静止 = 1000,
            下降 = 501
        };
       
        private static readonly object syncRoot = new object();

        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int HT_CAPTION = 2;
        private const int SW_SHOWNOACTIVATE = 4;
        CurrentState currentState;
        Rectangle TitleRectangle;
        Rectangle TitlebarRectangle;
        Rectangle ContentRectangle;
        Rectangle CloseBtnRectangle;
        Rectangle LastBtnRectangle;
        Rectangle NextBtnRectangle;
        Rectangle PromptBtnRectangle;
        Rectangle ClearBtnRectangle;
        Bitmap BackgroundBitmap;   //背景图片
        private int nIncrementShow = 0;
        private int keepTime = 0;
        

        //发送消息
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        //释放鼠标捕捉
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        //显示窗体
        [DllImportAttribute("user32.dll")] 
        private static extern Boolean ShowWindow(IntPtr hWnd, Int32 nCmdShow);

        public TaskbarForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获得唯一的实例
        /// </summary>
        /// <returns></returns>
        public static TaskbarForm GetInstance()
        {
            if (instance == null || instance.IsDisposed)
            {
                lock (syncRoot)
                {
                    if (instance == null || instance.IsDisposed)
                    {
                        instance = new TaskbarForm();
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// 显示消息提示窗体
        /// </summary>
        public static void ShowForm(Form _frmMain)
        {
            //产生唯一的实例
            frmMain = _frmMain;
            GetInstance().Show();
        }
        /// <summary>
        /// 显示消息提示窗体
        /// </summary>
        /// <param name="message"></param>
        public static void ShowForm(Form _frmMain, Messages message)
        {
            //产生唯一的实例
            frmMain = _frmMain;
            GetInstance().Show(message);
        }
        /// <summary>
        /// 显示消息提示窗体
        /// </summary>
        /// <param name="message"></param>
        public static void ShowForm(Form _frmMain, List<Messages> messages)
        {
            //产生唯一的实例
            frmMain = _frmMain;
            GetInstance().Show(messages);
        }

        public void ClearMessages()
        {
            messageList = new List<Messages>();
            this.currentMessageIndex = -1;
            RefreshForm(this.CreateGraphics());
        }

        /// <summary>
        /// 显示消息提示窗体
        /// </summary>
        public void Show()
        {
            this.SetBackgroundBitmap(this.BackgroundImage, this.BackColor);
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Normal;
            currentState = CurrentState.上升;
            keepTime = (int)currentState;
            timer.Enabled = true;

            nIncrementShow = this.BackgroundImage.Height / (keepTime / 10);
            SetBounds(Screen.GetBounds(this).Width - this.BackgroundImage.Width, Screen.GetBounds(this).Height, this.BackgroundImage.Width, 0);

            TitleRectangle = new Rectangle(5, 0, 290, 30);
            TitlebarRectangle = new Rectangle(5, 8, 290, 30);
            ContentRectangle = new Rectangle(5, 30, 290, 100);
            CloseBtnRectangle = new Rectangle(260, 0, 30, 30);

            LastBtnRectangle = new Rectangle(20, 145, 45, 12);
            NextBtnRectangle = new Rectangle(70, 145, 45, 12);
            PromptBtnRectangle = new Rectangle(140, 145, 50, 12);
            ClearBtnRectangle = new Rectangle(200, 145, 80, 12);

            ShowWindow(this.Handle, 4); //#define SW_SHOWNOACTIVATE 4
        }
        /// <summary>
        /// 显示消息提示窗体
        /// </summary>
        /// <param name="message"></param>
        public void Show(Messages message)
        {
            if (messageList.FindIndex(x => x.MessageId == message.MessageId) == -1)
            {
                messageList.Add(message);
                this.currentMessageIndex = messageList.Count - 1;

                this.Show();

                try
                {
                    System.Media.SoundPlayer player = new SoundPlayer("msg.wav");
                    player.Play();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 显示消息提示窗体
        /// </summary>
        /// <param name="messagel"></param>
        public void Show(List<Messages> messages)
        {
            bool isadd = false;
            foreach (Messages message in messages)
            {
                if (messageList.FindIndex(x => x.MessageId == message.MessageId) == -1)
                {
                    messageList.Add(message);
                    isadd = true;
                }
            }
            if (isadd)
            {
                this.currentMessageIndex = messageList.Count - 1;

                this.Show();

                try
                {
                    System.Media.SoundPlayer player = new SoundPlayer("msg.wav");
                    player.Play();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        /// <param name="g"></param>
        private void RefreshForm(Graphics g)
        {
            Graphics grfx = g;
            grfx.PageUnit = GraphicsUnit.Pixel;
            Graphics offScreenGraphics;
            Bitmap offscreenBitmap;
            offscreenBitmap = new Bitmap(BackgroundBitmap.Width, BackgroundBitmap.Height);
            offScreenGraphics = Graphics.FromImage(offscreenBitmap);
            if (BackgroundBitmap != null)
            {
                offScreenGraphics.DrawImage(BackgroundBitmap, 0, 0, BackgroundBitmap.Width, BackgroundBitmap.Height);
            }
            DrawText(offScreenGraphics);
            grfx.DrawImage(offscreenBitmap, 0, 0);
        }
        /// <summary>
        /// 设置背景图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="transparencyColor"></param>
        public void SetBackgroundBitmap(Image image, Color transparencyColor)
        {
            BackgroundBitmap = new Bitmap(image);
            Width = BackgroundBitmap.Width;
            Height = BackgroundBitmap.Height;
            //Region = BitmapToRegion(BackgroundBitmap, transparencyColor);
        }
        /// <summary>
        /// 没有使用过的方法
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="transparencyColor"></param>
        /// <returns></returns>
        public Region BitmapToRegion(Bitmap bitmap, Color transparencyColor)
        {
            if (bitmap == null)
                throw new ArgumentNullException("Bitmap", "Bitmap cannot be null!");

            int height = bitmap.Height;
            int width = bitmap.Width;
            GraphicsPath path = new GraphicsPath();
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                {
                    if (bitmap.GetPixel(i, j) == transparencyColor)
                        continue;
                    int x0 = i;
                    while ((i < width) && (bitmap.GetPixel(i, j) != transparencyColor))
                        i++;
                    path.AddRectangle(new Rectangle(x0, j, i - x0, 1));
                }
            Region region = new Region(path);
            path.Dispose();
            return region;
        }
        /// <summary>
        /// 绘制窗体上的文本
        /// </summary>
        /// <param name="graphics"></param>
        private void DrawText(Graphics graphics)
        {
            //没有任何消息时，显示空窗体
            if (this.currentMessageIndex < 0)
            {
                graphics.DrawString("您当前没有任何消息", new System.Drawing.Font("宋体", 10.5F), new SolidBrush(Color.Blue), this.TitlebarRectangle);
                graphics.DrawString("", new System.Drawing.Font("宋体", 10.5F), new SolidBrush(Color.Blue), this.ContentRectangle);
            }
            else
            {
                graphics.DrawString(" " + messageList[this.currentMessageIndex].TitleText, new System.Drawing.Font("宋体", 10.5F), new SolidBrush(Color.Blue), this.TitlebarRectangle);
                graphics.DrawString("\n    " + messageList[this.currentMessageIndex].ContentText, new System.Drawing.Font("宋体", 10.5F), new SolidBrush(Color.Blue), this.ContentRectangle);
            }
            //显示当前消息条数
            graphics.DrawString("第"+(this.currentMessageIndex+1).ToString()+"/"+messageList.Count+"条", new System.Drawing.Font("宋体", 9F), new SolidBrush(Color.Blue), this.PromptBtnRectangle);
            //显示清除所有消息按钮
            graphics.DrawString("清除所有消息", new System.Drawing.Font("宋体", 9F), new SolidBrush(Color.Blue), this.ClearBtnRectangle);
            //显示'上一条'和'下一条'按钮
            if (this.currentMessageIndex > 0)
            {
                graphics.DrawString("上一条", new System.Drawing.Font("宋体", 9F), new SolidBrush(Color.Blue), this.LastBtnRectangle);
            }
            else
            {
                graphics.DrawString("上一条", new System.Drawing.Font("宋体", 9F), new SolidBrush(Color.Gray), this.LastBtnRectangle);
            }

            if (this.currentMessageIndex <messageList.Count-1)
            {
                graphics.DrawString("下一条", new System.Drawing.Font("宋体", 9F), new SolidBrush(Color.Blue), this.NextBtnRectangle);
            }
            else
            {
                graphics.DrawString("下一条", new System.Drawing.Font("宋体", 9F), new SolidBrush(Color.Gray), this.NextBtnRectangle);
            }
        }
        /// <summary>
        /// 重新绘制窗体的背景
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            RefreshForm(e.Graphics);
        }
        /// <summary>
        /// 窗体的鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskbarForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (TitlebarRectangle.Contains(e.Location)) //单击标题栏时拖动
                {
                    ReleaseCapture(); //释放鼠标捕捉
                    SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); //发送左键点击的消息至该窗体(标题栏)
                }
                if (CloseBtnRectangle.Contains(e.Location)) //单击Close按钮关闭 
                {
                    this.Hide();
                }
                if (ContentRectangle.Contains(e.Location)) //单击内容区域
                {
                    if (this.currentMessageIndex > -1)
                    {
                        frmMain.Show();
                        //frmMain.ShowForm(messageList[currentMessageIndex].ShowMenu);
                        this.Hide();
                    }
                    //System.Diagnostics.Process.Start("http://zhidao.baidu.com/");
                }
                if (LastBtnRectangle.Contains(e.Location)) //单击上一条按钮显示上一条消息内容 
                {
                    if (this.currentMessageIndex > 0)
                    {
                        this.currentMessageIndex--;
                        RefreshForm(this.CreateGraphics());
                    }
                }
                if (NextBtnRectangle.Contains(e.Location)) //单击下一条按钮显示下一条消息内容
                {
                    if (this.currentMessageIndex <messageList.Count-1)
                    {
                        this.currentMessageIndex++;
                        RefreshForm(this.CreateGraphics());
                    }
                }
                if (ClearBtnRectangle.Contains(e.Location)) //单击清除所以消息按钮清除所有消息内容
                {
                    int[] msid = new int[messageList.Count];
                    for(int i=0;i<messageList.Count;i++)
                    {
                        msid[i] = messageList[i].MessageId;
                    }
                    Messages.setMessageRead(msid);

                    messageList = new List<Messages>();
                    this.currentMessageIndex = -1;
                    RefreshForm(this.CreateGraphics());
                }
            }
        }
        /// <summary>
        /// 窗体的鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskbarForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (ContentRectangle.Contains(e.Location) || CloseBtnRectangle.Contains(e.Location) || LastBtnRectangle.Contains(e.Location) || NextBtnRectangle.Contains(e.Location) || ClearBtnRectangle.Contains(e.Location))
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 计时器事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            switch (currentState)
            {
                case CurrentState.上升:
                    int barHight = Screen.GetBounds(this).Height - Screen.GetWorkingArea(this).Height;
                    if (this.Top <= Screen.GetWorkingArea(this).Height-this.BackgroundImage.Height)//
                    {
                        currentState = CurrentState.静止;
                        keepTime = (int)currentState;
                    }
                    else
                    {
                        if (this.Height <= this.BackgroundImage.Height)
                        {
                            SetBounds(Left, Top - nIncrementShow, Width, Height + nIncrementShow);
                        }
                        else
                        {
                            SetBounds(Left, Top - nIncrementShow, Width,Height);
                        }
                    }
                    break;
                //case CurrentState.静止:
                //    if (keepTime <= 0)
                //    {
                //        currentState = CurrentState.下降;
                //        keepTime = (int)currentState;
                //        nIncrementShow = this.BackgroundImage.Height / (keepTime / 10);
                //    }
                //    else
                //    {
                //        keepTime -= timer.Interval;
                //    }
                //    break;
                //case CurrentState.下降:
                //    if (this.Height <= 0)
                //    {
                //        timer.Enabled = false;
                //        this.Close();
                //    }
                //    else
                //    {
                //        SetBounds(Left, Top + nIncrementShow, Width, Height - nIncrementShow);
                //    }
                //    break;
                default:
                    timer.Enabled = false;
                    break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
//using EfwControls.Common;
using EfwControls.WebBrowser;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WinformFrame.Controller;
//using ThoughtWorks.QRCode.Codec;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.Common;
using WinMainUIFrame.Winform.IView;

namespace WinMainUIFrame.Winform.ViewForm
{
    public partial class FrmMainRibbon : DevComponents.DotNetBar.Office2007RibbonForm, IfrmMain
    {
        private object btnImage = null;
        public static BarCodeHook BarCode;

        public FrmMainRibbon()
        {
            InitializeComponent();
            //InitBarCode();
            ribbonTabItem1.Visible = false;
        }

        #region 扫描条码
        //方式一
        public void InitBarCode()
        {
            BarCode = new BarCodeHook();
            BarCode.BarCodeEvent += new BarCodeHook.BarCodeDelegate(BarCode_BarCodeEvent);
        }
        private delegate void ShowInfoDelegate(BarCodeHook.BarCodes barCode);
        private void ShowInfo(BarCodeHook.BarCodes barCode)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ShowInfoDelegate(ShowInfo), new object[] { barCode });
            }
            else
            {
                //txtBarCode.Text = barCode.IsValid ? barCode.BarCode : "";
                if (this.barMainContainer.SelectedDockContainerItem.Tag is BaseForm && barCode.IsValid)
                {
                    (this.barMainContainer.SelectedDockContainerItem.Tag as BaseForm).doBarCode(barCode.BarCode);
                }
            }
        }
        void BarCode_BarCodeEvent(BarCodeHook.BarCodes barCode)
        {
            this.ShowInfo(barCode);
        }
        //方式二
        protected override void DefWndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case CodeBarInput.WM_IDCARD_INFO:
                    {
                        byte[] cInput = new byte[101];
                        System.Runtime.InteropServices.Marshal.Copy(m.WParam, cInput, 0, 100);
                        String sInput = System.Text.Encoding.Default.GetString(cInput);
                        if (sInput.Length > 0)
                        {
                        }
                    }
                    break;
                case CodeBarInput.WM_RFID_INFO:
                    {
                        byte[] cInput = new byte[101];
                        System.Runtime.InteropServices.Marshal.Copy(m.WParam, cInput, 0, 100);
                        String sInput = System.Text.Encoding.Default.GetString(cInput);
                        if (sInput.Length > 0)
                        {
                        }
                    }
                    break;
                case CodeBarInput.WM_SCANNER_INPUT:
                    {
                        byte[] cInput = new byte[101];
                        System.Runtime.InteropServices.Marshal.Copy(m.WParam, cInput, 0, 100);
                        String sInput = System.Text.Encoding.Default.GetString(cInput);
                        string barcode = Convert.ToString(sInput).Trim('\0');

                        if (this.barMainContainer.SelectedDockContainerItem.Tag is EfwControls.CustomControl.BaseFormEx)
                        {
                            (this.barMainContainer.SelectedDockContainerItem.Tag as EfwControls.CustomControl.BaseFormEx).doBarCode(barcode);
                        }
                    }
                    break;

                default:
                    base.DefWndProc(ref m);//调用基类函数处理非自定义消息。
                    break;
            }
        }
        #endregion

        #region IfrmMain 成员

        public string UserName
        {
            set { this.labelItem2.Text = value + "     "; }
        }

        public string DeptName
        {
            set { this.labelItem4.Text = value + "     "; }
        }

        private string _workname;
        public string WorkName
        {
            get { return _workname; }
            set
            {
                _workname = value;
                this.labelItem6.Text = value + "     ";
            }
        }

        public List<BaseModule> modules
        {
            get;
            set;
        }

        public List<BaseMenu> menus
        {
            get;
            set;
        }

        public List<BaseDept> depts
        {
            get;
            set;
        }

        public void showSysMenu()
        {

            int ritemCount = ribbonControl1.Items.Count - 1;
            for (int i = 0; i < ritemCount; i++)
            {
                ribbonControl1.Items.Remove(0);
            }

            for (int i = 0; i < modules.Count; i++)
            {
                #region 循环插入模块
                DevComponents.DotNetBar.RibbonTabItem ribbonTabItemModule = new RibbonTabItem();
                DevComponents.DotNetBar.RibbonPanel rPanel = new RibbonPanel();
                ribbonTabItemModule.Panel = rPanel;
                //this.ribbonControl1.SuspendLayout();
                rPanel.SuspendLayout();
                rPanel.Dock = DockStyle.Fill;
                rPanel.Name = "panel" + modules[i].ModuleId.ToString();
                rPanel.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                rPanel.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                rPanel.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
                rPanel.Location = new Point(0, 0x3a);
                rPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
                rPanel.Padding = new System.Windows.Forms.Padding(3, 0, 3, 4);
                rPanel.Size = new Size(0x47a, 0x73);

                //ribbonTabItemModule.Checked = true;
                //ribbonTabItemModule.ItemAlignment = eItemAlignment.Near;
                ribbonTabItemModule.Name = "module" + modules[i].ModuleId.ToString();
                ribbonTabItemModule.Text = modules[i].Name;
                ribbonTabItemModule.Tag = modules[i];

                DevComponents.DotNetBar.RibbonBar rMenuClass = null;

                #region 增加子菜单
                List<BaseMenu> _menus = menus.FindAll(x => (x.ModuleId == modules[i].ModuleId && x.PMenuId == -1)).OrderByDescending(x => x.SortId).ToList();
                if (_menus.Count > 0)
                {
                    //List<RibbonBar> listbar = new List<RibbonBar>();
                    for (int j = 0; j < _menus.Count; j++)
                    {
                        if (string.IsNullOrEmpty(_menus[j].DllName) && string.IsNullOrEmpty(_menus[j].UrlId))
                        {
                            //为二级分类菜单
                            DevComponents.DotNetBar.RibbonBar menuClass = new RibbonBar();
                            menuClass.AutoOverflowEnabled = true;
                            menuClass.Dock = System.Windows.Forms.DockStyle.Left;
                            menuClass.ContainerControlProcessDialogKey = true;
                            menuClass.Text = _menus[j].Name;

                            //三级菜单
                            List<BaseMenu> mainMenu = menus.FindAll(x => x.PMenuId == _menus[j].MenuId).OrderByDescending(x => x.SortId).ToList();
                            foreach (BaseMenu menu in mainMenu)
                            {
                                DevComponents.DotNetBar.ButtonItem btnmenu = new ButtonItem(menu.MenuId.ToString(), menu.Name);
                                // btnmenu.Image = global::EFWBaseLib.Properties.Resources.defaulttool;
                                if (!string.IsNullOrEmpty(menu.Image) && System.IO.File.Exists(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + menu.Image))
                                {
                                    btnmenu.Image = new Bitmap(Image.FromFile(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + menu.Image), 32, 32);
                                }
                                else
                                {
                                    btnmenu.Image = new Bitmap(Image.FromFile(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + @"images\defaultmenu.ico"), 32, 32);
                                }
                                // btnmenu.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.Large;
                                btnmenu.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
                                btnmenu.Text = menu.Name;
                                btnmenu.Tag = menu;
                                btnmenu.Click += new EventHandler(btnmenu_Click);
                                menuClass.Items.Add(btnmenu);
                            }

                            //listbar.Add(menuClass);
                            rPanel.Controls.Add(menuClass);

                            //rPanel.Refresh();
                        }
                        else
                        {
                            //菜单没有二级分类菜单则创建默认二级分类菜单
                            if (rMenuClass == null)
                            {
                                rMenuClass = new RibbonBar();
                                rMenuClass.AutoOverflowEnabled = true;
                                rMenuClass.Dock = System.Windows.Forms.DockStyle.Left;
                                rMenuClass.Text = "操作功能";
                                rPanel.Controls.Add(rMenuClass);
                            }

                            DevComponents.DotNetBar.ButtonItem btnmenu = new ButtonItem(_menus[j].MenuId.ToString(), _menus[j].Name);
                            //btnmenu.Image = global::EFWBaseLib.Properties.Resources.defaulttool;
                            //btnmenu.ImageListSizeSelection = DevComponents.DotNetBar.eButtonImageListSelection.Large;
                            if (!string.IsNullOrEmpty(_menus[j].Image))
                            {
                                btnmenu.Image = Image.FromFile(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + _menus[j].Image);
                                btnmenu.Image = new Bitmap(Image.FromFile(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + _menus[j].Image), 32, 32);
                            }
                            else
                            {
                                btnmenu.Image = Image.FromFile(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + @"images\defaultmenu.ico");
                            }
                            btnmenu.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top;
                            btnmenu.Text = _menus[j].Name;
                            btnmenu.Tag = _menus[j];
                            btnmenu.Click += new EventHandler(btnmenu_Click);
                            rMenuClass.Items.Add(btnmenu);
                        }
                    }
                }
                #endregion

                ribbonControl1.Items.Insert(ribbonControl1.Items.Count - 1, ribbonTabItemModule);
                ribbonControl1.Controls.Add(rPanel);

                rPanel.ResumeLayout(false);
                ribbonTabItemModule.Refresh();

                #endregion
            }

            ribbonControl1.Show();
            //ribbonTabItem1.Visible = false;
            //ribbonTabItem2.Visible = false;
            ribbonControl1.Refresh();
            this.Refresh();
            ribbonControl1.SelectFirstVisibleRibbonTab();

        }

        //点击菜单
        void btnmenu_Click(object sender, EventArgs e)
        {
            btnImage = sender;
            BaseItem baseItem = sender as BaseItem;

            if (baseItem.Tag.ToString() != "" && baseItem.Tag.GetType() != typeof(BaseModule))
            {
                BaseMenu menu = (BaseMenu)baseItem.Tag;

                WinMenu winmenu = new WinMenu();
                if (AppGlobal.appType == AppType.Winform)
                {
                    winmenu.PluginName = menu.DllName;
                    winmenu.ControllerName = menu.FunName;
                }
                else if (AppGlobal.appType == AppType.WCFClient)
                {
                    if (!string.IsNullOrEmpty(menu.FunWcfName))
                    {
                        winmenu.PluginName = menu.FunWcfName.Split('@')[0];
                        winmenu.ControllerName = menu.FunWcfName.Split('@')[1];
                    }
                }
                //else if (Program.clienttype == "WEBClient")
                //{
                //    winmenu.DllName = "";
                //    winmenu.FunName = "";
                //}

                //winmenu.DllName = menu.DllName;
                //winmenu.FunName = menu.FunName;
                winmenu.IsOutlookBar = menu.MenuLookBar;
                winmenu.IsToolBar = menu.MenuToolBar;
                winmenu.Memo = menu.Memo;
                winmenu.MenuId = menu.MenuId;
                winmenu.ModuleId = menu.ModuleId;
                winmenu.Name = menu.Name;
                winmenu.PMenuId = menu.PMenuId;
                winmenu.SortId = menu.SortId;
                winmenu.UrlPath = InvokeController("GetWebserverUrl").ToString() + menu.UrlName;

                ShowForm(winmenu);
            }
        }

        //取得菜单按钮的图标并缩小成16*16点阵
        private Image GetButtonImage(object sender)
        {
            ButtonItem biSender = sender as ButtonItem;
            if (biSender == null)
                return null;

            //根据Image生成16*16的ICO
            if (biSender.Image != null)
            {
                Bitmap bmp = new Bitmap(biSender.Image, 16, 16);
                return bmp;
            }
            else
                return null;
        }

        public void ShowForm(WinMenu menu)
        {
            Form form = null;
            if (string.IsNullOrEmpty(menu.PluginName) == false && string.IsNullOrEmpty(menu.ControllerName)==false)
            {
                string controllername = menu.ControllerName.Split(new char[] { '|' })[0];
                string viewname = menu.ControllerName.Split(new char[] { '|' }).Length > 1 ? menu.ControllerName.Split(new char[] { '|' })[1] : null;
                if (controllername.Trim() == "") throw new Exception("菜单没有配置好！");

                if (AppGlobal.appType == AppType.Winform)
                {
#if WinformFrame
                    WinformController basec = ControllerHelper.CreateController(menu.PluginName + "@" + controllername);
                    if (string.IsNullOrEmpty(viewname))
                        form = (Form)basec.DefaultView;
                    else
                        form = (Form)basec.iBaseView[viewname];

#endif
                }
                else if (AppGlobal.appType == AppType.WCFClient)
                {
#if WcfFrame
                    EFWCoreLib.WcfFrame.ClientController.WcfClientController basec = EFWCoreLib.WcfFrame.ClientController.ControllerHelper.CreateController(menu.PluginName + "@" + controllername);
                    if (string.IsNullOrEmpty(viewname))
                        form = (Form)basec.DefaultView;
                    else
                        form = (Form)basec.iBaseView[viewname];
#endif
                }

            }

            ShowForm(form, menu.Name, menu.MenuId.ToString());
        }

        //打开首页
        public void ShowForm(Form form, string menuName, string menuId)
        {
            int index = this.barMainContainer.Items.IndexOf(menuId);
            if (index < 0)
            {
                if (form != null)
                {
                    //List<DockContainerItem> listitem = new List<DockContainerItem>();

                    //CloseTab delegateCloseTable = delegate()
                    //{
                    //    foreach (DockContainerItem item in listitem)
                    //        barMainContainer.CloseDockTab(item);
                    //};

                    barMainContainer.BeginInit();
                    int displayWay = CustomConfigManager.GetDisplayWay();//显示方式 0 标准 1全屏
                    if (displayWay == 1)
                        form.Dock = DockStyle.Fill;
                    form.Size = new Size(1000, 600);
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.TopLevel = false;
                    if (this.barMainContainer.Width > form.Width)
                    {
                        form.Location = new Point((barMainContainer.Width - form.Width) / 2, 0);
                    }
                    else
                        form.Location = new Point(0, 0);
                    form.Show();

                    PanelDockContainer panelDockMain = new PanelDockContainer();
                    panelDockMain.Dock = DockStyle.Fill;
                    panelDockMain.Controls.Add(form);
                    panelDockMain.Location = new System.Drawing.Point(3, 28);
                    panelDockMain.Style.Alignment = System.Drawing.StringAlignment.Center;
                    panelDockMain.Style.GradientAngle = 90;
                    panelDockMain.BackColor = Color.FromArgb(227, 239, 255);
                    panelDockMain.AutoScroll = true;



                    DockContainerItem item = new DockContainerItem(form.Text);
                    item.Text = menuName;
                    item.Name = menuId;
                    item.Control = panelDockMain;
                    item.Visible = true;
                    item.Tag = form;//绑定界面对象
                    //item.Image = GetButtonImage(btnImage);
                    //listitem.Add(item);

                    item.VisibleChanged += new EventHandler(item_VisibleChanged);
                    //this.barMainContainer.Controls.Add(panelDockMain);
                    this.barMainContainer.Items.Add(item);
                    this.barMainContainer.SelectedDockContainerItem = item;

                    barMainContainer.EndInit();

                    this.barMainContainer.Show();

                    if (form is BaseFormBusiness)
                    {
                        (form as BaseFormBusiness).ExecOpenWindowBefore(form, null);
                    }
                }
            }
            else
            {
                this.barMainContainer.SelectedDockContainerItem = (DockContainerItem)this.barMainContainer.Items[index];
                string formname = ((DockContainerItem)this.barMainContainer.Items[index]).Tag.GetType().Name;
                if (formname == "FrmWebBrowser")
                {
                    IfrmWebBrowserView webbrowser = (IfrmWebBrowserView)((DockContainerItem)this.barMainContainer.Items[index]).Tag;
                    webbrowser.NavigateUrl();//重新加载网址
                }
            }

        }

        //子窗体关闭事件
        void item_VisibleChanged(object sender, EventArgs e)
        {
            DockContainerItem item = (DockContainerItem)sender;
            if (item.Visible == false)
            {
                try
                {
                    this.barMainContainer.Items.Remove(item);
                    //Form form = (Form)item.Control;
                    //form.Dispose();
                    if (item.Tag is BaseFormBusiness)
                    {
                        (item.Tag as BaseFormBusiness).ExecCloseWindowAfter(item.Tag, null);
                    }
                }
                catch { }
            }
        }

        #endregion

        #region IBaseView 成员

        public ControllerEventHandler InvokeController
        {
            get;
            set;
        }

        #endregion

        //双击托盘显示主界面
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("您确定要退出系统吗？", "询问窗", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                InvokeController("Exit");
                //方式一
                if (BarCode != null)
                    BarCode.Stop();
                //方式二
                CodeBarInput.DisabledAllCodeBarInput();
            }
        }

        private void 消息面板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //InvokeController("ShowMessageForm");
            ShowMessageForm();
        }

        private void 主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }

        private void 切换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvokeController("ReLogin");
        }

        private void btnMessage_Click(object sender, EventArgs e)
        {
            //InvokeController("ShowMessageForm");
            ShowMessageForm();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void FrmMainRibbon_Load(object sender, EventArgs e)
        {
            InitMessageForm();

            //this.notifyIcon1.Visible = true;
            //this.notifyIcon1.Icon = EFWWin.Properties.Resources.msn;
            //this.notifyIcon1.Text = WorkName;
            this.Text = WorkName;

            //BarCodeHook方式获取扫描枪数据
            if (BarCode != null)
                BarCode.Start();

            //CodeBar方式获取扫描枪数据
            CodeBarInput.EnabledAllCodeBarInput(Handle);

            //QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            //qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            //qrCodeEncoder.QRCodeScale = 2;
            //qrCodeEncoder.QRCodeVersion = 7;
            //qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            //metroTileItem1.Image = qrCodeEncoder.Encode("回收");
            //metroTileItem2.Image = qrCodeEncoder.Encode("清洗");
            //metroTileItem3.Image = qrCodeEncoder.Encode("配包");
            //metroTileItem4.Image = qrCodeEncoder.Encode("灭菌");
        }
        //切换科室
        private void buttonItem2_Click(object sender, EventArgs e)
        {
            InvokeController("OpenReDept");
        }
        //修改密码
        private void buttonItem4_Click(object sender, EventArgs e)
        {
            InvokeController("OpenPass");
        }
        //参数设置
        private void buttonItem5_Click(object sender, EventArgs e)
        {
            InvokeController("OpenSetting");
        }
        //关于
        private void buttonItem17_Click(object sender, EventArgs e)
        {
            new FrmAbout().ShowDialog();
        }

        private void switchButtonItem1_ValueChanged(object sender, EventArgs e)
        {
            ribbonControl1.Expanded = !switchButtonItem1.Value;
        }

        private string ToTitleCase(string text)
        {
            if (text.Contains("&"))
            {
                int ampPosition = text.IndexOf('&');
                text = text.Replace("&", "");
                text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
                if (ampPosition > 0)
                    text = text.Substring(0, ampPosition) + "&" + text.Substring(ampPosition);
                else
                    text = "&" + text;
                return text;
            }
            else
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text.ToLower());
        }

        private void AppCommandTheme_Executed(object sender, EventArgs e)
        {
            ICommandSource source = sender as ICommandSource;
            if (source.CommandParameter is string)
            {
                eStyle style = (eStyle)Enum.Parse(typeof(eStyle), source.CommandParameter.ToString());
                // Using StyleManager change the style and color tinting
                if (style == eStyle.Metro)
                {
                    foreach (BaseItem item in RibbonControl.Items)
                    {
                        // Ribbon Control may contain items other than tabs so that needs to be taken in account
                        RibbonTabItem tab = item as RibbonTabItem;
                        if (tab != null)
                            tab.Text = tab.Text.ToUpper();
                    }

                    ribbonControl1.RibbonStripFont = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    StyleManager.MetroColorGeneratorParameters = DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters.DarkBlue;

                    // Adjust size of switch button to match Metro styling
                    switchButtonItem1.SwitchWidth = 12;
                    switchButtonItem1.ButtonWidth = 42;
                    switchButtonItem1.ButtonHeight = 19;

                    StyleManager.Style = eStyle.Metro; // BOOM
                }
                else
                {
                    // If previous style was Metro we need to update other properties as well
                    if (StyleManager.Style == eStyle.Metro)
                    {
                        ribbonControl1.RibbonStripFont = null;
                        foreach (BaseItem item in RibbonControl.Items)
                        {
                            // Ribbon Control may contain items other than tabs so that needs to be taken in account
                            RibbonTabItem tab = item as RibbonTabItem;
                            if (tab != null)
                                tab.Text = ToTitleCase(tab.Text);
                        }
                        // Adjust size of switch button to match Office styling
                        switchButtonItem1.SwitchWidth = 28;
                        switchButtonItem1.ButtonWidth = 62;
                        switchButtonItem1.ButtonHeight = 20;
                    }

                    StyleManager.ChangeStyle(style, Color.Empty);
                }
            }

        }

        private void buttonStyleCustom_ColorPreview(object sender, ColorPreviewEventArgs e)
        {
            if (StyleManager.Style == eStyle.Metro)
            {
                Color baseColor = e.Color;
                StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Color.White, baseColor);
            }
            else
                StyleManager.ColorTint = e.Color;
        }

        private void buttonStyleCustom_ExpandChange(object sender, EventArgs e)
        {

        }

        private void buttonStyleCustom_SelectedColorChanged(object sender, EventArgs e)
        {

            if (StyleManager.Style == eStyle.Metro)
                StyleManager.MetroColorGeneratorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(Color.White, buttonStyleCustom.SelectedColor);
            else
                StyleManager.ColorTint = buttonStyleCustom.SelectedColor;

        }

        MessageTimer mstimer = null;//消息提醒触发器
        public void InitMessageForm()
        {
            if (mstimer != null)
            {
                mstimer.Enabled = false;
                if (TaskbarForm.instance != null)
                    TaskbarForm.instance.ClearMessages();
            }

            mstimer = new MessageTimer();
            mstimer.FrmMain = this;
            //mstimer.Interval = 20000;
            mstimer.Enabled = true;
        }

        public void ShowMessageForm()
        {
            TaskbarForm.ShowForm(this);
        }


        #region IfrmMain 成员


        public void showDebugMenu()
        {
            ribbonControl1.Show();
            //ribbonTabItem1.Visible = false;
            //ribbonTabItem2.Visible = false;
            ribbonControl1.Refresh();
            this.Refresh();
            ribbonControl1.SelectFirstVisibleRibbonTab();
        }

        #endregion


        public void ShowRightForm(Form form,int width, bool Collapsed)
        {
            if (form != null)
            {
                rightdockSite.Visible = true;
                rightdockSite.Width = width;
                dockContainerItem1.Text = form.Text;

                rightContainer.Controls.Clear();
                form.FormBorderStyle = FormBorderStyle.None;
                form.TopLevel = false;
                form.Dock = DockStyle.Fill;
                rightContainer.Controls.Add(form);
                form.Show();
            }
            else
            {
                rightdockSite.Visible = false;
            }
        }


        public void CloseForm(string tabId)
        {
            if (barMainContainer.Items.Contains(tabId))
                barMainContainer.CloseDockTab(this.barMainContainer.Items[tabId] as DockContainerItem);
        }

        #region IBaseViewBusiness 成员

        public string frmName
        {
            get;
            set;
        }

        #endregion
    }
}

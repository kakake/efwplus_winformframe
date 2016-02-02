using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
//using EfwControls.Common;
using EfwControls.WebBrowser;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.Common;
using WinMainUIFrame.Winform.IView;

namespace WinMainUIFrame.Winform.ViewForm
{
    public partial class FrmMain : Office2007Form, IfrmMain
    {
        public FrmMain()
        {
            InitializeComponent();

            this.barMainContainer.SizeChanged += new EventHandler(barMainContainer_SizeChanged);
        }

        #region 方法
        
        /// <summary>
        /// 加载子菜单
        /// </summary>
        /// <param name="menuId">父菜单Id</param>
        /// <param name="patientItem">父菜单控件</param>
        /// <param name="menuData">菜单数据</param>
        private void AddSubMenu(int menuId, BaseItem patientItem, List<BaseMenu> allmenus)
        {
            List<BaseMenu> mainMenu = allmenus.FindAll(x => x.PMenuId == menuId).OrderBy(x => x.SortId).ToList();
            foreach (BaseMenu menu in mainMenu)
            {
                AddToolMenu(menu);
                BaseItem bottonItem = new ButtonItem(menu.MenuId.ToString(), menu.Name);
                bottonItem.Tag = menu;
                AddSubMenu(menu.MenuId, bottonItem, allmenus);
                patientItem.SubItems.Add(bottonItem);
            }
        }
        /// <summary>
        /// 添加工具栏菜单
        /// </summary>
        /// <param name="menu">需要加载的菜单对象</param>
        private void AddToolMenu(BaseMenu menu)
        {
            if (menu.MenuToolBar == 1)
            {
                ButtonItem toolbarItem = new ButtonItem(menu.MenuId.ToString(), menu.Name);
                toolbarItem.ImageFixedSize = new Size(30, 30);
                try
                {
                    toolbarItem.Image = Image.FromFile(menu.Image);
                }
                catch {
                    toolbarItem.Image = Image.FromFile(AppGlobal.AppRootPath + @"images\defaulttool.png");
                }
                toolbarItem.ImagePosition = eImagePosition.Top;
                toolbarItem.Tag = menu;
                this.barMainToolBar.Items.Add(toolbarItem);
            }
        }

        private void defaultToolMenu()
        {
            //ButtonItem toolbarItem0 = new ButtonItem("redept", "切换科室");

            //toolbarItem0.ImageFixedSize = new Size(30, 30);
            //toolbarItem0.Image = Image.FromFile(@"images\1.png");
            //toolbarItem0.ImagePosition = eImagePosition.Top;
            ButtonItem toolbarItem1 = new ButtonItem("relogin", "切换用户");
            toolbarItem1.ImageFixedSize = new Size(30, 30);
            toolbarItem1.Image = Image.FromFile(AppGlobal.AppRootPath + @"images\2.png");
            toolbarItem1.ImagePosition = eImagePosition.Top;
            ButtonItem toolbarItem2 = new ButtonItem("changepassword", "修改密码");
            toolbarItem2.ImageFixedSize = new Size(30, 30);
            toolbarItem2.Image = Image.FromFile(AppGlobal.AppRootPath + @"images\3.png");
            toolbarItem2.ImagePosition = eImagePosition.Top;
            ButtonItem toolbarItem3 = new ButtonItem("welcome", "系统首页");
            toolbarItem3.BeginGroup = true;
            toolbarItem3.ImageFixedSize = new Size(30, 30);
            toolbarItem3.Image = Image.FromFile(AppGlobal.AppRootPath + @"images\1.png");
            toolbarItem3.ImagePosition = eImagePosition.Top;

            ButtonItem toolbarItem4 = new ButtonItem("close", "最小化系统");
            toolbarItem4.ImageFixedSize = new Size(30, 30);
            toolbarItem4.Image = Image.FromFile(AppGlobal.AppRootPath + @"images\4.png");
            toolbarItem4.ImagePosition = eImagePosition.Top;
            toolbarItem4.BeginGroup = true;

            this.barMainToolBar.Items.AddRange(new BaseItem[] { toolbarItem3, 
                //toolbarItem0, 
                toolbarItem1, toolbarItem2, toolbarItem4 });
        }

        private void defaultMainMenu()
        {
            BaseItem menuItem = new ButtonItem("systemmanager","系统维护");

            BaseItem toolbarItem0 = new ButtonItem("redept", "切换科室");
            BaseItem toolbarItem1 = new ButtonItem("relogin", "切换用户");
            BaseItem toolbarItem2 = new ButtonItem("changepassword", "修改密码");
            BaseItem toolbarItem3 = new ButtonItem("setting", "参数设置");
            //BaseItem toolbarItem11 = new ("", "-");
            BaseItem toolbarItem4 = new ButtonItem("help", "帮助");
            toolbarItem4.BeginGroup = true;
            BaseItem toolbarItem5 = new ButtonItem("zhuce", "软件注册");
            toolbarItem5.BeginGroup = true;

            BaseItem toolbarItem8 = new ButtonItem("welcome", "系统首页");
            BaseItem toolbarItem6 = new ButtonItem("about", "关于");

            BaseItem toolbarItem7 = new ButtonItem("close", "最小化系统");
            toolbarItem7.BeginGroup = true;
            menuItem.SubItems.AddRange(new BaseItem[]{toolbarItem0, toolbarItem1, toolbarItem2, toolbarItem3, toolbarItem4, toolbarItem5,toolbarItem8, toolbarItem6, toolbarItem7});

            this.barMainMenu.Items.Add(menuItem);
        }
        /// <summary>
        /// 显示子窗体
        /// </summary>
        /// <param name="baseItem">点击的菜单选项</param>
        private void ShowSubForm(BaseItem baseItem)
        {
            if (baseItem.Tag.ToString() != "" && baseItem.Tag.GetType() != typeof(BaseModule))
            {
                BaseMenu menu = (BaseMenu)baseItem.Tag;

                WinMenu winmenu = new WinMenu();

                if (AppGlobal.appType ==AppType.Winform)
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

        void barMainContainer_SizeChanged(object sender, EventArgs e)
        {
            foreach (DockContainerItem item in this.barMainContainer.Items)
            {
                Form frm=(Form)item.Tag;
                if (this.barMainContainer.Width > frm.Width)
                {
                    frm.Location = new Point((barMainContainer.Width - frm.Width) / 2, 0);
                }
                else
                    frm.Location = new Point(0, 0);
            }
        }

        #endregion

        #region 窗体事件
        //窗体加载事件
        //MessageTimer mstimer = null;
        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.Text = WorkName;

            //InitMessageForm();
        }

        //工具栏菜单点击事件
        private void barMainToolBar_ItemClick(object sender, EventArgs e)
        {
            BaseItem item = (BaseItem)sender;
            switch (item.Name)
            {
                case "redept":
                    InvokeController("OpenReDept");
                    break;
                case "relogin":
                    InvokeController("ReLogin");
                    return;
                case "close":
                    this.Close();
                    return;
                case "changepassword":
                    InvokeController("OpenPass");
                    return;
                case "setting":
                    InvokeController("OpenSetting");
                    return;
                case "welcome":
                    InvokeController("ShowWeclomeForm");
                    return;
            }
            ShowSubForm(item);
            
        }
        //主菜单点击事件
        private void barMainMenu_ItemClick(object sender, EventArgs e)
        {

            BaseItem item = (BaseItem)sender;
            switch (item.Name)
            {
                case "redept":
                    InvokeController("OpenReDept");
                    break;
                case "about":
                    new FrmAbout().ShowDialog();
                    return;
                case "relogin":
                    InvokeController("ReLogin");
                    return;
                case "close":
                    this.Close();
                    return;
                case "welcome":
                    InvokeController("ShowWeclomeForm");
                    return;
                case "changepassword":
                    InvokeController("OpenPass");
                    return;
                case "setting":
                    InvokeController("OpenSetting");
                    return;
                case "zhuce":
                    //FrmRegister reg = new FrmRegister();
                    //reg.ShowDialog();
                    return;
            }
            ShowSubForm(item);

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

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("您确定要退出系统吗？", "询问窗", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                InvokeController("Exit");
            }
        }

        private void 消息面板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //InvokeController("ShowMessageForm");
            //ShowMessageForm();
        }

        private void 主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Show();
        }
        //双击托盘显示主界面
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void 切换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvokeController("ReLogin");
        }

        #region IfrmMain 成员


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


        public ControllerEventHandler InvokeController
        {
            get;
            set;
        }

        public void showSysMenu()
        {
            ((System.ComponentModel.ISupportInitialize)(this.barMainMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMainToolBar)).BeginInit();
            barMainMenu.Items.Clear();
            barMainToolBar.Items.Clear();
            barMainContainer.Items.Clear();


            for (int i = 0; i < modules.Count; i++)
            {
                BaseItem menuItem = new ButtonItem(modules[i].ModuleId.ToString(), modules[i].Name);
                menuItem.Tag = modules[i];
                List<BaseMenu> _menus = menus.FindAll(x => (x.ModuleId == modules[i].ModuleId && x.PMenuId == -1)).OrderBy(x => x.SortId).ToList();
                if (_menus.Count > 0)
                {
                    for (int j = 0; j < _menus.Count; j++)
                    {
                        ButtonItem menuItem1 = new ButtonItem(_menus[j].MenuId.ToString(), _menus[j].Name);
                        menuItem1.Tag = _menus[j];
                        menuItem.SubItems.Add(menuItem1);

                        AddToolMenu(_menus[j]);
                        AddSubMenu(_menus[j].MenuId, menuItem1, menus);
                    }
                }
                this.barMainMenu.Items.Add(menuItem);
            }

            defaultToolMenu();
            defaultMainMenu();

            ((System.ComponentModel.ISupportInitialize)(this.barMainMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMainToolBar)).EndInit();
            this.barMainMenu.Refresh();
            this.barMainToolBar.Refresh();


        }

        public void ShowForm(WinMenu menu)
        {
            Form form = null;

            if (string.IsNullOrEmpty(menu.PluginName) == false && string.IsNullOrEmpty(menu.ControllerName)==false)
            {
                string controllername = menu.ControllerName.Split(new char[] { '|' })[0];
                string viewname = menu.ControllerName.Split(new char[] { '|' }).Length > 1 ? menu.ControllerName.Split(new char[] { '|' })[1] : null;
   
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

        public void ShowForm(Form form, string tabName, string tabId)
        {
            int index = this.barMainContainer.Items.IndexOf(tabId);
            if (index < 0)
            {
                if (form != null)
                {
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
                    item.Text = tabName;
                    item.Name = tabId;
                    item.Control = panelDockMain;
                    item.Visible = true;
                    item.Tag = form;//绑定界面对象

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

        #endregion

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btnMessage_Click(object sender, EventArgs e)
        {
            //InvokeController("ShowMessageForm");
            //ShowMessageForm();
        }

        //MessageTimer mstimer = null;//消息提醒触发器
        //public void InitMessageForm()
        //{
        //    if (mstimer != null)
        //    {
        //        mstimer.Enabled = false;
        //        if (TaskbarForm.instance != null)
        //            TaskbarForm.instance.ClearMessages();
        //    }

        //    mstimer = new MessageTimer();
        //    mstimer.FrmMain =this;
        //    //mstimer.Interval = 20000;
        //    mstimer.Enabled = true;
        //}

        //public void ShowMessageForm()
        //{
        //    TaskbarForm.ShowForm(this);
        //}


        #region IfrmMain 成员


        public void showDebugMenu()
        {
            throw new NotImplementedException();
        }

        #endregion


        public void ShowRightForm(Form form,int width, bool Collapsed)
        {
            
        }


        public void CloseForm(string tabId)
        {
            
        }
    }
}

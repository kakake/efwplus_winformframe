using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WcfFrame.ClientController;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.Common;
using WinMainUIFrame.Winform.IView;
using EfwControls.CustomControl;
using System.Xml;

namespace WinMainUIFrame.Winform.Controller
{
    [WinformController(DefaultViewName = "FrmLogin")]//与系统菜单对应
    [WinformView(Name = "FrmLogin", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.FrmLogin")]
    [WinformView(Name = "FrmMain", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.FrmMain")]
    [WinformView(Name = "FrmMainRibbon", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.FrmMainRibbon")]
    [WinformView(Name = "FrmSetting", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.FrmSetting")]
    [WinformView(Name = "ReDept", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.ReDept")]
    [WinformView(Name = "FrmPassWord", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.FrmPassWord")]
    [WinformView(Name = "FrmWeclome", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.FrmWeclome")]
    public class wcfclientLoginController : JsonWcfClientController
    {
        public IfrmLogin frmlogin;
        public IfrmMain frmmain;

        //private Form _frmsplash;

        //public Form Frmsplash
        //{
        //    get { return _frmsplash; }
        //    set { _frmsplash = value; }
        //}

        public override void Init()
        {
            frmlogin = (IfrmLogin)iBaseView["FrmLogin"];

            int mainStyle = CustomConfigManager.GetMainStyle();
            if (mainStyle == 0)
                frmmain = (IfrmMain)iBaseView["FrmMain"];
            else
                frmmain = (IfrmMain)iBaseView["FrmMainRibbon"];

            DebugLogin();
        }
        //调试免登录
        private void DebugLogin()
        {
            #region 进入调试模式

            if (AppPluginManage.getbaseinfoDataValue(_pluginName, "isdebug") == "true")
            {
                //进入调试模式
                DefaultView = frmmain as IBaseViewBusiness;

                SysLoginRight right = new SysLoginRight();
                right.UserId = 1;
                right.EmpId = 1;
                right.WorkId = 1;
                right.DeptId = 1;
                right.DeptName = "调试科室";
                right.EmpName = "调试用户";
                right.WorkName = "调试机构";
                AppGlobal.cache.Add("RoleUser", right);

                frmmain.UserName = right.EmpName;
                frmmain.DeptName = right.DeptName;
                frmmain.WorkName = right.WorkName;
                if (AppPluginManage.getbaseinfoDataValue(_pluginName, "menuconfig") != null)
                {
                    string filepath = AppPluginManage.getbaseinfoDataValue(_pluginName, "menuconfig");
                    System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                    xmlDoc.Load(filepath);

                    XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("modules/module");
                    List<BaseModule> mdlist = new List<BaseModule>();
                    foreach (XmlNode n in nl)
                    {
                        BaseModule bmd = new BaseModule();
                        bmd.ModuleId = Convert.ToInt32(n.Attributes["ModuleId"].Value);
                        bmd.Name = n.Attributes["Name"].Value;
                        mdlist.Add(bmd);
                    }
                    nl = xmlDoc.DocumentElement.SelectNodes("menus/menu");
                    List<BaseMenu> menulist = new List<BaseMenu>();
                    foreach (XmlNode n in nl)
                    {
                        BaseMenu bmenu = new BaseMenu();
                        bmenu.MenuId = Convert.ToInt32(n.Attributes["MenuId"].Value);
                        bmenu.ModuleId = Convert.ToInt32(n.Attributes["ModuleId"].Value);
                        bmenu.PMenuId = Convert.ToInt32(n.Attributes["PMenuId"].Value);
                        bmenu.Name = n.Attributes["Name"].Value;
                        bmenu.Image = n.Attributes["Image"].Value;
                        bmenu.DllName = n.Attributes["DllName"].Value;
                        bmenu.FunName = n.Attributes["FunName"].Value;
                        menulist.Add(bmenu);
                    }
                    frmmain.modules = mdlist;
                    frmmain.menus = menulist;
                    //frmmain.depts = NewObject<Dept>().GetHaveDept(right.EmpId);
                }
                frmmain.showSysMenu();
                ShowWeclomeForm();

                ((System.Windows.Forms.Form)frmmain).ShowIcon = true;
                ((System.Windows.Forms.Form)frmmain).Icon = System.Drawing.Icon.ExtractAssociatedIcon(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + @"images\msn.ico");
            }
            #endregion
        }
        [WinformMethod]
        public void ShowWeclomeForm()
        {
            frmmain.ShowForm((Form)iBaseView["FrmWeclome"], "首页", "1");
        }
        [WinformMethod]
        public string GetBackGroundImage()
        {
            return CustomConfigManager.GetBackgroundImage();
        }
        [WinformMethod]
        public void ReLogin()
        {
            frmlogin.isReLogin = true;
            ((Form)frmlogin).ShowDialog();
        }


        #region 登录
        [WinformMethod]
        public void UserLogin()
        {
            Object retdata = InvokeWCFService("LoginController", "UserLogin", ToJson(frmlogin.usercode, frmlogin.password));
            object[] vals = ToArray(retdata);
            //MessageBox.Show(vals[0].ToString());
            frmmain.UserName = vals[0].ToString();
            frmmain.DeptName = vals[1].ToString();
            frmmain.WorkName = vals[2].ToString();

            frmmain.modules = ToListObj<BaseModule>(vals[3]);
            frmmain.menus = ToListObj<BaseMenu>(vals[4]);
            frmmain.depts = ToListObj<BaseDept>(vals[5]);

            AppGlobal.cache.Add("RoleUser", ToObject<SysLoginRight>(vals[6]));

            frmmain.showSysMenu();
            ShowWeclomeForm();
            //((Form)frmmain).Icon = System.Drawing.Icon.ExtractAssociatedIcon(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + @"images\msn.ico");
            ((Form)frmmain).Show();
            AppGlobal.winfromMain.MainForm = (System.Windows.Forms.Form)frmmain;
            ////InitMessageForm();//?
            CustomConfigManager.xmlDoc = null;
        }

        #endregion

        #region 设置
        [WinformMethod]
        public void OpenSetting()
        {
            List<InputLanguage> list = new List<InputLanguage>();
            foreach (InputLanguage val in InputLanguage.InstalledInputLanguages)
            {
                list.Add(val);
            }
            ((IfrmSetting)iBaseView["FrmSetting"]).languageList = list;
            ((IfrmSetting)iBaseView["FrmSetting"]).inputMethod_CH = CustomConfigManager.GetInputMethod(EN_CH.CH);
            ((IfrmSetting)iBaseView["FrmSetting"]).inputMethod_EN = CustomConfigManager.GetInputMethod(EN_CH.EN);

            //打印机
            ManagementObjectSearcher query;
            ManagementObjectCollection queryCollection;
            string _classname = "SELECT * FROM Win32_Printer";

            query = new ManagementObjectSearcher(_classname);
            queryCollection = query.Get();
            ((IfrmSetting)iBaseView["FrmSetting"]).loadPrinter(queryCollection, CustomConfigManager.GetPrinter(0), CustomConfigManager.GetPrinter(1), CustomConfigManager.GetPrinter(2));
            //消息
            ((IfrmSetting)iBaseView["FrmSetting"]).runacceptMessage = CustomConfigManager.GetrunacceptMessage() == 1 ? true : false;
            ((IfrmSetting)iBaseView["FrmSetting"]).displayWay = CustomConfigManager.GetDisplayWay() == 1 ? true : false;
            ((IfrmSetting)iBaseView["FrmSetting"]).setbackgroundImage = CustomConfigManager.GetBackgroundImage();
            ((IfrmSetting)iBaseView["FrmSetting"]).mainStyle = CustomConfigManager.GetMainStyle();
            ((Form)iBaseView["FrmSetting"]).ShowDialog();
        }
        [WinformMethod]
        public void SaveSetting()
        {
            ((Form)iBaseView["FrmSetting"]).Close();
            CustomConfigManager.SaveConfig(((IfrmSetting)iBaseView["FrmSetting"]).inputMethod_EN, ((IfrmSetting)iBaseView["FrmSetting"]).inputMethod_CH, ((IfrmSetting)iBaseView["FrmSetting"]).printfirst, ((IfrmSetting)iBaseView["FrmSetting"]).printsecond, ((IfrmSetting)iBaseView["FrmSetting"]).printthree, ((IfrmSetting)iBaseView["FrmSetting"]).runacceptMessage ? 1 : 0, ((IfrmSetting)iBaseView["FrmSetting"]).displayWay ? 1 : 0, ((IfrmSetting)iBaseView["FrmSetting"]).setbackgroundImage, ((IfrmSetting)iBaseView["FrmSetting"]).mainStyle);
        }
        #endregion

        #region 切换科室
        [WinformMethod]
        public void OpenReDept()
        {
            ((IfrmReSetDept)iBaseView["ReDept"]).UserName = base.LoginUserInfo.EmpName;
            ((IfrmReSetDept)iBaseView["ReDept"]).WorkName = base.LoginUserInfo.WorkName;
            ((IfrmReSetDept)iBaseView["ReDept"]).loadDepts(frmmain.depts, LoginUserInfo.DeptId);
            ((Form)iBaseView["ReDept"]).ShowDialog();
        }
        [WinformMethod]
        public void SaveReDept()
        {
            BaseDept dept = ((IfrmReSetDept)iBaseView["ReDept"]).getDept();
            ((SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser")).DeptId = dept.DeptId;
            ((SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser")).DeptName = dept.Name;
            frmmain.DeptName = dept.Name;

            InvokeWCFService("LoginController", "SaveReDept", ToJson(dept.DeptId, dept.Name));
        }
        #endregion

        #region 修改密码
        [WinformMethod]
        public void OpenPass()
        {
            ((IfrmPassWord)iBaseView["FrmPassWord"]).clearPass();
            ((Form)iBaseView["FrmPassWord"]).ShowDialog();
        }
        [WinformMethod]
        public void AlterPass()
        {
            string oldpass = ((IfrmPassWord)iBaseView["FrmPassWord"]).oldpass;
            string newpass = ((IfrmPassWord)iBaseView["FrmPassWord"]).newpass;

            Object retdata = InvokeWCFService("LoginController", "AlterPass", ToJson(LoginUserInfo.UserId, oldpass, newpass));
            if (ToBoolean(retdata) == false)
                throw new Exception("您输入的原始密码不正确！");
        }
        #endregion

        //public List<BaseMessage> GetNotReadMessages()
        //{
        //    Object retdata = InvokeWCFService("LoginController", "GetNotReadMessages");
        //    return ToListObj<BaseMessage>(retdata);
        //}

        //public void MessageRead(int messageId)
        //{
        //    Object retdata = InvokeWCFService("LoginController", "MessageRead",ToJson(messageId));
        //}

        [WinformMethod]
        public string GetSysName()
        {
            string val = AppPluginManage.getbaseinfoDataValue(_pluginName, "SystemName");
            return val ?? "";
        }
        [WinformMethod]
        public string GetLoginBackgroundImage()
        {
            return AppGlobal.AppRootPath + AppPluginManage.getbaseinfoDataValue(_pluginName, "LoginBackgroundImage");
        }
        [WinformMethod]
        public string GetWebserverUrl()
        {
            string val = AppPluginManage.getbaseinfoDataValue(_pluginName, "WEB_serverUrl");
            return val ?? "";
        }

        [WinformMethod]
        public void ShowForm(Form form, string tabName, string tabId)
        {
            frmmain.ShowForm(form, tabName, tabId);
        }

        [WinformMethod]
        public void CloseForm(string tabId)
        {
            frmmain.CloseForm(tabId);
        }

        [WinformMethod]
        public void ShowRightForm(Form form, int width, bool Collapsed)
        {
            frmmain.ShowRightForm(form, width, Collapsed);
        }
    }
}

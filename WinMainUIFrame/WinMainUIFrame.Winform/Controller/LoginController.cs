using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
//using EfwControls.Common;
using EfwControls.CustomControl;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Init;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;
using WinMainUIFrame.ObjectModel.UserLogin;
using WinMainUIFrame.Winform.Common;
using WinMainUIFrame.Winform.IView;
using System.Xml;
using System.Windows.Forms;
using System.Data;

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
    public class LoginController : WinformController
    {
        IfrmLogin frmlogin;
        IfrmMain frmmain;

        #region 登录
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
        public void UserLogin()
        {
            User user = NewObject<User>();
            bool islogin = user.UserLogin(frmlogin.usercode, frmlogin.password);

            if (islogin)
            {
                BaseUser EbaseUser = user.GetUser(frmlogin.usercode);
                SysLoginRight right = new SysLoginRight();
                right.UserId = EbaseUser.UserId;
                right.EmpId = EbaseUser.EmpId;
                right.WorkId = EbaseUser.WorkId;

                Dept dept = NewObject<Dept>();
                BaseDept EbaseDept = dept.GetDefaultDept(EbaseUser.EmpId);
                if (EbaseDept != null)
                {
                    right.DeptId = EbaseDept.DeptId;
                    right.DeptName = EbaseDept.Name;
                }

                BaseEmployee EbaseEmp = (BaseEmployee)NewObject<BaseEmployee>().getmodel(EbaseUser.EmpId);
                right.EmpName = EbaseEmp.Name;

                BaseWorkers EbaseWork = (BaseWorkers)NewObject<BaseWorkers>().getmodel(EbaseUser.WorkId);
                right.WorkName = EbaseWork.WorkName;

                if (EbaseWork.DelFlag == 0)
                {
                    string regkey = EbaseWork.RegKey;
                    DESEncryptor des = new DESEncryptor();
                    des.InputString = regkey;
                    des.DesDecrypt();
                    string[] ret = (des.OutString == null ? "" : des.OutString).Split(new char[] { '|' });
                    if (ret.Length == 2 && ret[0] == EbaseWork.WorkName && Convert.ToDateTime(ret[1]) > DateTime.Now)
                    {
                        AppGlobal.cache.Add("RoleUser", right);

                        //调用单点登录
                        //Guid TokenKey;
                        //EFWCoreLib.CoreFrame.SSO.SsoHelper.SignIn(right.UserId.ToString(), right.EmpName, out TokenKey);
                        //AppGlobal.cache.Add("TokenKey", TokenKey);

                        frmmain.UserName = right.EmpName;
                        frmmain.DeptName = right.DeptName;
                        frmmain.WorkName = right.WorkName;

                        frmmain.modules = NewObject<Module>().GetModuleList(right.UserId).OrderBy(x => x.SortId).ToList();
                        frmmain.menus = NewObject<WinMainUIFrame.ObjectModel.RightManager.Menu>().GetMenuList(right.UserId);
                        frmmain.depts = NewObject<Dept>().GetHaveDept(right.EmpId);

                        frmmain.showSysMenu();
                        ShowWeclomeForm();
                        ((System.Windows.Forms.Form)frmmain).ShowIcon = true;
                        ((System.Windows.Forms.Form)frmmain).Icon = System.Drawing.Icon.ExtractAssociatedIcon(EFWCoreLib.CoreFrame.Init.AppGlobal.AppRootPath + @"images\msn.ico");
                        ((System.Windows.Forms.Form)frmmain).Show();

                        AppGlobal.winfromMain.MainForm = (System.Windows.Forms.Form)frmmain;
                        //InitMessageForm();//?

                        //登录完成后执行扩展的插件方法
                        string val= AppPluginManage.getbaseinfoDataValue(_pluginName, "LoginCompleted");
                        if (val != null && val.Split('@').Length==3)
                        {
                            string pluginName=val.Split('@')[0];
                            string controllerName=val.Split('@')[1];
                            string methodName=val.Split('@')[2];
                            InvokeController(pluginName, controllerName, methodName, right);
                        }

                        CustomConfigManager.xmlDoc = null;
                    }
                    else
                    {
                        throw new Exception("登录用户的当前机构注册码不正确！");
                    }
                }
                else
                {
                    throw new Exception("登录用户的当前机构还未启用！");
                }
            }
            else
            {
                throw new Exception("输入的用户名密码不正确！");
            }
        }
        [WinformMethod]
        public void ShowWeclomeForm()
        {
            frmmain.ShowForm((System.Windows.Forms.Form)iBaseView["FrmWeclome"], "首页", "1");
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
            ((System.Windows.Forms.Form)frmlogin).ShowDialog();
        }

        #endregion

        #region 设置
        [WinformMethod]
        public void OpenSetting()
        {
            List<System.Windows.Forms.InputLanguage> list = new List<System.Windows.Forms.InputLanguage>();
            foreach (System.Windows.Forms.InputLanguage val in System.Windows.Forms.InputLanguage.InstalledInputLanguages)
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
            ((System.Windows.Forms.Form)iBaseView["FrmSetting"]).ShowDialog();
        }
        [WinformMethod]
        public void SaveSetting()
        {
            ((System.Windows.Forms.Form)iBaseView["FrmSetting"]).Close();
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
            ((System.Windows.Forms.Form)iBaseView["ReDept"]).ShowDialog();
        }
        [WinformMethod]
        public void SaveReDept()
        {
            BaseDept dept = ((IfrmReSetDept)iBaseView["ReDept"]).getDept();
            ((SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser")).DeptId = dept.DeptId;
            ((SysLoginRight)EFWCoreLib.CoreFrame.Init.AppGlobal.cache.GetData("RoleUser")).DeptName = dept.Name;
            frmmain.DeptName = dept.Name;
        }
        #endregion

        #region 修改密码
        [WinformMethod]
        public void OpenPass()
        {
            ((IfrmPassWord)iBaseView["FrmPassWord"]).clearPass();
            ((System.Windows.Forms.Form)iBaseView["FrmPassWord"]).ShowDialog();
        }
        [WinformMethod]
        public void AlterPass()
        {
            bool b = NewObject<User>().AlterPassWrod(LoginUserInfo.UserId, ((IfrmPassWord)iBaseView["FrmPassWord"]).oldpass, ((IfrmPassWord)iBaseView["FrmPassWord"]).newpass);
            if (b == false)
                throw new Exception("您输入的原始密码不正确！");
        }
        #endregion

        #region 消息提醒
        /*//?
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
            mstimer.FrmMain = (Form)frmmain;
            //mstimer.Interval = 20000;
            mstimer.Enabled = true;
        }
        

        [WinformMethod]
        public void ShowMessageForm()
        {
            TaskbarForm.ShowForm((Form)frmmain);
        }
        */

        [WinformMethod]
        public List<BaseMessage> GetNotReadMessages()
        {
            string strsql = @"select * from BaseMessage where (Limittime>getdate()) and MessageType in (
																		select Code from BaseMessageType a where  (select count(1) from BaseGroupUser  where GroupId in (a.ReceiveGroup) and userId={0})>0
																		)
                                and (id not in (select messageid from BaseMessageRead where userid={0})) 
                                and (ReceiveWork={2} or ReceiveWork=0)
                                and (ReceiveDept={1} or ReceiveDept=0)
                                and (ReceiveUser={0} or ReceiveUser=0)";
            strsql = string.Format(strsql, LoginUserInfo.UserId, LoginUserInfo.DeptId, LoginUserInfo.WorkId);

            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToList<BaseMessage>(dt, oleDb, _container, _cache, _pluginName, null);
            else
                return new List<BaseMessage>();
        }
        [WinformMethod]
        public void MessageRead(int messageId)
        {
            string strsql = "select count(*) from BaseMessageRead where messageid={0} and userid={1}";
            strsql = string.Format(strsql, messageId, LoginUserInfo.UserId);
            if (Convert.ToInt32(oleDb.GetDataResult(strsql)) == 0)
            {
                strsql = "insert into BaseMessageRead(messageid,userid,readtime) values(" + messageId + "," + LoginUserInfo.UserId + ",'" + DateTime.Now.Date.ToString() + "')";
                oleDb.DoCommand(strsql);
            }
        }
         
        #endregion

        [WinformMethod]
        public string GetSysName()
        {
             string val= AppPluginManage.getbaseinfoDataValue(_pluginName, "SystemName");
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

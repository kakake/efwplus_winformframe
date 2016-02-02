using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.WcfFrame.ClientController;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.IView.RightManager;

namespace WinMainUIFrame.Winform.Controller
{
    [WinformController(DefaultViewName = "frmMenu")]//与系统菜单对应
    [WinformView(Name = "frmMenu", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.RightManager.frmMenu")]
    [WinformView(Name = "frmGroupMenu", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.RightManager.frmGroupMenu")]
    public class wcfclientRightController : JsonWcfClientController
    {
        IfrmMenu frmMenu;
        IfrmGroupMenu frmgroupmenu;
        public override void Init()
        {
            frmMenu = (IfrmMenu)iBaseView["frmMenu"];
            frmgroupmenu = (IfrmGroupMenu)iBaseView["frmGroupMenu"];
        }
        [WinformMethod]
        public void InitMenuData()
        {
            Object retdata = InvokeWCFService("RightController", "InitMenuData");
            List<BaseModule> modulelist = ToListObj<BaseModule>(ToArray(retdata)[0]);
            List<BaseMenu> menulist = ToListObj<BaseMenu>(ToArray(retdata)[1]);

            frmMenu.loadMenuTree(modulelist, menulist);
        }
        [WinformMethod]
        public void NewMenu()
        {
            Object retdata = InvokeWCFService("RightController", "NewMenu");
            BaseMenu menu = ToObject<BaseMenu>(retdata);
            frmMenu.currentMenu = menu;

        }
        [WinformMethod]
        public void SaveMenu()
        {
            Object retdata = InvokeWCFService("RightController", "SaveMenu",ToJson( frmMenu.currentMenu));
            InitMenuData();
        }
        [WinformMethod]
        public void DeleteMenu(int menuId)
        {
            Object retdata = InvokeWCFService("RightController", "DeleteMenu", ToJson(menuId));
            InitMenuData();
        }

        [WinformMethod]
        public void InitGroupData()
        {
            Object retdata = InvokeWCFService("RightController", "InitGroupData");
            List<BaseGroup> grouplist = ToListObj<BaseGroup>(retdata);
            frmgroupmenu.loadGroupGrid(grouplist);
        }
        [WinformMethod]
        public void LoadGroupMenuData(int groupId)
        {
            Object retdata = InvokeWCFService("RightController", "LoadGroupMenuData",ToJson(groupId));

            List<BaseModule> modulelist = ToListObj<BaseModule>(ToArray(retdata)[0]);
            List<BaseMenu> menulist = ToListObj<BaseMenu>(ToArray(retdata)[1]);
            List<BaseMenu> groupmenulist = ToListObj<BaseMenu>(ToArray(retdata)[2]);

            frmgroupmenu.loadMenuTree(modulelist, menulist, groupmenulist);
        }
        [WinformMethod]
        public void SetGroupMenu(int groupId, int[] menuIds)
        {
            Object retdata = InvokeWCFService("RightController", "SetGroupMenu", ToJson(groupId,menuIds));
        }

        #region 页面权限
        [WinformMethod]
        public void GetPageMenuData()
        {
            frmgroupmenu.panelPageEnabled = false;
            if (frmgroupmenu.currGroupId == -1 || frmgroupmenu.currMenu == null)
            {
                frmgroupmenu.loadPageMenu(null);
            }
            else
            {
                if (frmgroupmenu.currMenu.FunName.Trim() == "" && frmgroupmenu.currMenu.UrlName.Trim() == "")
                    frmgroupmenu.panelPageEnabled = false;
                else
                    frmgroupmenu.panelPageEnabled = true;

                Object retdata = InvokeWCFService("RightController", "GetPageMenuData", ToJson(frmgroupmenu.currGroupId, frmgroupmenu.currMenu.MenuId));

                DataTable dt = ToDataTable(retdata);
                frmgroupmenu.loadPageMenu(dt);
            }
        }
        [WinformMethod]
        public void SavePageMenu(int moduleId, int menuId, string code, string name)
        {
            Object retdata = InvokeWCFService("RightController", "SavePageMenu", ToJson(moduleId, menuId,code,name));

            GetPageMenuData();
        }
        [WinformMethod]
        public void DeletePageMenu(int pageId)
        {
            Object retdata = InvokeWCFService("RightController", "DeletePageMenu", ToJson(pageId));

            GetPageMenuData();
        }
        [WinformMethod]
        public void SetGroupPage(int groupId, int pageId)
        {
            Object retdata = InvokeWCFService("RightController", "SetGroupPage", ToJson(groupId, pageId));

            GetPageMenuData();
        }
        #endregion
    }
}

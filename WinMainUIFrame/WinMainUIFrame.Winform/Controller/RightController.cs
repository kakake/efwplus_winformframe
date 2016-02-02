using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;
using WinMainUIFrame.Winform.IView.RightManager;
using System.Windows.Forms;

namespace WinMainUIFrame.Winform.Controller
{
    [WinformController(DefaultViewName = "frmMenu")]//与系统菜单对应
    [WinformView(Name = "frmMenu", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.RightManager.frmMenu")]
    [WinformView(Name = "frmGroupMenu", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.RightManager.frmGroupMenu")]
    [WinformView(Name = "frmAddGroup", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.RightManager.frmAddGroup")]
    [WinformView(Name = "frmAddModule", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.RightManager.frmAddModule")]
    public class RightController : WinformController
    {
        IfrmMenu frmMenu;
        IfrmGroupMenu frmgroupmenu;
        IfrmAddGroup frmaddgroup;
        IfrmAddmodule frmaddmodule;
        public override void Init()
        {
            frmMenu = (IfrmMenu)iBaseView["frmMenu"];
            frmgroupmenu = (IfrmGroupMenu)iBaseView["frmGroupMenu"];
            frmaddgroup = (IfrmAddGroup)iBaseView["frmAddGroup"];
            frmaddmodule = (IfrmAddmodule)iBaseView["frmAddModule"];
        }
        [WinformMethod]
        public void InitMenuData()
        {
            List<BaseModule> modulelist = NewObject<BaseModule>().getlist<BaseModule>();
            List<BaseMenu> menulist = NewObject<BaseMenu>().getlist<BaseMenu>();

            frmMenu.loadMenuTree(modulelist, menulist);
        }
        [WinformMethod]
        public void NewMenu()
        {
            BaseMenu menu = NewObject<BaseMenu>();
            menu.PMenuId = frmMenu.selectMenuId;
            menu.ModuleId = frmMenu.selectModuleId;
            frmMenu.currentMenu = menu;

        }
        [WinformMethod]
        public void SaveMenu()
        {
            BaseMenu menu = frmMenu.currentMenu;
            menu.save();
            InitMenuData();
        }
        [WinformMethod]
        public void DeleteMenu(int menuId)
        {
            NewObject<BaseMenu>().delete(menuId);
            InitMenuData();
        }

        [WinformMethod]
        public void InitGroupData()
        {
            List<BaseGroup> grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
            frmgroupmenu.loadGroupGrid(grouplist);
        }
        [WinformMethod]
        public void LoadGroupMenuData(int groupId)
        {
            List<BaseModule> modulelist = NewObject<BaseModule>().getlist<BaseModule>();
            List<BaseMenu> menulist = NewObject<BaseMenu>().getlist<BaseMenu>();
            List<BaseMenu> groupmenulist = NewObject<WinMainUIFrame.ObjectModel.RightManager.Menu>().GetGroupMenuList(groupId);

            frmgroupmenu.loadMenuTree(modulelist, menulist, groupmenulist);
        }
        [WinformMethod]
        public void SetGroupMenu(int groupId, int[] menuIds)
        {
            NewObject<Group>().SetGroupMenu(groupId, menuIds);
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

                string strsql = @"SELECT Id,Code,Name,
                                        (CASE WHEN (SELECT COUNT(*) FROM dbo.BaseGroupPage WHERE GroupId={0} AND PageId=BasePageMenu.Id)>0 THEN 1 ELSE 0 END) IsUse
                                         FROM BasePageMenu
                                         WHERE menuid={1}";
                strsql = string.Format(strsql, frmgroupmenu.currGroupId, frmgroupmenu.currMenu.MenuId);
                DataTable dt = oleDb.GetDataTable(strsql);
                frmgroupmenu.loadPageMenu(dt);
            }
        }
        [WinformMethod]
        public void SavePageMenu(int moduleId, int menuId, string code, string name)
        {
            string strsql = @" INSERT INTO dbo.BasePageMenu
                                         ( ModuleId ,MenuId ,Code ,Name)
                                         VALUES  ({0},{1},'{2}','{3}')";
            strsql = string.Format(strsql, moduleId, menuId, code, name);
            oleDb.DoCommand(strsql);

            GetPageMenuData();
        }
        [WinformMethod]
        public void DeletePageMenu(int pageId)
        {
            string strsql = @"delete from basepagemenu where id="+pageId;
            oleDb.DoCommand(strsql);

            GetPageMenuData();
        }
        [WinformMethod]
        public void SetGroupPage(int groupId, int pageId)
        {
            string strsql = @"select count(*) from BaseGroupPage where GroupId={0} and PageId={1}";
            strsql = string.Format(strsql, groupId, pageId);
            if (Convert.ToInt32(oleDb.GetDataResult(strsql)) > 0)
            {
                strsql = @"delete from BaseGroupPage where GroupId={0} and PageId={1}";
                strsql = string.Format(strsql, groupId, pageId);
                oleDb.DoCommand(strsql);
            }
            else
            {
                strsql = @"INSERT INTO BaseGroupPage(GroupId,PageId) VALUES({0},{1})";
                strsql = string.Format(strsql, groupId, pageId);
                oleDb.DoCommand(strsql);
            }

            GetPageMenuData();
        }
        #endregion

        [WinformMethod]
        public void NewGroup()
        {
            (frmaddgroup as Form).Text = "添加角色";
            
            BaseGroup group=NewObject<BaseGroup>();
            group.Name="";
            group.Memo="";
            frmaddgroup.currGroup = group;
            (frmaddgroup as Form).ShowDialog();
        }

        [WinformMethod]
        public void AlterGroup(int groupId)
        {
            (frmaddgroup as Form).Text = "修改角色";
            
            BaseGroup group = NewObject<BaseGroup>().getmodel(groupId) as BaseGroup;
            frmaddgroup.currGroup = group;
            (frmaddgroup as Form).ShowDialog();
        }

        [WinformMethod]
        public bool DeleteGroupisExist(int groupId)
        {
            string strsql = @"select count(*) from basegroupuser where groupid="+groupId;
            if (Convert.ToInt32(oleDb.GetDataResult(strsql)) > 0)
                return false;
            strsql = @"select count(*) from basegroupmenu where groupid="+groupId;
            if (Convert.ToInt32(oleDb.GetDataResult(strsql)) > 0)
                return false;
            return true;
        }

        [WinformMethod]
        public void DeleteGroup(int groupId)
        {
            string strsql = @"delete from basegroup where groupid="+groupId;
            oleDb.DoCommand(strsql);

            InitGroupData();
        }

        [WinformMethod]
        public void SaveGroup()
        {
            BaseGroup group = frmaddgroup.currGroup;
            group.BindDb(this);
            group.save();

            InitGroupData();
        }

        [WinformMethod]
        public void NewModule()
        {
            (frmaddmodule as Form).Text = "添加模块";

            BaseModule module = NewObject<BaseModule>();
            module.Name = "";
            module.Memo = "";
            frmaddmodule.currModule = module;
            (frmaddmodule as Form).ShowDialog();
        }

        [WinformMethod]
        public void AlterModule(int moduleId)
        {
            (frmaddmodule as Form).Text = "修改模块";

            BaseModule module = NewObject<BaseModule>().getmodel(moduleId) as BaseModule;
            frmaddmodule.currModule = module;
            (frmaddmodule as Form).ShowDialog();
        }

        [WinformMethod]
        public void DeleteModule(int moduleId)
        {
            string strsql = @"delete from BaseModule where ModuleId=" + moduleId;
            oleDb.DoCommand(strsql);

            InitMenuData();
        }

        [WinformMethod]
        public void SaveModule()
        {
            BaseModule module = frmaddmodule.currModule;
            module.BindDb(this);
            module.save();

            InitMenuData();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.WcfFrame.ServerController;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;

namespace WinMainUIFrame.WcfController
{
    [WCFController]
    public class RightController : JsonWcfServerController
    {
        [WCFMethod]
        public string InitMenuData()
        {
            List<BaseModule> modulelist = NewObject<BaseModule>().getlist<BaseModule>();
            List<BaseMenu> menulist = NewObject<BaseMenu>().getlist<BaseMenu>();

            return ToJson(modulelist, menulist);
        }
        [WCFMethod]
        public string NewMenu()
        {
            int selectMenuId = Convert.ToInt32(ToArray(ParamJsonData)[0]);
            int selectModuleId = Convert.ToInt32(ToArray(ParamJsonData)[1]);

            BaseMenu menu = NewObject<BaseMenu>();
            menu.PMenuId = selectMenuId;
            menu.ModuleId = selectModuleId;
            return ToJson(menu);
        }
        [WCFMethod]
        public string SaveMenu()
        {
            BaseMenu menu = ToObject<BaseMenu>(ParamJsonData);
            menu.BindDb(oleDb, _container, _cache, _pluginName);
            menu.save();
            return ToJson(true);
        }
        [WCFMethod]
        public string DeleteMenu()
        {
            int menuId = ToInt32(ParamJsonData);
            NewObject<BaseMenu>().delete(menuId);
            return ToJson(true);
        }

        [WCFMethod]
        public string InitGroupData()
        {
            List<BaseGroup> grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
            return ToJson(grouplist);
        }
        [WCFMethod]
        public string LoadGroupMenuData()
        {
            int groupId = ToInt32(ParamJsonData);
            List<BaseModule> modulelist = NewObject<BaseModule>().getlist<BaseModule>();
            List<BaseMenu> menulist = NewObject<BaseMenu>().getlist<BaseMenu>();
            List<BaseMenu> groupmenulist = NewObject<Menu>().GetGroupMenuList(groupId);

            return ToJson(modulelist, menulist, groupmenulist);
        }
        [WCFMethod]
        public string SetGroupMenu()
        {
            int groupId=Convert.ToInt32(ToArray(ParamJsonData)[0]);
            int[] menuIds = ToObject<int[]>(ToArray(ParamJsonData)[1]);
            NewObject<Group>().SetGroupMenu(groupId, menuIds);

            return ToJson(true);
        }


        #region 页面权限
        [WCFMethod]
        public string GetPageMenuData()
        {
            int currGroupId = Convert.ToInt32(ToArray(ParamJsonData)[0]);
            int MenuId = Convert.ToInt32(ToArray(ParamJsonData)[1]);

            string strsql = @"SELECT Id,Code,Name,
                                        (CASE WHEN (SELECT COUNT(*) FROM dbo.BaseGroupPage WHERE GroupId={0} AND PageId=BasePageMenu.Id)>0 THEN 1 ELSE 0 END) IsUse
                                         FROM BasePageMenu
                                         WHERE menuid={1}";
            strsql = string.Format(strsql, currGroupId, MenuId);
            DataTable dt = oleDb.GetDataTable(strsql);
            return ToJson(dt);
        }

        [WCFMethod]
        public string SavePageMenu()
        {
            int moduleId = Convert.ToInt32(ToArray(ParamJsonData)[0]);
            int menuId = Convert.ToInt32(ToArray(ParamJsonData)[1]);
            string code = ToArray(ParamJsonData)[2].ToString();
            string name = ToArray(ParamJsonData)[3].ToString();

            string strsql = @" INSERT INTO dbo.BasePageMenu
                                         ( ModuleId ,MenuId ,Code ,Name)
                                         VALUES  ({0},{1},'{2}','{3}')";
            strsql = string.Format(strsql, moduleId, menuId, code, name);
            oleDb.DoCommand(strsql);
            return ToJson(true);
        }
        [WCFMethod]
        public string DeletePageMenu()
        {
            int pageId = ToInt32(ParamJsonData);

            string strsql = @"delete from basepagemenu where id=" + pageId;
            oleDb.DoCommand(strsql);

            return ToJson(true);
        }
        [WCFMethod]
        public string SetGroupPage()
        {
            int groupId=Convert.ToInt32(ToArray(ParamJsonData)[0]);
            int pageId = Convert.ToInt32(ToArray(ParamJsonData)[0]);

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

            return ToJson(true);
        }
        #endregion
    }
}

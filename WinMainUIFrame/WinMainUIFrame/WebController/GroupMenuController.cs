using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.DbProvider.SqlPagination;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
using Newtonsoft.Json;
using WinMainUIFrame.Dao;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;

namespace WebUIFrame.WebController
{
    [WebController]
    public class GroupMenuController : JEasyUIController
    {
        [WebMethod]
        public void GetGroupList()
        {
            List<BaseGroup> grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
            JsonResult = ToGridJson(grouplist);
        }

        private void GetTreeNodeList(List<BaseMenu> _menuList, List<BaseMenu> menugroup, int PId, treeNode _node)
        {
            List<BaseMenu> menus = _menuList.FindAll(x => x.PMenuId == PId).OrderBy(x => x.SortId).ToList();
            if (menus.Count > 0)
            {
                _node.children = new List<treeNode>();
                for (int i = 0; i < menus.Count; i++)
                {
                    treeNode nodemenu = new treeNode(menus[i].MenuId, menus[i].Name);
                    nodemenu.check = menugroup.FindIndex(x => x.MenuId == menus[i].MenuId) >= 0 ? true : false;
                    GetTreeNodeList(_menuList, menugroup, menus[i].MenuId, nodemenu);
                    _node.children.Add(nodemenu);
                }
            }
        }
        [WebMethod]
        public void GetGroupMenu()
        {
            try
            {
                int groupid = Convert.ToInt32(ParamsData["groupId"]);

                List<BaseModule> moduleList = NewObject<BaseModule>().getlist<BaseModule>();
                List<BaseMenu> menuList = NewObject<BaseMenu>().getlist<BaseMenu>();

                Menu menu = NewObject<Menu>();
                List<BaseMenu> _menugroup = menu.GetGroupMenuList(groupid);
                List<BaseMenu> menugroup = new List<BaseMenu>();
                for (int i = 0; i < _menugroup.Count; i++)
                {
                    if (menuList.FindIndex(x => x.PMenuId == _menugroup[i].MenuId) == -1)
                        menugroup.Add(_menugroup[i]);
                }
                List<treeNode> tree = new List<treeNode>();

                for (int i = 0; i < moduleList.Count; i++)
                {
                    treeNode node = new treeNode(moduleList[i].ModuleId, moduleList[i].Name);
                    node.state = "open";
                    node.attributes = new Dictionary<string, object>();
                    node.attributes.Add("ismodule", true);
                    List<BaseMenu> menus = menuList.FindAll(x => (x.ModuleId == moduleList[i].ModuleId && x.PMenuId == -1)).OrderBy(x => x.SortId).ToList();
                    if (menus.Count > 0)
                    {
                        node.children = new List<treeNode>();
                        for (int j = 0; j < menus.Count; j++)
                        {
                            treeNode nodemenu = new treeNode(menus[j].MenuId, menus[j].Name);
                            nodemenu.check = menugroup.FindIndex(x => x.MenuId == menus[j].MenuId) >= 0 ? true : false;
                            GetTreeNodeList(menuList, menugroup, menus[j].MenuId, nodemenu);
                            node.children.Add(nodemenu);
                        }
                    }

                    tree.Add(node);
                }

                JsonResult = ToTreeJson(tree);
            }
            catch (Exception ex)
            {
                JsonResult = ex.Message;
            }
        }
        [WebMethod]
        public void SaveUserWorkGroup()
        {
            int groupid = Convert.ToInt32(ParamsData["groupId"]);
            string[] menuids = ParamsData["checkid"].Split(new char[] { ',' });
            int[] menuid = new int[menuids.Length];
            for (int i = 0; i < menuids.Length; i++)
            {
                menuid[i] = Convert.ToInt32(menuids[i].Trim());
            }
            NewObject<Group>().SetGroupMenu(groupid, menuid);
            JsonResult = RetSuccess("保存成功！");
        }
        [WebMethod]
        public void GetAllUserList()
        {
            int pageIndex = Convert.ToInt32(ParamsData["page"]);
            int rows = Convert.ToInt32(ParamsData["rows"]);
            PageInfo page = new PageInfo(rows, pageIndex);
            page.KeyName = "code";

            DataTable dt = null;
            UserDao dao = NewDao<UserDao>();
            if (ParamsData.ContainsKey("key"))
                dt = dao.GetUserData(ParamsData["key"], page);
            else
                dt = dao.GetUserData("", page);
            JsonResult = ToGridJson(dt, page.totalRecord);
        }
        [WebMethod]
        public void GetGroupUserList()
        {
            int groupId = Convert.ToInt32(ParamsData["groupId"]);
            int pageIndex = Convert.ToInt32(ParamsData["page"]);
            int rows = Convert.ToInt32(ParamsData["rows"]);
            PageInfo page = new PageInfo(rows, pageIndex);
            page.KeyName = "code";
            DataTable dt = null;
            UserDao dao = NewDao<UserDao>();
            if (ParamsData.ContainsKey("key"))
                dt = dao.GetUserData(groupId, ParamsData["key"], page);
            else
                dt = dao.GetUserData(groupId,"", page);
            JsonResult = ToGridJson(dt, page.totalRecord);
        }
        [WebMethod]
        public void GroupDeleteUser()
        {
            int groupId = Convert.ToInt32(ParamsData["groupId"]);
            string[] suserId = ParamsData["userids"].Split(new char[] { ',' });
            int[] userid = new int[suserId.Length];
            for (int i = 0; i < suserId.Length; i++)
            {
                userid[i] = Convert.ToInt32(suserId[i]);
            }
            //AbstractGroup group = NewObject<AbstractGroup>();
            //group.DeleteUserGroup(groupId, userid);
            JsonResult = RetSuccess("操作成功！");
        }
        [WebMethod]
        public void GroupAddUser()
        {
            int groupId = Convert.ToInt32(ParamsData["groupId"]);
            string[] suserId = ParamsData["userids"].Split(new char[] { ',' });
            int[] userid = new int[suserId.Length];
            for (int i = 0; i < suserId.Length; i++)
            {
                userid[i] = Convert.ToInt32(suserId[i]);
            }
            //AbstractGroup group = NewObject<AbstractGroup>();
            //group.AddUserGroup(groupId, userid);
            JsonResult = RetSuccess("操作成功！");
        }

    }

}
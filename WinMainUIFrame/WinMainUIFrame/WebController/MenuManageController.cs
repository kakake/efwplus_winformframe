using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
using Newtonsoft.Json;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;

namespace WebUIFrame.WebController
{
    [WebController]
    public class MenuManageController : JEasyUIController
    {
        private void GetTreeNodeList(List<BaseMenu> _menuList, int PId, treeNode _node)
        {
            List<BaseMenu> menus = _menuList.FindAll(x => x.PMenuId == PId).OrderBy(x => x.SortId).ToList();
            if (menus.Count > 0)
            {
                _node.children = new List<treeNode>();
                for (int i = 0; i < menus.Count; i++)
                {
                    treeNode nodemenu = new treeNode(menus[i].MenuId, menus[i].Name);
                    //nodemenu.check = menugroup.FindIndex(x => x.MenuId == menus[i].MenuId) >= 0 ? true : false;
                    GetTreeNodeList(_menuList, menus[i].MenuId, nodemenu);
                    nodemenu.attributes = new Dictionary<string, object>();
                    if (string.IsNullOrEmpty(menus[i].FunName) && string.IsNullOrEmpty(menus[i].FunWcfName) && string.IsNullOrEmpty(menus[i].UrlName))
                        nodemenu.attributes.Add("isclass", true);
                    else
                        nodemenu.attributes.Add("ismenu", true);
                    nodemenu.attributes.Add("moduleid", menus[i].ModuleId);
                    _node.children.Add(nodemenu);
                }
            }
        }
        [WebMethod]
        public void GetMenuTree()
        {
            try
            {
                List<BaseModule> moduleList = NewObject<BaseModule>().getlist<BaseModule>();
                List<BaseMenu> menuList = NewObject<BaseMenu>().getlist<BaseMenu>();

                //AbstractModule module = NewObject<AbstractModule>();
                //List<AbstractModule> moduleList = module.GetModuleList();
                //AbstractMenu menu = NewObject<AbstractMenu>();
                //List<AbstractMenu> menuList = menu.GetMenuList();

                List<treeNode> tree = new List<treeNode>();

                for (int i = 0; i < moduleList.Count; i++)
                {
                    treeNode node = new treeNode(moduleList[i].ModuleId, moduleList[i].Name);
                    node.state = "open";
                    node.attributes = new Dictionary<string, object>();
                    node.attributes.Add("ismodule", true);
                    node.attributes.Add("moduleid", moduleList[i].ModuleId);
                    List<BaseMenu> menus = menuList.FindAll(x => (x.ModuleId == moduleList[i].ModuleId && x.PMenuId == -1)).OrderBy(x => x.SortId).ToList();
                    if (menus.Count > 0)
                    {
                        node.children = new List<treeNode>();
                        for (int j = 0; j < menus.Count; j++)
                        {
                            treeNode nodemenu = new treeNode(menus[j].MenuId, menus[j].Name);
                            //nodemenu.check = menugroup.FindIndex(x => x.MenuId == menus[j].MenuId) >= 0 ? true : false;
                            GetTreeNodeList(menuList, menus[j].MenuId, nodemenu);
                            nodemenu.attributes = new Dictionary<string, object>();
                            if (string.IsNullOrEmpty(menus[j].FunName) && string.IsNullOrEmpty(menus[j].FunWcfName) && string.IsNullOrEmpty(menus[j].UrlName))
                                nodemenu.attributes.Add("isclass", true);
                            else
                                nodemenu.attributes.Add("ismenu", true);
                            nodemenu.attributes.Add("moduleid", menus[j].ModuleId);
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
        public void SelectMenuManage()
        {
            int menuId = Convert.ToInt32(ParamsData["menuId"]);
            BaseMenu menu = NewObject<BaseMenu>().getmodel(menuId) as BaseMenu;
            JsonResult = RetSuccess("", ToJson(menu));
        }
        [WebMethod]
        public void SaveMenu()
        {
            try
            {
                int menuId = Convert.ToInt32(ParamsData["MenuId"]);
                BaseMenu menu = NewObject<BaseMenu>().getmodel(menuId) as BaseMenu;
                base.GetModel<BaseMenu>(menu).save();
                JsonResult = RetSuccess("保存成功！");
            }
            catch (Exception err)
            {
                JsonResult = RetSuccess(err.Message);
            }
        }
        [WebMethod]
        public void DeleteMenu()
        {
            int menuId = Convert.ToInt32(ParamsData["menuId"]);
            NewObject<BaseMenu>().delete(menuId);
            JsonResult = RetSuccess("删除成功！");
        }
        /// <summary>
        /// 菜单排序
        /// </summary>
        [WebMethod]
        public void SortMenuUp()
        {
            try
            {
                int menuid = Convert.ToInt32(ParamsData["menuid"]);
                NewObject<Menu>().SortUp(menuid);
                JsonResult = RetSuccess("");
            }
            catch (Exception err)
            {
                JsonResult = RetError(err.Message);
            }
        }
        [WebMethod]
        public void SortMenuDown()
        {
            try
            {
                int menuid = Convert.ToInt32(ParamsData["menuid"]);
                NewObject<Menu>().SortDown(menuid);
                JsonResult = RetSuccess("");
            }
            catch (Exception err)
            {
                JsonResult = RetError(err.Message);
            }
        }
        /// <summary>
        /// 模块排序
        /// </summary>
        [WebMethod]
        public void SortModuleUp()
        {
            try
            {
                int moduleid = Convert.ToInt32(ParamsData["moduleid"]);
                NewObject<Module>().SortUp(moduleid);
                JsonResult = RetSuccess("");
            }
            catch (Exception err)
            {
                JsonResult = RetError(err.Message);
            }
        }
        [WebMethod]
        public void SortModuleDown()
        {
            try
            {
                int moduleid = Convert.ToInt32(ParamsData["moduleid"]);
                NewObject<Module>().SortDown(moduleid);
                JsonResult = RetSuccess("");
            }
            catch (Exception err)
            {
                JsonResult = RetError(err.Message);
            }
        }
    }
}
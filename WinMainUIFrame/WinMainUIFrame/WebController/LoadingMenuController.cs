using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
using Newtonsoft.Json;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;
using EFWCoreLib.CoreFrame.Init;
using System.Xml;

namespace WebUIFrame.WebController
{
    [WebController]
    public class LoadingMenuController : JEasyUIController
    {
        //metronic 菜单显示
        [WebMethod]
        public void GetLoginAllMenu()
        {
            string jsondata = "";
            DataTable dt = sessionData["ListModeules"] as DataTable;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int ModuleId = Convert.ToInt32(dt.Rows[i]["ModuleId"]);
                string ret = getmenusJson(ModuleId);
                if (jsondata == "")
                    jsondata += "{\"ModuleId\":" + dt.Rows[i]["ModuleId"].ToString() + ",\"Name\":\"" + dt.Rows[i]["Name"].ToString() + "\",\"Menus\":[" + ret + "]}";
                else
                    jsondata += ",{\"ModuleId\":" + dt.Rows[i]["ModuleId"].ToString() + ",\"Name\":\"" + dt.Rows[i]["Name"].ToString() + "\",\"Menus\":[" + ret + "]}";
            }
            JsonResult = RetSuccess("", "[" + jsondata + "]");
        }

        private string getmenusJson(int ModuleId)
        {
            List<BaseMenu> menuList = ConvertExtend.ToList<BaseMenu>(sessionData["ListMenus"] as DataTable);

            List<BaseMenu> AllmenuList = NewObject<BaseMenu>().getlist<BaseMenu>();
            List<BaseMenu> _menuList = new List<BaseMenu>();
            for (int i = 0; i < menuList.Count; i++)
            {
                if (menuList[i].PMenuId != -1)
                {
                    BaseMenu _menu = AllmenuList.Find(x => x.MenuId == menuList[i].PMenuId);
                    if (_menu != null)
                    {
                        if (_menuList.FindIndex(x => x.MenuId == _menu.MenuId) == -1 && menuList.FindIndex(x => x.MenuId == _menu.MenuId) == -1)
                            _menuList.Add(_menu);
                    }
                }
            }
            menuList.AddRange(_menuList);


            List<BaseMenu> menus = menuList.FindAll(x => (x.ModuleId == ModuleId && x.PMenuId == -1)).OrderBy(x => x.SortId).ToList();
            string ret = "";//"[{\"FirstMenu\":{},\"SecondMenu\":[]}]";
            for (int i = 0; i < menus.Count; i++)
            {
                string FirstMenu = "{}";
                string SecondMenu = "[]";

                FirstMenu = JavaScriptConvert.SerializeObject(menus[i]);
                List<BaseMenu> secondmenus = menuList.FindAll(x => x.PMenuId == menus[i].MenuId).OrderBy(x => x.SortId).ToList();
                SecondMenu = secondmenus.Count > 0 ? JavaScriptConvert.SerializeObject(secondmenus) : "[]";
                if (ret == "")
                    ret += "{\"FirstMenu\":" + FirstMenu + ",\"SecondMenu\":" + SecondMenu + "}";
                else
                    ret += ",{\"FirstMenu\":" + FirstMenu + ",\"SecondMenu\":" + SecondMenu + "}";
            }

            return ret;
        }

        [WebMethod]
        public void GetLoginModuleData()
        {
            DataTable dt = sessionData["ListModeules"] as DataTable;
            JsonResult = RetSuccess("", JavaScriptConvert.SerializeObject(dt));
        }
        [WebMethod]
        public void GetLoginFirstMenu()
        {

            int ModuleId = Convert.ToInt32(ParamsData["ModuleId"]);

            List<BaseMenu> menuList = ConvertExtend.ToList<BaseMenu>(sessionData["ListMenus"] as DataTable);

            List<BaseMenu> AllmenuList = NewObject<BaseMenu>().getlist<BaseMenu>();
            List<BaseMenu> _menuList = new List<BaseMenu>();
            for (int i = 0; i < menuList.Count; i++)
            {
                if (menuList[i].PMenuId != -1)
                {
                    BaseMenu _menu = AllmenuList.Find(x => x.MenuId == menuList[i].PMenuId);
                    if (_menu != null)
                    {
                        if (_menuList.FindIndex(x => x.MenuId == _menu.MenuId) == -1 && menuList.FindIndex(x => x.MenuId == _menu.MenuId) == -1)
                            _menuList.Add(_menu);
                    }
                }
            }
            menuList.AddRange(_menuList);


            List<BaseMenu> menus = menuList.FindAll(x => (x.ModuleId == ModuleId && x.PMenuId == -1)).OrderBy(x => x.SortId).ToList();
            string ret = "";//"[{\"FirstMenu\":{},\"SecondMenu\":[]}]";
            for (int i = 0; i < menus.Count; i++)
            {
                string FirstMenu = "{}";
                string SecondMenu = "[]";

                FirstMenu = JavaScriptConvert.SerializeObject(menus[i]);
                List<BaseMenu> secondmenus = menuList.FindAll(x => x.PMenuId == menus[i].MenuId).OrderBy(x => x.SortId).ToList();
                SecondMenu = secondmenus.Count > 0 ? JavaScriptConvert.SerializeObject(secondmenus) : "[]";
                if (ret == "")
                    ret += "{\"FirstMenu\":" + FirstMenu + ",\"SecondMenu\":" + SecondMenu + "}";
                else
                    ret += ",{\"FirstMenu\":" + FirstMenu + ",\"SecondMenu\":" + SecondMenu + "}";
            }
            JsonResult = RetSuccess("", "[" + ret + "]");
        }

        [WebMethod]
        public void GetLoginModuleData_Debug()
        {
            string filepath = AppPluginManage.getbaseinfoDataValue(_pluginName, "menuconfig");
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(AppGlobal.AppRootPath+filepath);

            XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("modules/module");
            List<BaseModule> mdlist = new List<BaseModule>();
            foreach (XmlNode n in nl)
            {
                BaseModule bmd = new BaseModule();
                bmd.ModuleId = Convert.ToInt32(n.Attributes["ModuleId"].Value);
                bmd.Name = n.Attributes["Name"].Value;
                mdlist.Add(bmd);
            }

            DataTable dt = ConvertExtend.ToDataTable(mdlist);
            JsonResult = RetSuccess("", JavaScriptConvert.SerializeObject(dt));
        }
        [WebMethod]
        public void GetLoginFirstMenu_Debug()
        {

            int ModuleId = Convert.ToInt32(ParamsData["ModuleId"]);

            string filepath = AppPluginManage.getbaseinfoDataValue(_pluginName, "menuconfig");
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(AppGlobal.AppRootPath + filepath);
            XmlNodeList nl = xmlDoc.DocumentElement.SelectNodes("menus/menu");
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
                bmenu.UrlName = n.Attributes["UrlName"].Value;
                menulist.Add(bmenu);
            }

            List<BaseMenu> menuList = menulist;
            //List<BaseMenu> AllmenuList = menulist;
            List<BaseMenu> _menuList = new List<BaseMenu>();
            for (int i = 0; i < menuList.Count; i++)
            {
                if (menuList[i].PMenuId != -1)
                {
                    BaseMenu _menu = menuList.Find(x => x.MenuId == menuList[i].PMenuId);
                    if (_menuList.FindIndex(x => x.MenuId == _menu.MenuId) == -1 && menuList.FindIndex(x => x.MenuId == _menu.MenuId) == -1)
                        _menuList.Add(_menu);
                }
            }
            menuList.AddRange(_menuList);


            List<BaseMenu> menus = menuList.FindAll(x => (x.ModuleId == ModuleId && x.PMenuId == -1)).OrderBy(x => x.SortId).ToList();
            string ret = "";//"[{\"FirstMenu\":{},\"SecondMenu\":[]}]";
            for (int i = 0; i < menus.Count; i++)
            {
                string FirstMenu = "{}";
                string SecondMenu = "[]";

                FirstMenu = JavaScriptConvert.SerializeObject(menus[i]);
                List<BaseMenu> secondmenus = menuList.FindAll(x => x.PMenuId == menus[i].MenuId).OrderBy(x => x.SortId).ToList();
                SecondMenu = secondmenus.Count > 0 ? JavaScriptConvert.SerializeObject(secondmenus) : "[]";
                if (ret == "")
                    ret += "{\"FirstMenu\":" + FirstMenu + ",\"SecondMenu\":" + SecondMenu + "}";
                else
                    ret += ",{\"FirstMenu\":" + FirstMenu + ",\"SecondMenu\":" + SecondMenu + "}";
            }
            JsonResult = RetSuccess("", "[" + ret + "]");
        }

        /*
        private void GetTreeNodeList(List<BaseMenu> _menuList, int PId, treeNode _node, string serverIP)
        {
            List<BaseMenu> menus = _menuList.FindAll(x => x.PMenuId == PId).OrderBy(x => x.SortId).ToList();
            if (menus.Count > 0)
            {
                _node.children = new List<treeNode>();
                for (int i = 0; i < menus.Count; i++)
                {
                    treeNode nodemenu = new treeNode(menus[i].MenuId, menus[i].Name);

                    GetTreeNodeList(_menuList, menus[i].MenuId, nodemenu, serverIP);
                    //添加url
                    if (menus[i].UrlName != null && menus[i].UrlName != "")//表示菜单分类节点
                    {
                        nodemenu.attributes = new Dictionary<string, object>();
                        string opurl = serverIP + menus[i].UrlName + "?pageid=" + menus[i].UrlId;
                        nodemenu.attributes.Add("opurl", opurl);
                    }
                    _node.children.Add(nodemenu);
                }
            }
        }
        [WebMethod]
        public void LoadMenus()
        {
            try
            {

                List<BaseModule> moduleList = ConvertExtend.ToList<BaseModule>(sessionData["ListModeules"] as DataTable);
                List<BaseMenu> menuList = ConvertExtend.ToList<BaseMenu>(sessionData["ListMenus"] as DataTable);

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
                            GetTreeNodeList(menuList, menus[j].MenuId, nodemenu, moduleList[i].ServerIP);
                            //添加url
                            if (menus[j].UrlName != null && menus[j].UrlName != "")//表示菜单分类节点
                            {
                                nodemenu.attributes = new Dictionary<string, object>();
                                //string opurl = afm.ListModeules[i].ServerIP + menus[j].UrlPath + "?pageid=" + menus[j].UrlId;
                                string opurl = moduleList[i].ServerIP + menus[j].UrlName;
                                nodemenu.attributes.Add("opurl", opurl);
                            }
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
        public void GetLoginModuleData_New()
        {
            DataTable dt = sessionData["ListModeules"] as DataTable;
            string module = JavaScriptConvert.SerializeObject(dt);

            string listdata = "";

            for (int n = 0; n < dt.Rows.Count; n++)
            {
                int ModuleId = Convert.ToInt32(dt.Rows[n]["ModuleId"]);
                List<BaseMenu> menuList = ConvertExtend.ToList<BaseMenu>(sessionData["ListMenus"] as DataTable);

                List<BaseMenu> AllmenuList = NewObject<BaseMenu>().getlist<BaseMenu>();
                List<BaseMenu> _menuList = new List<BaseMenu>();
                for (int i = 0; i < menuList.Count; i++)
                {
                    if (menuList[i].PMenuId != -1)
                    {
                        BaseMenu _menu = AllmenuList.Find(x => x.MenuId == menuList[i].PMenuId);
                        if (_menuList.FindIndex(x => x.MenuId == _menu.MenuId) == -1 && menuList.FindIndex(x => x.MenuId == _menu.MenuId) == -1)
                            _menuList.Add(_menu);
                    }
                }
                menuList.AddRange(_menuList);


                List<BaseMenu> menus = menuList.FindAll(x => (x.ModuleId == ModuleId && x.PMenuId == -1)).OrderBy(x => x.SortId).ToList();
                string ret = "";//"[{\"FirstMenu\":{},\"SecondMenu\":[]}]";
                for (int i = 0; i < menus.Count; i++)
                {
                    string FirstMenu = "{}";
                    string SecondMenu = "[]";

                    FirstMenu = JavaScriptConvert.SerializeObject(menus[i]);
                    List<BaseMenu> secondmenus = menuList.FindAll(x => x.PMenuId == menus[i].MenuId).OrderBy(x => x.SortId).ToList();
                    SecondMenu = secondmenus.Count > 0 ? JavaScriptConvert.SerializeObject(secondmenus) : "[]";
                    if (ret == "")
                        ret += "{\"FirstMenu\":" + FirstMenu + ",\"SecondMenu\":" + SecondMenu + "}";
                    else
                        ret += ",{\"FirstMenu\":" + FirstMenu + ",\"SecondMenu\":" + SecondMenu + "}";
                }

                if (listdata == "")
                    listdata = "{\"listdata\":[" + ret + "],\"id\":" + ModuleId + "}";
                else
                    listdata += ",{\"listdata\":[" + ret + "],\"id\":" + ModuleId + "}";
            }

            string data = "{\"module\":" + module + ",\"menu\":[" + listdata + "]}";

            JsonResult = RetSuccess("", data);
        }
        [WebMethod]
        public void GetLoginMenuData()
        {
            DataTable dt = sessionData["ListMenus"] as DataTable; 
            JsonResult = RetSuccess("", JavaScriptConvert.SerializeObject(dt));
        }
        
        [WebMethod]
        public void GetLoginSecondMenu()
        {
            int PMenuId = Convert.ToInt32(ParamsData["MenuId"]);
            List<BaseMenu> menuList = ConvertExtend.ToList<BaseMenu>(sessionData["ListMenus"] as DataTable);
            List<BaseMenu> menus = menuList.FindAll(x => x.PMenuId == PMenuId).OrderBy(x => x.SortId).ToList();
            JsonResult = RetSuccess("", JavaScriptConvert.SerializeObject(menus));
        }

        */
        [WebMethod]
        public void GetChangeDept()
        {
            DataTable dt = sessionData["ListDepts"] as DataTable;
            List<Hashtable> hashlist = new List<Hashtable>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Hashtable hash = new Hashtable();
                hash.Add("DeptId", dt.Rows[i]["DeptId"]);
                hash.Add("Name", dt.Rows[i]["Name"]);
                hashlist.Add(hash);
            }
            JsonResult = JavaScriptConvert.SerializeObject(hashlist);
        }
        [WebMethod]
        public void ChangeDept()
        {

            string deptid = FormData["selectDept"];
            DataTable dt = sessionData["ListDepts"] as DataTable;
            SysLoginRight afm = LoginUserInfo;
            afm.DeptId = Convert.ToInt32(deptid);
            afm.DeptName = dt.Select("DeptId=" + deptid)[0]["Name"].ToString();

            if (PutOutData.ContainsKey("RoleUser"))
                PutOutData.Remove("RoleUser");

            PutOutData.Add("RoleUser", afm);
            JsonResult = RetSuccess("");
        }
        [WebMethod]
        public void GetOutLookMenu()
        {
            try
            {
                int userid = LoginUserInfo.UserId;
                List<BaseModule> moduleList = NewObject<BaseModule>().getlist<BaseModule>();

                List<BaseMenu> menus = NewObject<WinMainUIFrame.ObjectModel.RightManager.Menu>().GetOutLookMenuList(userid);
                JsonResult = RetSuccess("", "{\"module\":" + ToJson(moduleList) + ",\"menu\":" + ToJson(menus) + "}");
            }
            catch (Exception ex)
            {
                JsonResult = ex.Message;
            }
        }
        [WebMethod]
        public void ExecMenuBindSQL()
        {
            DataTable dt = NewObject<Menu>().ExecMenuBindSQL(Convert.ToInt32(ParamsData["MenuId"]));
            JsonResult = RetSuccess("", "{\"menuId\":" + ParamsData["MenuId"] + ",\"Val\":" + ToJson(dt) + "}");
        }
        [WebMethod]
        public void GetNotReadMessageCount()
        {

        }
    }
}
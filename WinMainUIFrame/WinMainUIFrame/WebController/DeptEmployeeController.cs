using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.DbProvider.SqlPagination;
using EFWCoreLib.WebFrame.HttpHandler.Controller;
using Newtonsoft.Json;
using WinMainUIFrame.Dao;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;
using WinMainUIFrame.ObjectModel.UserLogin;


namespace WebUIFrame.WebController
{
    [WebController]
    public class DeptEmployeeController : JEasyUIController
    {
        #region 科室操作

        private void GetTreeNodeList(List<BaseDeptLayer> _layerlist, BaseDeptLayer _layer, treeNode _node)
        {
            List<BaseDeptLayer> list = _layerlist.FindAll(x => x.PId == _layer.LayerId);
            if (list.Count > 0)
            {
                _node.children = new List<treeNode>();
                for (int i = 0; i < list.Count; i++)
                {
                    treeNode node = new treeNode(list[i].LayerId, list[i].Name);
                    GetTreeNodeList(_layerlist, list[i], node);
                    _node.children.Add(node);
                }
            }
        }

        [WebMethod]
        public void GetDeptLayers()
        {
            List<BaseDeptLayer> layerlist = NewObject<BaseDeptLayer>().getlist<BaseDeptLayer>();

            List<BaseDeptLayer> root = layerlist.FindAll(x => x.PId == 0);
            List<treeNode> tree = new List<treeNode>();
            for (int i = 0; i < root.Count; i++)
            {
                treeNode node = new treeNode(root[i].LayerId, root[i].Name);
                GetTreeNodeList(layerlist, root[i], node);
                tree.Add(node);

            }

            JsonResult = ToTreeJson(tree);
        }

        //add by jyq on 2015-6-14
        [WebMethod]
        public void GetDeptLayersList()
        {
            List<BaseDeptLayer> layerlist = NewObject<BaseDeptLayer>().getlist<BaseDeptLayer>();

            JsonResult = ToJson(layerlist);
        }

        [WebMethod]
        public void SaveDeptLayer()
        {
            BaseDeptLayer layer = GetModel<BaseDeptLayer>();
            layer.save();

            JsonResult = RetSuccess("", layer.LayerId.ToString());
        }
        [WebMethod]
        public void DeleteDeptLayer()
        {
            int id = Convert.ToInt32(ParamsData["deptLayerId"]);

            string strwhere =  "Layer = " + id ;
            BaseDept dept = NewObject<BaseDept>();
            List<BaseDept> deptlist = dept.getlist<BaseDept>(strwhere);
            if (deptlist.Count > 0)
            {
                JsonResult = RetError("科室不为空，删除失败！");
                return;
            }
            BaseDeptLayer layer = NewObject<BaseDeptLayer>();
            layer.delete(id);

            JsonResult = RetSuccess("");
        }
        [WebMethod]
        public void SearchDeptData()
        {
            int pageIndex = Convert.ToInt32(ParamsData["page"]);
            int rows = Convert.ToInt32(ParamsData["rows"]);
            PageInfo page = new PageInfo(rows, pageIndex);
            page.KeyName = "Name";

            string ids = ParamsData["layerId"];
            //string strwhere = ids == "" ? "" : "Layer in (" + ids + ")";
            //BaseDept dept = NewObject<BaseDept>();
            //List<BaseDept> deptlist = dept.getlist<BaseDept>(page, strwhere);

            StringBuilder strSql = new StringBuilder();
            strSql.Append(string.Format(@"SELECT * FROM
(SELECT 
	ROW_NUMBER()OVER(ORDER BY Name) AS rowIndex,
	DeptId ,Layer ,Name , Pym ,Wbm ,Szm ,Code ,DelFlag ,SortOrder ,Memo ,WorkId,
	(CASE DelFlag WHEN 0 THEN '否' ELSE '是' END) AS DelFlagText
FROM EFWDB.dbo.BaseDept
WHERE 
	Layer IN({0})) T1
WHERE 
	rowIndex >= {1} AND rowIndex <= {2}", ids,page.startNum,page.endNum));
            DataTable dt = oleDb.GetDataTable(strSql.ToString());
            List<BaseDept> deptlist = ConvertExtend.ToList<BaseDept>(dt, oleDb, GetUnityContainer(), _cache, _pluginName, null);
            page.totalRecord = dt.Rows.Count;
            JsonResult = ToGridJson(deptlist, page.totalRecord);
        }
        [WebMethod]
        public void SaveDept()
        {
            int deptId = string.IsNullOrEmpty(ParamsData["DeptId"].ToString())?0:Convert.ToInt32(ParamsData["DeptId"].ToString());
            BaseDept dept = NewObject<BaseDept>().getmodel(deptId) as BaseDept;
            if (dept == null)
            {
                BaseDept d = NewObject<BaseDept>();
                d.Layer = Convert.ToInt32(ParamsData["Layer"]);
                d.Name = ParamsData["Name"];
                d.DelFlag = Convert.ToInt32(ParamsData["DelFlag"]);
                d.Pym = "";
                d.Wbm = "";
                d.Szm = "";
                d.Code = "";
                d.SortOrder = 1;
                d.Memo = "";
                dept = d;
            }
            GetModel<BaseDept>(dept).save();
            JsonResult = RetSuccess("保存成功！");
        }

        #endregion

        #region 人员操作
        [WebMethod]
        public void GetDeptTree()
        {
            //string key=ParamsData["key"];
            BaseDept dept = NewObject<BaseDept>();
            List<BaseDept> deptlist = dept.getlist<BaseDept>("DelFlag=0 ");
            JsonResult = ToJson(deptlist);
        }
        [WebMethod]
        public void GetEmployeeUserData()
        {
            string deptId = ParamsData["deptId"];
            string empName = ParamsData["empName"];
            int pageIndex = Convert.ToInt32(ParamsData["page"]);
            int rows = Convert.ToInt32(ParamsData["rows"]);
            PageInfo page = new PageInfo(rows, pageIndex);
            page.KeyName = "Name";

            int[] deptids = deptId == "" ? new int[] { 0 } : new int[] { Convert.ToInt32(deptId) };

            UserDao dao = NewObject<UserDao>();
            DataTable dt = dao.GetUserData(deptids, empName, page);
            JsonResult = ToGridJson(dt, page.totalRecord);
        }
        [WebMethod]
        public void GetSexData()
        {
            JsonResult = "[{\"Id\":-1,\"Name\":\"未知性别\"},{\"Id\":0,\"Name\":\"男性\"},{\"Id\":1,\"Name\":\"女性\"}]";
        }
        [WebMethod]
        public void SaveEmployeeUser()
        {
            string defaultDeptid = FormData["defaultDeptid"];
            string empdeptIds = FormData["empdeptIds"];
            string groupIds = FormData["groupIds"];

            //
            int empId = Convert.ToInt32(ParamsData["EmpId"]);
            int userId = Convert.ToInt32(ParamsData["UserId"]);
            BaseEmployee emp = NewObject<BaseEmployee>().getmodel(empId) as BaseEmployee;
            emp = GetModel<BaseEmployee>(emp);
            BaseUser user = NewObject<BaseUser>().getmodel(userId) as BaseUser;
            user = GetModel<BaseUser>(user);


            if (emp.EmpId == 0)
            {
                if (NewObject<User>().GetUser(user.Code) != null)
                {
                    JsonResult = RetError("输入的用户名已存在！");
                    return;
                }
            }

            string[] depts = empdeptIds.Split(new char[] { ',' });
            int[] deptid = new int[depts.Length];
            for (int i = 0; i < deptid.Length; i++)
            {
                deptid[i] = Convert.ToInt32(depts[i]);
            }

            string[] groups = groupIds.Split(new char[] { ',' });
            int[] groupid = new int[groups.Length];
            for (int i = 0; i < groupid.Length; i++)
            {
                groupid[i] = Convert.ToInt32(groups[i]);
            }

            NewObject<User>().SaveUser(emp, user, deptid, Convert.ToInt32(defaultDeptid), groupid);
            JsonResult = RetSuccess("保存成功！");

        }
        [WebMethod]
        public void GetEmpUserModel()
        {
            int empId = Convert.ToInt32(ParamsData["empId"]);
            int userId = Convert.ToInt32(ParamsData["userId"] == "" ? "0" : ParamsData["userId"]);

            BaseEmployee emp = NewObject<BaseEmployee>();
            emp = (BaseEmployee)emp.getmodel(empId);

            BaseUser user = NewObject<BaseUser>();
            user = (BaseUser)user.getmodel(userId);

            int defaultdeptId = NewObject<Dept>().GetDefaultDept(empId).DeptId;
            List<BaseDept> _empdept = NewObject<Dept>().GetHaveDept(empId);
            List<BaseGroup> _usergroup = NewObject<Group>().GetGroupList(userId);
            //DataTable deptdt = emp.GetDeptList(empId);
            //if (user == null) user = NewObject<AbstractUser>(); 
            //DataTable groupdt = user.GetUserGroup(userId);

            JsonResult = RetSuccess("", "{\"emp\":" + ToJson(emp) + ",\"user\":" + ToJson(user) + ",\"defaultdeptId\":" + defaultdeptId + ",\"deptdt\":" + ToJson(_empdept) + ",\"groupdt\":" + ToJson(_usergroup) + "}");
        }
        [WebMethod]
        public void GetGroupData()
        {
            DataTable dt = NewObject<BaseGroup>().gettable();
            JsonResult = ToJson(dt);
        }
        [WebMethod]
        public void ResetPassword()
        {
            try
            {
                int userId = Convert.ToInt32(ParamsData["userId"]);
                User user = NewObject<User>();
                user.ResetPassword(userId);
                JsonResult = RetSuccess("重置密码成功！");
            }
            catch (Exception err)
            {
                JsonResult = RetSuccess("重置密码失败！" + err.Message);
            }
        }
        #endregion

    }

}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WcfFrame.ServerController;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.RightManager;
using WinMainUIFrame.ObjectModel.UserLogin;

namespace WinMainUIFrame.WcfController
{
    [WCFController]
    public class EmpUserController : JsonWcfServerController
    {
        [WCFMethod]
        public string LoadDeptData()
        {
            List<BaseDeptLayer> layerlist = NewObject<BaseDeptLayer>().getlist<BaseDeptLayer>();
            List<BaseDept> deptlist = NewObject<BaseDept>().getlist<BaseDept>();
            return ToJson(layerlist, deptlist);
        }
        [WCFMethod]
        public string SaveDeptLayer()
        {
            int layerId = Convert.ToInt32(ToArray(ParamJsonData)[0]);
            string layername = ToArray(ParamJsonData)[1].ToString();
            int pId = Convert.ToInt32(ToArray(ParamJsonData)[3]);

            BaseDeptLayer layer = NewObject<BaseDeptLayer>();
            layer.LayerId = layerId;
            layer.Name = layername;
            layer.PId = pId;
            layer.save();
            return ToJson(layer);
        }
        [WCFMethod]
        public string DeleteDeptLayer()
        {
            int layerId = ToInt32(ParamJsonData);
            NewObject<BaseDeptLayer>().delete(layerId);
            return ToJson(true);
        }
        [WCFMethod]
        public string SaveDept()
        {
            int deptId=Convert.ToInt32(ToArray(ParamJsonData)[0]);
            string deptname=ToArray(ParamJsonData)[1].ToString();
            int layerId=Convert.ToInt32(ToArray(ParamJsonData)[2]);

            BaseDept dept = NewObject<BaseDept>();
            dept.DeptId = deptId;
            dept.Layer = layerId;
            dept.Name = deptname;
            dept.Pym = SpellAndWbCode.GetSpellCode(deptname);
            dept.Wbm = SpellAndWbCode.GetWBCode(deptname);
            dept.save();
            return ToJson(dept);
        }
        [WCFMethod]
        public string DeleteDept()
        {
            int deptId = ToInt32(ParamJsonData);
            NewObject<BaseDept>().delete(deptId);
            return ToJson(true);
        }
        [WCFMethod]
        public string LoadUserData()
        {
            int[] deptIds = ToObject<int[]>(ParamJsonData);
            DataTable dt = NewObject<User>().GetUserData(deptIds);
            return ToJson(dt);
        }
        [WCFMethod]
        public string LoadUserData_Key()
        {
            string key = ToString(ParamJsonData);
            DataTable dt = NewObject<User>().GetUserData(key);
            return ToJson(dt);
        }
        [WCFMethod]
        public string NewUser()
        {
            BaseEmployee _currEmp = NewObject<BaseEmployee>();
            BaseUser _currUser = NewObject<BaseUser>();
            List<BaseGroup> _grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
            List<BaseDept> _deptlist = NewObject<BaseDept>().getlist<BaseDept>();

            return ToJson(_currEmp, _currUser, _grouplist, _deptlist);
        }
        [WCFMethod]
        public string AlterUser()
        {
            int empid = Convert.ToInt32(ToArray(ParamJsonData)[0]);
            int userid = Convert.ToInt32(ToArray(ParamJsonData)[1]);

            BaseEmployee _currEmp = (BaseEmployee)NewObject<BaseEmployee>().getmodel(empid);
            BaseUser _currUser = (BaseUser)NewObject<BaseUser>().getmodel(userid);
            List<BaseGroup> _grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
            List<BaseDept> _deptlist = NewObject<BaseDept>().getlist<BaseDept>();

            List<BaseGroup> _usergroup = NewObject<Group>().GetGroupList(userid);
            List<BaseDept> _empdept = NewObject<Dept>().GetHaveDept(empid);
            BaseDept currdept = NewObject<Dept>().GetDefaultDept(empid);

            return ToJson(_currEmp, currdept == null ? -1 : currdept.DeptId, _currUser, _grouplist, _deptlist, _usergroup, _empdept, currdept);
        }
        [WCFMethod]
        public string SaveUser()
        {
            BaseEmployee emp = ToObject<BaseEmployee>(ToArray(ParamJsonData)[0]);
            BaseUser user = ToObject<BaseUser>(ToArray(ParamJsonData)[1]);
            int[] currEmpDeptList = ToObject<int[]>(ToArray(ParamJsonData)[2]);
            int currDefaultEmpDept = Convert.ToInt32(ToArray(ParamJsonData)[3]);
            int[] currGroupUserList = ToObject<int[]>(ToArray(ParamJsonData)[4]);

            emp.BindDb(oleDb, _container,_cache,_pluginName);
            user.BindDb(oleDb, _container, _cache, _pluginName);

            if (emp.EmpId == 0)
            {
                if (NewObject<User>().GetUser(user.Code) != null)
                    return ToJson(false);
            }

            emp.Pym = SpellAndWbCode.GetSpellCode(emp.Name);
            emp.Wbm = SpellAndWbCode.GetWBCode(emp.Name);
            emp.Brithday = DateTime.Now;
            emp.save();
            user.EmpId = emp.EmpId;
            user.PassWord = ConvertExtend.ToPassWord("1");
            //user.IsAdmin = 0;
            user.save();

            NewObject<Employee>().SetHaveEmpDeptRole(emp.EmpId, currEmpDeptList);
            NewObject<Employee>().SetDefaultEmpDeptRole(emp.EmpId, currDefaultEmpDept);
            NewObject<User>().SetGroupUserRole(user.UserId, currGroupUserList);

            return ToJson(true);
        }
        [WCFMethod]
        public string ResetUserPass()
        {
            int userId = ToInt32(ParamJsonData);
            NewObject<User>().ResetPassword(userId);
            return ToJson(true);
        }
        [WCFMethod]
        public string LoadWorkerList()
        {
            List<BaseWorkers> workerlist = NewObject<BaseWorkers>().getlist<BaseWorkers>();
            return ToJson(workerlist);
        }
        [WCFMethod]
        public string GetCurrWorker()
        {
            int workId = ToInt32(ParamJsonData);
            BaseWorkers worker = NewObject<BaseWorkers>().getmodel(workId) as BaseWorkers;
            return ToJson(worker);
        }
        [WCFMethod]
        public string NewWorker()
        {
            BaseWorkers worker = NewObject<BaseWorkers>();
            worker.DelFlag = -1;//-1新建 0启用 1禁用
            return ToJson(worker);
        }
        [WCFMethod]
        public string SaveWorker()
        {
            BaseWorkers worker = ToObject<BaseWorkers>(ParamJsonData);
            worker.BindDb(oleDb, _container, _cache, _pluginName);
            worker.save();

            //修改必须重新启用
            if (worker.DelFlag == 0)
            {
                //TurnOnOffWorker(worker.WorkId);
                worker.DelFlag = 1;
            }

            return ToJson(worker);
        }

        //启用禁用机构，先判读注册码是否正确
        [WCFMethod]
        public string TurnOnOffWorker()
        {
            int workId = ToInt32(ParamJsonData);
            BaseWorkers worker = NewObject<BaseWorkers>().getmodel(workId) as BaseWorkers;
            string msgText = "";

            if (worker.DelFlag == -1)//新建的启用
            {
                //判读注册码是否正确
                string regkey = worker.RegKey;
                DESEncryptor des = new DESEncryptor();
                des.InputString = regkey;
                des.DesDecrypt();
                string[] ret = (des.OutString == null ? "" : des.OutString).Split(new char[] { '|' });
                if (ret.Length == 2 && ret[0] == worker.WorkName && Convert.ToDateTime(ret[1]) > DateTime.Now)
                {
                    //新建机构，增加用户、关联科室、关联角色
                    oleDb.WorkId = worker.WorkId;
                    //修改状态为0
                    worker.DelFlag = 0;
                    worker.save();
                    //MessageBoxEx.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    msgText = "操作成功！";
                }
                else
                {
                    msgText = "机构注册码不正确！";
                }
            }
            else if (worker.DelFlag == 1)//禁用的启用
            {
                //判读注册码是否正确
                string regkey = worker.RegKey;
                DESEncryptor des = new DESEncryptor();
                des.InputString = regkey;
                des.DesDecrypt();
                string[] ret = (des.OutString == null ? "" : des.OutString).Split(new char[] { '|' });
                if (ret.Length == 2 && ret[0] == worker.WorkName && Convert.ToDateTime(ret[1]) > DateTime.Now)
                {
                    //修改状态为0
                    worker.DelFlag = 0;
                    worker.save();
                    //MessageBoxEx.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    msgText = "操作成功！";
                }
                else
                {
                    //MessageBoxEx.Show("机构注册码不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    msgText = "机构注册码不正确！";
                }
            }
            else//禁用
            {
                //修改状态为1
                worker.DelFlag = 1;
                worker.save();
                //MessageBoxEx.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                msgText = "操作成功！";
            }

            return ToJson(worker,msgText);
        }
        [WCFMethod]
        public string InitWorkerUser()
        {
            int workId = ToInt32(ParamJsonData);

            BaseEmployee _currEmp = NewObject<BaseEmployee>();
            BaseUser _currUser = NewObject<BaseUser>();
            List<BaseGroup> _grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
            List<BaseDept> _deptlist = NewObject<BaseDept>().getlist<BaseDept>();

            _currUser.IsAdmin = 1;

            return ToJson(_currEmp, _currUser, _grouplist, _deptlist);
        }

    }
}

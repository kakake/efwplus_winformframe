using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.WcfFrame.ClientController;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.IView.EmpUserManager;
using DevComponents.DotNetBar;


namespace WinMainUIFrame.Winform.Controller
{
    [WinformController(DefaultViewName = "FrmDeptEmp")]//与系统菜单对应
    [WinformView(Name = "FrmDeptEmp", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.EmpUserManager.FrmDeptEmp")]
    [WinformView(Name = "frmAddUser", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.EmpUserManager.frmAddUser")]
    [WinformView(Name = "frmWorker", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.EmpUserManager.frmWorker")]
    public class wcfclientEmpUserController : JsonWcfClientController
    {
        IfrmDeptEmp frmDeptEmp;
        IfrmWorker frmWorker;
        public override void Init()
        {
            frmDeptEmp = (IfrmDeptEmp)iBaseView["FrmDeptEmp"];
            frmWorker = (IfrmWorker)iBaseView["frmWorker"];
        }
        [WinformMethod]
        public void LoadDeptData()
        {
            Object retdata = InvokeWCFService("EmpUserController", "LoadDeptData");

            List<BaseDeptLayer> layerlist = ToListObj<BaseDeptLayer>(ToArray(retdata)[0]);
            List<BaseDept> deptlist = ToListObj<BaseDept>(ToArray(retdata)[1]);
            frmDeptEmp.loadDeptTree(layerlist, deptlist);
        }
        [WinformMethod]
        public BaseDeptLayer SaveDeptLayer(int layerId, string layername, int pId)
        {
            Object retdata = InvokeWCFService("EmpUserController", "LoadDeptData",ToJson(layerId,layername,pId));
            return ToObject<BaseDeptLayer>(retdata);
        }
        [WinformMethod]
        public void DeleteDeptLayer(int layerId)
        {
            InvokeWCFService("EmpUserController", "DeleteDeptLayer", ToJson(layerId));
        }
        [WinformMethod]
        public BaseDept SaveDept(int deptId,string deptname,int layerId)
        {
            Object retdata = InvokeWCFService("EmpUserController", "SaveDept", ToJson(deptId, deptname, layerId));
            return ToObject<BaseDept>(retdata); ;
        }
        [WinformMethod]
        public void DeleteDept(int deptId)
        {
            Object retdata = InvokeWCFService("EmpUserController", "DeleteDept", ToJson(deptId));
        }
        [WinformMethod]
        public void LoadUserData(int[] deptIds)
        {
            Object retdata = InvokeWCFService("EmpUserController", "LoadUserData", ToJson(deptIds));
            DataTable dt = ToDataTable(retdata);
            frmDeptEmp.loadUserGrid(dt);
        }
        [WinformMethod]
        public void LoadUserData_Key(string key)
        {
            Object retdata = InvokeWCFService("EmpUserController", "LoadUserData_Key", ToJson(key));
            DataTable dt = ToDataTable(retdata);
            frmDeptEmp.loadUserGrid(dt);
        }
        [WinformMethod]
        public DialogResult NewUser()
        {
            Object retdata = InvokeWCFService("EmpUserController", "NewUser");
            BaseEmployee _currEmp = ToObject<BaseEmployee>(ToArray(retdata)[0]);
            BaseUser _currUser = ToObject<BaseUser>(ToArray(retdata)[1]);
            List<BaseGroup> _grouplist = ToListObj<BaseGroup>(ToArray(retdata)[2]);
            List<BaseDept> _deptlist = ToListObj<BaseDept>(ToArray(retdata)[3]);
            ((IfrmAddUser)iBaseView["frmAddUser"]).loadAddUserView(_currEmp, -1, _currUser, _grouplist, _deptlist, null, null);
            (iBaseView["frmAddUser"] as Form).Text = "新增用户";
            return (iBaseView["frmAddUser"] as Form).ShowDialog();
        }
        [WinformMethod]
        public void AlterUser(int empid, int userid)
        {
            Object retdata = InvokeWCFService("EmpUserController", "AlterUser",ToJson(empid,userid));
            BaseEmployee _currEmp = ToObject<BaseEmployee>(ToArray(retdata)[0]);
            int currDeptId = Convert.ToInt32(ToArray(retdata)[1]);

            BaseUser _currUser = ToObject<BaseUser>(ToArray(retdata)[2]);
            List<BaseGroup> _grouplist = ToListObj<BaseGroup>(ToArray(retdata)[3]);
            List<BaseDept> _deptlist = ToListObj<BaseDept>(ToArray(retdata)[4]);

            List<BaseGroup> _usergroup = ToListObj<BaseGroup>(ToArray(retdata)[5]);
            List<BaseDept> _empdept = ToListObj<BaseDept>(ToArray(retdata)[6]);

            BaseDept currdept = ToObject<BaseDept>(ToArray(retdata)[7]);

            ((IfrmAddUser)iBaseView["frmAddUser"]).loadAddUserView(_currEmp, currDeptId, _currUser, _grouplist, _deptlist, _usergroup, _empdept);

            (iBaseView["frmAddUser"] as Form).Text = "修改用户";
            (iBaseView["frmAddUser"] as Form).ShowDialog();
        }
        [WinformMethod]
        public void SaveUser()
        {
            BaseEmployee emp = (iBaseView["frmAddUser"] as IfrmAddUser).currEmp;
            BaseUser user = (iBaseView["frmAddUser"] as IfrmAddUser).currUser;
            List<int> currEmpDeptList = (iBaseView["frmAddUser"] as IfrmAddUser).currEmpDeptList;
            int currDefaultEmpDept = (iBaseView["frmAddUser"] as IfrmAddUser).currDefaultEmpDept;
            List<int> currGroupUserList = (iBaseView["frmAddUser"] as IfrmAddUser).currGroupUserList;
            Object retdata = InvokeWCFService("EmpUserController", "SaveUser",ToJson(emp,user,currEmpDeptList,currDefaultEmpDept,currGroupUserList));
            if (ToBoolean(retdata) == false)
            {
                throw new Exception("该用户名已存在！");
            }
        }
        [WinformMethod]
        public void ResetUserPass(int userId)
        {
            Object retdata = InvokeWCFService("EmpUserController", "ResetUserPass", ToJson(userId));
        }
        [WinformMethod]
        public void LoadWorkerList()
        {
            Object retdata = InvokeWCFService("EmpUserController", "LoadWorkerList");
            List<BaseWorkers> workerlist = ToListObj<BaseWorkers>(retdata);
            frmWorker.loadWorkerGrid(workerlist);
        }
        [WinformMethod]
        public void GetCurrWorker(int workId)
        {
            Object retdata = InvokeWCFService("EmpUserController", "GetCurrWorker",ToJson(workId));
            BaseWorkers worker = ToObject<BaseWorkers>(retdata);
            frmWorker.currWorker = worker;
        }
        [WinformMethod]
        public void NewWorker()
        {
            Object retdata = InvokeWCFService("EmpUserController", "NewWorker");
            BaseWorkers worker = ToObject<BaseWorkers>(retdata);
            frmWorker.currWorker = worker;
        }
        [WinformMethod]
        public void SaveWorker()
        {
            BaseWorkers worker = frmWorker.currWorker;
            Object retdata = InvokeWCFService("EmpUserController", "SaveWorker",ToJson(worker));
            frmWorker.currWorker = ToObject<BaseWorkers>(retdata);
        }

        //启用禁用机构，先判读注册码是否正确
        [WinformMethod]
        public void TurnOnOffWorker(int workId)
        {
            Object retdata = InvokeWCFService("EmpUserController", "TurnOnOffWorker", ToJson(workId));
            BaseWorkers worker = ToObject<BaseWorkers>(ToArray(retdata)[0]);
            string msg = ToArray(retdata)[1].ToString();
            MessageBoxEx.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            frmWorker.currWorker = worker;
        }
    }
}

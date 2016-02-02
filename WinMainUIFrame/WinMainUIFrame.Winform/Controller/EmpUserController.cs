using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EFWCoreLib.CoreFrame.Business.AttributeInfo;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.IView.EmpUserManager;
using WinMainUIFrame.ObjectModel.UserLogin;
using WinMainUIFrame.ObjectModel.RightManager;
using EfwControls.CustomControl;

namespace WinMainUIFrame.Winform.Controller
{
    [WinformController(DefaultViewName = "FrmDeptEmp")]//与系统菜单对应
    [WinformView(Name = "FrmDeptEmp", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.EmpUserManager.FrmDeptEmp")]
    [WinformView(Name = "frmAddUser", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.EmpUserManager.frmAddUser")]
    [WinformView(Name = "frmWorker", DllName = "WinMainUIFrame.Winform.dll", ViewTypeName = "WinMainUIFrame.Winform.ViewForm.EmpUserManager.frmWorker")]
    public class EmpUserController : WinformController
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
            List<BaseDeptLayer> layerlist = NewObject<BaseDeptLayer>().getlist<BaseDeptLayer>();
            List<BaseDept> deptlist = NewObject<BaseDept>().getlist<BaseDept>();
            frmDeptEmp.loadDeptTree(layerlist, deptlist);
        }
        [WinformMethod]
        public BaseDeptLayer SaveDeptLayer(int layerId, string layername, int pId)
        {
            BaseDeptLayer layer = NewObject<BaseDeptLayer>();
            layer.LayerId = layerId;
            layer.Name = layername;
            layer.PId = pId;
            layer.save();
            return layer;
        }
        [WinformMethod]
        public void DeleteDeptLayer(int layerId)
        {
            NewObject<BaseDeptLayer>().delete(layerId);
        }
        [WinformMethod]
        public BaseDept SaveDept(int deptId,string deptname,int layerId)
        {
            BaseDept dept = NewObject<BaseDept>();
            dept.DeptId = deptId;
            dept.Layer = layerId;
            dept.Name = deptname;
            if (deptname != null)
            {
                dept.Pym = SpellAndWbCode.GetSpellCode(deptname);
                dept.Wbm = SpellAndWbCode.GetWBCode(deptname);
            }
            dept.save();
            return dept;
        }
        [WinformMethod]
        public void DeleteDept(int deptId)
        {
            NewObject<BaseDept>().delete(deptId);
        }
        [WinformMethod]
        public void LoadUserData(int[] deptIds)
        {
           DataTable dt= NewObject<User>().GetUserData(deptIds);
           frmDeptEmp.loadUserGrid(dt);
        }
        [WinformMethod]
        public void LoadUserData_Key(string key)
        {
            DataTable dt = NewObject<User>().GetUserData(key);
            frmDeptEmp.loadUserGrid(dt);
        }
        [WinformMethod]
        public DialogResult NewUser()
        {
            BaseEmployee _currEmp = NewObject<BaseEmployee>();
            BaseUser _currUser = NewObject<BaseUser>();
            List<BaseGroup> _grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
            List<BaseDept> _deptlist = NewObject<BaseDept>().getlist<BaseDept>();

            ((IfrmAddUser)iBaseView["frmAddUser"]).loadAddUserView(_currEmp, -1, _currUser, _grouplist, _deptlist, null, null);
            (iBaseView["frmAddUser"] as Form).Text = "新增用户";
            return (iBaseView["frmAddUser"] as Form).ShowDialog();
        }
        [WinformMethod]
        public void AlterUser(int empid, int userid)
        {
            BaseEmployee _currEmp = (BaseEmployee)NewObject<BaseEmployee>().getmodel(empid);
            BaseUser _currUser = (BaseUser)NewObject<BaseUser>().getmodel(userid);
            List<BaseGroup> _grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
            List<BaseDept> _deptlist = NewObject<BaseDept>().getlist<BaseDept>();

            List<BaseGroup> _usergroup = NewObject<Group>().GetGroupList(userid);
            List<BaseDept> _empdept = NewObject<Dept>().GetHaveDept(empid);
            BaseDept currdept = NewObject<Dept>().GetDefaultDept(empid);

            ((IfrmAddUser)iBaseView["frmAddUser"]).loadAddUserView(_currEmp, currdept == null ? -1 : currdept.DeptId, _currUser, _grouplist, _deptlist, _usergroup, _empdept);

            (iBaseView["frmAddUser"] as Form).Text = "修改用户";
            (iBaseView["frmAddUser"] as Form).ShowDialog();
        }
        [WinformMethod]
        public void SaveUser()
        {
            BaseEmployee emp = (iBaseView["frmAddUser"] as IfrmAddUser).currEmp;
            BaseUser user = (iBaseView["frmAddUser"] as IfrmAddUser).currUser;

            if (emp.EmpId == 0)
            {
                if (NewObject<User>().GetUser(user.Code) != null)
                    throw new Exception("该用户名已存在！");
            }
            int[] deptid = (iBaseView["frmAddUser"] as IfrmAddUser).currEmpDeptList.ToArray();
            int defaultDeptId = (iBaseView["frmAddUser"] as IfrmAddUser).currDefaultEmpDept;
            int[] groupIds = (iBaseView["frmAddUser"] as IfrmAddUser).currGroupUserList.ToArray();
            NewObject<User>().SaveUser(emp, user, deptid, defaultDeptId, groupIds);
        }
        [WinformMethod]
        public void ResetUserPass(int userId)
        {
            NewObject<User>().ResetPassword(userId);
        }
        [WinformMethod]
        public void LoadWorkerList()
        {
            List<BaseWorkers> workerlist = NewObject<BaseWorkers>().getlist<BaseWorkers>();
            frmWorker.loadWorkerGrid(workerlist);
        }
        [WinformMethod]
        public void GetCurrWorker(int workId)
        {
            BaseWorkers worker = NewObject<BaseWorkers>().getmodel(workId) as BaseWorkers;
            frmWorker.currWorker = worker;
        }
        [WinformMethod]
        public void NewWorker()
        {
            BaseWorkers worker = NewObject<BaseWorkers>();
            worker.DelFlag = -1;//-1新建 0启用 1禁用
            frmWorker.currWorker = worker;
        }
        [WinformMethod]
        public void SaveWorker()
        {
            BaseWorkers worker = frmWorker.currWorker;
            worker.save();

            //修改必须重新启用
            if (worker.DelFlag == 0)
            {
                TurnOnOffWorker(worker.WorkId);
                worker.DelFlag = 1;
            }

            frmWorker.currWorker = worker;
        }

        //启用禁用机构，先判读注册码是否正确
        [WinformMethod]
        public void TurnOnOffWorker(int workId)
        {
            BaseWorkers worker = NewObject<BaseWorkers>().getmodel(workId) as BaseWorkers;


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

                    BaseEmployee _currEmp = NewObject<BaseEmployee>();
                    BaseUser _currUser = NewObject<BaseUser>();
                    List<BaseGroup> _grouplist = NewObject<BaseGroup>().getlist<BaseGroup>();
                    List<BaseDept> _deptlist = NewObject<BaseDept>().getlist<BaseDept>();

                    _currUser.IsAdmin = 1;
                    ((IfrmAddUser)iBaseView["frmAddUser"]).loadAddUserView(_currEmp, -1, _currUser, _grouplist, _deptlist, null, null);
                    (iBaseView["frmAddUser"] as Form).Text = "设置默认超级用户";
                    (iBaseView["frmAddUser"] as Form).ShowDialog();

                    if ((iBaseView["frmAddUser"] as DialogForm).IsOk)
                    {
                        //修改状态为0
                        worker.DelFlag = 0;
                        worker.save();
                        MessageBoxEx.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    oleDb.WorkId = LoginUserInfo.WorkId;
                }
                else
                {
                    MessageBoxEx.Show("机构注册码不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBoxEx.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBoxEx.Show("机构注册码不正确！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else//禁用
            {
                //修改状态为1
                worker.DelFlag = 1;
                worker.save();
                MessageBoxEx.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            frmWorker.currWorker = worker;
        }
    }
}

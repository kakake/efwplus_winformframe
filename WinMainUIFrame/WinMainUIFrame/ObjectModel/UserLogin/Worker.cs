using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using WinMainUIFrame.Entity;


namespace WinMainUIFrame.ObjectModel.UserLogin
{
    public class Worker : AbstractObjectModel
    {
        
        public void SaveWorker(BaseWorkers worker)
        {
            worker.save();
        }

        public BaseWorkers GetWorker(int workId)
        {
            return (BaseWorkers)NewObject<BaseWorkers>().getmodel(workId);
        }

        public List<BaseWorkers> GetWorkerList()
        {
            return NewObject<BaseWorkers>().getlist<BaseWorkers>();
        }

        /// <summary>
        /// 初始化机构
        /// </summary>
        /// <returns></returns>
        public  bool InitWorkerData()
        {
            //basic = NewDao<BasicDao>();
            ////1.增加科室分类
            //int playerId = basic.AddDeptLayer(0, "全部");
            //playerId = basic.AddDeptLayer(playerId, "信息科");
            ////2.增加信息科
            //AbstractDept dept = NewObject<AbstractDept>();
            //dept.PDeptId = playerId;
            ////dept.TypeCode = "00";
            //dept.Name = "信息科";
            //dept.Pym = "";
            //dept.Wbm = "";
            //dept.Szm = "";
            ////dept.DeptAddr = "";
            //dept.save();
            ////3.增加人员
            //AbstractEmployee employee = NewObject<AbstractEmployee>();
            //employee.Name = "超级用户";
            //employee.Pym = "";
            //employee.Wbm = "";
            //employee.Szm = "";
            //employee.save();
            ////4.增加人员科室对应关系
            //employee.AddEmployeeDeptRole(employee.EmpId, new int[] { dept.DeptId }, dept.DeptId);
            ////5.增加用户
            //AbstractUser user = NewObject<AbstractUser>();
            //user.EmpId = employee.EmpId;
            //user.Code = "Admin"+WorkId.ToString();
            ////加密
            //DESEncryptor des = new DESEncryptor();
            //des.InputString = "1";
            //des.DesEncrypt();

            //user.PassWord = des.OutString;
            //user.save();
            
            //6.增加用户超级权限
            //int groupAdminId=basic.GetGroupAdmin();
            //user.AddGroupUser(user.UserId, new int[] { groupAdminId });
            return true;
        }

        /// <summary>
        /// 禁用机构
        /// </summary>
        /// <param name="workId"></param>
        public void DisabledWorker(int workId)
        {

        }
        /// <summary>
        /// 启用机构
        /// </summary>
        /// <param name="workId"></param>
        public void StartWorker(int workId)
        {

        }
    }
}

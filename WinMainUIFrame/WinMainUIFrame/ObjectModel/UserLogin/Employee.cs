using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.DbProvider.SqlPagination;
using WinMainUIFrame.Dao;

namespace WinMainUIFrame.ObjectModel.UserLogin
{
    public class Employee : AbstractObjectModel
    {

        public System.Data.DataTable GetEmpUserData(int[] deptId,string empName, PageInfo page)
        {
            return null;
        }

        /// <summary>
        /// 设置用户默认科室
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="defaultDeptId"></param>
        public void SetDefaultEmpDeptRole(int EmpId,int defaultDeptId)
        {
            NewDao<UserDao>().SetDefaultEmpDeptRole(EmpId, defaultDeptId);
        }

        /// <summary>
        /// 设置用户所管辖科室
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="deptId"></param>
        public void SetHaveEmpDeptRole(int EmpId, int[] deptId)
        {
            NewDao<UserDao>().SetHaveEmpDeptRole(EmpId, deptId);
        }

        public bool IsAdmin(int empId)
        {
            return NewDao<UserDao>().IsAdmin(empId);
        }


    }
}

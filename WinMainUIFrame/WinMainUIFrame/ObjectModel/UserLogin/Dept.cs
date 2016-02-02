using System;
using System.Collections.Generic;
using EFWCoreLib.CoreFrame.Business;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Dao;

namespace WinMainUIFrame.ObjectModel.UserLogin
{
    [Serializable]
    public class Dept : AbstractObjectModel
    {

        public void SaveDeptLayer(BaseDeptLayer layer)
        {
            layer.save();
        }

        public void DeleteDeptLayer(int LayerId)
        {
            NewObject<BaseDeptLayer>().delete(LayerId);
        }
        
        public System.Data.DataTable GetDeptLayers()
        {
            return NewObject<BaseDeptLayer>().gettable();
        }


        public void SaveDept(BaseDept dept)
        {
            dept.save();
        }
        /// <summary>
        /// 停用科室
        /// </summary>
        /// <param name="deptId"></param>
        public void StopDept(int deptId)
        {

        }

        public BaseDept GetDept(int deptId)
        {
            return (BaseDept)NewObject<BaseDept>().getmodel(deptId);
        }

        public System.Data.DataTable SearchDeptData(params object[] values)
        {
            return null;
        }


        /// <summary>
        /// 用户默认科室
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public BaseDept GetDefaultDept(int empId)
        {
            return NewDao<UserDao>().GetDefaultDept(empId);
        }

        /// <summary>
        /// 用户拥有的科室
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public List<BaseDept> GetHaveDept(int empId)
        {
            if (NewDao<UserDao>().IsAdmin(empId))
            {
                return NewObject<BaseDept>().getlist<BaseDept>();
            }
            else
            {
                return NewDao<UserDao>().GetHaveDept(empId);
            }
        }

    }
}

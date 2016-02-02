using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.DbProvider.SqlPagination;
using WinMainUIFrame.Dao;
using WinMainUIFrame.Entity;


namespace WinMainUIFrame.ObjectModel.UserLogin
{
    [Serializable]
    public class User : AbstractObjectModel
    {
        public bool UserLogin(string code, string password)
        {
            string despass = ConvertExtend.ToPassWord(password);
            return NewDao<UserDao>().Login(code, despass);
        }

        public BaseUser GetUser(string code)
        {
            return NewDao<UserDao>().GetUser(code);
        }

        public bool IsAdmin(int userId)
        {
             BaseUser user=(BaseUser)NewObject<BaseUser>().getmodel(userId);
             return user.IsAdmin == 1 ? true : false;
        }

        public bool AlterPassWrod(int userId, string oldpassword, string password)
        {
            string oldpass = ConvertExtend.ToPassWord(oldpassword);
            string newpass = ConvertExtend.ToPassWord(password);
            return NewDao<UserDao>().AlterPassWrod(userId, oldpass, newpass);
        }

        public void ResetPassword(int userId)
        {
            string pass= ConvertExtend.ToPassWord("1");
            NewDao<UserDao>().ResetPassword(userId, pass);
        }

        public DataTable GetUserData(int[] deptIds)
        {
            if (deptIds.Length == 0) return null;
            return NewDao<UserDao>().GetUserData(deptIds);
        }

        public DataTable GetUserData(string key)
        {
            return NewDao<UserDao>().GetUserData(key);
        }

        public bool SetGroupUserRole(int userId, int[] groupIds)
        {
            return NewDao<UserDao>().SetGroupUserRole(userId, groupIds);
        }

        public void SaveUser(BaseEmployee emp, BaseUser user, int[] deptId, int defaultDeptId, int[] groupIds)
        {
            emp.Pym = SpellAndWbCode.GetSpellCode(emp.Name);
            emp.Wbm = SpellAndWbCode.GetWBCode(emp.Name);
            emp.Brithday = DateTime.Now;
            emp.Szm = "";
            emp.save();
            user.EmpId = emp.EmpId;
            user.PassWord = ConvertExtend.ToPassWord("1");
            user.Memo = "";
            user.save();

            NewObject<Employee>().SetHaveEmpDeptRole(emp.EmpId, deptId);
            NewObject<Employee>().SetDefaultEmpDeptRole(emp.EmpId, defaultDeptId);
            NewObject<User>().SetGroupUserRole(user.UserId, groupIds);

        }
    }
}

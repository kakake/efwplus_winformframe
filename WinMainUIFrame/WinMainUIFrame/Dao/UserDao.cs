using System;
using System.Collections.Generic;
using System.Data;
using WinMainUIFrame.Entity;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.DbProvider.SqlPagination;
using EFWCoreLib.CoreFrame.Business;

namespace WinMainUIFrame.Dao
{
    public class UserDao : AbstractDao
    {
        public bool Login(string code,string pass)
        {
            string strsql = @"select count(1) from BaseUser where Code='{0}' and PassWord='{1}'";
            strsql = string.Format(strsql, code, pass);
            int ret = Convert.ToInt32(oleDb.GetDataResult(strsql));
            return ret > 0;
        }

        public BaseUser GetUser(string code)
        {
            string strsql = @"select * from BaseUser where Code='{0}'";
            strsql = string.Format(strsql, code);
            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToObject<BaseUser>(dt, 0, oleDb, GetUnityContainer(),_cache,_pluginName, null);
            else
                return null;
        }

        public bool IsAdmin(int empId)
        {
            string strsql = @"select IsAdmin from BaseUser where EmpId=" + empId;
            int ret = Convert.ToInt32(oleDb.GetDataResult(strsql));
            return ret == 1 ? true : false;
        }

        public List<BaseDept> GetHaveDept(int empId)
        {
            string strsql = @"select distinct b.* from BaseEmpDept a left join BaseDept b on a.DeptId=b.DeptId where a.EmpId=" + empId;
            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToList<BaseDept>(dt, oleDb, GetUnityContainer(), _cache, _pluginName,null);
            else
                return new List<BaseDept>();
        }

        public List<BaseModule> GetModuleList(int userId)
        {
            string strsql = @"select distinct c.* from BaseGroupUser a left join BaseGroupMenu b on a.GroupId=b.GroupId
                                            left join BaseModule c on b.ModuleId=c.ModuleId
                                            where a.UserId=" +userId;

            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToList<BaseModule>(dt, oleDb, GetUnityContainer(), _cache, _pluginName,null);
            else
                return new List<BaseModule>();
        }

        public List<BaseMenu> GetMenuList(int userId)
        {
            string strsql = @"select distinct c.* from BaseGroupUser a left join BaseGroupMenu b on a.GroupId=b.GroupId
                                            left join BaseMenu c on b.MenuId=c.MenuId
                                            where a.UserId=" + userId;

            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToList<BaseMenu>(dt, oleDb, GetUnityContainer(), _cache, _pluginName,null);
            else
                return new List<BaseMenu>();
        }

        public List<BaseMenu> GetOutLookMenuList(int userId)
        {
            string strsql = @"select distinct c.* from BaseGroupUser a left join BaseGroupMenu b on a.GroupId=b.GroupId
                                            left join BaseMenu c on b.MenuId=c.MenuId
                                            where c.MenuLookBar=1 and a.UserId=" + userId;

            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToList<BaseMenu>(dt, oleDb, GetUnityContainer(), _cache, _pluginName,null);
            else
                return new List<BaseMenu>();
        }

        public List<BaseMenu> GetGroupMenuList(int groupId)
        {
            string strsql = @"select distinct c.* from  BaseGroupMenu b  left join BaseMenu c on b.MenuId=c.MenuId
                                            where b.GroupId=" + groupId;

            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToList<BaseMenu>(dt, oleDb, GetUnityContainer(), _cache, _pluginName, null);
            else
                return new List<BaseMenu>();
        }

        public BaseDept GetDefaultDept(int empId)
        {
            string strsql = @"select distinct b.* from BaseEmpDept a left join basedept b on a.deptid=b.deptid where a.DefaultFlag=1 and a.EmpId=" + empId;

            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToObject<BaseDept>(dt, 0, oleDb, GetUnityContainer(), _cache, _pluginName, null);
            else
                return null;
        }

        public bool AlterPassWrod(int userId, string oldpassword, string password)
        {
            string strsql = @"update BaseUser set PassWord='" + password + "' where UserId=" + userId + " and PassWord='" + oldpassword + "'";
            return oleDb.DoCommand(strsql) > 0;
        }

        public DataTable GetUserData(int[] deptIds)
        {
            string strdeptid = "";
            foreach (int id in deptIds)
            {
                if (strdeptid == "") strdeptid += id.ToString();
                else strdeptid += "," + id.ToString();
            }
            string strsql = @"select a.empid,c.userid,c.Code,b.Name,b.sex,a.DeptId,c.Lock from
                                BaseEmpDept a left join BaseEmployee b on a.EmpId=b.EmpId
                                left join BaseUser c on a.EmpId=c.EmpId
                                where a.DeptId in ({0}) and a.DefaultFlag=1 and c.workId={1}";
            strsql = string.Format(strsql, strdeptid, oleDb.WorkId).ToLower();

            return oleDb.GetDataTable(strsql);
        }

        public DataTable GetUserData(string key)
        {
            string strsql = @"select a.empid,b.userid,b.Code,a.Name,a.sex,c.DeptId,b.Lock from 
                                BaseEmployee a left join BaseUser b on a.EmpId=b.EmpId
                                left join BaseEmpDept c on a.EmpId=c.EmpId and c.DefaultFlag=1
                                where (b.code like '%{0}%' or a.name like '%{0}%' or a.pym like '%{0}%' or a.wbm like '%{0}%' or a.szm like '%{0}%') and a.workId={1}";
            strsql = string.Format(strsql, key, oleDb.WorkId);
            return oleDb.GetDataTable(strsql);
        }

        public DataTable GetUserData(string key, PageInfo page)
        {
            string strsql = @"select a.empid,b.userid,b.Code,a.Name,a.sex,c.DeptId,b.Lock from 
                                BaseEmployee a left join BaseUser b on a.EmpId=b.EmpId
                                left join BaseEmpDept c on a.EmpId=c.EmpId and c.DefaultFlag=1
                                where (b.code like '%{0}%' or a.name like '%{0}%' or a.pym like '%{0}%' or a.wbm like '%{0}%' or a.szm like '%{0}%') and a.workId={1}";
            strsql = string.Format(strsql, key, oleDb.WorkId);
            strsql = SqlPage.FormatSql(strsql, page, oleDb);
            return oleDb.GetDataTable(strsql);
        }

        public DataTable GetUserData(int groupId, string key, PageInfo page)
        {
            string strsql = @"select a.empid,b.userid,b.Code,a.Name,a.sex,c.DeptId,b.Lock from 
                                        BaseEmployee a 
                                        left join BaseUser b on a.EmpId=b.EmpId
                                        left join BaseEmpDept c on a.EmpId=c.EmpId and c.DefaultFlag=1
                                        left join BaseGroupUser d ON b.UserId=d.UserId
                                        WHERE  (b.code like '%{0}%' or a.name like '%{0}%' or a.pym like '%{0}%' or a.wbm like '%{0}%' or a.szm like '%{0}%') and a.workId={1}
                                        AND d.GroupId={2}";
            strsql = string.Format(strsql, key, oleDb.WorkId,groupId);
            strsql = SqlPage.FormatSql(strsql, page, oleDb);
            return oleDb.GetDataTable(strsql);
        }

        public DataTable GetUserData(int[] deptIds, string key, PageInfo page)
        {
            string strdeptid = "";
            foreach (int id in deptIds)
            {
                if (strdeptid == "") strdeptid += id.ToString();
                else strdeptid += "," + id.ToString();
            }

            string strsql = @"
select 
    a.empid,b.userid,b.Code,a.Name,a.sex,c.DeptId,b.Lock,
(CASE Sex WHEN -1 THEN '未知性别' WHEN 0 THEN '男性' ELSE '女性' END) AS SexText,
(CASE Lock WHEN 0 THEN '否' ELSE '是' END) AS LockText,D. Name AS DeptName
from BaseEmployee a 
    left join BaseUser b on a.EmpId=b.EmpId
    left join BaseEmpDept c on a.EmpId=c.EmpId and c.DefaultFlag=1
    LEFT JOIN BaseDept D ON C.DeptId = D.DeptId
where 
    (c.DeptId in ({0}) or {0}=0) 
    and (b.code like '%{1}%' or a.name like '%{1}%' or a.pym like '%{1}%' or a.wbm like '%{1}%' or a.szm like '%{1}%') 
    and a.workId={2}";
            strsql = string.Format(strsql, strdeptid, key, oleDb.WorkId);
            strsql = SqlPage.FormatSql(strsql, page, oleDb);
            return oleDb.GetDataTable(strsql);
        }

        public void ResetPassword(int userId,string password)
        {
            string strsql = @"update baseuser set PassWord='{0}' where userid={1}";
            strsql = string.Format(strsql, password, userId);
            oleDb.DoCommand(strsql);
        }

        public bool SetGroupUserRole(int userId, int[] groupIds)
        {
            string strsql = @"delete from BaseGroupUser where UserId=" + userId;
            oleDb.DoCommand(strsql);
            for (int i = 0; i < groupIds.Length; i++)
            {
                strsql = @"insert into BaseGroupUser(UserId,GroupId,WorkId)
                                                values({0},{1},{2})";
                strsql = String.Format(strsql, userId, groupIds[i], oleDb.WorkId);
                oleDb.DoCommand(strsql);
            }
            return true;
        }

        public void SetHaveEmpDeptRole(int EmpId, int[] deptId)
        {
            string strsql = @"delete from BaseEmpDept where EmpId=" + EmpId;
            oleDb.DoCommand(strsql);

            for (int i = 0; i < deptId.Length; i++)
            {
                strsql = @"insert into BaseEmpDept(EmpId,DefaultFlag,DeptId,WorkId)
                                                values({0},{1},{2},{3})";
                strsql = String.Format(strsql, EmpId, 0, deptId[i], oleDb.WorkId);
                oleDb.DoCommand(strsql);
            }
        }

        public void SetDefaultEmpDeptRole(int EmpId, int defaultDeptId)
        {
            string strsql = @"delete from BaseEmpDept where EmpId=" + EmpId + " and DeptId=" + defaultDeptId;
            oleDb.DoCommand(strsql);

            strsql = @"insert into BaseEmpDept(EmpId,DefaultFlag,DeptId,WorkId)
                                                values({0},{1},{2},{3})";
            strsql = String.Format(strsql, EmpId, 1, defaultDeptId, oleDb.WorkId);
            oleDb.DoCommand(strsql);
        }
    }
}

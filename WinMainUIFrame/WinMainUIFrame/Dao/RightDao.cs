using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EFWCoreLib.CoreFrame.Common;
using EFWCoreLib.CoreFrame.Business;
using WinMainUIFrame.Entity;

namespace WinMainUIFrame.Dao
{
    public class RightDao : AbstractDao
    {
        public void DeleteGroupMenu(int groupId)
        {
            string strsql = @"delete from BaseGroupMenu where GroupId=" + groupId + "";
            oleDb.DoCommand(strsql);
        }

        public int GetMenuPId(int menuId)
        {
            string strsql = @"select PMenuId from BaseMenu where MenuId=" + menuId;
            return Convert.ToInt32(oleDb.GetDataResult(strsql));
        }

        public bool GetIsGroupMenu(int groupid, int menuId)
        {
            string strsql = @"select count(*) from BaseGroupMenu where GroupId=" + groupid + " and MenuId=" + menuId;
            return Convert.ToInt32(oleDb.GetDataResult(strsql)) > 0;
        }

        public void SaveUserWorkGroup(int Groupid, int menuid)
        {

            string strsql = @"select ModuleId from BaseMenu where MenuId=" + menuid;
            int moduleid = Convert.ToInt32(oleDb.GetDataResult(strsql));

            BaseGroupMenu groupmenu = NewObject<BaseGroupMenu>();
            groupmenu.GroupId = Groupid;
            groupmenu.ModuleId = moduleid;
            groupmenu.MenuId = menuid;
            groupmenu.save();

        }

        public List<BaseGroup> GetGroupList(int userId)
        {
            string strsql = @"select b.* from BaseGroupUser a left join BaseGroup b on a.GroupId=b.GroupId 
                                where a.UserId="+userId;
            DataTable dt = oleDb.GetDataTable(strsql);
            if (dt.Rows.Count > 0)
                return ConvertExtend.ToList<BaseGroup>(dt, oleDb, GetUnityContainer(), _cache, _pluginName, null);
            else
                return new List<BaseGroup>();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Dao;


namespace WinMainUIFrame.ObjectModel.RightManager
{
    [Serializable]
    public class Group : AbstractObjectModel
    {

        public List<BaseGroup> GetGroupList(int userId)
        {
            return NewDao<RightDao>().GetGroupList(userId);
        }

        public void SetGroupMenu(int groupId,int[] menuIds)
        {
            NewDao<RightDao>().DeleteGroupMenu(groupId);
            for (int i = 0; i < menuIds.Length; i++)
            {
                AddPmenuId(groupId, menuIds[i]);
                NewDao<RightDao>().SaveUserWorkGroup(groupId, menuIds[i]);
            }
        }


        private void AddPmenuId(int GroupId, int menuid)//递归添加父菜单
        {
            int Pid = NewDao<RightDao>().GetMenuPId(menuid);//获取父菜单ID
            if (Pid != -1 && NewDao<RightDao>().GetIsGroupMenu(GroupId, Pid) == false)
            {
                NewDao<RightDao>().SaveUserWorkGroup(GroupId, Pid);//保存当条菜单
                AddPmenuId(GroupId, Pid);
            }
        }

    }
}

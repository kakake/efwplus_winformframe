
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EFWCoreLib.CoreFrame.Business;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.UserLogin;
using WinMainUIFrame.Dao;

namespace WinMainUIFrame.ObjectModel.RightManager
{
    [Serializable]
    public class Menu : AbstractObjectModel
    {
          
        public List<BaseMenu> GetMenuList(int userId)
        {
            if (NewObject<User>().IsAdmin(userId))
            {
                return NewObject<BaseMenu>().getlist<BaseMenu>();
            }
            else
            {
                return NewDao<UserDao>().GetMenuList(userId);
            }
        }

        public List<BaseMenu> GetOutLookMenuList(int userId)
        {
            if (NewObject<User>().IsAdmin(userId))
            {
                return NewObject<BaseMenu>().getlist<BaseMenu>("MenuLookBar=1");
            }
            else
            {
                return NewDao<UserDao>().GetOutLookMenuList(userId);
            }
        }

        public List<BaseMenu> GetGroupMenuList(int groupId)
        {
            return NewObject<UserDao>().GetGroupMenuList(groupId);
        }


        public bool SortUp(int menuId)
        {
            throw new NotImplementedException();
        }

        public bool SortDown(int menuId)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable ExecMenuBindSQL(int menuId)
        {
            throw new NotImplementedException();
        }
    }
}

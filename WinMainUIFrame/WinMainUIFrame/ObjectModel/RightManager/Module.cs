
//==================================================
// 作 者：曾浩
// 日 期：2011/03/06
// 描 述：介绍本文件所要完成的功能以及背景信息等等
//==================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.CoreFrame.Business;
using WinMainUIFrame.Dao;
using WinMainUIFrame.Entity;
using WinMainUIFrame.ObjectModel.UserLogin;

namespace WinMainUIFrame.ObjectModel.RightManager
{
    [Serializable]
    public class Module : AbstractObjectModel
    {

        public List<BaseModule> GetModuleList(int userId)
        {
            if (NewObject<User>().IsAdmin(userId))
            {
                return NewObject<BaseModule>().getlist<BaseModule>();
            }
            else
            {
                return NewDao<UserDao>().GetModuleList(userId);
            }
        }

        public bool SortUp(int moduleid)
        {
            throw new NotImplementedException();
        }

        public bool SortDown(int moduleid)
        {
            throw new NotImplementedException();
        }
    }
}

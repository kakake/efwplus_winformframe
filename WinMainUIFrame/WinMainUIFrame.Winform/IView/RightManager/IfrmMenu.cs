using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;

namespace WinMainUIFrame.Winform.IView.RightManager
{
    public interface IfrmMenu:IBaseView
    {
        void loadMenuTree(List<BaseModule> moduleList,List<BaseMenu> menuList);
        BaseMenu currentMenu{get;set;}

        int selectMenuId { get; }
        int selectModuleId { get; }
    }
}

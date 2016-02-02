using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;

namespace WinMainUIFrame.Winform.IView
{
    public interface IfrmReSetDept:IBaseView
    {
        string UserName { set; }
        string WorkName { set; }
        void loadDepts(List<BaseDept> list, int selectDeptId);
        BaseDept getDept();
    }
}

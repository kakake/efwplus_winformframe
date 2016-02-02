using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;
using System.Data;

namespace WinMainUIFrame.Winform.IView.EmpUserManager
{
    public interface IfrmDeptEmp:IBaseView
    {
        void loadDeptTree(List<BaseDeptLayer> layerList, List<BaseDept> deptList);
        void loadUserGrid(DataTable dt);
    }
}

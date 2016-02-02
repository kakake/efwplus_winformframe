using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;

namespace WinMainUIFrame.Winform.IView.EmpUserManager
{
    public interface IfrmAddUser : IBaseView
    {
        BaseEmployee currEmp { get; }
        BaseUser currUser { get; }
        List<int> currGroupUserList { get; }
        int currDefaultEmpDept { get; }
        List<int> currEmpDeptList { get; }

        void loadAddUserView(BaseEmployee _currEmp, int _currdeptId, BaseUser _currUser, List<BaseGroup> _groupList, List<BaseDept> _deptList, List<BaseGroup> _userGroupId, List<BaseDept> _empDeptId);
    }
}

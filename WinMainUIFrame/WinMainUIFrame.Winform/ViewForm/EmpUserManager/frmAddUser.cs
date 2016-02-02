using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinMainUIFrame.Entity;
using EFWCoreLib.CoreFrame.Common;
using WinMainUIFrame.Winform.IView.EmpUserManager;
using EfwControls.CustomControl;

namespace WinMainUIFrame.Winform.ViewForm.EmpUserManager
{
    public partial class frmAddUser : DialogForm, IfrmAddUser
    {
        public frmAddUser()
        {
            InitializeComponent();
            frmForm1.AddItem(this.textBoxX1, "Name", "请输入用户姓名");
            frmForm1.AddItem(this.radioButton1, "Sex");
            frmForm1.AddItem(this.radioButton2, "Sex");
            frmForm1.AddItem(this.textShowCard1, "deptid", "请选择所在科室");
            frmForm1.AddItem(this.textBoxX2, "Code", "请输入用户名");
            frmForm1.AddItem(this.checkBox1, "Lock");
        }

        #region IfrmAddUser 成员
        private BaseEmployee emp;
        public BaseEmployee currEmp
        {
            get
            {
                frmForm1.GetValue<BaseEmployee>(emp);
                return emp;
            }
        }
        private BaseUser user;
        public BaseUser currUser
        {
            get
            {
                frmForm1.GetValue<BaseUser>(user);
                return user;
            }
        }
        private List<int> groupuserlist;
        public List<int> currGroupUserList
        {
            get
            {
                groupuserlist = new List<int>();
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i)==true)
                    {
                        groupuserlist.Add(groupList[i].GroupId);
                    }
                }
                return groupuserlist;
            }
        }

        public int currDefaultEmpDept
        {
            get { return Convert.ToInt32(textShowCard1.MemberValue); }
        }


        private List<int> empdeptlist;
        public List<int> currEmpDeptList
        {
            get
            {
                empdeptlist = new List<int>();
                for (int i = 0; i < checkedListBox2.Items.Count; i++)
                {
                    if (checkedListBox2.GetItemChecked(i) == true)
                    {
                        empdeptlist.Add(deptList[i].DeptId);
                    }
                }
                return empdeptlist;
            }
        }

        private List<BaseGroup> groupList;
        private List<BaseDept> deptList;

        public void loadAddUserView(BaseEmployee _currEmp, int _currdeptId, BaseUser _currUser, List<BaseGroup> _groupList, List<BaseDept> _deptList, List<BaseGroup> _userGroupId, List<BaseDept> _empDeptId)
        {
            frmForm1.Clear();

            emp = _currEmp;
            user = _currUser;
            groupList = _groupList;
            deptList = _deptList;
            frmForm1.Load<BaseEmployee>(emp);
            frmForm1.Load<BaseUser>(user);


            this.textShowCard1.ShowCardDataSource = ConvertExtend.ToDataTable(_deptList);
            textShowCard1.MemberValue = _currdeptId;

            this.checkedListBox1.Items.Clear();
            foreach (BaseGroup val in _groupList)
            {
                checkedListBox1.Items.Add(val.Name);
            }

            this.checkedListBox2.Items.Clear();
            foreach (BaseDept val in _deptList)
            {
                checkedListBox2.Items.Add(val.Name);
            }

            if (_userGroupId != null)
                foreach (BaseGroup val in _userGroupId)
                {
                    checkedListBox1.SetItemChecked(_groupList.FindIndex(x => x.GroupId == val.GroupId), true);
                }
            if (_empDeptId != null)
                foreach (BaseDept val in _empDeptId)
                {
                    checkedListBox2.SetItemChecked(_deptList.FindIndex(x => x.DeptId == val.DeptId), true);
                }
        }

        #endregion


        private void frmAddUser_SaveEventHandler(object sender, EventArgs e)
        {
            InvokeController(this.SaveFunName);
        }
    }
}

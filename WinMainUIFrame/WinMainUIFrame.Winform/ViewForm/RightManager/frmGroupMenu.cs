using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using EfwControls.CustomControl;
using EFWCoreLib.WinformFrame.Controller;
using WinMainUIFrame.Entity;
using WinMainUIFrame.Winform.IView.RightManager;

namespace WinMainUIFrame.Winform.ViewForm.RightManager
{
    public partial class frmGroupMenu : BaseFormEx, IfrmGroupMenu
    {
        public frmGroupMenu()
        {
            InitializeComponent();
        }

        #region IfrmGroupMenu 成员

        public void loadGroupGrid(List<BaseGroup> groupList)
        {
            this.dataGrid1.DataSource = null;
            this.dataGrid1.DataSource = groupList;
        }

        private void recursionMenu(List<BaseMenu> allmenu, int pmenuId, TreeNode pNode, List<BaseMenu> groupmenuList)
        {
            List<BaseMenu> _menulist = allmenu.FindAll(x => x.PMenuId == pmenuId).OrderBy(x => x.SortId).ToList();
            foreach (BaseMenu val in _menulist)
            {
                TreeNode _node = new TreeNode();
                _node.Text = val.Name;
                _node.Tag = val;
                _node.Checked = groupmenuList.FindIndex(x => x.MenuId == val.MenuId) != -1 ? true : false;
                pNode.Nodes.Add(_node);
                recursionMenu(allmenu, val.MenuId, _node,groupmenuList);
            }
        }

        public void loadMenuTree(List<BaseModule> moduleList, List<BaseMenu> menuList, List<BaseMenu> groupmenuList)
        {
            this.treeView1.Nodes.Clear();
            foreach (BaseModule item in moduleList)
            {
                TreeNode node = new TreeNode();
                node.Text = item.Name;
                node.Tag = item;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
                this.treeView1.Nodes.Add(node);

                List<BaseMenu> _menulist = menuList.FindAll(x => x.ModuleId == item.ModuleId && x.PMenuId == -1).OrderBy(x => x.SortId).ToList();
                foreach (BaseMenu val in _menulist)
                {
                    TreeNode _node = new TreeNode();
                    _node.Text = val.Name;
                    _node.Tag = val;
                    _node.Checked = groupmenuList.FindIndex(x => x.MenuId == val.MenuId) != -1 ? true : false;
                    node.Nodes.Add(_node);
                    recursionMenu(menuList, val.MenuId, _node,groupmenuList);
                }
            }

            treeView1.ExpandAll();
        }

        #endregion

        private void recursionCheckNode(TreeNode node,bool check)
        {
            foreach (TreeNode _node in node.Nodes)
            {
                
                _node.Checked = check;
                
                recursionCheckNode(_node, check);
            }
        }

        private void recursionGetNodeCheck(TreeNode node, List<int> menuIdList)
        {
            foreach (TreeNode _node in node.Nodes)
            {

                if (_node.Checked == true && _node.Tag.GetType() == typeof(BaseMenu))
                    menuIdList.Add(((BaseMenu)_node.Tag).MenuId);
                recursionGetNodeCheck(_node, menuIdList);
            }
        }

        private int[] GetNodeCheck()
        {
            List<int> menuIdList = new List<int>();
            foreach (TreeNode _node in treeView1.Nodes)
            {
                if (_node.Checked == true && _node.Tag.GetType() == typeof(BaseMenu))
                    menuIdList.Add(((BaseMenu)_node.Tag).MenuId);
                recursionGetNodeCheck(_node, menuIdList);
            }

            return menuIdList.ToArray();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            if (treeView1.SelectedNode != null)
            {
                bool check = treeView1.SelectedNode.Checked;
                treeView1.AfterCheck -= new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
                recursionCheckNode(treeView1.SelectedNode, check);
                treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);


                if (dataGrid1.CurrentCell != null)
                {
                    int rowindex = dataGrid1.CurrentCell.RowIndex;
                    int groupId = Convert.ToInt32(dataGrid1["groupId", rowindex].Value.ToString());
                    InvokeController("SetGroupMenu", groupId, GetNodeCheck());
                }

            }
        }

        private void frmGroupMenu_Load(object sender, EventArgs e)
        {
            panelPage.Enabled = false;
            InvokeController("InitGroupData");
        }

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGrid1.CurrentCell != null)
            {
                int rowindex = dataGrid1.CurrentCell.RowIndex;
                int groupId=Convert.ToInt32(dataGrid1["groupId", rowindex].Value.ToString());
                InvokeController("LoadGroupMenuData",groupId);

                InvokeController("GetPageMenuData");
            }
        }

        #region 页面权限
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            InvokeController("GetPageMenuData");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Tag is BaseMenu)
                {
                    BaseMenu menu = ((BaseMenu)treeView1.SelectedNode.Tag);
                    if (string.IsNullOrEmpty(menu.FunName) == false)
                    {
                        if (txtCode.Text != "" && txtName.Text != "")
                        {
                            InvokeController("SavePageMenu", menu.ModuleId, menu.MenuId, txtCode.Text.Trim(), txtName.Text.Trim());
                        }
                        else
                        {
                            MessageBoxEx.Show("标识和名称都不能为空！");
                        }
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridPageMenu.CurrentCell != null)
            {
                int rowindex = gridPageMenu.CurrentCell.RowIndex;
                int pageId = Convert.ToInt32((gridPageMenu.DataSource as DataTable).Rows[rowindex]["Id"]);
                InvokeController("DeletePageMenu", pageId);
            }
        }

        private void gridPageMenu_Click(object sender, EventArgs e)
        {
            if (gridPageMenu.CurrentCell != null)
            {
                if (gridPageMenu.CurrentCell.ColumnIndex == colpageck.Index)
                {
                    if (dataGrid1.CurrentCell == null)
                    {
                        return;
                    }

                    int groupId = Convert.ToInt32(dataGrid1["groupId", dataGrid1.CurrentCell.RowIndex].Value.ToString());
                    int rowindex = gridPageMenu.CurrentCell.RowIndex;
                    int pageId = Convert.ToInt32((gridPageMenu.DataSource as DataTable).Rows[rowindex]["Id"]);
                    InvokeController("SetGroupPage", groupId, pageId);
                }
            }
        }

        #endregion
        #region IfrmGroupMenu 成员


        public BaseMenu currMenu
        {
            get
            {
                if (treeView1.SelectedNode!=null && treeView1.SelectedNode.Tag is BaseMenu)
                {
                    return ((BaseMenu)treeView1.SelectedNode.Tag);
                }
                return null;
            }
        }

        public void loadPageMenu(DataTable dt)
        {
            gridPageMenu.DataSource = dt;
        }

        public int currGroupId
        {
            get
            {
                if (dataGrid1.CurrentCell == null)
                {
                    return -1;
                }
                int rowindex = dataGrid1.CurrentCell.RowIndex;
                return Convert.ToInt32(dataGrid1["groupId", rowindex].Value.ToString());
            }
        }

        public bool panelPageEnabled
        {
            set { panelPage.Enabled = value; }
        }

        #endregion

        private void tsbtnNew_Click(object sender, EventArgs e)
        {
            InvokeController("NewGroup");
        }

        private void tsbtnAlter_Click(object sender, EventArgs e)
        {
            if (dataGrid1.CurrentCell != null)
            {
                int rowIndex = dataGrid1.CurrentCell.RowIndex;
                BaseGroup group = (dataGrid1.DataSource as List<BaseGroup>)[rowIndex];
                InvokeController("AlterGroup", group.GroupId);
            }
        }

        private void tsbtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGrid1.CurrentCell != null)
            {
                int rowIndex = dataGrid1.CurrentCell.RowIndex;
                BaseGroup group = (dataGrid1.DataSource as List<BaseGroup>)[rowIndex];
                if (Convert.ToBoolean(InvokeController("DeleteGroupisExist",group.GroupId)))
                {
                    if (MessageBoxEx.Show("是否删除此角色？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                    {
                        InvokeController("DeleteGroup", group.GroupId);
                    }
                }
                else
                {
                    MessageBoxEx.Show("此角色已使用，不能删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

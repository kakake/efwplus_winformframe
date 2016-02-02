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
    public partial class frmMenu : BaseFormEx, IfrmMenu
    {
        public frmMenu()
        {
            InitializeComponent();

            frmForm1.AddItem(this.textBoxX1, "Name", "请输入菜单名称！");
            frmForm1.AddItem(this.textBoxX2, "DllName");
            frmForm1.AddItem(this.textBoxX3, "FunName");
            frmForm1.AddItem(this.textBoxX4, "UrlName");
            frmForm1.AddItem(this.checkBoxX1, "MenuToolBar");
            frmForm1.AddItem(this.checkBoxX2, "MenuLookBar");
            frmForm1.AddItem(this.textBoxX7, "Image");
            frmForm1.AddItem(this.textBoxX8, "Memo");
            //frmForm1.AddItem(this.textBoxX9, "BindSQL");
        }

        #region IfrmMenu 成员

        public void loadMenuTree(List<BaseModule> moduleList, List<BaseMenu> menuList)
        {
            this.treeMenu.Nodes.Clear();
            foreach (BaseModule item in moduleList)
            {
                TreeNode node = new TreeNode();
                node.Text = item.Name;
                node.Tag = item;
                node.ImageIndex = 0;
                node.SelectedImageIndex = 0;
                this.treeMenu.Nodes.Add(node);

                List<BaseMenu> _menulist = menuList.FindAll(x => x.ModuleId == item.ModuleId && x.PMenuId == -1).OrderBy(x => x.SortId).ToList();
                foreach (BaseMenu val in _menulist)
                {
                    TreeNode _node = new TreeNode();
                    _node.Text = val.Name;
                    _node.Tag = val;
                    _node.ForeColor = Color.Blue;
                    node.Nodes.Add(_node);
                    recursionMenu(menuList, val.MenuId, _node);
                }
            }

            treeMenu.ExpandAll();
        }

        private void recursionMenu(List<BaseMenu> allmenu, int pmenuId, TreeNode pNode)
        {
            List<BaseMenu> _menulist = allmenu.FindAll(x => x.PMenuId == pmenuId).OrderBy(x => x.SortId).ToList();
            foreach (BaseMenu val in _menulist)
            {
                TreeNode _node = new TreeNode();
                _node.Text = val.Name;
                _node.Tag = val;
                _node.ForeColor = Color.Blue;
                pNode.Nodes.Add(_node);
                recursionMenu(allmenu, val.MenuId, _node);
            }
        }
        private BaseMenu _currentMenu;
        public BaseMenu currentMenu
        {
            get
            {
                if (_currentMenu != null)
                    frmForm1.GetValue<BaseMenu>(_currentMenu);
                return _currentMenu;
            }
            set
            {
                _currentMenu = value;
                frmForm1.Load<BaseMenu>(_currentMenu);
            }
        }

        public int selectMenuId
        {
            get
            {
                if (this.treeMenu.SelectedNode.Tag.GetType() == typeof(BaseMenu))
                {
                    return ((BaseMenu)this.treeMenu.SelectedNode.Tag).MenuId;
                }
                else
                    return -1;
            }
        }

        public int selectModuleId
        {
            get
            {
                if (this.treeMenu.SelectedNode.Tag.GetType() == typeof(BaseMenu))
                {
                    return ((BaseMenu)this.treeMenu.SelectedNode.Tag).ModuleId;
                }
                else
                {
                    return ((BaseModule)this.treeMenu.SelectedNode.Tag).ModuleId;
                }
            }
        }

        #endregion

        private void frmMenu_Load(object sender, EventArgs e)
        {
            InvokeController("InitMenuData");
        }

        private void treeMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeMenu.SelectedNode.Tag == null) return;
            if (this.treeMenu.SelectedNode.Tag.GetType() == typeof(BaseMenu))
            {
                currentMenu = (BaseMenu)this.treeMenu.SelectedNode.Tag;
            }
        }

        private void btnNewMenu_Click(object sender, EventArgs e)
        {
            InvokeController("NewMenu");
            frmForm1.Clear();
        }

        private void BtnSaveMenu_Click(object sender, EventArgs e)
        {
            if (frmForm1.Validate())
            {
                InvokeController("SaveMenu");
                MessageBoxEx.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeMenu.SelectedNode.Tag.GetType() == typeof(BaseMenu))
            {
                if (MessageBoxEx.Show("你确实需要删除此菜单？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    BaseMenu menu = (BaseMenu)this.treeMenu.SelectedNode.Tag;
                    InvokeController("DeleteMenu", menu.MenuId);
                    MessageBoxEx.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBoxEx.Show("无法删除此菜单模块！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tsbtnAdd_Click(object sender, EventArgs e)
        {
            InvokeController("NewModule");
        }

        private void tsbtnAlter_Click(object sender, EventArgs e)
        {
            if(treeMenu.SelectedNode==null)return;

            if (treeMenu.SelectedNode.Tag is BaseModule)
            {
                BaseModule module = treeMenu.SelectedNode.Tag as BaseModule;
                InvokeController("AlterModule", module.ModuleId);
            }
        }

        private void tsbtnDelete_Click(object sender, EventArgs e)
        {
            if(treeMenu.SelectedNode==null || !(treeMenu.SelectedNode.Tag is BaseModule))return;

            if ( treeMenu.SelectedNode.Nodes.Count==0)
            {
                if (MessageBoxEx.Show("是否删除此模块？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                {
                    BaseModule module = treeMenu.SelectedNode.Tag as BaseModule;
                    InvokeController("DeleteModule", module.ModuleId);
                }
            }
            else
            {
                MessageBoxEx.Show("此模块已有菜单，不能删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}

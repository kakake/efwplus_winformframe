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
using WinMainUIFrame.Winform.IView.EmpUserManager;

namespace WinMainUIFrame.Winform.ViewForm.EmpUserManager
{
    public partial class FrmDeptEmp : BaseFormEx, IfrmDeptEmp
    {
        public FrmDeptEmp()
        {
            InitializeComponent();
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            InvokeController("NewUser");
        }

        #region IfrmDeptEmp 成员

        private void recursionDeptLayer(List<BaseDeptLayer> allLayer, int pId, TreeNode pNode,List<BaseDept> deptlist)
        {
            List<BaseDeptLayer> _layerlist = allLayer.FindAll(x => x.PId == pId);
            foreach (BaseDeptLayer layer in _layerlist)
            {
                TreeNode _node = new TreeNode();
                _node.Text = layer.Name;
                _node.Tag = layer;
                _node.ImageIndex = 0;
                _node.SelectedImageIndex = 0;
                pNode.Nodes.Add(_node);

                loadDeptNode(deptlist, layer.LayerId, _node);
                recursionDeptLayer(allLayer, layer.LayerId, _node,deptlist);
            }
        }

        private void loadDeptNode(List<BaseDept> deptlist, int layerId, TreeNode node)
        {
            List<BaseDept> _deptlist = deptlist.FindAll(x => x.Layer == layerId);
            foreach (BaseDept val in _deptlist)
            {
                TreeNode _node = new TreeNode();
                _node.Text = val.Name;
                _node.Tag = val;
                _node.ForeColor = Color.Blue;
                node.Nodes.Add(_node);
                
            }
        }

        public void loadDeptTree(List<BaseDeptLayer> layerList, List<BaseDept> deptList)
        {
            treeView1.Nodes.Clear();
            List<BaseDeptLayer> root = layerList.FindAll(x => x.PId == 0);
            foreach (BaseDeptLayer layer in root)
            {
                TreeNode _node = new TreeNode();
                _node.Text = layer.Name;
                _node.Tag = layer;
                _node.ImageIndex = 0;
                _node.SelectedImageIndex = 0;
                treeView1.Nodes.Add(_node);
                loadDeptNode(deptList, layer.LayerId, _node);
                recursionDeptLayer(layerList, layer.LayerId, _node,deptList);
            }

            treeView1.ExpandAll();
        }

        public void loadUserGrid(DataTable dt)
        {
            dataGrid1.DataSource = dt;
        }

        #endregion

        private void FrmDeptEmp_Load(object sender, EventArgs e)
        {
            InvokeController("LoadDeptData");
        }
        private void recursiongetalldeptIds(TreeNode node, List<int> deptidList)
        {
            foreach (TreeNode _node in node.Nodes)
            {

                if (_node.Tag.GetType() == typeof(BaseDept))
                    deptidList.Add(((BaseDept)_node.Tag).DeptId);
                recursiongetalldeptIds(_node, deptidList);
            }
        }

        private int[] getalldeptIds()
        {
            List<int> deptidList = new List<int>();

            if (treeView1.SelectedNode.Tag.GetType() == typeof(BaseDept))
                deptidList.Add(((BaseDept)treeView1.SelectedNode.Tag).DeptId);
            recursiongetalldeptIds(treeView1.SelectedNode, deptidList);
            return deptidList.ToArray();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeView1.SelectedNode.Tag == null) return;
            if (this.treeView1.SelectedNode.Tag.GetType() == typeof(BaseDept))
            {
                this.新建科室分类ToolStripMenuItem.Enabled = false;
                this.修改科室分类ToolStripMenuItem.Enabled = false;
                this.删除科室分类ToolStripMenuItem.Enabled = false;
                this.新建科室ToolStripMenuItem.Enabled = false;
                this.修改科室ToolStripMenuItem.Enabled = true;
                this.删除科室ToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.新建科室分类ToolStripMenuItem.Enabled = true;
                this.修改科室分类ToolStripMenuItem.Enabled = true;
                this.删除科室分类ToolStripMenuItem.Enabled = true;
                this.新建科室ToolStripMenuItem.Enabled = true;
                this.修改科室ToolStripMenuItem.Enabled = false;
                this.删除科室ToolStripMenuItem.Enabled = false;
            }

            InvokeController("LoadUserData", getalldeptIds());
        }

        private void 新建科室分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseDeptLayer layer = (BaseDeptLayer)InvokeController("SaveDeptLayer", 0, "新建科室分类", ((BaseDeptLayer)treeView1.SelectedNode.Tag).LayerId);
            TreeNode _node = new TreeNode();
            _node.Text = layer.Name;
            _node.Tag = layer;
            _node.ImageIndex = 0;
            _node.SelectedImageIndex = 0;
            treeView1.SelectedNode.Nodes.Add(_node);
        }

        private void 修改科室分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.treeView1.SelectedNode.BeginEdit();
        }

        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null) return;
            if (e.Node.Tag.GetType() == typeof(BaseDept))
            {
                BaseDept dept = (BaseDept)InvokeController("SaveDept", ((BaseDept)e.Node.Tag).DeptId, e.Label, ((BaseDeptLayer)e.Node.Parent.Tag).LayerId);
                e.Node.Tag = dept;
            }
            else
            {
                BaseDeptLayer layer = (BaseDeptLayer)InvokeController("SaveDeptLayer", ((BaseDeptLayer)e.Node.Tag).LayerId, e.Label, e.Node.Parent == null ? 0 : ((BaseDeptLayer)e.Node.Parent.Tag).LayerId);
                e.Node.Tag = layer;
            }
        }

        private void 删除科室分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("你确实需要删除此节点？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                InvokeController("DeleteDeptLayer", ((BaseDeptLayer)treeView1.SelectedNode.Tag).LayerId);
                treeView1.SelectedNode.Remove();
            }
        }

        private void 新建科室ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaseDept dept = (BaseDept)InvokeController("SaveDept", 0, "新建科室", ((BaseDeptLayer)treeView1.SelectedNode.Tag).LayerId);
            TreeNode _node = new TreeNode();
            _node.Text = dept.Name;
            _node.Tag = dept;
            _node.ForeColor = Color.Blue;
            treeView1.SelectedNode.Nodes.Add(_node);
        }

        private void 修改科室ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.treeView1.SelectedNode.BeginEdit();
        }

        private void 删除科室ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBoxEx.Show("你确实需要删除此节点？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                InvokeController("DeleteDept", ((BaseDept)treeView1.SelectedNode.Tag).DeptId);
                treeView1.SelectedNode.Remove();
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            if (dataGrid1.CurrentCell != null)
            {
                if (MessageBoxEx.Show("你确实重置此用户的密码？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    int userid = Convert.ToInt32(dataGrid1["userid", dataGrid1.CurrentCell.RowIndex].Value);
                    InvokeController("ResetUserPass", userid);
                    MessageBoxEx.Show("密码重置成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void textBoxX1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InvokeController("LoadUserData_Key", textBoxX1.Text.Trim());
            }
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            if (dataGrid1.CurrentCell != null)
            {
                int userid = Convert.ToInt32(dataGrid1["userid", dataGrid1.CurrentCell.RowIndex].Value);
                int empid = Convert.ToInt32(dataGrid1["empid", dataGrid1.CurrentCell.RowIndex].Value);

                InvokeController("AlterUser", empid, userid);
            }

        }

        private void dataGrid1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGrid1.CurrentCell != null)
            {
                int userid = Convert.ToInt32(dataGrid1["userid", dataGrid1.CurrentCell.RowIndex].Value);
                int empid = Convert.ToInt32(dataGrid1["empid", dataGrid1.CurrentCell.RowIndex].Value);

                InvokeController("AlterUser", empid, userid);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            InvokeController("LoadUserData", getalldeptIds());
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvokeController("LoadDeptData");
        }
    }
}

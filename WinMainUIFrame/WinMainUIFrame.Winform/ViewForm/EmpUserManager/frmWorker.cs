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
    public partial class frmWorker : BaseFormEx, IfrmWorker
    {
        public frmWorker()
        {
            InitializeComponent();

            frmForm1.AddItem(textBoxX1, "WorkNo", "请输入机构编码");
            frmForm1.AddItem(textBoxX2, "WorkName", "请输入机构名称");
            frmForm1.AddItem(textBoxX3, "Memo");
            frmForm1.AddItem(textBoxX4, "RegKey");
        }

        #region IfrmWorker 成员

        public void loadWorkerGrid(List<BaseWorkers> workerList)
        {
            this.dataGrid1.DataSource = null;
            this.dataGrid1.DataSource = workerList;
        }

        private BaseWorkers _currWorker;
        public BaseWorkers currWorker
        {
            get
            {
                frmForm1.GetValue<BaseWorkers>(_currWorker);
                return _currWorker;
            }
            set
            {
                _currWorker = value;
                frmForm1.Load<BaseWorkers>(_currWorker);

                if (_currWorker.WorkId == 0)
                    buttonX3.Enabled = false;
                else
                    buttonX3.Enabled = true;

                if (_currWorker.DelFlag == -1 || _currWorker.DelFlag == 1)
                {
                    buttonX3.Text = "启用";
                    buttonX3.ForeColor = Color.Green;
                }
                else
                {
                    buttonX3.Text = "禁用";
                    buttonX3.ForeColor = Color.Red;
                }
            }
        }

        #endregion

        private void frmWorker_Load(object sender, EventArgs e)
        {
            InvokeController("LoadWorkerList");
        }

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {
            buttonX1.Enabled = true;
            if (dataGrid1.CurrentCell != null)
            {
                int workId = Convert.ToInt32(dataGrid1["workId", dataGrid1.CurrentCell.RowIndex].Value);
                InvokeController("GetCurrWorker", workId);
            }
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            InvokeController("NewWorker");
            buttonX1.Enabled = false;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            if (frmForm1.Validate())
            {
                int state = _currWorker.DelFlag;
                InvokeController("SaveWorker");
                buttonX1.Enabled = true;
                if (state == 0)
                    MessageBoxEx.Show("操作成功，修改已启用的机构会恢复到禁用状态，需重新启用！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBoxEx.Show("操作成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            InvokeController("LoadWorkerList");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            InvokeController("TurnOnOffWorker", _currWorker.WorkId);
        }

        private void textBoxX4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                string workname = textBoxX2.Text;
                string date = DateTime.Now.AddYears(1).Date.ToString();

                string regcode = EFWCoreLib.CoreFrame.Common.ConvertExtend.ToPassWord(workname + "|" + date);
                textBoxX4.Text = regcode;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            InvokeController("LoadWorkerList");
        }
    }
}

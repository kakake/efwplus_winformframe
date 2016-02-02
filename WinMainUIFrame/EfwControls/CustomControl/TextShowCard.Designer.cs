namespace EFWCoreLib.WinformFrame.CustomControl
{
    partial class TextShowCard
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textpanel = new System.Windows.Forms.Panel();
            this.textdataGrid = new EFWCoreLib.WinformFrame.CustomControl.DataGrid();
            this.textpager = new EFWCoreLib.WinformFrame.CustomControl.Pager();
            this.textpanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textdataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // textpanel
            // 
            this.textpanel.Controls.Add(this.textdataGrid);
            this.textpanel.Controls.Add(this.textpager);
            this.textpanel.Location = new System.Drawing.Point(189, 69);
            this.textpanel.Name = "textpanel";
            this.textpanel.Size = new System.Drawing.Size(339, 245);
            this.textpanel.TabIndex = 4;
            this.textpanel.Visible = false;
            // 
            // textdataGrid
            // 
            this.textdataGrid.AllowSortWhenClickColumnHeader = false;
            this.textdataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.textdataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.textdataGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.textdataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textdataGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.textdataGrid.Location = new System.Drawing.Point(0, 0);
            this.textdataGrid.Name = "textdataGrid";
            this.textdataGrid.RowTemplate.Height = 23;
            this.textdataGrid.Size = new System.Drawing.Size(339, 217);
            this.textdataGrid.TabIndex = 3;
            // 
            // textpager
            // 
            this.textpager.BindDataControl = this.textdataGrid;
            this.textpager.DataSource = null;
            this.textpager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textpager.IsPage = false;
            this.textpager.Location = new System.Drawing.Point(0, 217);
            this.textpager.Name = "textpager";
            this.textpager.pageNo = 1;
            this.textpager.pageSize = 10;
            this.textpager.PageSizeType = EFWCoreLib.WinformFrame.CustomControl.pageSizeType.Size10;
            this.textpager.Size = new System.Drawing.Size(339, 28);
            this.textpager.TabIndex = 0;
            this.textpager.totalRecord = 100;
            // 
            // TextShowCard
            // 
            // 
            // 
            // 
            this.textpanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textdataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel textpanel;
        private DataGrid textdataGrid;
        private Pager textpager;

    }
}

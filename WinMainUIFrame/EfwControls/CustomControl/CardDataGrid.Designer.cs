namespace EfwControls.CustomControl
{
    partial class CardDataGrid
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.letterpanel = new DevComponents.DotNetBar.PanelEx();
            this.textdataGrid = new DataGrid();
            this.textpager = new Pager();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textdataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.panelEx1.Controls.Add(this.textdataGrid);
            this.panelEx1.Controls.Add(this.textpager);
            this.panelEx1.Controls.Add(this.letterpanel);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(400, 300);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            // 
            // letterpanel
            // 
            this.letterpanel.CanvasColor = System.Drawing.SystemColors.Control;
            this.letterpanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.letterpanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.letterpanel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.letterpanel.Location = new System.Drawing.Point(0, 0);
            this.letterpanel.Name = "letterpanel";
            this.letterpanel.Size = new System.Drawing.Size(400, 20);
            this.letterpanel.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.letterpanel.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.letterpanel.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.letterpanel.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.letterpanel.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.letterpanel.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.letterpanel.Style.GradientAngle = 90;
            this.letterpanel.TabIndex = 3;
            // 
            // textdataGrid
            // 
            this.textdataGrid.AllowSortWhenClickColumnHeader = false;
            this.textdataGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.textdataGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
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
            this.textdataGrid.Location = new System.Drawing.Point(0, 20);
            this.textdataGrid.Name = "textdataGrid";
            this.textdataGrid.RowTemplate.Height = 23;
            this.textdataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.textdataGrid.SeqVisible = true;
            this.textdataGrid.Size = new System.Drawing.Size(400, 252);
            this.textdataGrid.TabIndex = 5;
            // 
            // textpager
            // 
            this.textpager.BindDataControl = this.textdataGrid;
            this.textpager.DataSource = null;
            this.textpager.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textpager.IsPage = true;
            this.textpager.Location = new System.Drawing.Point(0, 272);
            this.textpager.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textpager.Name = "textpager";
            this.textpager.pageNo = 1;
            this.textpager.pageSize = 10;
            this.textpager.PageSizeType = pageSizeType.Size10;
            this.textpager.Size = new System.Drawing.Size(400, 28);
            this.textpager.TabIndex = 4;
            this.textpager.totalRecord = 100;
            // 
            // CardDataGrid
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelEx1);
            this.Name = "CardDataGrid";
            this.Size = new System.Drawing.Size(400, 300);
            this.panelEx1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textdataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        public DataGrid textdataGrid;
        public DevComponents.DotNetBar.PanelEx letterpanel;
        public Pager textpager;
    }
}

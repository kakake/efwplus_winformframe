namespace EfwControls.CustomControl
{
    partial class MultiSelectText
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textContext = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.panelGrid = new DevComponents.DotNetBar.PanelEx();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.panelValue = new DevComponents.DotNetBar.PanelEx();
            this.txtChar = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.btnClear = new System.Windows.Forms.LinkLabel();
            this.panelSelect = new System.Windows.Forms.Panel();
            this.dataGridSelect = new EfwControls.CustomControl.DataGrid();
            this.popup = new EfwControls.CustomControl.Popup(this.components);
            this.panelGrid.SuspendLayout();
            this.panelEx2.SuspendLayout();
            this.panelValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // textContext
            // 
            this.textContext.BackColor = System.Drawing.Color.White;
            // 
            // 
            // 
            this.textContext.Border.Class = "TextBoxBorder";
            this.textContext.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.textContext.ButtonCustom.Visible = true;
            this.textContext.Dock = System.Windows.Forms.DockStyle.Top;
            this.textContext.Location = new System.Drawing.Point(0, 0);
            this.textContext.Name = "textContext";
            this.textContext.ReadOnly = true;
            this.textContext.Size = new System.Drawing.Size(455, 23);
            this.textContext.TabIndex = 1;
            // 
            // panelGrid
            // 
            this.panelGrid.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelGrid.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelGrid.Controls.Add(this.panelEx2);
            this.panelGrid.Controls.Add(this.panelValue);
            this.panelGrid.Location = new System.Drawing.Point(19, 41);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Padding = new System.Windows.Forms.Padding(1);
            this.panelGrid.Size = new System.Drawing.Size(210, 241);
            this.panelGrid.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelGrid.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelGrid.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelGrid.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelGrid.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelGrid.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelGrid.Style.GradientAngle = 90;
            this.panelGrid.TabIndex = 2;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.dataGridSelect);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx2.Location = new System.Drawing.Point(1, 26);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(208, 214);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 1;
            this.panelEx2.Text = "panelEx2";
            // 
            // panelValue
            // 
            this.panelValue.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelValue.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelValue.Controls.Add(this.txtChar);
            this.panelValue.Controls.Add(this.btnClear);
            this.panelValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelValue.Location = new System.Drawing.Point(1, 1);
            this.panelValue.Name = "panelValue";
            this.panelValue.Size = new System.Drawing.Size(208, 25);
            this.panelValue.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelValue.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelValue.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelValue.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelValue.Style.GradientAngle = 90;
            this.panelValue.TabIndex = 0;
            // 
            // txtChar
            // 
            this.txtChar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.txtChar.Border.Class = "TextBoxBorder";
            this.txtChar.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.txtChar.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtChar.Location = new System.Drawing.Point(168, 1);
            this.txtChar.Name = "txtChar";
            this.txtChar.Size = new System.Drawing.Size(40, 23);
            this.txtChar.TabIndex = 0;
            this.txtChar.TextChanged += new System.EventHandler(this.txtChar_TextChanged);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(140, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(32, 20);
            this.btnClear.TabIndex = 1;
            this.btnClear.TabStop = true;
            this.btnClear.Text = "清空";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSelect
            // 
            this.panelSelect.Location = new System.Drawing.Point(236, 41);
            this.panelSelect.Name = "panelSelect";
            this.panelSelect.Padding = new System.Windows.Forms.Padding(5);
            this.panelSelect.Size = new System.Drawing.Size(202, 241);
            this.panelSelect.TabIndex = 3;
            // 
            // dataGridSelect
            // 
            this.dataGridSelect.AllowSortWhenClickColumnHeader = false;
            this.dataGridSelect.AllowUserToAddRows = false;
            this.dataGridSelect.AllowUserToDeleteRows = false;
            this.dataGridSelect.AllowUserToResizeRows = false;
            this.dataGridSelect.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.dataGridSelect.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridSelect.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridSelect.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridSelect.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.dataGridSelect.HighlightSelectedColumnHeaders = false;
            this.dataGridSelect.Location = new System.Drawing.Point(0, 0);
            this.dataGridSelect.Name = "dataGridSelect";
            this.dataGridSelect.ReadOnly = true;
            this.dataGridSelect.RowHeadersVisible = false;
            this.dataGridSelect.RowHeadersWidth = 25;
            this.dataGridSelect.RowTemplate.Height = 23;
            this.dataGridSelect.SelectAllSignVisible = false;
            this.dataGridSelect.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridSelect.SeqVisible = false;
            this.dataGridSelect.Size = new System.Drawing.Size(208, 214);
            this.dataGridSelect.TabIndex = 0;
            this.dataGridSelect.Click += new System.EventHandler(this.dataGridSelect_Click);
            // 
            // MultiSelectText
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelSelect);
            this.Controls.Add(this.panelGrid);
            this.Controls.Add(this.textContext);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "MultiSelectText";
            this.Size = new System.Drawing.Size(455, 302);
            this.panelGrid.ResumeLayout(false);
            this.panelEx2.ResumeLayout(false);
            this.panelValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX textContext;
        private Popup popup;
        private DevComponents.DotNetBar.PanelEx panelGrid;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private DevComponents.DotNetBar.PanelEx panelValue;
        private DataGrid dataGridSelect;
        private DevComponents.DotNetBar.Controls.TextBoxX txtChar;
        private System.Windows.Forms.Panel panelSelect;
        private System.Windows.Forms.LinkLabel btnClear;
    }
}

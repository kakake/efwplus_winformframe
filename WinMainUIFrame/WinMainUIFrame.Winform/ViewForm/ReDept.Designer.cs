namespace WinMainUIFrame.Winform.ViewForm
{
    partial class ReDept
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonConfirm = new DevComponents.DotNetBar.ButtonX();
            this.labelX1 = new DevComponents.DotNetBar.LabelX();
            this.labelX2 = new DevComponents.DotNetBar.LabelX();
            this.labelX3 = new DevComponents.DotNetBar.LabelX();
            this.comboBoxDepts = new DevComponents.DotNetBar.Controls.ComboBoxEx();
            this.labelUserName = new DevComponents.DotNetBar.LabelX();
            this.labelWorkName = new DevComponents.DotNetBar.LabelX();
            this.BasepanelEx.SuspendLayout();
            this.SuspendLayout();
            // 
            // BasepanelEx
            // 
            this.BasepanelEx.Controls.Add(this.labelWorkName);
            this.BasepanelEx.Controls.Add(this.labelUserName);
            this.BasepanelEx.Controls.Add(this.comboBoxDepts);
            this.BasepanelEx.Controls.Add(this.labelX3);
            this.BasepanelEx.Controls.Add(this.labelX2);
            this.BasepanelEx.Controls.Add(this.labelX1);
            this.BasepanelEx.Controls.Add(this.buttonConfirm);
            this.BasepanelEx.Size = new System.Drawing.Size(309, 141);
            this.BasepanelEx.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.BasepanelEx.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.BasepanelEx.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.BasepanelEx.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.BasepanelEx.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.BasepanelEx.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.BasepanelEx.Style.GradientAngle = 90;
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonConfirm.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonConfirm.Location = new System.Drawing.Point(224, 109);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(75, 23);
            this.buttonConfirm.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonConfirm.TabIndex = 0;
            this.buttonConfirm.Text = "确定";
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // labelX1
            // 
            // 
            // 
            // 
            this.labelX1.BackgroundStyle.Class = "";
            this.labelX1.Location = new System.Drawing.Point(12, 12);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(75, 23);
            this.labelX1.TabIndex = 1;
            this.labelX1.Text = "用户名称：";
            // 
            // labelX2
            // 
            // 
            // 
            // 
            this.labelX2.BackgroundStyle.Class = "";
            this.labelX2.Location = new System.Drawing.Point(12, 41);
            this.labelX2.Name = "labelX2";
            this.labelX2.Size = new System.Drawing.Size(75, 23);
            this.labelX2.TabIndex = 2;
            this.labelX2.Text = "机构名称：";
            // 
            // labelX3
            // 
            // 
            // 
            // 
            this.labelX3.BackgroundStyle.Class = "";
            this.labelX3.Location = new System.Drawing.Point(12, 70);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(75, 23);
            this.labelX3.TabIndex = 3;
            this.labelX3.Text = "科室名称：";
            // 
            // comboBoxDepts
            // 
            this.comboBoxDepts.DisplayMember = "Name";
            this.comboBoxDepts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxDepts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDepts.FormattingEnabled = true;
            this.comboBoxDepts.ItemHeight = 15;
            this.comboBoxDepts.Location = new System.Drawing.Point(80, 71);
            this.comboBoxDepts.Name = "comboBoxDepts";
            this.comboBoxDepts.Size = new System.Drawing.Size(219, 21);
            this.comboBoxDepts.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.comboBoxDepts.TabIndex = 4;
            this.comboBoxDepts.ValueMember = "DeptId";
            // 
            // labelUserName
            // 
            // 
            // 
            // 
            this.labelUserName.BackgroundStyle.Class = "";
            this.labelUserName.Location = new System.Drawing.Point(80, 12);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(219, 23);
            this.labelUserName.TabIndex = 5;
            this.labelUserName.Text = "labelX4";
            // 
            // labelWorkName
            // 
            // 
            // 
            // 
            this.labelWorkName.BackgroundStyle.Class = "";
            this.labelWorkName.Location = new System.Drawing.Point(80, 41);
            this.labelWorkName.Name = "labelWorkName";
            this.labelWorkName.Size = new System.Drawing.Size(219, 23);
            this.labelWorkName.TabIndex = 6;
            this.labelWorkName.Text = "labelX5";
            // 
            // ReDept
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 141);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReDept";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "切换科室";
            this.Load += new System.EventHandler(this.ReDept_Load);
            this.BasepanelEx.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.LabelX labelX3;
        private DevComponents.DotNetBar.LabelX labelX2;
        private DevComponents.DotNetBar.LabelX labelX1;
        private DevComponents.DotNetBar.ButtonX buttonConfirm;
        private DevComponents.DotNetBar.LabelX labelWorkName;
        private DevComponents.DotNetBar.LabelX labelUserName;
        private DevComponents.DotNetBar.Controls.ComboBoxEx comboBoxDepts;
    }
}
namespace FireWalletLite
{
    partial class DomainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DomainForm));
            groupBoxInfo = new GroupBox();
            labelInfo = new Label();
            labelName = new Label();
            buttonExplorer = new Button();
            button1 = new Button();
            button2 = new Button();
            groupBoxManage = new GroupBox();
            groupBoxInfo.SuspendLayout();
            groupBoxManage.SuspendLayout();
            SuspendLayout();
            // 
            // groupBoxInfo
            // 
            groupBoxInfo.Controls.Add(labelInfo);
            groupBoxInfo.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxInfo.Location = new Point(12, 52);
            groupBoxInfo.Name = "groupBoxInfo";
            groupBoxInfo.Size = new Size(280, 163);
            groupBoxInfo.TabIndex = 0;
            groupBoxInfo.TabStop = false;
            groupBoxInfo.Text = "Info";
            // 
            // labelInfo
            // 
            labelInfo.AutoSize = true;
            labelInfo.Location = new Point(6, 28);
            labelInfo.Name = "labelInfo";
            labelInfo.Size = new Size(45, 25);
            labelInfo.TabIndex = 0;
            labelInfo.Text = "Info";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            labelName.Location = new Point(12, 9);
            labelName.Name = "labelName";
            labelName.Size = new Size(95, 30);
            labelName.TabIndex = 0;
            labelName.Text = "domain/";
            // 
            // buttonExplorer
            // 
            buttonExplorer.FlatStyle = FlatStyle.Flat;
            buttonExplorer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonExplorer.Location = new Point(679, 9);
            buttonExplorer.Name = "buttonExplorer";
            buttonExplorer.Size = new Size(89, 30);
            buttonExplorer.TabIndex = 1;
            buttonExplorer.Text = "Explorer";
            buttonExplorer.UseVisualStyleBackColor = true;
            buttonExplorer.Click += buttonExplorer_Click;
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(6, 31);
            button1.Name = "button1";
            button1.Size = new Size(89, 30);
            button1.TabIndex = 1;
            button1.Text = "Explorer";
            button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(6, 67);
            button2.Name = "button2";
            button2.Size = new Size(89, 30);
            button2.TabIndex = 1;
            button2.Text = "Explorer";
            button2.UseVisualStyleBackColor = true;
            // 
            // groupBoxManage
            // 
            groupBoxManage.Controls.Add(button1);
            groupBoxManage.Controls.Add(button2);
            groupBoxManage.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxManage.Location = new Point(298, 52);
            groupBoxManage.Name = "groupBoxManage";
            groupBoxManage.Size = new Size(470, 163);
            groupBoxManage.TabIndex = 2;
            groupBoxManage.TabStop = false;
            groupBoxManage.Text = "Manage";
            // 
            // DomainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 457);
            Controls.Add(groupBoxManage);
            Controls.Add(buttonExplorer);
            Controls.Add(labelName);
            Controls.Add(groupBoxInfo);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DomainForm";
            ShowInTaskbar = false;
            Text = "DomainsForm";
            Load += DomainForm_Load;
            groupBoxInfo.ResumeLayout(false);
            groupBoxInfo.PerformLayout();
            groupBoxManage.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBoxInfo;
        private Label labelName;
        private Button buttonExplorer;
        private Label labelInfo;
        private Button button1;
        private Button button2;
        private GroupBox groupBoxManage;
    }
}
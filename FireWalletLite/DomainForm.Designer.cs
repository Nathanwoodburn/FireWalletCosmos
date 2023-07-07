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
            buttonRenew = new Button();
            buttonTransfer = new Button();
            groupBoxManage = new GroupBox();
            textBoxTransferAddress = new TextBox();
            buttonCancel = new Button();
            buttonFinalize = new Button();
            groupBoxDNS = new GroupBox();
            panelDNS = new Panel();
            groupBoxInfo.SuspendLayout();
            groupBoxManage.SuspendLayout();
            groupBoxDNS.SuspendLayout();
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
            // buttonRenew
            // 
            buttonRenew.FlatStyle = FlatStyle.Flat;
            buttonRenew.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonRenew.Location = new Point(6, 31);
            buttonRenew.Name = "buttonRenew";
            buttonRenew.Size = new Size(89, 30);
            buttonRenew.TabIndex = 1;
            buttonRenew.Text = "Renew";
            buttonRenew.UseVisualStyleBackColor = true;
            buttonRenew.Click += buttonRenew_Click;
            // 
            // buttonTransfer
            // 
            buttonTransfer.FlatStyle = FlatStyle.Flat;
            buttonTransfer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonTransfer.Location = new Point(375, 67);
            buttonTransfer.Name = "buttonTransfer";
            buttonTransfer.Size = new Size(89, 30);
            buttonTransfer.TabIndex = 1;
            buttonTransfer.Text = "Transfer";
            buttonTransfer.UseVisualStyleBackColor = true;
            buttonTransfer.Click += buttonTransfer_Click;
            // 
            // groupBoxManage
            // 
            groupBoxManage.Controls.Add(textBoxTransferAddress);
            groupBoxManage.Controls.Add(buttonRenew);
            groupBoxManage.Controls.Add(buttonCancel);
            groupBoxManage.Controls.Add(buttonFinalize);
            groupBoxManage.Controls.Add(buttonTransfer);
            groupBoxManage.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxManage.Location = new Point(298, 52);
            groupBoxManage.Name = "groupBoxManage";
            groupBoxManage.Size = new Size(470, 163);
            groupBoxManage.TabIndex = 2;
            groupBoxManage.TabStop = false;
            groupBoxManage.Text = "Manage";
            // 
            // textBoxTransferAddress
            // 
            textBoxTransferAddress.Location = new Point(6, 66);
            textBoxTransferAddress.Name = "textBoxTransferAddress";
            textBoxTransferAddress.Size = new Size(363, 32);
            textBoxTransferAddress.TabIndex = 2;
            // 
            // buttonCancel
            // 
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCancel.Location = new Point(6, 67);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(89, 30);
            buttonCancel.TabIndex = 1;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Visible = false;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonFinalize
            // 
            buttonFinalize.FlatStyle = FlatStyle.Flat;
            buttonFinalize.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonFinalize.Location = new Point(6, 67);
            buttonFinalize.Name = "buttonFinalize";
            buttonFinalize.Size = new Size(89, 30);
            buttonFinalize.TabIndex = 1;
            buttonFinalize.Text = "Finalize";
            buttonFinalize.UseVisualStyleBackColor = true;
            buttonFinalize.Visible = false;
            buttonFinalize.Click += buttonFinalize_Click;
            // 
            // groupBoxDNS
            // 
            groupBoxDNS.Controls.Add(panelDNS);
            groupBoxDNS.Location = new Point(12, 221);
            groupBoxDNS.Name = "groupBoxDNS";
            groupBoxDNS.Size = new Size(756, 224);
            groupBoxDNS.TabIndex = 3;
            groupBoxDNS.TabStop = false;
            groupBoxDNS.Text = "DNS";
            // 
            // panelDNS
            // 
            panelDNS.AutoSize = true;
            panelDNS.Dock = DockStyle.Fill;
            panelDNS.Location = new Point(3, 19);
            panelDNS.Name = "panelDNS";
            panelDNS.Size = new Size(750, 202);
            panelDNS.TabIndex = 0;
            // 
            // DomainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 457);
            Controls.Add(groupBoxDNS);
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
            groupBoxManage.PerformLayout();
            groupBoxDNS.ResumeLayout(false);
            groupBoxDNS.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBoxInfo;
        private Label labelName;
        private Button buttonExplorer;
        private Label labelInfo;
        private Button buttonRenew;
        private Button buttonTransfer;
        private GroupBox groupBoxManage;
        private TextBox textBoxTransferAddress;
        private Button buttonFinalize;
        private Button buttonCancel;
        private GroupBox groupBoxDNS;
        private Panel panelDNS;
    }
}
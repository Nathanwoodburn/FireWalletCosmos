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
            labelName = new Label();
            buttonExplorer = new Button();
            buttonRenew = new Button();
            buttonTransfer = new Button();
            groupBoxManage = new GroupBox();
            labelTransfer = new Label();
            textBoxTransferAddress = new TextBox();
            buttonCancel = new Button();
            buttonFinalize = new Button();
            groupBoxSign = new GroupBox();
            textBoxSignMessage = new TextBox();
            labelSignMessage = new Label();
            groupBoxManage.SuspendLayout();
            groupBoxSign.SuspendLayout();
            SuspendLayout();
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
            buttonRenew.Size = new Size(141, 30);
            buttonRenew.TabIndex = 1;
            buttonRenew.Text = "Renew";
            buttonRenew.UseVisualStyleBackColor = true;
            buttonRenew.Click += buttonRenew_Click;
            // 
            // buttonTransfer
            // 
            buttonTransfer.FlatStyle = FlatStyle.Flat;
            buttonTransfer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonTransfer.Location = new Point(655, 70);
            buttonTransfer.Name = "buttonTransfer";
            buttonTransfer.Size = new Size(89, 30);
            buttonTransfer.TabIndex = 1;
            buttonTransfer.Text = "Transfer";
            buttonTransfer.UseVisualStyleBackColor = true;
            buttonTransfer.Click += buttonTransfer_Click;
            // 
            // groupBoxManage
            // 
            groupBoxManage.Controls.Add(labelTransfer);
            groupBoxManage.Controls.Add(textBoxTransferAddress);
            groupBoxManage.Controls.Add(buttonRenew);
            groupBoxManage.Controls.Add(buttonCancel);
            groupBoxManage.Controls.Add(buttonFinalize);
            groupBoxManage.Controls.Add(buttonTransfer);
            groupBoxManage.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxManage.Location = new Point(12, 52);
            groupBoxManage.Name = "groupBoxManage";
            groupBoxManage.Size = new Size(756, 136);
            groupBoxManage.TabIndex = 2;
            groupBoxManage.TabStop = false;
            groupBoxManage.Text = "Manage";
            // 
            // labelTransfer
            // 
            labelTransfer.AutoSize = true;
            labelTransfer.Location = new Point(6, 72);
            labelTransfer.Name = "labelTransfer";
            labelTransfer.Size = new Size(83, 25);
            labelTransfer.TabIndex = 3;
            labelTransfer.Text = "Transfer:";
            // 
            // textBoxTransferAddress
            // 
            textBoxTransferAddress.Location = new Point(95, 68);
            textBoxTransferAddress.Name = "textBoxTransferAddress";
            textBoxTransferAddress.Size = new Size(554, 32);
            textBoxTransferAddress.TabIndex = 2;
            // 
            // buttonCancel
            // 
            buttonCancel.FlatStyle = FlatStyle.Flat;
            buttonCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCancel.Location = new Point(242, 70);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(141, 30);
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
            buttonFinalize.Location = new Point(95, 70);
            buttonFinalize.Name = "buttonFinalize";
            buttonFinalize.Size = new Size(141, 30);
            buttonFinalize.TabIndex = 1;
            buttonFinalize.Text = "Finalize";
            buttonFinalize.UseVisualStyleBackColor = true;
            buttonFinalize.Visible = false;
            buttonFinalize.Click += buttonFinalize_Click;
            // 
            // groupBoxSign
            // 
            groupBoxSign.Controls.Add(textBoxSignMessage);
            groupBoxSign.Controls.Add(labelSignMessage);
            groupBoxSign.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxSign.Location = new Point(12, 194);
            groupBoxSign.Name = "groupBoxSign";
            groupBoxSign.Size = new Size(756, 257);
            groupBoxSign.TabIndex = 3;
            groupBoxSign.TabStop = false;
            groupBoxSign.Text = "Sign";
            // 
            // textBoxSignMessage
            // 
            textBoxSignMessage.Location = new Point(6, 56);
            textBoxSignMessage.Multiline = true;
            textBoxSignMessage.Name = "textBoxSignMessage";
            textBoxSignMessage.Size = new Size(744, 195);
            textBoxSignMessage.TabIndex = 1;
            // 
            // labelSignMessage
            // 
            labelSignMessage.AutoSize = true;
            labelSignMessage.Location = new Point(6, 28);
            labelSignMessage.Name = "labelSignMessage";
            labelSignMessage.Size = new Size(90, 25);
            labelSignMessage.TabIndex = 0;
            labelSignMessage.Text = "Message:";
            // 
            // DomainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 457);
            Controls.Add(groupBoxSign);
            Controls.Add(groupBoxManage);
            Controls.Add(buttonExplorer);
            Controls.Add(labelName);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DomainForm";
            ShowInTaskbar = false;
            Text = "DomainsForm";
            Load += DomainForm_Load;
            groupBoxManage.ResumeLayout(false);
            groupBoxManage.PerformLayout();
            groupBoxSign.ResumeLayout(false);
            groupBoxSign.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label labelName;
        private Button buttonExplorer;
        private Button buttonRenew;
        private Button buttonTransfer;
        private GroupBox groupBoxManage;
        private TextBox textBoxTransferAddress;
        private Button buttonFinalize;
        private Button buttonCancel;
        private Label labelTransfer;
        private GroupBox groupBoxSign;
        private TextBox textBoxSignMessage;
        private Label labelSignMessage;
    }
}
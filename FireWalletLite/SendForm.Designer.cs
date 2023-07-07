namespace FireWalletLite
{
    partial class SendForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendForm));
            labelAddress = new Label();
            textBoxAddress = new TextBox();
            labelAmount = new Label();
            textBoxAmount = new TextBox();
            labelHNSToken = new Label();
            buttonSend = new Button();
            labelMax = new Label();
            SuspendLayout();
            // 
            // labelAddress
            // 
            labelAddress.AutoSize = true;
            labelAddress.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelAddress.Location = new Point(268, 10);
            labelAddress.Name = "labelAddress";
            labelAddress.Size = new Size(69, 21);
            labelAddress.TabIndex = 0;
            labelAddress.Text = "Address:";
            // 
            // textBoxAddress
            // 
            textBoxAddress.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxAddress.Location = new Point(131, 34);
            textBoxAddress.Name = "textBoxAddress";
            textBoxAddress.Size = new Size(391, 29);
            textBoxAddress.TabIndex = 1;
            // 
            // labelAmount
            // 
            labelAmount.AutoSize = true;
            labelAmount.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelAmount.Location = new Point(268, 98);
            labelAmount.Name = "labelAmount";
            labelAmount.Size = new Size(69, 21);
            labelAmount.TabIndex = 2;
            labelAmount.Text = "Amount:";
            // 
            // textBoxAmount
            // 
            textBoxAmount.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxAmount.Location = new Point(183, 122);
            textBoxAmount.Name = "textBoxAmount";
            textBoxAmount.Size = new Size(208, 29);
            textBoxAmount.TabIndex = 3;
            // 
            // labelHNSToken
            // 
            labelHNSToken.AutoSize = true;
            labelHNSToken.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelHNSToken.Location = new Point(397, 125);
            labelHNSToken.Name = "labelHNSToken";
            labelHNSToken.Size = new Size(42, 21);
            labelHNSToken.TabIndex = 4;
            labelHNSToken.Text = "HNS";
            // 
            // buttonSend
            // 
            buttonSend.FlatStyle = FlatStyle.Flat;
            buttonSend.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSend.Location = new Point(257, 247);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(97, 40);
            buttonSend.TabIndex = 5;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // labelMax
            // 
            labelMax.AutoSize = true;
            labelMax.Location = new Point(277, 164);
            labelMax.Name = "labelMax";
            labelMax.Size = new Size(33, 15);
            labelMax.TabIndex = 6;
            labelMax.Text = "MAX";
            // 
            // SendForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(653, 299);
            Controls.Add(labelMax);
            Controls.Add(buttonSend);
            Controls.Add(labelHNSToken);
            Controls.Add(textBoxAmount);
            Controls.Add(labelAmount);
            Controls.Add(textBoxAddress);
            Controls.Add(labelAddress);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SendForm";
            ShowInTaskbar = false;
            Text = "Send HNS";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelAddress;
        private TextBox textBoxAddress;
        private Label labelAmount;
        private TextBox textBoxAmount;
        private Label labelHNSToken;
        private Button buttonSend;
        private Label labelMax;
    }
}
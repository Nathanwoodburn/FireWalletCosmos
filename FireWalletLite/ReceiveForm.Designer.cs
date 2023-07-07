namespace FireWalletLite
{
    partial class ReceiveForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReceiveForm));
            textBoxReceive = new TextBox();
            label1 = new Label();
            buttonCopy = new Button();
            pictureBoxReceiveQR = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBoxReceiveQR).BeginInit();
            SuspendLayout();
            // 
            // textBoxReceive
            // 
            textBoxReceive.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxReceive.Location = new Point(63, 78);
            textBoxReceive.Name = "textBoxReceive";
            textBoxReceive.ReadOnly = true;
            textBoxReceive.Size = new Size(574, 32);
            textBoxReceive.TabIndex = 0;
            textBoxReceive.TextAlign = HorizontalAlignment.Center;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(275, 28);
            label1.Name = "label1";
            label1.Size = new Size(151, 25);
            label1.TabIndex = 1;
            label1.Text = "Receive Address:";
            // 
            // buttonCopy
            // 
            buttonCopy.FlatStyle = FlatStyle.Flat;
            buttonCopy.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            buttonCopy.Location = new Point(275, 116);
            buttonCopy.Name = "buttonCopy";
            buttonCopy.Size = new Size(127, 46);
            buttonCopy.TabIndex = 2;
            buttonCopy.Text = "Copy";
            buttonCopy.UseVisualStyleBackColor = true;
            buttonCopy.Click += buttonCopy_Click;
            // 
            // pictureBoxReceiveQR
            // 
            pictureBoxReceiveQR.Location = new Point(275, 195);
            pictureBoxReceiveQR.Name = "pictureBoxReceiveQR";
            pictureBoxReceiveQR.Size = new Size(100, 50);
            pictureBoxReceiveQR.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxReceiveQR.TabIndex = 3;
            pictureBoxReceiveQR.TabStop = false;
            // 
            // ReceiveForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(680, 507);
            Controls.Add(pictureBoxReceiveQR);
            Controls.Add(buttonCopy);
            Controls.Add(label1);
            Controls.Add(textBoxReceive);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ReceiveForm";
            ShowInTaskbar = false;
            Text = "Receive";
            ((System.ComponentModel.ISupportInitialize)pictureBoxReceiveQR).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBoxReceive;
        private Label label1;
        private Button buttonCopy;
        private PictureBox pictureBoxReceiveQR;
    }
}
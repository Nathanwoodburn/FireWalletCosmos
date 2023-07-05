namespace FireWalletLite
{
    partial class FirstLoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FirstLoginForm));
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            textBoxSeed = new TextBox();
            label3 = new Label();
            textBoxPassword = new TextBox();
            textBoxPassword2 = new TextBox();
            label4 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            button1.Location = new Point(684, 406);
            button1.Name = "button1";
            button1.Size = new Size(84, 32);
            button1.TabIndex = 0;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Start_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(284, 9);
            label1.Name = "label1";
            label1.Size = new Size(232, 30);
            label1.TabIndex = 1;
            label1.Text = "Welcome to FireWallet";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(12, 51);
            label2.Name = "label2";
            label2.Size = new Size(350, 57);
            label2.TabIndex = 2;
            label2.Text = "Here is your seed phrase:\r\nYou should save this somewhere safe (preferably offline)\r\nThis will not be shown again";
            // 
            // textBoxSeed
            // 
            textBoxSeed.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxSeed.Location = new Point(12, 111);
            textBoxSeed.Multiline = true;
            textBoxSeed.Name = "textBoxSeed";
            textBoxSeed.ReadOnly = true;
            textBoxSeed.Size = new Size(756, 128);
            textBoxSeed.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(12, 270);
            label3.Name = "label3";
            label3.Size = new Size(125, 19);
            label3.TabIndex = 4;
            label3.Text = "Create a password:";
            // 
            // textBox1
            // 
            textBoxPassword.Location = new Point(143, 269);
            textBoxPassword.Name = "textBox1";
            textBoxPassword.Size = new Size(182, 23);
            textBoxPassword.TabIndex = 5;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // textBox2
            // 
            textBoxPassword2.Location = new Point(143, 298);
            textBoxPassword2.Name = "textBox2";
            textBoxPassword2.Size = new Size(182, 23);
            textBoxPassword2.TabIndex = 6;
            textBoxPassword2.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(12, 299);
            label4.Name = "label4";
            label4.Size = new Size(123, 19);
            label4.TabIndex = 4;
            label4.Text = "Confirm password:";
            // 
            // FirstLoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(780, 450);
            Controls.Add(textBoxPassword2);
            Controls.Add(textBoxPassword);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(textBoxSeed);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FirstLoginForm";
            Text = "FireWallet";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private TextBox textBoxSeed;
        private Label label3;
        private TextBox textBoxPassword;
        private TextBox textBoxPassword2;
        private Label label4;
    }
}
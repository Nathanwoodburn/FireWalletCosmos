namespace FireWalletLite
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            statusStripMain = new StatusStrip();
            SyncLabel = new ToolStripStatusLabel();
            timerUpdate = new System.Windows.Forms.Timer(components);
            panelLogin = new Panel();
            labelWelcome = new Label();
            textBoxPassword = new TextBox();
            labelPassword = new Label();
            LoginButton = new Button();
            statusStripMain.SuspendLayout();
            panelLogin.SuspendLayout();
            SuspendLayout();
            // 
            // statusStripMain
            // 
            statusStripMain.Dock = DockStyle.Top;
            statusStripMain.Items.AddRange(new ToolStripItem[] { SyncLabel });
            statusStripMain.Location = new Point(0, 0);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.RenderMode = ToolStripRenderMode.Professional;
            statusStripMain.Size = new Size(784, 22);
            statusStripMain.TabIndex = 0;
            statusStripMain.Text = "statusStrip1";
            // 
            // SyncLabel
            // 
            SyncLabel.Name = "SyncLabel";
            SyncLabel.Size = new Size(158, 17);
            SyncLabel.Text = "Status: Node Not Connected";
            // 
            // timerUpdate
            // 
            timerUpdate.Enabled = true;
            timerUpdate.Interval = 10000;
            timerUpdate.Tick += timerUpdate_Tick;
            // 
            // panelLogin
            // 
            panelLogin.Controls.Add(labelWelcome);
            panelLogin.Controls.Add(textBoxPassword);
            panelLogin.Controls.Add(labelPassword);
            panelLogin.Controls.Add(LoginButton);
            panelLogin.Location = new Point(776, 443);
            panelLogin.Name = "panelLogin";
            panelLogin.Size = new Size(784, 428);
            panelLogin.TabIndex = 1;
            // 
            // labelWelcome
            // 
            labelWelcome.AutoSize = true;
            labelWelcome.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            labelWelcome.Location = new Point(341, 143);
            labelWelcome.Name = "labelWelcome";
            labelWelcome.Size = new Size(118, 25);
            labelWelcome.TabIndex = 3;
            labelWelcome.Text = "Please Login";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxPassword.Location = new Point(384, 201);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(137, 29);
            textBoxPassword.TabIndex = 2;
            textBoxPassword.UseSystemPasswordChar = true;
            textBoxPassword.KeyDown += textBoxPassword_KeyDown;
            // 
            // labelPassword
            // 
            labelPassword.AutoSize = true;
            labelPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            labelPassword.Location = new Point(299, 204);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(79, 21);
            labelPassword.TabIndex = 1;
            labelPassword.Text = "Password:";
            // 
            // LoginButton
            // 
            LoginButton.FlatStyle = FlatStyle.Flat;
            LoginButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            LoginButton.Location = new Point(335, 300);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(130, 37);
            LoginButton.TabIndex = 0;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += Login_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 450);
            Controls.Add(panelLogin);
            Controls.Add(statusStripMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            Text = "MainForm";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            statusStripMain.ResumeLayout(false);
            statusStripMain.PerformLayout();
            panelLogin.ResumeLayout(false);
            panelLogin.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private StatusStrip statusStripMain;
        private ToolStripStatusLabel SyncLabel;
        private System.Windows.Forms.Timer timerUpdate;
        private Panel panelLogin;
        private Button LoginButton;
        private Label labelWelcome;
        private TextBox textBoxPassword;
        private Label labelPassword;
    }
}
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
            groupBoxLogin = new GroupBox();
            labelWelcome = new Label();
            textBoxPassword = new TextBox();
            labelPassword = new Label();
            LoginButton = new Button();
            panelPortfolio = new Panel();
            buttonRenew = new Button();
            groupBoxDomains = new GroupBox();
            panelDomainList = new Panel();
            panelNav = new Panel();
            buttonSend = new Button();
            buttonReceive = new Button();
            groupBoxAccount = new GroupBox();
            labelDomains = new Label();
            labelBalance = new Label();
            statusStripMain.SuspendLayout();
            panelLogin.SuspendLayout();
            groupBoxLogin.SuspendLayout();
            panelPortfolio.SuspendLayout();
            groupBoxDomains.SuspendLayout();
            panelNav.SuspendLayout();
            groupBoxAccount.SuspendLayout();
            SuspendLayout();
            // 
            // statusStripMain
            // 
            statusStripMain.Dock = DockStyle.Top;
            statusStripMain.Items.AddRange(new ToolStripItem[] { SyncLabel });
            statusStripMain.Location = new Point(0, 0);
            statusStripMain.Name = "statusStripMain";
            statusStripMain.Size = new Size(1099, 22);
            statusStripMain.SizingGrip = false;
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
            panelLogin.Controls.Add(groupBoxLogin);
            panelLogin.Location = new Point(1081, 556);
            panelLogin.Name = "panelLogin";
            panelLogin.Size = new Size(1099, 558);
            panelLogin.TabIndex = 1;
            // 
            // groupBoxLogin
            // 
            groupBoxLogin.Controls.Add(labelWelcome);
            groupBoxLogin.Controls.Add(textBoxPassword);
            groupBoxLogin.Controls.Add(labelPassword);
            groupBoxLogin.Controls.Add(LoginButton);
            groupBoxLogin.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxLogin.Location = new Point(279, 109);
            groupBoxLogin.Name = "groupBoxLogin";
            groupBoxLogin.Size = new Size(450, 250);
            groupBoxLogin.TabIndex = 4;
            groupBoxLogin.TabStop = false;
            groupBoxLogin.Text = "Login";
            // 
            // labelWelcome
            // 
            labelWelcome.AutoSize = true;
            labelWelcome.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            labelWelcome.Location = new Point(119, 25);
            labelWelcome.Name = "labelWelcome";
            labelWelcome.Size = new Size(212, 25);
            labelWelcome.TabIndex = 3;
            labelWelcome.Text = "Login to access account";
            // 
            // textBoxPassword
            // 
            textBoxPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            textBoxPassword.Location = new Point(209, 70);
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
            labelPassword.Location = new Point(114, 73);
            labelPassword.Name = "labelPassword";
            labelPassword.Size = new Size(79, 21);
            labelPassword.TabIndex = 1;
            labelPassword.Text = "Password:";
            // 
            // LoginButton
            // 
            LoginButton.FlatStyle = FlatStyle.Flat;
            LoginButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            LoginButton.Location = new Point(160, 190);
            LoginButton.Name = "LoginButton";
            LoginButton.Size = new Size(130, 37);
            LoginButton.TabIndex = 0;
            LoginButton.Text = "Login";
            LoginButton.UseVisualStyleBackColor = true;
            LoginButton.Click += Login_Click;
            // 
            // panelPortfolio
            // 
            panelPortfolio.Controls.Add(buttonRenew);
            panelPortfolio.Controls.Add(groupBoxDomains);
            panelPortfolio.Controls.Add(panelNav);
            panelPortfolio.Controls.Add(groupBoxAccount);
            panelPortfolio.Location = new Point(0, 22);
            panelPortfolio.Name = "panelPortfolio";
            panelPortfolio.Size = new Size(1052, 529);
            panelPortfolio.TabIndex = 2;
            panelPortfolio.Visible = false;
            // 
            // buttonRenew
            // 
            buttonRenew.Enabled = false;
            buttonRenew.FlatStyle = FlatStyle.Flat;
            buttonRenew.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            buttonRenew.Location = new Point(102, 117);
            buttonRenew.Name = "buttonRenew";
            buttonRenew.Size = new Size(299, 49);
            buttonRenew.TabIndex = 3;
            buttonRenew.Text = "Renew 0 domains";
            buttonRenew.UseVisualStyleBackColor = true;
            buttonRenew.Click += buttonRenew_Click;
            // 
            // groupBoxDomains
            // 
            groupBoxDomains.Controls.Add(panelDomainList);
            groupBoxDomains.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxDomains.Location = new Point(407, 3);
            groupBoxDomains.Name = "groupBoxDomains";
            groupBoxDomains.Size = new Size(642, 523);
            groupBoxDomains.TabIndex = 2;
            groupBoxDomains.TabStop = false;
            groupBoxDomains.Text = "Domains";
            // 
            // panelDomainList
            // 
            panelDomainList.AutoScroll = true;
            panelDomainList.Dock = DockStyle.Fill;
            panelDomainList.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            panelDomainList.Location = new Point(3, 28);
            panelDomainList.Name = "panelDomainList";
            panelDomainList.Size = new Size(636, 492);
            panelDomainList.TabIndex = 0;
            // 
            // panelNav
            // 
            panelNav.Controls.Add(buttonSend);
            panelNav.Controls.Add(buttonReceive);
            panelNav.Dock = DockStyle.Left;
            panelNav.Location = new Point(0, 0);
            panelNav.Name = "panelNav";
            panelNav.Size = new Size(96, 529);
            panelNav.TabIndex = 1;
            // 
            // buttonSend
            // 
            buttonSend.FlatStyle = FlatStyle.Flat;
            buttonSend.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            buttonSend.Location = new Point(3, 45);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(90, 36);
            buttonSend.TabIndex = 1;
            buttonSend.Text = "Send";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // buttonReceive
            // 
            buttonReceive.FlatStyle = FlatStyle.Flat;
            buttonReceive.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            buttonReceive.Location = new Point(3, 3);
            buttonReceive.Name = "buttonReceive";
            buttonReceive.Size = new Size(90, 36);
            buttonReceive.TabIndex = 0;
            buttonReceive.Text = "Receive";
            buttonReceive.UseVisualStyleBackColor = true;
            buttonReceive.Click += buttonReceive_Click;
            // 
            // groupBoxAccount
            // 
            groupBoxAccount.Controls.Add(labelDomains);
            groupBoxAccount.Controls.Add(labelBalance);
            groupBoxAccount.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            groupBoxAccount.Location = new Point(102, 3);
            groupBoxAccount.Name = "groupBoxAccount";
            groupBoxAccount.Size = new Size(299, 108);
            groupBoxAccount.TabIndex = 0;
            groupBoxAccount.TabStop = false;
            groupBoxAccount.Text = "Account";
            // 
            // labelDomains
            // 
            labelDomains.AutoSize = true;
            labelDomains.Location = new Point(9, 65);
            labelDomains.Name = "labelDomains";
            labelDomains.Size = new Size(127, 25);
            labelDomains.TabIndex = 1;
            labelDomains.Text = "labelDomains";
            // 
            // labelBalance
            // 
            labelBalance.AutoSize = true;
            labelBalance.Location = new Point(9, 28);
            labelBalance.Name = "labelBalance";
            labelBalance.Size = new Size(79, 25);
            labelBalance.TabIndex = 0;
            labelBalance.Text = "labelBal";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1099, 580);
            Controls.Add(panelLogin);
            Controls.Add(panelPortfolio);
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
            groupBoxLogin.ResumeLayout(false);
            groupBoxLogin.PerformLayout();
            panelPortfolio.ResumeLayout(false);
            groupBoxDomains.ResumeLayout(false);
            panelNav.ResumeLayout(false);
            groupBoxAccount.ResumeLayout(false);
            groupBoxAccount.PerformLayout();
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
        private Panel panelPortfolio;
        private GroupBox groupBoxAccount;
        private GroupBox groupBoxLogin;
        private Label labelBalance;
        private Label labelDomains;
        private Panel panelNav;
        private Button buttonReceive;
        private GroupBox groupBoxDomains;
        private Panel panelDomainList;
        private Button buttonRenew;
        private Button buttonSend;
    }
}
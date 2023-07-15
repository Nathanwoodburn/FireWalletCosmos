using System.Diagnostics;
using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite
{
    public partial class MainForm : Form
    {
        #region Constants and Config
        // Directory to store files including logs, theme and hsd node
        public string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\"
            + Application.ProductName.Trim().Replace(" ", "") + "\\";

        // How many days to check for domain exiries. If domain will expire in less than this, prompt user to renew.
        public int daysToExpire = 90;

        // Explorer URLs for transactions and domains
        public string TXExplorer = "https://niami.io/tx/";
        public string DomainExplorer = "https://niami.io/domain/";

        // Links to show in help dropdown menu
        public Dictionary<string, string> HelpLinks = new Dictionary<string, string>()
        {
            { "Discord", "https://l.woodburn.au/discord" },
            { "Separator" ,""},
            { "Github", "https://github.com/nathanwoodburn/FireWalletLite" }
        };

        #endregion
        #region Variables
        public Dictionary<string, string> Theme { get; set; }
        HttpClient httpClient = new HttpClient();
        Decimal Balance { get; set; }
        public String Account = "primary";
        public String Password { get; set; }
        #endregion
        public MainForm()
        {
            InitializeComponent();
            UpdateTheme();
            this.Text = Application.ProductName;
            foreach (KeyValuePair<string, string> link in HelpLinks)
            {
                if (link.Key == "Separator")
                {
                    DropDownHelp.DropDownItems.Add(new ToolStripSeparator());
                    continue;
                }
                ToolStripMenuItem tsmi = new ToolStripMenuItem(link.Key);
                tsmi.Click += (s, e) =>
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = link.Value,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                };
                DropDownHelp.DropDownItems.Add(tsmi);
            }
            DropDownHelp.Margin = new Padding(statusStripMain.Width - DropDownHelp.Width - SyncLabel.Width - 20, 0, 0, 0);
        }
        #region Theming
        private void UpdateTheme()
        {
            // Check if file exists
            if (!Directory.Exists(dir))
            {
                CreateConfig(dir);
            }
            if (!File.Exists(dir + "theme.txt"))
            {
                CreateConfig(dir);
            }

            // Read file
            StreamReader sr = new StreamReader(dir + "theme.txt");
            Theme = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] split = line.Split(':');
                Theme.Add(split[0].Trim(), split[1].Trim());
            }
            sr.Dispose();

            if (!Theme.ContainsKey("background") || !Theme.ContainsKey("background-alt") || !Theme.ContainsKey("foreground") || !Theme.ContainsKey("foreground-alt"))
            {
                AddLog("Theme file is missing key");
                return;
            }

            // Apply theme
            this.BackColor = ColorTranslator.FromHtml(Theme["background"]);

            // Foreground
            this.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);


            // Need to specify this for each groupbox to override the black text
            foreach (Control c in Controls)
            {
                ThemeControl(c);
            }
            this.Width = Screen.PrimaryScreen.Bounds.Width / 5 * 3;
            this.Height = Screen.PrimaryScreen.Bounds.Height / 5 * 3;
        }
        public void ThemeControl(Control c)
        {
            if (c.GetType() == typeof(GroupBox) || c.GetType() == typeof(Panel))
            {
                c.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
                foreach (Control sub in c.Controls)
                {
                    ThemeControl(sub);
                }
            }
            if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(Button)
                || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(StatusStrip) || c.GetType() == typeof(ToolStrip)
                || c.GetType() == typeof(NumericUpDown))
            {
                c.ForeColor = ColorTranslator.FromHtml(Theme["foreground-alt"]);
                c.BackColor = ColorTranslator.FromHtml(Theme["background-alt"]);
            }
            if (c.GetType() == typeof(Panel)) c.Dock = DockStyle.Fill;
        }
        private void CreateConfig(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            StreamWriter sw = new StreamWriter(dir + "theme.txt");
            sw.WriteLine("background: #000000");
            sw.WriteLine("foreground: #8e05c2");
            sw.WriteLine("background-alt: #3e065f");
            sw.WriteLine("foreground-alt: #ffffff");
            sw.WriteLine("error: #ff0000");
            sw.Dispose();
            AddLog("Created theme file");
        }
        #endregion
        #region Logging
        public void AddLog(string message)
        {
            if (message.Contains("Get Error: No connection could be made because the target machine actively refused it")) return;

            // If file size is over 1MB, rename it to old.log.txt
            if (File.Exists(dir + "log.txt"))
            {
                FileInfo fi = new FileInfo(dir + "log.txt");
                if (fi.Length > 1000000)
                {
                    if (File.Exists(dir + "old.log.txt")) File.Delete(dir + "old.log.txt"); // Delete old log file as it is super old
                    File.Move(dir + "log.txt", dir + "old.log.txt");
                }
            }

            StreamWriter sw = new StreamWriter(dir + "log.txt", true);
            sw.WriteLine(DateTime.Now.ToString() + ": " + message);
            sw.Dispose();
        }
        #endregion
        bool testedLogin = false;
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            if (SyncLabel.Text != "Status: Node Not Connected")
            {
                if (!testedLogin)
                {
                    testedLogin = true;
                    this.Hide();
                    TestForLogin();
                    this.Show();
                }
            }
            NodeStatus();
        }

        #region API
        private async void NodeStatus()
        {
            if (await APIGet("", false) == "Error")
            {
                SyncLabel.Text = "Status: Node Not Connected";
                return;
            }
            else
            {
                // Get sync status
                String APIresponse = await APIGet("", false);
                JObject resp = JObject.Parse(APIresponse);
                JObject chain = JObject.Parse(resp["chain"].ToString());
                decimal progress = Convert.ToDecimal(chain["progress"].ToString());
                SyncLabel.Text = "Sync: " + decimal.Round(progress * 100, 2) + "%";
                if (progress < 1)
                {
                    LabelSyncWarning.Visible = true;
                    DropDownHelp.Margin = new Padding(statusStripMain.Width - DropDownHelp.Width - SyncLabel.Width - LabelSyncWarning.Width - 20, 0, 0, 0);
                }
                else
                {
                    LabelSyncWarning.Visible = false;
                    DropDownHelp.Margin = new Padding(statusStripMain.Width - DropDownHelp.Width - SyncLabel.Width - 20, 0, 0, 0);
                }

            }
            // Try to keep wallet unlocked
            string path = "wallet/" + Account + "/unlock";
            string content = "{\"passphrase\": \"" + Password + "\",\"timeout\": 60}";
            await APIPost(path, true, content);
            path = "";
            content = "{\"method\": \"selectwallet\",\"params\":[ \"" + Account + "\"]}";

            await APIPost(path, true, content);

        }
        private async Task UpdateBalance()
        {
            string response = await APIGet("wallet/" + Account + "/balance?account=default", true);
            if (response == "Error") return;

            JObject resp = JObject.Parse(response);

            decimal available = (Convert.ToDecimal(resp["unconfirmed"].ToString()) - Convert.ToDecimal(resp["lockedUnconfirmed"].ToString())) / 1000000;
            decimal locked = Convert.ToDecimal(resp["lockedUnconfirmed"].ToString()) / 1000000;
            available = decimal.Round(available, 2);
            locked = decimal.Round(locked, 2);
            Balance = available;
            labelBalance.Text = "Balance: " + available + " HNS";

            // Get domain count
            UpdateDomains();

        }
        public async Task<string> APIPost(string path, bool wallet, string content)
        {
            if (content == "{\"passphrase\": \"\",\"timeout\": 60}")
            {
                return "";
            }
            string ip = "127.0.0.1";
            string port = "1203";
            if (wallet) port = port + "9";
            else port = port + "7";
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "http://" + ip + ":" + port + "/" + path);
            //req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("x:" + key)));
            req.Content = new StringContent(content);
            // Send request
            try
            {
                HttpResponseMessage resp = await httpClient.SendAsync(req);
                if (!resp.IsSuccessStatusCode)
                {
                    AddLog("Post Error: " + resp.StatusCode);
                    AddLog(await resp.Content.ReadAsStringAsync());
                    return "Error";
                }
                return await resp.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                AddLog("Post Error: " + ex.Message);
                if (ex.Message.Contains("The request was canceled due to the configured HttpClient.Timeout"))
                {
                    Environment.Exit(91);
                }
                return "Error";
            }
        }
        public async Task<string> APIGet(string path, bool wallet)
        {
            string ip = "127.0.0.1";

            string port = "1203";
            if (wallet) port = port + "9";
            else port = port + "7";
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://" + ip + ":" + port + "/" + path);
                // Add API key to header
                //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("x:" + key)));
                // Send request and log response
                HttpResponseMessage response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    AddLog("Get Error: " + response.StatusCode);
                    AddLog(await response.Content.ReadAsStringAsync());
                    return "Error";
                }
                return await response.Content.ReadAsStringAsync();

            }
            // Log errors to log textbox
            catch (Exception ex)
            {
                AddLog("Get Error: " + ex.Message);
                if (ex.Message.Contains("The request was canceled due to the configured HttpClient.Timeout"))
                {
                    Environment.Exit(91);
                }
                return "Error";
            }
        }
        public async Task<bool> ValidAddress(string address)
        {
            string output = await APIPost("", false, "{\"method\": \"validateaddress\",\"params\": [ \"" + address + "\" ]}");
            JObject APIresp = JObject.Parse(output);
            JObject result = JObject.Parse(APIresp["result"].ToString());
            if (result["isvalid"].ToString() == "True") return true;
            else return false;
        }
        public string[] Domains { get; set; }
        public string[] DomainsRenewable { get; set; }
        private async void UpdateDomains()
        {
            string response = await APIGet("wallet/" + Account + "/name?own=true", true);

            try
            {
                JArray names = JArray.Parse(response);
                Domains = new string[names.Count];
                DomainsRenewable = new string[names.Count];
                int i = 0;
                int renewable = 0;
                // Sort by Alphabetic order
                names = new JArray(names.OrderBy(obj => (string)obj["name"]));
                panelDomainList.Controls.Clear();

                // If no domains, add label and return
                if (names.Count == 0)
                {
                    Label noDomainsLabel = new Label();
                    noDomainsLabel.Text = "No domains yet.\nPlease note domain transfers take at least 2 days";
                    noDomainsLabel.TextAlign = ContentAlignment.MiddleCenter;
                    noDomainsLabel.AutoSize = true;
                    panelDomainList.Controls.Add(noDomainsLabel);
                    noDomainsLabel.Left = panelDomainList.Width / 2 - noDomainsLabel.Width / 2;
                    noDomainsLabel.Top = 10;
                }

                foreach (JObject name in names)
                {
                    Domains[i] = name["name"].ToString();
                    Panel domainTMP = new Panel();
                    domainTMP.Width = panelDomainList.Width - 20 - SystemInformation.VerticalScrollBarWidth;
                    domainTMP.Height = 30;
                    domainTMP.Top = 30 * (i);
                    domainTMP.Left = 10;
                    domainTMP.BorderStyle = BorderStyle.FixedSingle;

                    Label domainName = new Label();
                    domainName.Text = Domains[i];
                    domainName.Top = 5;
                    domainName.Left = 5;
                    domainName.AutoSize = true;
                    domainTMP.Controls.Add(domainName);

                    if (!name.ContainsKey("stats"))
                    {
                        AddLog("Domain " + Domains[i] + " does not have stats");
                        continue;
                    }
                    Label expiry = new Label();
                    JObject stats = JObject.Parse(name["stats"].ToString());
                    if (stats.ContainsKey("daysUntilExpire"))
                    {
                        expiry.Text = "Expires: " + stats["daysUntilExpire"].ToString() + " days";
                        expiry.Top = 5;
                        expiry.AutoSize = true;
                        expiry.Left = domainTMP.Width - expiry.Width - 100;
                        domainTMP.Controls.Add(expiry);

                        // Add to domains renewable if less than set days
                        decimal days = decimal.Parse(stats["daysUntilExpire"].ToString());
                        if (days <= daysToExpire)
                        {
                            DomainsRenewable[renewable] = Domains[i];
                            renewable++;
                        }
                    }
                    else
                    {
                        expiry.Text = "Expires: Not Registered yet";
                        expiry.Top = 5;
                        expiry.AutoSize = true;
                        expiry.Left = domainTMP.Width - expiry.Width - 100;
                        domainTMP.Controls.Add(expiry);
                    }
                    // On Click open domain

                    domainTMP.Click += new EventHandler((sender, e) =>
                    {
                        DomainForm domainForm = new DomainForm(this, name["name"].ToString());
                        domainForm.Show();
                    });
                    foreach (Control c in domainTMP.Controls)
                    {
                        c.Click += new EventHandler((sender, e) =>
                        {
                            DomainForm domainForm = new DomainForm(this, name["name"].ToString());
                            domainForm.Show();
                        });
                    }
                    panelDomainList.Controls.Add(domainTMP);
                    i++;
                }
                labelDomains.Text = "Domains: " + names.Count;

                if (renewable > 0)
                {
                    buttonRenew.Text = "Renew " + renewable + " domains";
                    buttonRenew.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                AddLog("Error getting domains");
                AddLog(ex.Message);
            }
        }
        #endregion
        private void MainForm_Load(object sender, EventArgs e)
        {
            groupBoxLogin.Left = (this.ClientSize.Width - groupBoxLogin.Width) / 2;
            groupBoxLogin.Top = (this.ClientSize.Height - groupBoxLogin.Height) / 2;
            pictureBoxLogo.Height = groupBoxLogin.Top - 20;
            pictureBoxLogo.Width = pictureBoxLogo.Height;
            pictureBoxLogo.Top = 10;
            pictureBoxLogo.Left = (this.ClientSize.Width - pictureBoxLogo.Width) / 2;
            this.TopMost = true;
            textBoxPassword.Focus();
            this.TopMost = false;
        }
        private async void TestForLogin()
        {
            string path = "wallet/" + Account + "/master";
            string response = await APIGet(path, true);
            JObject resp = JObject.Parse(response);
            if (resp["encrypted"].ToString() == "False")
            {
                JObject mnemonic = JObject.Parse(resp["mnemonic"].ToString());
                string phrase = mnemonic["phrase"].ToString();

                // Show form to save mnemonic and encrypt wallet
                FirstLoginForm firstLoginForm = new FirstLoginForm(phrase, this);
                firstLoginForm.ShowDialog();
                firstLoginForm.Dispose();
            }
        }
        private async void Login_Click(object sender, EventArgs e)
        {
            LoginButton.Enabled = false; // To prevent double clicking
            Password = textBoxPassword.Text;

            string path = "wallet/" + Account + "/unlock";
            string content = "{\"passphrase\": \"" + Password + "\",\"timeout\": 60}";

            string response = await APIPost(path, true, content);
            if (response == "Error")
            {
                Password = "";
                NotifyForm notifyForm = new NotifyForm("Incorrect Password");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                LoginButton.Enabled = true;
                return;
            }
            groupBoxDomains.Width = this.Width - groupBoxDomains.Left - 20;
            await UpdateBalance();
            panelLogin.Hide();
            panelNav.Dock = DockStyle.Left;
            panelPortfolio.Show();

            // Some UI stuff
            groupBoxAccount.Top = statusStripMain.Height + 10;
            groupBoxDomains.Top = statusStripMain.Height + 10;
            groupBoxDomains.Height = this.Height - groupBoxDomains.Top - 40;
            buttonReceive.Top = statusStripMain.Height + 10;
            buttonSend.Top = buttonReceive.Top + buttonReceive.Height + 10;
            buttonRenew.Top = groupBoxAccount.Top + groupBoxAccount.Height + 10;
            groupBoxHistory.Top = buttonRenew.Top + buttonRenew.Height + 10;
            groupBoxHistory.Height = this.Height - groupBoxHistory.Top - 40;
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Run taskkill /im "node.exe" /f /t
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "taskkill.exe";
            startInfo.Arguments = "/im \"node.exe\" /f /t";
            startInfo.CreateNoWindow = true;
            Process.Start(startInfo);
            Environment.Exit(0);
        }

        private void textBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                Login_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private async void buttonReceive_Click(object sender, EventArgs e)
        {
            string path = "wallet/" + Account + "/address";
            string content = "{\"account\": \"default\"}";
            string response = await APIPost(path, true, content);
            if (response == "Error")
            {
                NotifyForm notifyForm = new NotifyForm("Error getting address");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            JObject resp = JObject.Parse(response);
            string address = resp["address"].ToString();

            ReceiveForm receiveForm = new ReceiveForm(address, this);
            receiveForm.ShowDialog();
            receiveForm.Dispose();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            SendForm sendForm = new SendForm(Balance, this);
            sendForm.ShowDialog();
            sendForm.Dispose();
        }

        private async void buttonRenew_Click(object sender, EventArgs e)
        {
            Batch[] batches = new Batch[DomainsRenewable.Length];
            for (int i = 0; i < DomainsRenewable.Length; i++)
            {
                batches[i] = new Batch(DomainsRenewable[i], "RENEW");
            }

            string batchTX = "[" + string.Join(", ", batches.Select(batch => batch.ToString())) + "]";
            string content = "{\"method\": \"sendbatch\",\"params\":[ " + batchTX + "]}";
            string response = await APIPost("", true, content);

            if (response == "Error")
            {
                AddLog("Error sending renewals");
                NotifyForm notifyForm = new NotifyForm("Error sending renewals");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }

            JObject jObject = JObject.Parse(response);
            if (jObject["error"].ToString() != "")
            {
                AddLog("Error: ");
                AddLog(jObject["error"].ToString());
                if (jObject["error"].ToString().Contains("Batch output addresses would exceed lookahead"))
                {
                    NotifyForm notifyForm = new NotifyForm("Error: \nBatch output addresses would exceed lookahead\nYour batch might have too many TXs.");
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                else
                {
                    NotifyForm notifyForm = new NotifyForm("Error: \n" + jObject["error"].ToString());
                    notifyForm.ShowDialog();
                    notifyForm.Dispose();
                }
                return;
            }
            JObject result = JObject.Parse(jObject["result"].ToString());
            string hash = result["hash"].ToString();
            AddLog("Batch sent with hash: " + hash);
            NotifyForm notifyForm2 = new NotifyForm("Renewals sent\nThis might take a while to mine.", "Explorer", TXExplorer + hash);
            notifyForm2.ShowDialog();
            notifyForm2.Dispose();
        }
    }
    public class Batch
    {
        public string domain { get; }
        public string method { get; }
        public decimal bid { get; }
        public decimal lockup { get; }
        public string toAddress { get; }
        //public DNS[]? update { get; }
        public Batch(string domain, string method) // Normal TXs
        {
            this.domain = domain;
            this.method = method;
            bid = 0;
            lockup = 0;
            toAddress = "";
            //update = null;
        }
        public Batch(string domain, string method, string toAddress) // Transfers
        {
            this.domain = domain;
            this.method = method;
            this.toAddress = toAddress;
            bid = 0;
            lockup = 0;
            //update = null;
        }
        public Batch(string domain, string method, decimal bid, decimal lockup) // Bids
        {
            this.domain = domain;
            this.method = method;
            this.bid = bid;
            this.lockup = lockup;
            toAddress = "";
            //update = null;
        }
        // DNS Update not implemented yet
        /*
        public Batch(string domain, string method, DNS[] update) // DNS Update
        {
            this.domain = domain;
            this.method = method;
            this.update = update;

        }*/
        public override string ToString()
        {
            if (method == "BID")
            {
                return "[\"BID\", \"" + domain + "\", " + bid + ", " + lockup + "]";
            }
            else if (method == "TRANSFER")
            {
                return "[\"TRANSFER\", \"" + domain + "\", \"" + toAddress + "\"]";
            }
            /*else if (method == "UPDATE" && update != null)
            {

                string records = "{\"records\":[" + string.Join(", ", update.Select(record => record.ToString())) + "]}";
                return "[\"UPDATE\", \"" + domain + "\", " + records + "]";

            }
            else if (method == "UPDATE")
            {
                return "[\"UPDATE\", \"" + domain + "\", {\"records\":[]}]";
            }*/
            return "[\"" + method + "\", \"" + domain + "\"]";
        }
    }
}

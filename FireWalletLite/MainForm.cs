using System.Diagnostics;
using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite;

public partial class MainForm : Form
{
    private bool testedLogin;

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
    public Dictionary<string, string> HelpLinks = new()
    {
        { "Discord", "https://l.woodburn.au/discord" },
        { "Separator", "" },
        { "Github", "https://github.com/nathanwoodburn/FireWalletLite" }
    };

    // Min Confirmations for transactions to be considered valid
    public int MinConfirmations = 1;

    public Dictionary<string, string> Theme { get; set; } = new()
    {
        { "background", "#000000" },
        { "foreground", "#fcba03"},
        { "background-alt", "#3e065f"},
        { "foreground-alt", "#ffffff"}
    };

    #endregion

    #region Variables
    private readonly HttpClient httpClient = new();
    private decimal Balance { get; set; }
    public string Account = "primary";
    public string Password { get; set; }

    #endregion

    public MainForm()
    {
        InitializeComponent();
        UpdateTheme();
        Text = Application.ProductName;
        foreach (var link in HelpLinks)
        {
            if (link.Key == "Separator")
            {
                DropDownHelp.DropDownItems.Add(new ToolStripSeparator());
                continue;
            }

            var tsmi = new ToolStripMenuItem(link.Key);
            tsmi.Click += (s, e) =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = link.Value,
                    UseShellExecute = true
                };
                Process.Start(psi);
            };
            DropDownHelp.DropDownItems.Add(tsmi);
        }

        DropDownHelp.Margin =
            new Padding(statusStripMain.Width - DropDownHelp.Width - SyncLabel.Width - 20, 0, 0, 0);
    }

    #region Logging

    public void AddLog(string message)
    {
        if (message.Contains(
                "Get Error: No connection could be made because the target machine actively refused it")) return;

        // If file size is over 1MB, rename it to old.log.txt
        if (File.Exists(dir + "log.txt"))
        {
            var fi = new FileInfo(dir + "log.txt");
            if (fi.Length > 1000000)
            {
                if (File.Exists(dir + "old.log.txt"))
                    File.Delete(dir + "old.log.txt"); // Delete old log file as it is super old
                File.Move(dir + "log.txt", dir + "old.log.txt");
            }
        }

        var sw = new StreamWriter(dir + "log.txt", true);
        sw.WriteLine(DateTime.Now + ": " + message);
        sw.Dispose();
    }

    #endregion

    private void timerUpdate_Tick(object sender, EventArgs e)
    {
        if (SyncLabel.Text != "Status: Node Not Connected")
            if (!testedLogin)
            {
                testedLogin = true;
                Hide();
                TestForLogin();
                Show();
            }

        NodeStatus();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        groupBoxLogin.Left = (ClientSize.Width - groupBoxLogin.Width) / 2;
        groupBoxLogin.Top = (ClientSize.Height - groupBoxLogin.Height) / 2;
        pictureBoxLogo.Height = groupBoxLogin.Top - 20;
        pictureBoxLogo.Width = pictureBoxLogo.Height;
        pictureBoxLogo.Top = 10;
        pictureBoxLogo.Left = (ClientSize.Width - pictureBoxLogo.Width) / 2;
        TopMost = true;
        textBoxPassword.Focus();
        TopMost = false;
    }

    private async void TestForLogin()
    {
        var path = "wallet/" + Account + "/master";
        var response = await APIGet(path, true);
        var resp = JObject.Parse(response);
        if (resp["encrypted"].ToString() == "False")
        {
            var mnemonic = JObject.Parse(resp["mnemonic"].ToString());
            var phrase = mnemonic["phrase"].ToString();

            // Show form to save mnemonic and encrypt wallet
            var firstLoginForm = new FirstLoginForm(phrase, this);
            firstLoginForm.ShowDialog();
            firstLoginForm.Dispose();
        }
    }

    private async void Login_Click(object sender, EventArgs e)
    {
        LoginButton.Enabled = false; // To prevent double clicking
        Password = textBoxPassword.Text;

        var path = "wallet/" + Account + "/unlock";
        var content = "{\"passphrase\": \"" + Password + "\",\"timeout\": 60}";

        var response = await APIPost(path, true, content);
        if (response == "Error")
        {
            Password = "";
            var notifyForm = new NotifyForm("Incorrect Password");
            notifyForm.ShowDialog();
            notifyForm.Dispose();
            LoginButton.Enabled = true;
            return;
        }

        groupBoxDomains.Width = Width - groupBoxDomains.Left - 20;
        await UpdateBalance();
        await GetTXHistory();
        panelLogin.Hide();
        panelNav.Dock = DockStyle.Left;
        panelPortfolio.Show();

        // Some UI stuff
        groupBoxAccount.Top = statusStripMain.Height + 10;
        groupBoxDomains.Top = statusStripMain.Height + 10;
        groupBoxDomains.Height = Height - groupBoxDomains.Top - 40;
        buttonReceive.Top = statusStripMain.Height + 10;
        buttonSend.Top = buttonReceive.Top + buttonReceive.Height + 10;
        buttonRenew.Top = groupBoxAccount.Top + groupBoxAccount.Height + 10;
        groupBoxHistory.Top = buttonRenew.Top + buttonRenew.Height + 10;
        groupBoxHistory.Height = Height - groupBoxHistory.Top - 40;
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        // Run taskkill /im "node.exe" /f /t
        var startInfo = new ProcessStartInfo();
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
        var path = "wallet/" + Account + "/address";
        var content = "{\"account\": \"default\"}";
        var response = await APIPost(path, true, content);
        if (response == "Error")
        {
            var notifyForm = new NotifyForm("Error getting address");
            notifyForm.ShowDialog();
            notifyForm.Dispose();
            return;
        }

        var resp = JObject.Parse(response);
        var address = resp["address"].ToString();

        var receiveForm = new ReceiveForm(address, this);
        receiveForm.ShowDialog();
        receiveForm.Dispose();
    }

    private void buttonSend_Click(object sender, EventArgs e)
    {
        var sendForm = new SendForm(Balance, this);
        sendForm.ShowDialog();
        sendForm.Dispose();
    }

    private async void buttonRenew_Click(object sender, EventArgs e)
    {
        var batches = new Batch[DomainsRenewable.Length];
        for (var i = 0; i < DomainsRenewable.Length; i++) batches[i] = new Batch(DomainsRenewable[i], "RENEW");

        var batchTX = "[" + string.Join(", ", batches.Select(batch => batch.ToString())) + "]";
        var content = "{\"method\": \"sendbatch\",\"params\":[ " + batchTX + "]}";
        var response = await APIPost("", true, content);

        if (response == "Error")
        {
            AddLog("Error sending renewals");
            var notifyForm = new NotifyForm("Error sending renewals");
            notifyForm.ShowDialog();
            notifyForm.Dispose();
            return;
        }

        var jObject = JObject.Parse(response);
        if (jObject["error"].ToString() != "")
        {
            AddLog("Error: ");
            AddLog(jObject["error"].ToString());
            if (jObject["error"].ToString().Contains("Batch output addresses would exceed lookahead"))
            {
                var notifyForm =
                    new NotifyForm(
                        "Error: \nBatch output addresses would exceed lookahead\nYour batch might have too many TXs.");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
            }
            else
            {
                var notifyForm = new NotifyForm("Error: \n" + jObject["error"]);
                notifyForm.ShowDialog();
                notifyForm.Dispose();
            }

            return;
        }

        var result = JObject.Parse(jObject["result"].ToString());
        var hash = result["hash"].ToString();
        AddLog("Batch sent with hash: " + hash);
        var notifyForm2 =
            new NotifyForm("Renewals sent\nThis might take a while to mine.", "Explorer", TXExplorer + hash);
        notifyForm2.ShowDialog();
        notifyForm2.Dispose();
    }

    #region Theming

    private void UpdateTheme()
    {
        // Check if file exists
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

        if (File.Exists(dir + "theme.txt")) // Use custom theme
        {

            // Read file
            var sr = new StreamReader(dir + "theme.txt");
            Theme = new Dictionary<string, string>();
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                var split = line.Split(':');
                Theme.Add(split[0].Trim(), split[1].Trim());
            }

            sr.Dispose();
        }

        if (!Theme.ContainsKey("background") || !Theme.ContainsKey("background-alt") ||
            !Theme.ContainsKey("foreground") || !Theme.ContainsKey("foreground-alt"))
        {
            AddLog("Theme file is missing key");
            return;
        }

        // Apply theme
        BackColor = ColorTranslator.FromHtml(Theme["background"]);

        // Foreground
        ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);


        // Need to specify this for each groupbox to override the black text
        foreach (Control c in Controls) ThemeControl(c);

        Width = Screen.PrimaryScreen.Bounds.Width / 5 * 3;
        Height = Screen.PrimaryScreen.Bounds.Height / 5 * 3;
    }

    public void ThemeControl(Control c)
    {
        if (c.GetType() == typeof(GroupBox) || c.GetType() == typeof(Panel))
        {
            c.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
            foreach (Control sub in c.Controls) ThemeControl(sub);
        }

        if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(Button)
                                           || c.GetType() == typeof(ComboBox) ||
                                           c.GetType() == typeof(StatusStrip) || c.GetType() == typeof(ToolStrip)
                                           || c.GetType() == typeof(NumericUpDown))
        {
            c.ForeColor = ColorTranslator.FromHtml(Theme["foreground-alt"]);
            c.BackColor = ColorTranslator.FromHtml(Theme["background-alt"]);
        }
        if (c.GetType() == typeof(Panel)) c.Dock = DockStyle.Fill;
    }

    #endregion

    #region API

    private async void NodeStatus()
    {
        if (await APIGet("", false) == "Error")
        {
            SyncLabel.Text = "Status: Node Not Connected";
            return;
        }

        // Get sync status
        var APIresponse = await APIGet("", false);
        var resp = JObject.Parse(APIresponse);
        var chain = JObject.Parse(resp["chain"].ToString());
        var progress = Convert.ToDecimal(chain["progress"].ToString());
        SyncLabel.Text = "Sync: " + decimal.Round(progress * 100, 2) + "%";
        if (progress < 1)
        {
            LabelSyncWarning.Visible = true;
            DropDownHelp.Margin =
                new Padding(
                    statusStripMain.Width - DropDownHelp.Width - SyncLabel.Width - LabelSyncWarning.Width - 20,
                    0, 0, 0);
        }
        else
        {
            LabelSyncWarning.Visible = false;
            DropDownHelp.Margin = new Padding(statusStripMain.Width - DropDownHelp.Width - SyncLabel.Width - 20,
                0, 0, 0);
        }

        // Try to keep wallet unlocked
        var path = "wallet/" + Account + "/unlock";
        var content = "{\"passphrase\": \"" + Password + "\",\"timeout\": 60}";
        await APIPost(path, true, content);
        path = "";
        content = "{\"method\": \"selectwallet\",\"params\":[ \"" + Account + "\"]}";

        await APIPost(path, true, content);
    }

    private async Task UpdateBalance()
    {
        var response = await APIGet("wallet/" + Account + "/balance?account=default", true);
        if (response == "Error") return;

        var resp = JObject.Parse(response);

        var available = (Convert.ToDecimal(resp["unconfirmed"].ToString()) -
                         Convert.ToDecimal(resp["lockedUnconfirmed"].ToString())) / 1000000;
        var locked = Convert.ToDecimal(resp["lockedUnconfirmed"].ToString()) / 1000000;
        available = decimal.Round(available, 2);
        locked = decimal.Round(locked, 2);
        Balance = available;
        labelBalance.Text = "Balance: " + available + " HNS";

        // Get domain count
        UpdateDomains();
    }

    public async Task<string> APIPost(string path, bool wallet, string content)
    {
        if (content == "{\"passphrase\": \"\",\"timeout\": 60}") return "";

        var ip = "127.0.0.1";
        var port = "1203";
        if (wallet) port = port + "9";
        else port = port + "7";
        var req = new HttpRequestMessage(HttpMethod.Post, "http://" + ip + ":" + port + "/" + path);
        //req.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("x:" + key)));
        req.Content = new StringContent(content);
        // Send request
        try
        {
            var resp = await httpClient.SendAsync(req);
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
                Environment.Exit(91);

            return "Error";
        }
    }

    public async Task<string> APIGet(string path, bool wallet)
    {
        var ip = "127.0.0.1";

        var port = "1203";
        if (wallet) port = port + "9";
        else port = port + "7";
        try
        {
            var request =
                new HttpRequestMessage(HttpMethod.Get, "http://" + ip + ":" + port + "/" + path);
            // Add API key to header
            //request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("x:" + key)));
            // Send request and log response
            var response = await httpClient.SendAsync(request);
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
                Environment.Exit(91);

            return "Error";
        }
    }

    public async Task<bool> ValidAddress(string address)
    {
        var output = await APIPost("", false,
            "{\"method\": \"validateaddress\",\"params\": [ \"" + address + "\" ]}");
        var APIresp = JObject.Parse(output);
        var result = JObject.Parse(APIresp["result"].ToString());
        if (result["isvalid"].ToString() == "True") return true;
        return false;
    }

    public string[] Domains { get; set; }
    public string[] DomainsRenewable { get; set; }

    private async void UpdateDomains()
    {
        var response = await APIGet("wallet/" + Account + "/name?own=true", true);

        try
        {
            var names = JArray.Parse(response);
            Domains = new string[names.Count];
            DomainsRenewable = new string[names.Count];
            var i = 0;
            var renewable = 0;
            // Sort by Alphabetic order
            names = new JArray(names.OrderBy(obj => (string)obj["name"]));
            panelDomainList.Controls.Clear();

            // If no domains, add label and return
            if (names.Count == 0)
            {
                var noDomainsLabel = new Label();
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
                var domainTMP = new Panel();
                domainTMP.Width = panelDomainList.Width - 20 - SystemInformation.VerticalScrollBarWidth;
                domainTMP.Height = 30;
                domainTMP.Top = 30 * i;
                domainTMP.Left = 10;
                domainTMP.BorderStyle = BorderStyle.FixedSingle;

                var domainName = new Label();
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

                var expiry = new Label();
                var stats = JObject.Parse(name["stats"].ToString());
                if (stats.ContainsKey("daysUntilExpire"))
                {
                    expiry.Text = "Expires: " + stats["daysUntilExpire"] + " days";
                    expiry.Top = 5;
                    expiry.AutoSize = true;
                    expiry.Left = domainTMP.Width - expiry.Width - 100;
                    domainTMP.Controls.Add(expiry);

                    // Add to domains renewable if less than set days
                    var days = decimal.Parse(stats["daysUntilExpire"].ToString());
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

                domainTMP.Click += (sender, e) =>
                {
                    var domainForm = new DomainForm(this, name["name"].ToString());
                    domainForm.Show();
                };
                foreach (Control c in domainTMP.Controls)
                    c.Click += (sender, e) =>
                    {
                        var domainForm = new DomainForm(this, name["name"].ToString());
                        domainForm.Show();
                    };

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

    private async Task GetTXHistory()
    {
        // Check how many TX there are
        var APIresponse = await APIGet("wallet/" + Account, true);
        var wallet = JObject.Parse(APIresponse);
        if (!wallet.ContainsKey("balance"))
        {
            AddLog("GetInfo Error");
            AddLog(APIresponse);
            return;
        }

        var balance = JObject.Parse(wallet["balance"].ToString());
        var TotalTX = Convert.ToInt32(balance["tx"].ToString());
        var toGet = 10;

        if (toGet > TotalTX) toGet = TotalTX;
        var toSkip = TotalTX - toGet;

        // GET TXs
        APIresponse = await APIPost("", true,
            "{\"method\": \"listtransactions\",\"params\": [\"default\"," + toGet + "," + toSkip + "]}");

        if (APIresponse == "Error")
        {
            AddLog("GetInfo Error");
            return;
        }

        var TXGET = JObject.Parse(APIresponse);

        // Check for error
        if (TXGET["error"].ToString() != "")
        {
            AddLog("GetInfo Error");
            AddLog(APIresponse);
            return;
        }

        var txs = JArray.Parse(TXGET["result"].ToString());
        if (toGet > txs.Count)
            toGet = txs.Count; // In case there are less TXs than expected (usually happens when the get TX's fails)
        var tmpControls = new Control[toGet];
        for (var i = 0; i < toGet; i++)
        {
            // Get last tx
            var tx =
                JObject.Parse(await APIGet("wallet/" + Account + "/tx/" + txs[toGet - i - 1]["txid"],
                    true));

            var hash = tx["hash"].ToString();
            var date = tx["mdate"].ToString();

            date = DateTime.Parse(date).ToShortDateString();


            var tmpPanel = new Panel();
            tmpPanel.Width = groupBoxHistory.Width - SystemInformation.VerticalScrollBarWidth - 20;
            tmpPanel.Height = 50;
            tmpPanel.Location = new Point(5, i * 55);
            tmpPanel.BorderStyle = BorderStyle.FixedSingle;
            tmpPanel.BackColor = ColorTranslator.FromHtml(Theme["background-alt"]);
            tmpPanel.ForeColor = ColorTranslator.FromHtml(Theme["foreground-alt"]);
            tmpPanel.Controls.Add(
                new Label
                {
                    Text = "Date: " + date,
                    Location = new Point(10, 5)
                }
            );
            var confirmations = Convert.ToInt32(tx["confirmations"].ToString());
            if (confirmations < MinConfirmations)
            {
                var txPending = new Label
                {
                    Text = "Pending",
                    Location = new Point(100, 5)
                };
                tmpPanel.Controls.Add(txPending);
                txPending.BringToFront();
            }


            var labelHash = new Label
            {
                Text = "Hash: " + hash.Substring(0, 10) + "..." + hash.Substring(hash.Length - 10),
                AutoSize = true,
                Location = new Point(10, 25)
            };

            tmpPanel.Controls.Add(labelHash);
            tmpPanel.Click += (sender, e) =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = TXExplorer + hash,
                    UseShellExecute = true
                };
                Process.Start(psi);
            };
            foreach (Control c in tmpPanel.Controls)
                c.Click += (sender, e) =>
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = TXExplorer + hash,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                };

            tmpControls[i] = tmpPanel;
        }

        groupBoxHistory.Controls.Clear();
        var txPanel = new Panel();
        txPanel.Width = groupBoxHistory.Width - SystemInformation.VerticalScrollBarWidth;
        txPanel.Controls.AddRange(tmpControls);
        txPanel.AutoScroll = true;
        txPanel.Dock = DockStyle.Fill;
        groupBoxHistory.Controls.Add(txPanel);
    }

    #endregion
}

public class Batch
{
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

    public string domain { get; }
    public string method { get; }
    public decimal bid { get; }
    public decimal lockup { get; }

    public string toAddress { get; }

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
            return "[\"BID\", \"" + domain + "\", " + bid + ", " + lockup + "]";
        if (method == "TRANSFER") return "[\"TRANSFER\", \"" + domain + "\", \"" + toAddress + "\"]";
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
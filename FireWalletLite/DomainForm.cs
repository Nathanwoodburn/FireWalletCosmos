using System.Diagnostics;
using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite
{
    public partial class DomainForm : Form
    {
        private MainForm Main;
        private string Domain;
        public DomainForm(MainForm main, string domain)
        {
            InitializeComponent();
            this.Main = main;
            this.Domain = domain;
            Text = domain + "/";
            // Theme form
            BackColor = ColorTranslator.FromHtml(main.Theme["background"]);
            ForeColor = ColorTranslator.FromHtml(main.Theme["foreground"]);
            foreach (Control control in Controls)
            {
                main.ThemeControl(control);
            }
            labelName.Text = domain + "/";
        }

        private void buttonExplorer_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = Main.DomainExplorer + Domain,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void DomainForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Main.dir + "domains.json")) return;

            JArray domains = JArray.Parse(File.ReadAllText(Main.dir + "domains.json"));
            foreach (JObject domain in domains)
            {
                if (domain["name"].ToString() == Domain)
                {
                    if (domain.ContainsKey("status"))
                    {
                        switch (domain["status"].ToString())
                        {
                            case "transferring":
                                buttonFinalize.Visible = true;
                                buttonCancel.Visible = true;
                                buttonTransfer.Visible = false;
                                textBoxTransferAddress.Visible = false;
                                break;
                            case "closed":
                                buttonCancel.Visible = false;
                                buttonFinalize.Visible = false;
                                break;
                        }
                    }
                }
            }
        }

        private async void buttonRenew_Click(object sender, EventArgs e)
        {
            string content = "{\"method\": \"renew\", \"params\": [\"" + Domain + "\"]}";
            string response = await Main.APIPost("", true, content);
            if (response == "Error")
            {
                NotifyForm notify = new NotifyForm("Error renewing domain");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            JObject jObject = JObject.Parse(response);
            if (jObject.ContainsKey("result"))
            {
                Main.AddLog(jObject["result"].ToString());
                JObject result = (JObject)jObject["result"];
                if (result.ContainsKey("txid"))
                {
                    string txid = result["txid"].ToString();
                    NotifyForm notify = new NotifyForm("Renewed domain", "Explorer", Main.TXExplorer + txid);
                    notify.ShowDialog();
                    notify.Dispose();
                }
                else
                {
                    NotifyForm notify = new NotifyForm("Error renewing domain");
                    notify.ShowDialog();
                    notify.Dispose();
                }
            }
            else
            {
                NotifyForm notify = new NotifyForm("Error renewing domain");
                notify.ShowDialog();
                notify.Dispose();
                Main.AddLog(jObject.ToString());
            }
        }

        private async void buttonTransfer_Click(object sender, EventArgs e)
        {
            string address = textBoxTransferAddress.Text;
            bool valid = await Main.ValidAddress(address);
            if (!valid)
            {
                NotifyForm notify = new NotifyForm("Invalid address");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            string path = "wallet/" + Main.Account + "/transfer";
            string content = "{\"passphrase\": \"" + Main.Password + "\", \"name\": \"" + Domain + "\", \"broadcast\": true, \"sign\": true, \"address\": \"" + address + "\"}";
            string response = await Main.APIPost(path, true, content);
            if (response == "Error")
            {
                NotifyForm notify = new NotifyForm("Error transferring domain");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            JObject jObject = JObject.Parse(response);
            if (jObject.ContainsKey("hash"))
            {
                string txid = jObject["hash"].ToString();
                NotifyForm notify = new NotifyForm("Transferred domain", "Explorer", Main.TXExplorer + txid);
                notify.ShowDialog();
                notify.Dispose();
                AddDomainInfo("transferring");
            }
            else
            {
                NotifyForm notify = new NotifyForm("Error transferring domain");
                notify.ShowDialog();
                notify.Dispose();
            }
        }

        private void buttonFinalize_Click(object sender, EventArgs e)
        {
            string path = "wallet/" + Main.Account + "/finalize";
            string content = "{\"passphrase\": \"" + Main.Password + "\", \"name\": \"" + Domain + "\", \"broadcast\": true, \"sign\": true}";
            string response = Main.APIPost(path, true, content).Result;
            if (response == "Error")
            {
                NotifyForm notify = new NotifyForm("Error finalizing transfer");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            JObject jObject = JObject.Parse(response);
            if (jObject.ContainsKey("hash"))
            {
                string txid = jObject["hash"].ToString();
                NotifyForm notify = new NotifyForm("Finalized domain", "Explorer", Main.TXExplorer + txid);
                notify.ShowDialog();
                notify.Dispose();
                AddDomainInfo("closed");
            }
            else
            {
                NotifyForm notify = new NotifyForm("Error finalizing domain");
                notify.ShowDialog();
                notify.Dispose();
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            string path = "wallet/" + Main.Account + "/cancel";
            string content = "{\"passphrase\": \"" + Main.Password + "\", \"name\": \"" + Domain + "\", \"broadcast\": true, \"sign\": true}";
            string response = Main.APIPost(path, true, content).Result;
            if (response == "Error")
            {
                NotifyForm notify = new NotifyForm("Error cancelling transfer");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            JObject jObject = JObject.Parse(response);
            if (jObject.ContainsKey("hash"))
            {
                string txid = jObject["hash"].ToString();
                NotifyForm notify = new NotifyForm("Canceled transfer", "Explorer", Main.TXExplorer + txid);
                notify.ShowDialog();
                notify.Dispose();
                AddDomainInfo("closed");
            }
            else
            {
                NotifyForm notify = new NotifyForm("Error cancelling transfer");
                notify.ShowDialog();
                notify.Dispose();
            }
        }

        private void AddDomainInfo(string status)
        {
            if (File.Exists(Main.dir + "domains.json"))
            {
                bool found = false;
                JArray domains = JArray.Parse(File.ReadAllText(Main.dir + "domains.json"));
                foreach (JObject domain in domains)
                {
                    if (domain["name"].ToString() == Domain)
                    {
                        found = true;
                        if (domain.ContainsKey("status"))
                        {
                            domain["status"] = status;
                        }
                        else
                        {
                            domain.Add("status", status);
                        }
                    }
                }

                if (!found)
                {
                    JObject domain = new JObject();
                    domain["name"] = Domain;
                    domain["status"] = status;
                    domains.Add(domain);
                }
                File.WriteAllText(Main.dir + "domains.json", domains.ToString());
            }
            else
            {
                JArray domains = new JArray();
                JObject domain = new JObject();
                domain["name"] = Domain;
                domain["status"] = status;
                domains.Add(domain);
                File.WriteAllText(Main.dir + "domains.json", domains.ToString());
            }
        }

        private async void buttonSign_Click(object sender, EventArgs e)
        {
            if (textBoxSignMessage.Text == "")
            {
                NotifyForm notify = new NotifyForm("Enter a message to sign");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            string content = "{\"method\": \"signmessagewithname\", \"params\": [\"" + Domain + "\", \"" + textBoxSignMessage.Text + "\"]}";
            string response = await Main.APIPost("", true, content);
            Main.AddLog(response);
            if (response == "Error")
            {
                NotifyForm notify = new NotifyForm("Error signing message");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            JObject jObject = JObject.Parse(response);
            if (jObject.ContainsKey("result"))
            {
                textBoxSignature.Text = jObject["result"].ToString();
            }
            else
            {
                NotifyForm notify = new NotifyForm("Error signing message");
                notify.ShowDialog();
                notify.Dispose();
            }
        }
    }
}

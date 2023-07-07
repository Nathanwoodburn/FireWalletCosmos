using System.Diagnostics;
using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite
{
    public partial class DomainForm : Form
    {
        private MainForm main;
        private string domain;
        public DomainForm(MainForm main, string domain)
        {
            InitializeComponent();
            this.main = main;
            this.domain = domain;
            this.Text = domain + "/";
            // Theme form
            this.BackColor = ColorTranslator.FromHtml(main.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(main.Theme["foreground"]);
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
                FileName = main.DomainExplorer + domain,
                UseShellExecute = true
            };
            Process.Start(psi);
        }




        private async void GetName()
        {
            try
            {
                string content = "{\"method\": \"getnameinfo\", \"params\": [\"" + domain + "\"]}";
                string response = await main.APIPost("", false, content);
                JObject jObject = JObject.Parse(response);
                // Get block height
                string Nodeinfo = await main.APIGet("", false);
                JObject jObjectInfo = JObject.Parse(Nodeinfo);
                JObject chain = (JObject)jObjectInfo["chain"];
                int height = Convert.ToInt32(chain["height"]);

                labelInfo.Text = "";
                if (jObject.ContainsKey("result"))
                {
                    JObject result = (JObject)jObject["result"];
                    JObject start = (JObject)result["start"];
                    if (start["reserved"].ToString().ToLower() == "true") labelInfo.Text = "Reserved for ICANN domain owner\n";
                    if (result.ContainsKey("info"))
                    {
                        try
                        {
                            JObject info = (JObject)result["info"];
                            string state = info["state"].ToString();
                            JObject stats = (JObject)info["stats"];

                            if (info["transfer"].ToString() != "0")
                            {
                                labelInfo.Text += "Transferring\n";
                                int TransferEnd = Convert.ToInt32(stats["transferLockupEnd"].ToString());
                                textBoxTransferAddress.Hide();
                                buttonTransfer.Hide();
                                if (height >= TransferEnd)
                                {
                                    buttonFinalize.Show();
                                }
                                else
                                {
                                    labelInfo.Text += "Finalize in " + (TransferEnd - height).ToString() + " blocks\n";
                                    buttonCancel.Show();
                                }
                            }

                            if (state == "CLOSED")
                            {
                                string expires = "Expires in ~" + stats["daysUntilExpire"].ToString() + " days\n";
                                labelInfo.Text += expires;
                            }
                            else labelInfo.Text += state + "\n";


                            // Get DNS if the domain isn't in auction
                            if (state == "CLOSED") GetDNS();


                        }
                        catch (Exception ex)
                        {
                            // No info -> Domain not in wallet
                            labelInfo.Text = "No info in node";
                        }
                    }
                }
                else
                {
                    labelInfo.Text = "Error getting Name";
                }
            }
            catch (Exception ex)
            {
                main.AddLog(ex.Message);
            }


            if (!main.Domains.Contains(domain))
            {
                groupBoxManage.Hide();
            }
        }

        private void DomainForm_Load(object sender, EventArgs e)
        {
            GetName();
        }

        private async void buttonRenew_Click(object sender, EventArgs e)
        {
            string content = "{\"method\": \"renew\", \"params\": [\"" + domain + "\"]}";
            string response = await main.APIPost("", true, content);
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
                main.AddLog(jObject["result"].ToString());
                JObject result = (JObject)jObject["result"];
                if (result.ContainsKey("txid"))
                {
                    string txid = result["txid"].ToString();
                    NotifyForm notify = new NotifyForm("Renewed domain", "Explorer", main.TXExplorer + txid);
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
                main.AddLog(jObject.ToString());
            }
        }

        private async void buttonTransfer_Click(object sender, EventArgs e)
        {
            string address = textBoxTransferAddress.Text;
            bool valid = await main.ValidAddress(address);
            if (!valid)
            {
                NotifyForm notify = new NotifyForm("Invalid address");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            string content = "{\"method\": \"transfer\", \"params\": [\"" + domain + "\", \"" + address + "\"]}";
            string response = await main.APIPost("", true, content);
            if (response == "Error")
            {
                NotifyForm notify = new NotifyForm("Error transferring domain");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            JObject jObject = JObject.Parse(response);
            if (jObject.ContainsKey("result"))
            {
                main.AddLog(jObject["result"].ToString());
                JObject result = (JObject)jObject["result"];
                if (result.ContainsKey("txid"))
                {
                    string txid = result["txid"].ToString();
                    NotifyForm notify = new NotifyForm("Renewed domain", "Explorer", main.TXExplorer + txid);
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
                main.AddLog(jObject.ToString());
            }
        }

        private void buttonFinalize_Click(object sender, EventArgs e)
        {
            string content = "{\"method\": \"finalize\", \"params\": [\"" + domain + "\"]}";
            string response = main.APIPost("", true, content).Result;
            if (response == "Error")
            {
                NotifyForm notify = new NotifyForm("Error finalizing tranfer");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            JObject jObject = JObject.Parse(response);
            if (jObject.ContainsKey("result"))
            {
                main.AddLog(jObject["result"].ToString());
                JObject result = (JObject)jObject["result"];
                if (result.ContainsKey("txid"))
                {
                    string txid = result["txid"].ToString();
                    NotifyForm notify = new NotifyForm("Finalized tranfer", "Explorer", main.TXExplorer + txid);
                    notify.ShowDialog();
                    notify.Dispose();
                }
                else
                {
                    NotifyForm notify = new NotifyForm("Error finalizing tranfer");
                    notify.ShowDialog();
                    notify.Dispose();
                }
            }
            else
            {
                NotifyForm notify = new NotifyForm("Error finalizing tranfer");
                notify.ShowDialog();
                notify.Dispose();
                main.AddLog(jObject.ToString());
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            string content = "{\"method\": \"cancel\", \"params\": [\"" + domain + "\"]}";
            string response = main.APIPost("", true, content).Result;
            if (response == "Error")
            {
                NotifyForm notify = new NotifyForm("Error cancelling tranfer");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            JObject jObject = JObject.Parse(response);
            if (jObject.ContainsKey("result"))
            {
                main.AddLog(jObject["result"].ToString());
                JObject result = (JObject)jObject["result"];
                if (result.ContainsKey("txid"))
                {
                    string txid = result["txid"].ToString();
                    NotifyForm notify = new NotifyForm("Cancelled tranfer", "Explorer", main.TXExplorer + txid);
                    notify.ShowDialog();
                    notify.Dispose();
                }
                else
                {
                    NotifyForm notify = new NotifyForm("Error cancelling tranfer");
                    notify.ShowDialog();
                    notify.Dispose();
                }
            }
            else
            {
                NotifyForm notify = new NotifyForm("Error cancelling tranfer");
                notify.ShowDialog();
                notify.Dispose();
                main.AddLog(jObject.ToString());
            }
        }


        // Get DNS records for domain. Not implemented yet
        private async void GetDNS()
        {
            // Get DNS records
            string contentDNS = "{\"method\": \"getnameresource\", \"params\": [\"" + domain + "\"]}";
            string responseDNS = await main.APIPost("", false, contentDNS);
            JObject jObjectDNS = JObject.Parse(responseDNS);

            if (jObjectDNS["result"].ToString() == "")
            {
                // Not registered
                groupBoxDNS.Visible = false;
                return;
            }

            JObject result = (JObject)jObjectDNS["result"];
            JArray records = (JArray)result["records"];
            // For each record
            int i = 0;
            foreach (JObject record in records)
            {
                Panel DNSPanel = new Panel();
                // Count for scroll width
                DNSPanel.Width = panelDNS.Width - SystemInformation.VerticalScrollBarWidth - 2;
                DNSPanel.Height = 60;
                DNSPanel.BorderStyle = BorderStyle.FixedSingle;
                DNSPanel.Top = 62 * i;

                Label DNSType = new Label();
                DNSType.Text = record["type"].ToString();
                DNSType.Location = new System.Drawing.Point(10, 10);
                DNSType.AutoSize = true;
                DNSType.Font = new Font(DNSType.Font.FontFamily, 11.0f, FontStyle.Bold);
                DNSPanel.Controls.Add(DNSType);


                switch (DNSType.Text)
                {
                    case "NS":
                        Label DNSNS = new Label();
                        DNSNS.Text = record["ns"].ToString();
                        DNSNS.Location = new System.Drawing.Point(10, 30);
                        DNSNS.AutoSize = true;
                        DNSPanel.Controls.Add(DNSNS);
                        break;
                    case "GLUE4":
                    case "GLUE6":
                        Label DNSNS1 = new Label();
                        DNSNS1.Text = record["ns"].ToString();
                        DNSNS1.Location = new System.Drawing.Point(10, 30);
                        DNSNS1.AutoSize = true;
                        DNSPanel.Controls.Add(DNSNS1);
                        Label address = new Label();
                        address.Text = record["address"].ToString();
                        address.Location = new System.Drawing.Point(DNSNS1.Left + DNSNS1.Width + 20, 30);
                        address.AutoSize = true;
                        DNSPanel.Controls.Add(address);
                        break;
                    case "DS":
                        Label keyTag = new Label();
                        keyTag.Text = record["keyTag"].ToString();
                        keyTag.Location = new System.Drawing.Point(10, 30);
                        keyTag.AutoSize = true;
                        DNSPanel.Controls.Add(keyTag);
                        Label algorithm = new Label();
                        algorithm.Text = record["algorithm"].ToString();
                        algorithm.Location = new System.Drawing.Point(keyTag.Left + keyTag.Width + 10, 30);
                        algorithm.AutoSize = true;
                        DNSPanel.Controls.Add(algorithm);
                        Label digestType = new Label();
                        digestType.Text = record["digestType"].ToString();
                        digestType.Location = new System.Drawing.Point(algorithm.Left + algorithm.Width + 10, 30);
                        digestType.AutoSize = true;
                        DNSPanel.Controls.Add(digestType);
                        Label digest = new Label();
                        digest.Text = record["digest"].ToString();
                        digest.Location = new System.Drawing.Point(digestType.Left + digestType.Width + 10, 30);
                        digest.AutoSize = true;
                        DNSPanel.Controls.Add(digest);
                        break;
                    case "TXT":
                        JArray txts = (JArray)record["txt"];
                        int j = 0;
                        foreach (string txt in txts)
                        {
                            Label DNSTXT = new Label();
                            DNSTXT.Text = txt;
                            DNSTXT.Location = new System.Drawing.Point(10, 30 + (j * 20));
                            DNSTXT.AutoSize = true;
                            DNSPanel.Controls.Add(DNSTXT);
                            DNSPanel.Height = 60 + (j * 20);
                            j++;
                        }
                        break;

                }
                panelDNS.Controls.Add(DNSPanel);
                i++;
            }
        }
    }
}

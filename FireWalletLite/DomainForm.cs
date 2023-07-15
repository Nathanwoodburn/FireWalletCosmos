using System.Diagnostics;
using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite;

public partial class DomainForm : Form
{
    private readonly string Domain;
    private readonly MainForm Main;

    public DomainForm(MainForm main, string domain)
    {
        InitializeComponent();
        Main = main;
        Domain = domain;
        Text = domain + "/";
        // Theme form
        BackColor = ColorTranslator.FromHtml(main.Theme["background"]);
        ForeColor = ColorTranslator.FromHtml(main.Theme["foreground"]);
        foreach (Control control in Controls) main.ThemeControl(control);
        labelName.Text = domain + "/";
    }

    private void buttonExplorer_Click(object sender, EventArgs e)
    {
        var psi = new ProcessStartInfo
        {
            FileName = Main.DomainExplorer + Domain,
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    private void DomainForm_Load(object sender, EventArgs e)
    {
        if (!File.Exists(Main.dir + "domains.json")) return;

        var domains = JArray.Parse(File.ReadAllText(Main.dir + "domains.json"));
        foreach (JObject domain in domains)
            if (domain["name"].ToString() == Domain)
                if (domain.ContainsKey("status"))
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

    private async void buttonRenew_Click(object sender, EventArgs e)
    {
        var path = "wallet/" + Main.Account + "/renewal";
        var content = "{\"passphrase\": \"" + Main.Password + "\", \"name\": \"" + Domain +
                      "\", \"broadcast\": true, \"sign\": true}";
        var response = await Main.APIPost(path, true, content);
        if (response == "Error")
        {
            var notify = new NotifyForm("Error renewing domain");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var jObject = JObject.Parse(response);
        if (jObject.ContainsKey("hash"))
        {
            var txid = jObject["hash"].ToString();
            var notify = new NotifyForm("Renew sent", "Explorer", Main.TXExplorer + txid);
            notify.ShowDialog();
            notify.Dispose();
            AddDomainInfo("closed");
        }
        else
        {
            var notify = new NotifyForm("Error renewing domain");
            notify.ShowDialog();
            notify.Dispose();
        }
    }

    private async void buttonTransfer_Click(object sender, EventArgs e)
    {
        var address = textBoxTransferAddress.Text;
        var valid = await Main.ValidAddress(address);
        if (!valid)
        {
            var notify = new NotifyForm("Invalid address");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var path = "wallet/" + Main.Account + "/transfer";
        var content = "{\"passphrase\": \"" + Main.Password + "\", \"name\": \"" + Domain +
                      "\", \"broadcast\": true, \"sign\": true, \"address\": \"" + address + "\"}";
        var response = await Main.APIPost(path, true, content);
        if (response == "Error")
        {
            var notify = new NotifyForm("Error transferring domain");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var jObject = JObject.Parse(response);
        if (jObject.ContainsKey("hash"))
        {
            var txid = jObject["hash"].ToString();
            var notify = new NotifyForm("Transferred domain", "Explorer", Main.TXExplorer + txid);
            notify.ShowDialog();
            notify.Dispose();
            AddDomainInfo("transferring");
        }
        else
        {
            var notify = new NotifyForm("Error transferring domain");
            notify.ShowDialog();
            notify.Dispose();
        }
    }

    private async void buttonFinalize_Click(object sender, EventArgs e)
    {
        var path = "wallet/" + Main.Account + "/finalize";
        var content = "{\"passphrase\": \"" + Main.Password + "\", \"name\": \"" + Domain +
                      "\", \"broadcast\": true, \"sign\": true}";
        var response = await Main.APIPost(path, true, content);
        if (response == "Error")
        {
            var notify = new NotifyForm("Error finalizing transfer");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var jObject = JObject.Parse(response);
        if (jObject.ContainsKey("hash"))
        {
            var txid = jObject["hash"].ToString();
            var notify = new NotifyForm("Finalized domain", "Explorer", Main.TXExplorer + txid);
            notify.ShowDialog();
            notify.Dispose();
            AddDomainInfo("closed");
        }
        else
        {
            var notify = new NotifyForm("Error finalizing domain");
            notify.ShowDialog();
            notify.Dispose();
        }
    }

    private async void buttonCancel_Click(object sender, EventArgs e)
    {
        var path = "wallet/" + Main.Account + "/cancel";
        var content = "{\"passphrase\": \"" + Main.Password + "\", \"name\": \"" + Domain +
                      "\", \"broadcast\": true, \"sign\": true}";
        var response = await Main.APIPost(path, true, content);
        if (response == "Error")
        {
            var notify = new NotifyForm("Error cancelling transfer");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var jObject = JObject.Parse(response);
        if (jObject.ContainsKey("hash"))
        {
            var txid = jObject["hash"].ToString();
            var notify = new NotifyForm("Canceled transfer", "Explorer", Main.TXExplorer + txid);
            notify.ShowDialog();
            notify.Dispose();
            AddDomainInfo("closed");
        }
        else
        {
            var notify = new NotifyForm("Error cancelling transfer");
            notify.ShowDialog();
            notify.Dispose();
        }
    }

    private void AddDomainInfo(string status)
    {
        if (File.Exists(Main.dir + "domains.json"))
        {
            var found = false;
            var domains = JArray.Parse(File.ReadAllText(Main.dir + "domains.json"));
            foreach (JObject domain in domains)
                if (domain["name"].ToString() == Domain)
                {
                    found = true;
                    if (domain.ContainsKey("status"))
                        domain["status"] = status;
                    else
                        domain.Add("status", status);
                }

            if (!found)
            {
                var domain = new JObject();
                domain["name"] = Domain;
                domain["status"] = status;
                domains.Add(domain);
            }

            File.WriteAllText(Main.dir + "domains.json", domains.ToString());
        }
        else
        {
            var domains = new JArray();
            var domain = new JObject();
            domain["name"] = Domain;
            domain["status"] = status;
            domains.Add(domain);
            File.WriteAllText(Main.dir + "domains.json", domains.ToString());
        }
    }

    private async void buttonSign_Click(object sender, EventArgs e)
    {
        if (buttonSign.Text == "Save")
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";
            saveFileDialog.Title = "Save Signature";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var signature = new JObject();
                signature["domain"] = Domain;
                signature["message"] = textBoxSignMessage.Text;
                signature["signature"] = textBoxSignature.Text;
                signature["time"] = DateTime.Now.ToString();
                File.WriteAllText(saveFileDialog.FileName, signature.ToString());
            }

            return;
        }

        if (textBoxSignMessage.Text == "")
        {
            var notify = new NotifyForm("Enter a message to sign");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var content = "{\"method\": \"signmessagewithname\", \"params\": [\"" + Domain + "\", \"" +
                      textBoxSignMessage.Text + "\"]}";
        var response = await Main.APIPost("", true, content);
        if (response == "Error")
        {
            var notify = new NotifyForm("Error signing message");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var jObject = JObject.Parse(response);
        if (jObject.ContainsKey("result"))
        {
            textBoxSignature.Text = jObject["result"].ToString();
            buttonSign.Text = "Save";
        }
        else
        {
            Main.AddLog(response);
            var notify = new NotifyForm("Error signing message");
            notify.ShowDialog();
            notify.Dispose();
        }
    }

    private void textBoxSignMessage_TextChanged(object sender, EventArgs e)
    {
        buttonSign.Text = "Sign";
    }
}
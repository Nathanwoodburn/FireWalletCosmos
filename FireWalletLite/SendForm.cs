using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite;

public partial class SendForm : Form
{
    private readonly int fee = 1;
    private readonly MainForm main;
    private readonly decimal unlockedbalance;

    public SendForm(decimal Unlockedbalance, MainForm main)
    {
        InitializeComponent();
        this.main = main;
        unlockedbalance = Unlockedbalance;

        // Theme form
        BackColor = ColorTranslator.FromHtml(main.Theme["background"]);
        ForeColor = ColorTranslator.FromHtml(main.Theme["foreground"]);
        foreach (Control control in Controls) main.ThemeControl(control);

        labelMax.Text = "Max: " + (unlockedbalance - fee) + " HNS";
        if (unlockedbalance < fee) labelMax.Text = "Max: 0 HNS";
        //buttonSend.Enabled = false;
        // Allign controls
        labelAddress.Left = (ClientSize.Width - labelAddress.Width) / 2;
        labelAmount.Left = (ClientSize.Width - labelAmount.Width) / 2;
        textBoxAddress.Left = (ClientSize.Width - textBoxAddress.Width) / 2;
        labelMax.Left = (ClientSize.Width - labelMax.Width) / 2;
        textBoxAmount.Left = (ClientSize.Width - textBoxAmount.Width - labelHNSToken.Width - 10) / 2;
        labelHNSToken.Left = textBoxAmount.Left + textBoxAmount.Width + 10;
        buttonSend.Left = (ClientSize.Width - buttonSend.Width) / 2;
    }

    private async void buttonSend_Click(object sender, EventArgs e)
    {
        buttonSend.Enabled = false;
        var address = textBoxAddress.Text;
        if (textBoxAddress.Text.Substring(0, 1) == "@")
        {
            // HIP-02 not supported yet
            var notify = new NotifyForm("HIP-02 not supported yet");
            notify.ShowDialog();
            notify.Dispose();
            buttonSend.Enabled = true;
            return;
        }

        var valid = await main.ValidAddress(address);
        if (!valid)
        {
            var notify = new NotifyForm("Invalid address");
            notify.ShowDialog();
            notify.Dispose();
            buttonSend.Enabled = true;
            return;
        }

        decimal amount = 0;
        if (!decimal.TryParse(textBoxAmount.Text, out amount))
        {
            var notify = new NotifyForm("Invalid amount");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        if (amount > unlockedbalance - fee)
        {
            var notify = new NotifyForm("Insufficient balance");
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var content = "{\"method\": \"sendtoaddress\",\"params\": [ \"" + address + "\", " +
                      amount + "]}";
        var output = await main.APIPost("", true, content);
        var APIresp = JObject.Parse(output);
        if (APIresp["error"].ToString() != "")
        {
            main.AddLog("Failed:");
            main.AddLog(APIresp.ToString());
            var error = JObject.Parse(APIresp["error"].ToString());
            var ErrorMessage = error["message"].ToString();

            var notify = new NotifyForm("Error Transaction Failed\n" + ErrorMessage);
            notify.ShowDialog();
            notify.Dispose();
            return;
        }

        var hash = APIresp["result"].ToString();
        var link = main.TXExplorer + hash;
        var notifySuccess = new NotifyForm("Transaction Sent\nThis transaction could take up to 20 minutes to mine",
            "Explorer", link);
        notifySuccess.ShowDialog();
        notifySuccess.Dispose();
        Close();
    }
}
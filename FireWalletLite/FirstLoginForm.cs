using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite;

public partial class FirstLoginForm : Form
{
    private readonly MainForm main;
    private string seedPhrase;

    public FirstLoginForm(string seedPhrase, MainForm mainForm)
    {
        InitializeComponent();
        this.seedPhrase = seedPhrase;
        main = mainForm;
        // Theme form
        BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
        ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
        foreach (Control control in Controls) mainForm.ThemeControl(control);
        textBoxSeed.Text = seedPhrase;
    }

    private async void Start_Click(object sender, EventArgs e)
    {
        if (textBoxPassword.Text.Length < 8)
        {
            var notifyForm = new NotifyForm("Please choose a longer password!");
            notifyForm.ShowDialog();
            notifyForm.Dispose();
            return;
        }

        if (textBoxPassword.Text != textBoxPassword2.Text)
        {
            var notifyForm = new NotifyForm("Passwords do not match!");
            notifyForm.ShowDialog();
            notifyForm.Dispose();
            return;
        }

        // Encrypt wallet
        var content = "{\"method\":\"encryptwallet\",\"params\":[\"" + textBoxPassword.Text + "\"]}";
        var response = await main.APIPost("", true, content);
        main.AddLog("Encrypt wallet: " + response);
        var jObject = JObject.Parse(response);
        if (jObject["error"].ToString() != "")
        {
            var notifyForm = new NotifyForm("Error encrypting wallet: " + jObject["error"]);
            notifyForm.ShowDialog();
            notifyForm.Dispose();
            return;
        }

        Close();
    }
}
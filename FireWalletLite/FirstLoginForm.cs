using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite
{
    public partial class FirstLoginForm : Form
    {
        String seedPhrase;
        MainForm main;
        public FirstLoginForm(string seedPhrase, MainForm mainForm)
        {
            InitializeComponent();
            this.seedPhrase = seedPhrase;
            this.main = mainForm;
            // Theme form
            this.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
            foreach (Control control in Controls)
            {
                mainForm.ThemeControl(control);
            }
            textBoxSeed.Text = seedPhrase;
        }

        private async void Start_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.Length < 8)
            {
                NotifyForm notifyForm = new NotifyForm("Please choose a longer password!");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }
            if (textBoxPassword.Text != textBoxPassword2.Text)
            {
                NotifyForm notifyForm = new NotifyForm("Passwords do not match!");
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            }

            // Encrypt wallet
            string content = "{\"method\":\"encryptwallet\",\"params\":[\"" + textBoxPassword.Text + "\"]}";
            string response = await main.APIPost("",true,content);
            main.AddLog("Encrypt wallet: " + response);
            JObject jObject = JObject.Parse(response);
            if (jObject["error"].ToString() != "")
            {
                NotifyForm notifyForm = new NotifyForm("Error encrypting wallet: " + jObject["error"].ToString());
                notifyForm.ShowDialog();
                notifyForm.Dispose();
                return;
            } else
            {
                this.Close();
            }
        }
    }
}

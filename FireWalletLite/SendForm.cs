using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireWallet;
using Newtonsoft.Json.Linq;

namespace FireWalletLite
{
    public partial class SendForm : Form
    {
        int fee = 1;
        decimal unlockedbalance;
        MainForm main;
        public SendForm(decimal Unlockedbalance, MainForm main)
        {
            InitializeComponent();
            this.main = main;
            this.unlockedbalance = Unlockedbalance;

            // Theme form
            this.BackColor = ColorTranslator.FromHtml(main.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(main.Theme["foreground"]);
            foreach (Control control in Controls)
            {
                main.ThemeControl(control);
            }

            labelMax.Text = "Max: " + (unlockedbalance - fee).ToString() + " HNS";
            if (unlockedbalance < fee)
            {
                labelMax.Text = "Max: 0 HNS";
                //buttonSend.Enabled = false;
            }

            // Allign controls
            labelAddress.Left = (this.ClientSize.Width - labelAddress.Width) / 2;
            labelAmount.Left = (this.ClientSize.Width - labelAmount.Width) / 2;
            textBoxAddress.Left = (this.ClientSize.Width - textBoxAddress.Width) / 2;
            labelMax.Left = (this.ClientSize.Width - labelMax.Width) / 2;
            textBoxAmount.Left = (this.ClientSize.Width - textBoxAmount.Width - labelHNSToken.Width - 10) / 2;
            labelHNSToken.Left = textBoxAmount.Left + textBoxAmount.Width + 10;
            buttonSend.Left = (this.ClientSize.Width - buttonSend.Width) / 2;
        }

        private async void buttonSend_Click(object sender, EventArgs e)
        {
            buttonSend.Enabled = false;
            string address = textBoxAddress.Text;
            if (textBoxAddress.Text.Substring(0,1) == "@")
            {
                // HIP-02 not supported yet
                NotifyForm notify = new NotifyForm("HIP-02 not supported yet");
                notify.ShowDialog();
                notify.Dispose();
                buttonSend.Enabled = true;
                return;
            }
            bool valid = await main.ValidAddress(address);
            if (!valid)
            {
                NotifyForm notify = new NotifyForm("Invalid address");
                notify.ShowDialog();
                notify.Dispose();
                buttonSend.Enabled = true;
                return;
            }

            decimal amount = 0;
            if (!decimal.TryParse(textBoxAmount.Text, out amount))
            {
                NotifyForm notify = new NotifyForm("Invalid amount");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            if (amount > unlockedbalance - fee)
            {
                NotifyForm notify = new NotifyForm("Insufficient balance");
                notify.ShowDialog();
                notify.Dispose();
                return;
            }

            string content = "{\"method\": \"sendtoaddress\",\"params\": [ \"" + address + "\", " +
                                    amount.ToString() + "]}";
            string output = await main.APIPost("", true, content);
            JObject APIresp = JObject.Parse(output);
            if (APIresp["error"].ToString() != "")
            {
                main.AddLog("Failed:");
                main.AddLog(APIresp.ToString());
                JObject error = JObject.Parse(APIresp["error"].ToString());
                string ErrorMessage = error["message"].ToString();

                NotifyForm notify = new NotifyForm("Error Transaction Failed\n" + ErrorMessage);
                notify.ShowDialog();
                notify.Dispose();
                return;
            }
            string hash = APIresp["result"].ToString();
            string link = main.TXExplorer + hash;
            NotifyForm notifySuccess = new NotifyForm("Transaction Sent\nThis transaction could take up to 20 minutes to mine",
                "Explorer", link);
            notifySuccess.ShowDialog();
            notifySuccess.Dispose();
            this.Close();
        }
    }
}

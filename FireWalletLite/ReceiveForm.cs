using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;

namespace FireWalletLite
{
    public partial class ReceiveForm : Form
    {
        string address;
        MainForm main;
        public ReceiveForm(string address, MainForm main)
        {
            InitializeComponent();
            this.address = address;
            this.main = main;
            // Theme form
            this.BackColor = ColorTranslator.FromHtml(main.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(main.Theme["foreground"]);
            foreach (Control control in Controls)
            {
                main.ThemeControl(control);
            }
            textBoxReceive.Text = address;
            textBoxReceive.Left = (this.ClientSize.Width - textBoxReceive.Width) / 2;
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;
            buttonCopy.Left = (this.ClientSize.Width - buttonCopy.Width) / 2;



            QRCodeGenerator qrcode = new QRCodeGenerator();
            QRCodeData qrData = qrcode.CreateQrCode(address, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrData);
            pictureBoxReceiveQR.Image = qrCode.GetGraphic(20, main.Theme["foreground"], main.Theme["background"]);
            pictureBoxReceiveQR.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxReceiveQR.Width = this.ClientSize.Width / 2;
            pictureBoxReceiveQR.Height = this.ClientSize.Width / 2;
            pictureBoxReceiveQR.Left = (this.ClientSize.Width - pictureBoxReceiveQR.Width) / 2;
        }
        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(address);
        }
    }
}

using QRCoder;

namespace FireWalletLite;

public partial class ReceiveForm : Form
{
    private readonly string address;
    private MainForm main;

    public ReceiveForm(string address, MainForm main)
    {
        InitializeComponent();
        this.address = address;
        this.main = main;
        // Theme form
        BackColor = ColorTranslator.FromHtml(main.Theme["background"]);
        ForeColor = ColorTranslator.FromHtml(main.Theme["foreground"]);
        foreach (Control control in Controls) main.ThemeControl(control);
        textBoxReceive.Text = address;
        textBoxReceive.Left = (ClientSize.Width - textBoxReceive.Width) / 2;
        label1.Left = (ClientSize.Width - label1.Width) / 2;
        buttonCopy.Left = (ClientSize.Width - buttonCopy.Width) / 2;


        var qrcode = new QRCodeGenerator();
        var qrData = qrcode.CreateQrCode(address, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new QRCode(qrData);
        pictureBoxReceiveQR.Image = qrCode.GetGraphic(20, main.Theme["foreground"], main.Theme["background"]);
        pictureBoxReceiveQR.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBoxReceiveQR.Width = ClientSize.Width / 2;
        pictureBoxReceiveQR.Height = ClientSize.Width / 2;
        pictureBoxReceiveQR.Left = (ClientSize.Width - pictureBoxReceiveQR.Width) / 2;
    }

    private void buttonCopy_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(address);
    }
}
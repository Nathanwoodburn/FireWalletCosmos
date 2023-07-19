using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FireWallet;

public partial class NotifyForm : Form
{
    private bool allowClose = true;
    private readonly string altLink;

    private readonly bool Linkcopy;
    public Dictionary<string, string> Theme { get; set; } = new()
    {
        { "background", "#000000" },
        { "foreground", "#fcba03"},
        { "background-alt", "#3e065f"},
        { "foreground-alt", "#ffffff"}
    };

    public NotifyForm(string Message)
    {
        InitializeComponent();
        labelmessage.Text = Message;
        altLink = "";
    }

    public NotifyForm(string Message, string altText, string altLink)
    {
        InitializeComponent();
        labelmessage.Text = Message;
        buttonALT.Text = altText;
        buttonALT.Visible = true;
        this.altLink = altLink;
        buttonOK.Focus();
        Linkcopy = false;
    }

    public NotifyForm(string Message, bool allowClose)
    {
        InitializeComponent();
        labelmessage.Text = Message;
        buttonOK.Focus();
        Linkcopy = false;
        buttonOK.Visible = allowClose;
        allowClose = allowClose;
    }

    public NotifyForm(string Message, string altText, string altLink, bool Linkcopy)
    {
        InitializeComponent();
        labelmessage.Text = Message;
        buttonALT.Text = altText;
        buttonALT.Visible = true;
        this.altLink = altLink;
        buttonOK.Focus();
        this.Linkcopy = Linkcopy;

        if (Linkcopy)
            // Small font to fix more data
            labelmessage.Font = new Font(labelmessage.Font.FontFamily, 10);
    }

    public void CloseNotification()
    {
        Close();
    }

    private void NotifyForm_Load(object sender, EventArgs e)
    {
        UpdateTheme();
    }

    private void OK_Click(object sender, EventArgs e)
    {
        allowClose = true;
        Close();
    }

    private void buttonALT_Click(object sender, EventArgs e)
    {
        if (Linkcopy)
        {
            // Copy link to clipboard
            Clipboard.SetText(altLink);
            return;
        }

        // Open link
        var psi = new ProcessStartInfo
        {
            FileName = altLink,
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    private void NotifyForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!allowClose) e.Cancel = true;
    }

    #region Theming

    private void UpdateTheme()
    {
        if (!Theme.ContainsKey("background") || !Theme.ContainsKey("background-alt") ||
            !Theme.ContainsKey("foreground") || !Theme.ContainsKey("foreground-alt")) return;

        // Apply Theme
        BackColor = ColorTranslator.FromHtml(Theme["background"]);

        // Foreground
        ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);


        // Need to specify this for each groupbox to override the black text
        foreach (Control c in Controls) ThemeControl(c);
    }

    private void ThemeControl(Control c)
    {
        FontFamily ff = new FontFamily("Verdana");
        c.Font = new Font(ff, c.Font.Size, c.Font.Style);
        if (c.GetType() == typeof(GroupBox) || c.GetType() == typeof(Panel))
        {
            c.ForeColor = ColorTranslator.FromHtml(Theme["foreground"]);
            foreach (Control sub in c.Controls) ThemeControl(sub);
        }

        if (c.GetType() == typeof(TextBox) || c.GetType() == typeof(Button)
                                           || c.GetType() == typeof(ComboBox) || c.GetType() == typeof(StatusStrip))
        {
            c.ForeColor = ColorTranslator.FromHtml(Theme["foreground-alt"]);
            c.BackColor = ColorTranslator.FromHtml(Theme["background-alt"]);
        }
    }
    #endregion
}
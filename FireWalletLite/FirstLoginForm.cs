using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FireWalletLite
{
    public partial class FirstLoginForm : Form
    {
        String seedPhrase;
        public FirstLoginForm(string seedPhrase, MainForm mainForm)
        {
            InitializeComponent();
            this.seedPhrase = seedPhrase;
            // Theme form
            this.BackColor = ColorTranslator.FromHtml(mainForm.Theme["background"]);
            this.ForeColor = ColorTranslator.FromHtml(mainForm.Theme["foreground"]);
            foreach (Control control in Controls)
            {
                mainForm.ThemeControl(control);
            }
            textBoxSeed.Text = seedPhrase;
        }
    }
}

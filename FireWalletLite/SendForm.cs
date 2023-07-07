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
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orkidea.Utilities.Crypto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescifrado.Text))
                txtDescifrado.Text = Orkidea.PollaExpress.Utilities.Cryptography.Decrypt(txtCifrado.Text);
            else
                txtCifrado.Text = Orkidea.PollaExpress.Utilities.Cryptography.Encrypt(txtDescifrado.Text);

        }
    }
}

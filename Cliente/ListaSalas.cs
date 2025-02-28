using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class ListaSalas : Form
    {
        public ListaSalas()
        {
            InitializeComponent();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 formLogin = new Form1();
            this.Hide();
            formLogin.Show();
        }

        private void iniciarConversaDiretaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MDSettings mds = new MDSettings();
            this.Hide();
            mds.Show();
        }
    }
}

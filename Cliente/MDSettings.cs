using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class MDSettings : Form
    {
        public MDSettings()
        {
            InitializeComponent();
        }

        private void btnEntrarMD_Click(object sender, EventArgs e)
        {
            FormEntrar formEntrar = new FormEntrar();
            this.Hide();
            formEntrar.Show();
        }

        private void btnCriarMD_Click(object sender, EventArgs e)
        {
            FormCriar formCriar = new FormCriar();
            this.Hide();
            formCriar.Show();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            ListaSalas listaSalas = new ListaSalas();
            this.Hide();
            listaSalas.Show();
        }
    }
}

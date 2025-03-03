using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class FormEntrar : Form
    {
        private TcpListener chatServer;
        private TcpClient cliente;
        private NetworkStream stream;

        public FormEntrar()
        {
            InitializeComponent();
        }

        private void btnVoltarAtras_Click(object sender, EventArgs e)
        {
            MDSettings mdsSettings = new MDSettings();
            this.Hide();
            mdsSettings.Show();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            int porta = Convert.ToInt32(txtPorta.Value);
            string pass = txtPassword.Text;
            string username = Form1.userLogado;
            try
            {
                ConversaDireta cd = new ConversaDireta(username, porta, pass);
                this.Hide();
                cd.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao tentar entrar no chat: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

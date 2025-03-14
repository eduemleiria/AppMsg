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
    public partial class CriarSala : Form
    {
        public string usernameL;
        public CriarSala(string username)
        {
            InitializeComponent();
            this.usernameL = username;
        }

        private void btnCriarSala_Click(object sender, EventArgs e)
        {
            string nome_Sala = txtNomeSala.Text;
            string descricao_Sala = txtDescricaoSala.Text;
            DateTime hoje = DateTime.Today;

            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 3700);
                NetworkStream stream = client.GetStream();

                var request = JsonSerializer.Serialize(new
                {
                    action = "criar_sala",
                    user = usernameL,
                    nomeSala = nome_Sala,
                    descricaoSala = descricao_Sala,
                    dataHoje = hoje.ToString("dd/MM/yyyy")
                });

                Console.WriteLine("Request de criação da sala: " + request);
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Resposta do server: {jsonResponse}");

                if (jsonResponse.Contains("sucesso"))
                {
                    MessageBox.Show("A sala foi criada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ListaSalas ls = new ListaSalas(usernameL);
                    this.Hide();
                    ls.Show();
                }
                else
                {
                    MessageBox.Show("Erro ao criar a sala.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Erro ao criar a sala: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            txtNomeSala.Clear();
            txtDescricaoSala.Clear();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            ListaSalas ls = new ListaSalas(usernameL);
            this.Hide();
            ls.Show();
        }
    }
}

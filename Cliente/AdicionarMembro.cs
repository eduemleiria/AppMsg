using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class AdicionarMembro: Form
    {
        public string nomeDaSala;
        public int salaId;
        public string username = Form1.userLogado;

        public AdicionarMembro(int idSala, string nomeSala)
        {
            this.nomeDaSala = nomeSala;
            this.salaId = idSala;
            InitializeComponent();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 3700);
                NetworkStream stream = client.GetStream();

                string userAdd = txtUserAdd.Text;
                Console.WriteLine("A adicionar user à sala...");

                var request = JsonSerializer.Serialize(new
                {
                    action = "adicionar_membro_sala",
                    idSala = salaId.ToString(),
                    convidar = userAdd,
                    convidado_por = username
                });

                Console.WriteLine($"Request enviada para o servidor: {request}");
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Resposta do server: {jsonResponse}");

                var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

                if (response["status"].ToString() == "Sucesso")
                {
                    MessageBox.Show($"O user {userAdd} foi adicionado à sala!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    detalhesSala ds = new detalhesSala(salaId, nomeDaSala);
                    this.Hide();
                    ds.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar membro à sala: {ex}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            detalhesSala ds = new detalhesSala(salaId, nomeDaSala);
            this.Hide();
            ds.Show();
        }
    }
}

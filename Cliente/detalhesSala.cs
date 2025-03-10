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
    public partial class detalhesSala: Form
    {
        public string nomeSala;
        string username = Form1.userLogado;

        public detalhesSala(string sala)
        {
            InitializeComponent();
            this.nomeSala = sala;
            LoadDadosSala();
        }

        public void LoadDadosSala()
        {
            try
            {
                Console.WriteLine("A buscar os detalhes da sala selecionada...");
                TcpClient client = new TcpClient("127.0.0.1", 3700);
                NetworkStream stream = client.GetStream();

                var request = JsonSerializer.Serialize(new
                {
                    action = "detalhes_da_sala",
                    salaSelecionada = nomeSala
                });

                Console.WriteLine($"Request de dados da sala: {request}");
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Resposta do server: {jsonResponse}");

                var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

                if (response["status"].ToString() == "sucesso" && response.ContainsKey("membros") && response.ContainsKey("detalhes"))
                {
                    var detalhesDaSala = JsonSerializer.Deserialize<List<string>>(response["detalhes"].ToString());
                    var membrosDaSala = JsonSerializer.Deserialize<Dictionary<string, string>>(response["membros"].ToString());

                    labelNomeSala.Text = detalhesDaSala[0];
                    labelDescricaoSala.Text = detalhesDaSala[1];
                    labelData.Text = detalhesDaSala[2];

                    foreach (var membro in membrosDaSala)
                    {
                        lbMembros.Items.Add(string.Format("{0} | {1}", membro.Key, membro.Value));
                    }

                    if (membrosDaSala.ContainsKey(username) && membrosDaSala[username].Contains("admin"))
                    {
                        btnAdicionar.Visible = true;
                    }
                }
                else
                {
                    MessageBox.Show("Não foi possivel encontrar os dados desta sala...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao buscar os dados da sala: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            AdicionarMembro am = new AdicionarMembro(nomeSala);
            this.Hide();
            am.Show();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            ListaSalas ls = new ListaSalas();
            this.Hide();
            ls.Show();
        }
    }
}

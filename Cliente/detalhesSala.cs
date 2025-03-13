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
    public partial class detalhesSala: Form
    {
        public string nomeSala;
        public int salaId;
        string username = Form1.userLogado;
        public string userSelec;
        TcpClient cliente = new TcpClient("127.0.0.1", 3700);

        public detalhesSala(int idSala, string sala)
        {
            InitializeComponent();
            this.nomeSala = sala;
            this.salaId = idSala;
            LoadDadosSala();
        }

        public void LoadDadosSala()
        {
            try
            {
                Console.WriteLine("A buscar os detalhes da sala selecionada...");
                NetworkStream stream = cliente.GetStream();

                var request = JsonSerializer.Serialize(new
                {
                    action = "detalhes_da_sala",
                    idSala = salaId.ToString(),
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
                        btnRemover.Visible = true;
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
            AdicionarMembro am = new AdicionarMembro(salaId, nomeSala);
            this.Hide();
            am.Show();
        }

        private void btnRemover_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient cliente = new TcpClient("127.0.0.1", 3700);
                NetworkStream stream = cliente.GetStream();
                Console.WriteLine("A remover o user...");

                string user_remover = lbMembros.GetItemText(lbMembros.SelectedItem);
                this.userSelec = user_remover;

                Console.WriteLine("user selecionado: " + user_remover);

                var request = JsonSerializer.Serialize(new
                {
                    action = "remover_user_da_sala",
                    idSala = salaId.ToString(),
                    user_a_remover = user_remover,
                    removido_por = username
                });

                Console.WriteLine($"Request de remoção do user: {request}");
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Resposta do server: {jsonResponse}");

                var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

                if (jsonResponse.Contains("sucesso"))
                {
                    lbMembros.Items.Remove(user_remover);
                    MessageBox.Show($"O user {user_remover} foi removido com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Console.WriteLine("Erro ao remover user da sala.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao remover user da sala: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            ChatSala cs = new ChatSala(salaId, nomeSala);
            this.Hide();
            cs.Show();
        }

        private void lbMembros_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.userSelec = lbMembros.GetItemText(lbMembros.SelectedItem);

            if (userSelec != null)
            {
                labelUserRole.Text = userSelec;
                labelUserRole.Visible = true;
                cbRoles.Visible = true;
                btnAtualizar.Visible = true;
                Console.WriteLine(labelUserRole.Text);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            string user_atualizar = userSelec;
            string role_selecionada = cbRoles.Text;

            try
            {
                TcpClient cliente = new TcpClient("127.0.0.1", 3700);
                NetworkStream stream = cliente.GetStream();

                var request = JsonSerializer.Serialize(new
                {
                    action = "atualizar_role",
                    idSala = salaId.ToString(),
                    user_a_atualizar = user_atualizar,
                    role_escolhida = role_selecionada,
                    atualizado_por = username
                });

                Console.WriteLine($"Request de atualização do user: {request}");
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Resposta do server: {jsonResponse}");

                var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

                if (response["message"].ToString().Contains("para admin!"))
                {
                    MessageBox.Show($"O user {userSelec} foi promovido para admin!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    detalhesSala ds = new detalhesSala(salaId, nomeSala);
                    this.Hide();
                    ds.Show();
                }else if (response["message"].ToString().Contains("para user!"))
                {
                    MessageBox.Show($"O user {userSelec} foi despromovido para user!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    detalhesSala ds = new detalhesSala(salaId,nomeSala);
                    this.Hide();
                    ds.Show();
                }
                else
                {
                    MessageBox.Show($"Erro ao atualizar a role do user!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar role do user: {ex.Message}");
                MessageBox.Show($"Erro ao atualizar role do user: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient cliente = new TcpClient("127.0.0.1", 3700);
                NetworkStream stream = cliente.GetStream();
                Console.WriteLine("A remover o user...");

                var request = JsonSerializer.Serialize(new
                {
                    action = "remover_user_da_sala",
                    idSala = salaId.ToString(),
                    user_a_remover = username,
                    removido_por = username
                });

                Console.WriteLine($"Request de remoção do user: {request}");
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Resposta do server: {jsonResponse}");

                var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

                if (jsonResponse.Contains("sucesso") && jsonResponse.Contains("removido"))
                {
                    lbMembros.Items.Remove(username);
                    MessageBox.Show($"Saíste da sala com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListaSalas ls = new ListaSalas();
                    this.Hide();
                    ls.Show();
                }else if(jsonResponse.Contains("sucesso") && jsonResponse.Contains("apagado"))
                {
                    lbMembros.Items.Remove(username);
                    MessageBox.Show($"A sala {nomeSala} foi apagada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ListaSalas ls = new ListaSalas();
                    this.Hide();
                    ls.Show();
                }
                else
                {
                    Console.WriteLine("Erro ao saír user da sala.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao saír da sala: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class ChatSala: Form
    {
        public string salaEscolhida;
        public int salaId;
        public string usernameL;
        private TcpClient client;
        private NetworkStream stream;

        public ChatSala(int idSala, string sala, string username)
        {
            InitializeComponent();
            
            this.salaEscolhida = sala;
            this.salaId = idSala;
            labelNomeSala.Text = sala;
            this.usernameL = username;
            conectarSala();
            LoadMensagens();
            carregarEmojis();
            carregarUsersDaSala();
        }

        private void conectarSala()
        {
            client = new TcpClient("127.0.0.1", 3700);
            stream = client.GetStream();

            var request = JsonSerializer.Serialize(new
            {
                action = "conectar_sala",
                idSala = salaId.ToString(),
                user = usernameL
            });

            Console.WriteLine($"Request enviada para o serivdor: {request}");
            byte[] data = Encoding.UTF8.GetBytes(request);
            stream.Write(data, 0, data.Length);
            stream.Flush();

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Resposta do server: {jsonResponse}");

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

            if (response["status"].ToString() == "sucesso" && response.ContainsKey("message"))
            {
                Console.WriteLine("O user conectou-se com sucesso, a começar a ouvir por mensagens.");
                Task.Run(() => OuvirMsgs());
            }
        }

        private void LoadMensagens()
        {
            TcpClient client = new TcpClient("127.0.0.1", 3700);
            NetworkStream stream = client.GetStream();

            var request = JsonSerializer.Serialize(new
            {
                action = "load_msgs_sala",
                idSala = salaId.ToString()
            });

            Console.WriteLine($"Request de load das mensagens da sala com o id {salaId}: " + request);
            byte[] data = Encoding.UTF8.GetBytes(request);
            stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Resposta do server: {jsonResponse}");

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

            if (response["status"].ToString() == "sucesso" && response.ContainsKey("msgs"))
            {
                var msgsEncontradas = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(response["msgs"].ToString());

                foreach (var entry in msgsEncontradas)
                {
                    string user = entry.Key;
                    List<string> mensagens = entry.Value;

                    foreach (string mensagem in mensagens)
                    {
                        if (lbMsgs.InvokeRequired)
                        {
                            lbMsgs.Invoke(new Action(() => { 
                                lbMsgs.Items.Add($"{user}: {mensagem}"); 
                            }));
                        }
                        else
                        {
                            lbMsgs.Items.Add($"{user}: {mensagem}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Erro ao buscar as mensagens da sala...");
            }
        }

        private void OuvirMsgs()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Mensagem recebida: {jsonMessage}");
                    var receivedData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMessage);

                    if (receivedData.ContainsKey("mensagem") && InvokeRequired == true)
                    {
                        string msgFormatada = receivedData["mensagem"];
                        Console.WriteLine("Mensagem formatada: " + msgFormatada);
                        BeginInvoke(new Action(() => lbMsgs.Items.Add(msgFormatada + Environment.NewLine)));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao receber mensagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
        }

        private void btnEnviarMsg_Click(object sender, EventArgs e)
        {
            string msg = txtMsg.Text;
            DateTime dataHoraHj = DateTime.Now;

            TcpClient client = new TcpClient("127.0.0.1", 3700);
            NetworkStream stream = client.GetStream();
            try
            {
                var request = JsonSerializer.Serialize(new
                {
                    action = "enviar_msg_sala",
                    idSala = salaId.ToString(),
                    user = usernameL,
                    dataHora = dataHoraHj.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss"),
                    mensagem = msg
                });

                Console.WriteLine($"Request enviada para o serivdor: {request}");
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                txtMsg.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar mensagem.");
            }
        }

        public void carregarEmojis()
        {
            for (int i = 0x1F599; i < 0x1F999; i++)
            {
                cbEmojis.Items.Add(char.ConvertFromUtf32(i));
            }
        }

        private void cbEmojis_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMsg.Text += cbEmojis.SelectedItem;
        }

        private void txtMsg_TextChanged(object sender, EventArgs e)
        {
            string at = "@";
            string text = txtMsg.Text;

            if (text.EndsWith(at))
            {
                int index = txtMsg.Text.IndexOf("@");

                Point textBoxLocation = txtMsg.GetPositionFromCharIndex(index);
                Point windowLocation = new Point((txtMsg.Location.X + 15) + textBoxLocation.X, txtMsg.Location.Y + textBoxLocation.Y);

                cbUsersSala.Visible = true;
                cbUsersSala.Location = windowLocation;
            }
        }

        private void carregarUsersDaSala()
        {
            client = new TcpClient("127.0.0.1", 3700);
            stream = client.GetStream();

            try
            {
                var request = JsonSerializer.Serialize(new
                {
                    action = "users_da_sala",
                    idSala = salaId.ToString(),
                });

                Console.WriteLine("Request de buscar users da sala: " + request);
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex);
            }
        }

        private void cbUsersSala_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selec = cbUsersSala.SelectedItem;

            if (selec != null)
            {
                txtMsg.Text += selec;
                cbUsersSala.Visible = false;
                txtMsg.SelectionStart = txtMsg.Text.Length;
            }
        }

        private void btnDetalhes_Click(object sender, EventArgs e)
        {
            detalhesSala ds = new detalhesSala(salaId, salaEscolhida, usernameL);
            this.Hide();
            ds.Show();
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            ListaSalas ls = new ListaSalas(usernameL);
            this.Hide();
            ls.Show();
        }
    }
}

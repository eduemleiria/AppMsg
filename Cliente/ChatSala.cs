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
    public partial class ChatSala: Form
    {
        public string salaEscolhida;
        public int salaId;
        public string usernameL;

        public ChatSala(int idSala, string sala, string username)
        {
            this.salaEscolhida = sala;
            this.salaId = idSala;
            InitializeComponent();
            Task.Run(() => LoadMensagens());
            labelNomeSala.Text = sala;
            this.usernameL = username;
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

                txtMsg.Clear();

                Console.WriteLine($"Request enviada para o serivdor: {request}");
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Mensagem recebida: {jsonMessage}");
                var receivedData = JsonSerializer.Deserialize<Dictionary< string, string>>(jsonMessage);

                if (receivedData.ContainsKey("mensagem"))
                {
                    string formattedMessage2 = receivedData["mensagem"];
                    string formattedMessage1 = receivedData["emissor"];
                    Console.WriteLine("Mensagem formatada: " + formattedMessage2);
                    lbMsgs.Items.Add(formattedMessage1 + ": " + formattedMessage2 + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao enviar mensagem.");
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

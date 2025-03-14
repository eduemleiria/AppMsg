using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Cliente
{
    public partial class ConversaDireta : Form
    {
        private string userLogado;
        private string passwordCriada;
        private int porta;
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer;

        public ConversaDireta(string username, int porta, string password)
        {
            InitializeComponent();
            this.userLogado = username;
            this.passwordCriada = password;
            this.porta = porta;
            this.buffer = new byte[1024];
            InitializeChat();
        }

        private void InitializeChat()
        {
            try
            {
                client = new TcpClient("127.0.0.1", porta);
                stream = client.GetStream();

                var request = JsonSerializer.Serialize(new
                {
                    action = "entrar_chat_privado",
                    password = passwordCriada,
                    user = userLogado
                });

                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Resposta do servidor: {response}");

                if (response.Contains("erro"))
                {
                    Console.WriteLine("estou dentro do verif de 2 pessoas dentro do chat");
                    this.Close();
                    MDSettings mds = new MDSettings();
                    mds.Show();
                    
                }

                lbMsgsD.Items.Add("Bem-vindo ao chat!");
                Task.Run(() => ListenForMessages());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar ao chat: {ex.Message}");
                MessageBox.Show($"Erro ao conectar ao chat: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                MDSettings mds = new MDSettings();
                mds.Show();
            }
        }

        private void ListenForMessages()
        {
            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Mensagem recebida: {jsonMessage}");
                    var receivedData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMessage);

                    if (receivedData.ContainsKey("mensagem") && InvokeRequired == true)
                    {
                        string formattedMessage2 = receivedData["mensagem"];
                        Console.WriteLine("Mensagem formatada: " + formattedMessage2);
                        BeginInvoke(new Action(() => lbMsgsD.Items.Add(formattedMessage2 + Environment.NewLine)));
                    }
                    else if (receivedData.ContainsKey("notificacao"))
                    {
                        string notificationText = receivedData["notificacao"];

                        if (notificationText == "Chat vazio..." && lblAfalarCom.Text.Contains("Estás a falar com"))
                        {
                            return;
                        }

                        Console.WriteLine($"Updating lblAfalarCom.Text to: {notificationText}");
                        Task.Delay(200).Wait();
                        BeginInvoke(new Action(() => lblAfalarCom.Text = notificationText));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao receber mensagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }
        }

        private void btnEnviarMsgD_Click(object sender, EventArgs e)
        {
            string msg = txtMsgD.Text;
            try
            {
                var request = JsonSerializer.Serialize(new
                {
                    action = "msg",
                    port = porta.ToString(),
                    user = userLogado,
                    mensagem_enviada = msg
                });

                Console.WriteLine($"O request enviado do lado do cliente: {request}");

                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                txtMsgD.Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Erro ao enviar mensagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 formLogin = new Form1();
            this.Hide();
            formLogin.Show();
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            var request = JsonSerializer.Serialize(new
            {
                action = "sair_chat_privado",
                port = porta.ToString(),
                user = userLogado
            });

            Console.WriteLine($"Request de saida no lado do cliente: {request}");

            byte[] data = Encoding.UTF8.GetBytes(request);
            stream.Write(data, 0, data.Length);
            stream.Flush();

            ListaSalas listaSalas = new ListaSalas();
            this.Hide();
            listaSalas.Show();
        }
    }
}

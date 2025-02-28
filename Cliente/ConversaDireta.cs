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
        private int porta;
        private TcpClient client;
        private NetworkStream stream;
        private byte[] buffer;

        public ConversaDireta(string username, int porta)
        {
            InitializeComponent();
            this.userLogado = username;
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
                    user = userLogado
                });

                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);

                Task.Run(() => ListenForMessages());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar ao chat: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (this.InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            lbMsgsD.Items.Add(message + Environment.NewLine);
                        }));
                    }
                    else
                    {
                        lbMsgsD.Items.Add(message + Environment.NewLine);
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
            try
            {
                string message = txtMsgD.Text;
                if (!string.IsNullOrWhiteSpace(message))
                {
                    var messageData = JsonSerializer.Serialize(new
                    {
                        enviado_por = userLogado,
                        mensagem = message
                    });

                    byte[] data = Encoding.UTF8.GetBytes(messageData);
                    stream.Write(data, 0, data.Length);

                    txtMsgD.Clear();
                }
            }
            catch (Exception ex)
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
            Console.WriteLine($"O user {userLogado} saiu da conversa!");
            ListaSalas listaSalas = new ListaSalas();
            this.Hide();
            listaSalas.Show();
        }
    }
}

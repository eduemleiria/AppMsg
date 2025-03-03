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
                TcpClient client = new TcpClient("127.0.0.1", porta);
                NetworkStream stream = client.GetStream();

                var request = JsonSerializer.Serialize(new
                {
                    action = "entrar_chat_privado",
                    port = porta.ToString(),
                    password = pass,
                    user = username
                });

                Console.WriteLine("Request de entrada: " + request);
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);
                stream.Flush();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Resposta do servidor: " + jsonResponse);
                    ConversaDireta cd = new ConversaDireta(username, porta, pass);
                    this.Hide();
                    cd.Show();
                }
                else
                {
                    Console.WriteLine("Nenhuma resposta do servidor.");
                }

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao tentar entrar no chat: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

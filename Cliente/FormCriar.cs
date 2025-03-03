using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class FormCriar : Form
    {
        private TcpListener chatServer;
        private TcpClient cliente;
        private NetworkStream stream;

        public FormCriar()
        {
            InitializeComponent();
        }

        private void btnVoltarAtras_Click(object sender, EventArgs e)
        {
            MDSettings mdsSettings = new MDSettings();
            this.Hide();
            mdsSettings.Show();
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            int porta = Convert.ToInt32(txtDefPorta.Value);
            string pass = txtPass.Text;
            string username = Form1.userLogado;

            try
            {
                TcpClient client = new TcpClient("127.0.0.1", 3700);
                NetworkStream stream = client.GetStream();

                var request = JsonSerializer.Serialize(new
                {
                    action = "criar_chat_privado",
                    port = porta.ToString(),
                    password = pass.ToString(),
                    user = username
                });

                Console.WriteLine("Request de criação: " + request);
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Resposta do server: {jsonResponse}");
                if (jsonResponse.Contains("Chat privado criado com sucesso"))
                {
                    MessageBox.Show("O chat privado foi criado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ConversaDireta cd = new ConversaDireta(username, porta, pass);
                    this.Hide();
                    cd.Show();
                }
                else
                {
                    MessageBox.Show("Erro ao criar o chat privado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar o chat privado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

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
                using (TcpClient client = new TcpClient("127.0.0.1", 3700))
                {
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

                    byte[] responseBuffer = new byte[1024];
                    int bytesRead = stream.Read(responseBuffer, 0, responseBuffer.Length);
                    string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                    
                    var responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(response);
                    Console.WriteLine("Response do server: " + responseData);
                    if (responseData != null && responseData.ContainsKey("status") && responseData["status"] == "sucesso")
                    {
                        MessageBox.Show($"Você entrou no chat privado da porta {porta}", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        ConversaDireta cd = new ConversaDireta(username, porta);
                        this.Hide();
                        cd.Show();
                    }
                    else
                    {
                        MessageBox.Show(responseData?["message"] ?? "Erro desconhecido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao tentar entrar no chat: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

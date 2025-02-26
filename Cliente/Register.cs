using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Text.Json;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cliente
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void btnIrLogin_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            this.Hide();
            loginForm.Show();
        }

        private void btnRegistar_Click(object sender, EventArgs e)
        {
            string username = txtRUsername.Text;
            string password = txtRPass.Text;
            string password2 = txtRPass2.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Preencha os campos todos porfavor!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != password2)
            {
                MessageBox.Show("As passwords não são parecidas!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var request = new
            {
                action = "register",
                username = username,
                password = password
            };

            string jsonRequest = JsonSerializer.Serialize(request);
            Console.WriteLine(jsonRequest);
            string response = SendToServer(jsonRequest);

            var serverResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

            if (serverResponse != null && serverResponse.ContainsKey("status") && serverResponse["status"] == "success")
            {
                MessageBox.Show("Conta criada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show(serverResponse?["message"], "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string SendToServer(string message)
        {
            try
            {
                using (TcpClient client = new TcpClient("127.0.0.1", 3700))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    return Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "error", message = ex.Message });
            }
        }
    }
}


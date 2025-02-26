using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Cliente
{
    public partial class Form1 : Form
    {
        private TcpClient cliente;
        private NetworkStream stream;

        public Form1()
        {
            InitializeComponent();
            ConectarAoServidor();
        }

        private void btnIrRegister_Click(object sender, EventArgs e)
        {
            Register registerForm = new Register();
            this.Hide();
            registerForm.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, preencha todos os campos!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var loginRequest = new
            {
                action = "login",
                username = username,
                password = password
            };

            string jsonRequest = JsonSerializer.Serialize(loginRequest);
            string response = EnviarParaServidor(jsonRequest);

            var serverResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(response);

            if (serverResponse != null && serverResponse.ContainsKey("status"))
            {
                if (serverResponse["status"] == "sucesso")
                {
                    MessageBox.Show("Login realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show(serverResponse["message"], "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ConectarAoServidor()
        {
            try
            {
                cliente = new TcpClient("127.0.0.1", 3700);
                stream = cliente.GetStream();
                Console.WriteLine("Conectado ao servidor.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string EnviarParaServidor(string mensagem)
        {
            try
            {
                if (cliente == null || !cliente.Connected)
                {
                    MessageBox.Show("Não foi possível estabelecer uma conexão com o servidor!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return JsonSerializer.Serialize(new { status = "erro", message = "nao conectado" });
                }
                byte[] data = Encoding.UTF8.GetBytes(mensagem);
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
            catch (Exception ex)
            {
                return JsonSerializer.Serialize(new { status = "erro", message = ex.Message });
            }
        }
    }
}

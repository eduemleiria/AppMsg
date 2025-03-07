using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Cliente
{
    public partial class ListaSalas : Form
    {

        public ListaSalas()
        {
            InitializeComponent();
            LoadListaSalas();
        }

        private void LoadListaSalas()
        {
            TcpClient client = new TcpClient("127.0.0.1", 3700);
            NetworkStream stream = client.GetStream();

            string username = Form1.userLogado;

            var request = JsonSerializer.Serialize(new
            {
                action = "load_salas",
                user = username
            });

            Console.WriteLine("Request de load das salas: " + request);
            byte[] data = Encoding.UTF8.GetBytes(request);
            stream.Write(data, 0, data.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Resposta do server: {jsonResponse}");

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

            if (response["status"].ToString() == "sucesso" && response.ContainsKey("salas"))
            {
                var salasEncontradas = JsonSerializer.Deserialize<List<string>>(response["salas"].ToString());

                Console.WriteLine("Salas encontradas:");
                foreach (string sala in salasEncontradas)
                {
                    Console.WriteLine($"- {sala}");
                    lbSalas.Items.Add(sala);
                }
            }
            else
            {
                Console.WriteLine("Erro ao carregar salas.");
            }
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 formLogin = new Form1();
            this.Hide();
            formLogin.Show();
        }

        private void iniciarConversaDiretaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MDSettings mds = new MDSettings();
            this.Hide();
            mds.Show();
        }

        private void criarUmaSalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CriarSala cs = new CriarSala();
            this.Hide();
            cs.Show();
        }
    }
}

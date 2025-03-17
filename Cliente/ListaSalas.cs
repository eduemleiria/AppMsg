using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cliente
{
    public partial class ListaSalas : Form
    {
        public string salaSelec;
        public string usernameL;
        private List<KeyValuePair<int, string>> listaSalas = new List<KeyValuePair<int, string>>();

        public ListaSalas(string username)
        {
            InitializeComponent();
            this.usernameL = username;
            LoadListaSalas();
        }

        private void LoadListaSalas()
        {
            TcpClient client = new TcpClient("127.0.0.1", 3700);
            NetworkStream stream = client.GetStream();

            var request = JsonSerializer.Serialize(new
            {
                action = "load_salas",
                user = usernameL
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
                var salasEncontradas = JsonSerializer.Deserialize<Dictionary<int, string>>(response["salas"].ToString());

                Console.WriteLine("Salas encontradas:");
                foreach (var sala in salasEncontradas)
                {
                    Console.WriteLine($"- {sala.Key} | {sala.Value}");

                    listaSalas.Add(new KeyValuePair<int, string>(sala.Key, sala.Value));
                    lbSalas.Items.Add(sala.Value);
                }
            }
            else
            {
                Console.WriteLine("O user não pertence a nenhuma sala...");
            }
        }

        private void lbSalas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSalas.SelectedIndex >= 0)
            {
                string salaSelec = lbSalas.SelectedItem.ToString();
                int idSala = lbSalas.SelectedIndex; 

                var salaEscolhida = listaSalas[idSala];

                int salaId = salaEscolhida.Key;
                string salaSelecionada = salaEscolhida.Value;

                this.salaSelec = salaSelecionada;

                ChatSala cs = new ChatSala(salaId, salaSelecionada, usernameL);
                this.Hide();
                cs.Show();
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
            MDSettings mds = new MDSettings(usernameL);
            this.Hide();
            mds.Show();
        }

        private void criarUmaSalaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CriarSala cs = new CriarSala(usernameL);
            this.Hide();
            cs.Show();
        }
    }
}

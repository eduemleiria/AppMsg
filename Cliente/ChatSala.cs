﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            InitializeComponent();
            
            this.salaEscolhida = sala;
            this.salaId = idSala;
            labelNomeSala.Text = sala;
            this.usernameL = username;
            conectarSala();
            LoadMensagens();
        }

        private void conectarSala()
        {
            TcpClient client = new TcpClient("127.0.0.1", 3700);
            NetworkStream stream = client.GetStream();

            var request = JsonSerializer.Serialize(new
            {
                action = "conectar_sala",
                idSala = salaId.ToString(),
                user = usernameL
            });

            Console.WriteLine($"Request enviada para o serivdor: {request}");
            byte[] data = Encoding.UTF8.GetBytes(request);
            stream.Write(data, 0, data.Length);
            stream.Flush();
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

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string jsonResponse = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Resposta do server: {jsonResponse}");

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonResponse);

            if (response["status"].ToString() == "sucesso" && response.ContainsKey("msgs"))
            {
                var msgsEncontradas = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(response["msgs"].ToString());

                foreach (var entry in msgsEncontradas)
                {
                    string user = entry.Key;
                    List<string> mensagens = entry.Value;

                    foreach (string mensagem in mensagens)
                    {
                        if (lbMsgs.InvokeRequired)
                        {
                            lbMsgs.Invoke(new Action(() => { 
                                lbMsgs.Items.Add($"{user}: {mensagem}"); 
                            }));
                        }
                        else
                        {
                            lbMsgs.Items.Add($"{user}: {mensagem}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Erro ao buscar as mensagens da sala...");
            }
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

                Console.WriteLine($"Request enviada para o serivdor: {request}");
                byte[] data = Encoding.UTF8.GetBytes(request);
                stream.Write(data, 0, data.Length);
                stream.Flush();

                txtMsg.Clear();
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

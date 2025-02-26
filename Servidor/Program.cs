using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Servidor
{
    internal class Program
    {
        static Dictionary<string, TcpClient> usersConectados = new Dictionary<string, TcpClient>();
        static TcpListener listener;
        static string usersFile = "users.json";
        static string salasFile = "salas.json";

        static void Main(string[] args)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3700);
            listener = new TcpListener(endPoint);
            listener.Start();

            Console.WriteLine("----> Servidor ligado <----");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Novo cliente conectado.");
                Task.Run(() => HandleClient(client));
            }
        }

        private static void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Recebido: {jsonMessage}");
                    var request = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMessage);

                    if (request != null && request.ContainsKey("action"))
                    {
                        if (request["action"] == "register")
                        {
                            HandleRegister(client, request);
                        }
                        else if (request["action"] == "login")
                        {
                            HandleLogin(client, request);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Utilizador desconectado.");
                client.Close();
            }
        }

        private static void HandleRegister(TcpClient client, Dictionary<string, string> request)
        {
            string username = request["username"];
            string password = request["password"];

            var users = LoadUsers();
            if (users.ContainsKey(username))
            {
                SendResponse(client, new { status = "erro", message = "Utilizador já existe." });
                return;
            }
            else
            {
                users[username] = password;
                SaveUsers(users);
                SendResponse(client, new { status = "sucesso", message = "Conta criada com sucesso!" });
            }
        }

        private static void HandleLogin(TcpClient client, Dictionary<string, string> request)
        {
            string username = request["username"];
            string password = request["password"];

            var users = LoadUsers();
            if (users.ContainsKey(username) && users[username] == password)
            {
                usersConectados[username] = client;
                SendResponse(client, new { status = "sucesso", message = "Login realizado com sucesso!" });
                Console.WriteLine($"O utilizador {username} foi conectado!");
            }
            else
            {
                SendResponse(client, new { status = "erro", message = "Credenciais inválidas." });
            }
        }

        private static Dictionary<string, string> LoadUsers()
        {
            if (!File.Exists(usersFile)) return new Dictionary<string, string>();
            string json = File.ReadAllText(usersFile);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }

        private static void SaveUsers(Dictionary<string, string> users)
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(usersFile, json);
        }

        private static void SendResponse(TcpClient client, object data)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}

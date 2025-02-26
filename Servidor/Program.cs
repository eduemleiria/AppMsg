using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Servidor
{
    internal class Program
    {
        static List<TcpClient> clientes = new List<TcpClient>();
        static TcpListener listener;
        static string usersFile = "users.json";
        static string salasFile = "salas.json";
        static Dictionary<string, string> users = new();

        static void Main(string[] args)
        {
            LoadUsers();

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3700);
            listener = new TcpListener(endPoint);
            listener.Start();

            Console.WriteLine("----> Servidor ligado <----");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                clientes.Add(client);
                Console.WriteLine("Novo cliente conectado.");

                Thread clientThread = new Thread(HandleClient);
                clientThread.Start(client);
            }
        }

        static void HandleClient(object obj)
        {
            TcpClient client = (TcpClient)obj;
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

                    if (request.ContainsKey("action"))
                    {
                        switch (request["action"])
                        {
                            case "register":
                                HandleRegister(client, request);
                                break;

                            case "login":
                                HandleLogin(client, request);
                                break;

                            default:
                                SendResponse(client, new { status = "error", message = "Ação desconhecida!" });
                                break;
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
                Console.WriteLine("Cliente desconectado.");
                clientes.Remove(client);
                client.Close();
            }
        }

        static void HandleRegister(TcpClient client, Dictionary<string, string> request)
        {
            string username = request["username"];
            string password = request["password"];

            if (users.ContainsKey(username))
            {
                SendResponse(client, new { status = "erro", message = "Nome do utilizador já existe!" });
            }
            else
            {
                users[username] = password;
                SaveUsers();
                SendResponse(client, new { status = "sucesso", message = "Conta criada com sucesso!" });
            }
        }

        static void HandleLogin(TcpClient client, Dictionary<string, string> request)
        {
            string username = request["username"];
            string password = request["password"];

            if (users.ContainsKey(username) && users[username] == password)
            {
                SendResponse(client, new { status = "sucesso", message = "Login realizado com sucesso!" });
            }
            else
            {
                SendResponse(client, new { status = "erro", message = "Utilizador ou password incorretos!" });
            }
        }

        static void LoadUsers()
        {
            if (File.Exists(usersFile))
            {
                string json = File.ReadAllText(usersFile);
                users = JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
        }

        static void SaveUsers()
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(usersFile, json);
        }

        static void SendResponse(TcpClient client, object data)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}

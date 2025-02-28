using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Servidor
{
    internal class Program
    {
        static Dictionary<string, TcpClient> usersConectados = new Dictionary<string, TcpClient>();
        static Dictionary<int, List<TcpClient>> chatsPrivados = new Dictionary<int, List<TcpClient>>();
        static Dictionary<int, List<string>> usersNoChatPrivado = new Dictionary<int, List<string>>();

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
                Console.WriteLine("Um utilizador anónimo inicio o programa.");
                Task.Run(() => HandleClient(client));
            }
        }

        private static void HandleClient(TcpClient cliente)
        {
            NetworkStream stream = cliente.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) return;

                string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var request = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMessage);
                Console.WriteLine($"Request received: {jsonMessage}");

                if (request != null && request.ContainsKey("action"))
                {
                    if (request["action"] == "register")
                    {
                        HandleRegister(cliente, request);
                    }
                    else if (request["action"] == "login")
                    {
                        string username = request["username"];
                        usersConectados[username] = cliente;
                        Console.WriteLine($"O utilizador {username} conectou-se.");
                        HandleLogin(cliente, request);
                    }else if (request["action"] == "criar_chat_privado")
                    {
                        int porta = int.Parse(request["port"]);
                        string password = request["password"];
                        Console.WriteLine($"Criar chat privado na porta: {porta} com a password: {password}");
                        Task.Run(() => StartPrivateChat(cliente, porta, password));
                    }else if (request["action"] == "entrar_chat_privado")
                    {
                        int porta = int.Parse(request["port"]);
                        string password = request["password"];
                        string username = request["user"];

                        if (chatsPrivados.ContainsKey(porta))
                        {
                            Task.Run(() => HandlePrivateChatClient(cliente, password, porta, username));
                        }
                        else
                        {
                            SendResponse(cliente, new { status = "erro", message = "Chat privado não encontrado." });
                            cliente.Close();
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
                cliente.Close();
            }
        }

        static void StartPrivateChat(TcpClient cliente, int porta, string chatPassword)
        {
            if (chatsPrivados.ContainsKey(porta))
            {
                Console.WriteLine($"Chat privado na porta {porta} já existe.");
                return;
            }

            try
            {
                TcpListener privateChatServer = new TcpListener(IPAddress.Any, porta);
                privateChatServer.Start();
                Console.WriteLine($"Chat privado iniciado na porta {porta}.");

                chatsPrivados[porta] = new List<TcpClient>();
                usersNoChatPrivado[porta] = new List<string>();

                SendResponse(cliente, new { status = "sucesso", message = "Chat privado criado com sucesso!" });

                while (true)
                {
                    TcpClient client = privateChatServer.AcceptTcpClient();
                    if (client.Connected)
                    {
                        Console.WriteLine("Novo utilizador conectado ao chat privado.");
                        Task.Run(() => HandlePrivateChatClient(client, chatPassword, porta, ""));
                    }
                    else
                    {
                        Console.WriteLine("Erro: Cliente não conectado corretamente.");
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar o chat privado: {ex.Message}");
            }
        }

        private static void HandlePrivateChatClient(TcpClient client, string chatPassword, int porta, string username)
        {
            if (client == null || !client.Connected)
            {
                Console.WriteLine("Erro: Cliente não conectado.");
                return;
            }

            NetworkStream stream = client.GetStream();

            if (stream == null)
            {
                Console.WriteLine("Erro: Não foi possível obter o NetworkStream.");
                client.Close();
                return;
            }

            byte[] buffer = new byte[1024];
            try
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) return;

                string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var request = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMessage);
                Console.WriteLine($"Request recebida: {jsonMessage}");

                if (request == null || !request.ContainsKey("password") || request["password"] != chatPassword)
                {
                    var response = JsonSerializer.Serialize(new { status = "erro", message = "Password errada!" });
                    stream.Write(Encoding.UTF8.GetBytes(response));
                    client.Close();
                    return;
                }

                if (!chatsPrivados.ContainsKey(porta))
                {
                    chatsPrivados[porta] = new List<TcpClient>();
                    usersNoChatPrivado[porta] = new List<string>();
                }

                chatsPrivados[porta].Add(client);
                usersNoChatPrivado[porta].Add(username);

                Console.WriteLine($"O user {username} juntou-se ao chat privado na porta {porta}.");
                BroadcastMessage($"{username} juntou-se ao chat!", chatsPrivados[porta]);

                while (true)
                {
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var messageData = JsonSerializer.Deserialize<Dictionary<string, string>>(receivedMessage);

                    if (messageData.ContainsKey("mensagem"))
                    {
                        Console.WriteLine($"{messageData["enviado_por"]}: {messageData["mensagem"]}");
                        BroadcastMessage($"{messageData["enviado_por"]}: {messageData["mensagem"]}", chatsPrivados[porta]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            finally
            {
                chatsPrivados[porta].Remove(client);
                Console.WriteLine($"O utilizador {username} saiu do chat privado.");
                usersNoChatPrivado[porta].Remove(username);
                client.Close();
            }
        }


        private static void BroadcastMessage(string message, List<TcpClient> clients)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { mensagem = message }));

            foreach (var client in clients)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(data, 0, data.Length);
                }
                catch
                {
                    Console.WriteLine("Falha ao enviar mensagem para o cliente.");
                }
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

            users[username] = password;
            SaveUsers(users);
            SendResponse(client, new { status = "sucesso", message = "Conta criada com sucesso!" });
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Intrinsics.X86;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Servidor.Sala;

namespace Servidor
{
    internal class Program
    {
        // Dicionario dos users conectados na app
        private static Dictionary<string, TcpClient> usersConectados = new Dictionary<string, TcpClient>();
        // Dicionario os users conectados a chats privados
        private static Dictionary<int, List<TcpClient>> chatsPrivados = new Dictionary<int, List<TcpClient>>();
        // Dicionario dos chats privados
        private static Dictionary<int, TcpListener> ListenersDeChatsPrivados = new Dictionary<int, TcpListener>();
        // Dicionario dos usernames users conectados a chats privados
        private static Dictionary<int, List<string>> usersNoChatPrivado = new Dictionary<int, List<string>>();
        // Dicionario com as salas existentes
        private static Dictionary<string, Sala> salas = LoadSalas();
        // Dicionario com os users existentes
        private static Dictionary<string, string> users = LoadUsers();

        // tcplistener é o que "ouve" as conexões do TCPClient (os clientes)
        static TcpListener listener;

        static string usersFile = "users.json";
        static string salasFile = "salas.json";

        static void Main(string[] args)
        {
            // Define o endpoint para o Tcplistener aceitar conexões de qualquer IP na porta 3700
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 3700);
            //inicializar o listener a ouvir pelos endpoints
            listener = new TcpListener(endPoint);
            //inicializar o listener (o servidor basicamente)
            listener.Start();

            Console.WriteLine("----> Servidor ligado <----");            

            // Enquanto o servidor estiver ligado, aceitar qualquer ip a tentar aceder ao servidor,
            // anunciar essa conexão e começar o HandleClient
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Um utilizador anónimo inicio o programa.");
                Task.Run(() => HandleClient(client));
            }
        }

        private static void HandleClient(TcpClient cliente)
        {
            // Inicializar o NetworkStream que recebe os dados do cliente conectado
            NetworkStream stream = cliente.GetStream();
            // Serve de armazenamento temporário para ler ou escrever dados
            byte[] buffer = new byte[1024];

            try
            {
                // Lê os dados recebidos da stream
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead == 0) return;
                // Traduzir os bytes em uma string
                string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // Deserialização da mensagem traduzida para o request
                var request = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMessage);

                Console.WriteLine($"Request recebida no HandleClient: {jsonMessage}");

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
                        Console.WriteLine("Usuários conectados atualmente:");
                        foreach (var nome in usersConectados.Keys)
                        {
                            Console.WriteLine($"- {nome}");
                        }
                        HandleLogin(cliente, request);
                    }else if (request["action"] == "criar_chat_privado")
                    {
                        int porta = int.Parse(request["port"]);
                        string password = request["password"];
                        string username = request["user"];
                        Console.WriteLine($"Criar chat privado na porta: {porta} com a password: {password}");
                        Task.Run(() => StartPrivateChat(cliente, porta, password, username));
                    }else if (request["action"] == "entrar_chat_privado")
                    {
                        int porta = int.Parse(request["port"]);
                        string password = request["password"];
                        string username = request["user"];
                        Console.WriteLine($"A tentar entrar no chat privado: \n - Com a porta: {porta} \n - Com a pass: {password} \n - Com o username: {username}");
                        if (chatsPrivados.ContainsKey(porta))
                        {
                            Task.Run(() => HandlePrivateChatClient(cliente, password, porta, username));
                        }
                        else
                        {
                            SendResponse(cliente, new { status = "erro", message = "Chat privado não encontrado." });
                            Console.WriteLine("cliente.close() no handleclient");
                            cliente.Close();
                        }
                    }else if (request["action"] == "criar_sala")
                    {
                        string username = request["user"];
                        string nome_Sala = request["nomeSala"];
                        string descricao_Sala = request["descricaoSala"];
                        string data_Hoje = request["dataHoje"];
                        Console.WriteLine($"A tentar criar a sala: {nome_Sala} \n - Com a descrição:{descricao_Sala} \n - No dia: {data_Hoje} \n - Criada pelo(a): {username} \n");
                        HandleCriarSala(cliente, username, nome_Sala, descricao_Sala, data_Hoje);
                    }else if (request["action"] == "load_salas")
                    {
                        string username = request["user"];
                        Console.WriteLine($"A verificar as salas a que o {username} pertence...");
                        HandleLoadSalas(cliente, username);
                    }else if (request["action"] == "detalhes_da_sala")
                    {
                        string salaSelec = request["salaSelecionada"];
                        HandleDetalhesSala(cliente, salaSelec);
                    }else if (request["action"] == "adicionar_membro_sala")
                    {
                        string sala = request["sala"];
                        string convidar = request["convidar"];
                        string convidado_por = request["convidado_por"];
                        HandleConvidarParaSala(cliente, sala, convidar, convidado_por);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }

        static void StartPrivateChat(TcpClient cliente, int porta, string chatPassword, string username)
        {
            TcpListener privateChatServer = null;

            if (ListenersDeChatsPrivados.ContainsKey(porta))
            {
                Console.WriteLine("Já está a ser usada...");
                SendResponse(cliente, new { status = "erro", message = "Essa porta já está a ser usada!" });
            }

            try
            {
                privateChatServer = new TcpListener(IPAddress.Any, porta);
                ListenersDeChatsPrivados[porta] = privateChatServer;
                privateChatServer.Start();
                Console.WriteLine($"Chat privado iniciado na porta {porta}.");

                chatsPrivados[porta] = new List<TcpClient>();
                usersNoChatPrivado[porta] = new List<string>();

                SendResponse(cliente, new { status = "sucesso", message = "Chat privado criado com sucesso!" });

                while (true)
                {
                    TcpClient client = privateChatServer.AcceptTcpClient();
                    if (client != null && client.Connected)
                    {
                       Console.WriteLine("Novo utilizador conectado ao chat privado.");
                       Task.Run(() => HandlePrivateChatClient(client, chatPassword, porta, username));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar o chat privado: {ex.Message}");
                if (privateChatServer != null)
                {
                    privateChatServer.Stop();
                }
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
                stream.Close();
                client.Close();
                return;
            }

            Console.WriteLine("Chats privados existentes: ");
            foreach (var chats in chatsPrivados.Keys)
            {
                Console.WriteLine($"- {chats}");
            }

            if (usersNoChatPrivado[porta].Count() == 2)
            {
                Console.WriteLine($"Número de users dentro da sala com a porta {porta}: {usersNoChatPrivado[porta].Count()}");
                SendResponse(client, new { status = "erro", message = "Chat privado está cheio!" });
                Console.WriteLine("Estou depois do SendResponse...");
                return;
            }

            byte[] buffer = new byte[1024];
            try
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                Console.WriteLine("Bytes lidos: " + bytesRead);

                if (bytesRead == 0)
                {
                    Console.WriteLine($"Cliente {username} desconectou-se inesperadamente.");
                    return;
                }

                string jsonMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var request = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMessage);
                username = request.ContainsKey("user") ? request["user"] : "Desconhecido";

                Console.WriteLine($"Request recebida: {jsonMessage}");

                if (request == null || !request.ContainsKey("password") || request["password"] != chatPassword)
                {
                    var response = JsonSerializer.Serialize(new { status = "erro", message = "Password errada!" });
                    stream.Write(Encoding.UTF8.GetBytes(response));
                    Console.WriteLine("cliente.close() no handleprivatechatclient dentro do if da validacao de pass");
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
                BroadcastMessage($"O {username} juntou-se ao chat!", chatsPrivados[porta]);

                

                while (true)
                {
                    if (usersNoChatPrivado[porta].Count == 2)
                    {
                        Console.WriteLine("num de users: " + usersNoChatPrivado[porta].Count);
                        string user1 = usersNoChatPrivado[porta][0];
                        string user2 = usersNoChatPrivado[porta][1];

                        var notificarUser1 = JsonSerializer.Serialize(new { notificacao = $"Estás a falar com {user2}" });
                        var notificarUser2 = JsonSerializer.Serialize(new { notificacao = $"Estás a falar com {user1}" });

                        byte[] dataForFirstUser = Encoding.UTF8.GetBytes(notificarUser1);
                        byte[] dataForSecondUser = Encoding.UTF8.GetBytes(notificarUser2);

                        TcpClient firstClient = chatsPrivados[porta][0];
                        firstClient.GetStream().Write(dataForFirstUser, 0, dataForFirstUser.Length);
                        firstClient.GetStream().Flush();

                        TcpClient secondClient = chatsPrivados[porta][1];
                        secondClient.GetStream().Write(dataForSecondUser, 0, dataForSecondUser.Length);
                        secondClient.GetStream().Flush();
                    }

                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    Console.WriteLine($"Bytes lidos no server depois de enviar msg: {bytesRead}");

                    string jsonMensagem = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    var pedido = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonMensagem);

                    Console.WriteLine($"Pedido recebido: {jsonMensagem}");

                    if (jsonMensagem.Contains("sair_chat_privado"))
                    {
                        BroadcastMessage($"O utilizador {username} saiu do chat!", chatsPrivados[porta]);
                        chatsPrivados[porta].Remove(client);
                        Console.WriteLine($"O utilizador {username} saiu do chat privado.");
                        usersNoChatPrivado[porta].Remove(username);

                        if (usersNoChatPrivado[porta].Count == 1)
                        {
                            var notificarUser = JsonSerializer.Serialize(new { notificacao = "Chat vazio..." });

                            byte[] data = Encoding.UTF8.GetBytes(notificarUser);

                            TcpClient firstClient = chatsPrivados[porta][0];
                            firstClient.GetStream().Write(data, 0, data.Length);
                            firstClient.GetStream().Flush();
                        }


                        if (usersNoChatPrivado[porta].Count == 0)
                        {
                            Console.WriteLine("Está vazio: " + usersNoChatPrivado[porta].Count());
                            chatsPrivados.Remove(porta);

                            if (ListenersDeChatsPrivados.ContainsKey(porta))
                            {
                                ListenersDeChatsPrivados[porta].Stop();
                                ListenersDeChatsPrivados.Remove(porta);
                                Console.WriteLine($"Chat privado na porta {porta} foi liberado.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ainda tem pessoas lá dentro: " + usersNoChatPrivado[porta].Count());
                            Console.WriteLine("Chats privados existentes: ");
                            foreach (var chats in chatsPrivados.Keys)
                            {
                                Console.WriteLine($"- {chats}");
                            }
                        }
                    }
                    else if (jsonMensagem.Contains("msg"))
                    {
                        string mensagem = pedido.ContainsKey("mensagem_enviada") ? pedido["mensagem_enviada"] : "Erro";
                        Console.WriteLine($"O utilizador {username} disse: {mensagem}");
                        BroadcastMessage($"{username}: {mensagem}", chatsPrivados[porta]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                client.Close();
                Console.WriteLine("client.close no catch");
            }
        }

        private static void BroadcastMessage(string message, List<TcpClient> clientes)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { mensagem = message }));

            foreach (var client in clientes)
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

        private static void HandleLoadSalas(TcpClient cliente, string username)
        {
            Console.WriteLine($"Estou a iniciar a procura das salas do user: {username}");

            string jsonString = File.ReadAllText(salasFile);
            Dictionary<string, Sala> salas = JsonSerializer.Deserialize<Dictionary<string, Sala>>(jsonString);

            if (salas != null)
            {
                List<string> salasDoUser = new List<string>();
                Console.WriteLine("Salas encontradas:");
                foreach (var salaEntry in salas)
                {
                    Sala sala = salaEntry.Value;
                    if (sala.Membros.ContainsKey(username))
                    {
                        Console.WriteLine($"- {sala.NomeSala}");
                        salasDoUser.Add(sala.NomeSala);
                    }
                }

                if (salasDoUser.Count > 0)
                {
                    SendResponse(cliente, new { status = "sucesso", salas = salasDoUser });
                }
                else
                {
                    SendResponse(cliente, new { status = "erro", message = $"Não foram encontradas salas onde o user {username} pertence." });
                }
            }
            else
            {
                Console.WriteLine("Não existem salas dentro do ficheiro.");
            }
        }

        private static void HandleCriarSala(TcpClient cliente, string username, string nomeSala, string descriSala, string dataHj)
        {
            var role = "admin";

            var salas = LoadSalas();

            salas[nomeSala] = new Sala
            {
                NomeSala = nomeSala,
                Descricao = descriSala,
                DataCriacao = dataHj,
                Membros = new Dictionary<string, string> {
                    { username, role }
                }
            };

            SaveSalas(salas);
            Console.WriteLine("A sala foi criada com sucesso!");

            SendResponse(cliente, new { status = "sucesso", message = "Sala criada com sucesso!"});
        }

        private static void HandleDetalhesSala(TcpClient cliente, string salaSeleci)
        {
            Console.WriteLine($"A recolher os dados da sala '{salaSeleci}'");
            string jsonString = File.ReadAllText(salasFile);
            Dictionary<string, Sala> salas = JsonSerializer.Deserialize<Dictionary<string, Sala>>(jsonString);


            if (salas.ContainsKey(salaSeleci))
            {
                var sala = salas[salaSeleci];
                List<string> detalhesDaSala = new List<string>();
                Dictionary<string, string> membrosDaSala = new Dictionary<string, string>();

                foreach(var membro in sala.Membros)
                {
                    Console.WriteLine(membro);
                    membrosDaSala.Add(membro.Key, membro.Value);
                }

                detalhesDaSala.AddRange(new List<string>() { sala.NomeSala, sala.Descricao, sala.DataCriacao });

                if (detalhesDaSala.Count > 0 && membrosDaSala.Count > 0)
                {
                    SendResponse(cliente, new { status = "sucesso", detalhes = detalhesDaSala, membros = membrosDaSala });
                }
                else
                {
                    SendResponse(cliente, new { status = "erro", message = $"Não foram encontradas dados da sala..." });
                }
            }
        }

        private static void HandleConvidarParaSala(TcpClient cliente, string salaSeleci, string convidar, string convidado_por)
        {
            Console.WriteLine($"A validar o convite ao user {convidar} efetuado pelo user {convidado_por} para a sala '{salaSeleci}'");
            string jsonSala = File.ReadAllText(salasFile);
            string jsonUser = File.ReadAllText(usersFile);

            Dictionary<string, Sala> salas = JsonSerializer.Deserialize<Dictionary<string, Sala>>(jsonSala);
            Dictionary<string, string> users = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonUser);

            if (salas.ContainsKey(salaSeleci))
            {
                var sala = salas[salaSeleci];

                if (!sala.NomeSala.Contains(convidar) && users.ContainsKey(convidar))
                {
                    string role = "user";
                    sala.Membros.Add(convidar, role);
                    SaveSalas(salas);
                    SendResponse(cliente, new { status = "Sucesso", message = $"O user {convidar} foi adicionado à sala com sucesso!" });

                }
                else
                {
                    SendResponse(cliente, new { status = "Erro", message = $"O user {convidar} não existe..." });
                    Console.WriteLine("Esse utilizador não existe...");
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
        private static Dictionary<string, Sala> LoadSalas()
        {
            if (!File.Exists(salasFile)) return new Dictionary<string, Sala>();
            string json = File.ReadAllText(salasFile);
            return JsonSerializer.Deserialize<Dictionary<string, Sala>>(json) ?? new Dictionary<string, Sala>();
        }

        private static void SaveSalas(Dictionary<string, Sala> salas)
        {
            string json = JsonSerializer.Serialize(salas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(salasFile, json);
        }

        private static void SendResponse(TcpClient client, object data)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}

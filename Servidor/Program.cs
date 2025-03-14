using System.Collections.Specialized;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        private static Dictionary<int, Sala> salas = LoadSalas();
        // Dicionario com os users existentes
        private static Dictionary<string, string> users = LoadUsers();
        // Dicionario com as mensagens dos users em salas
        private static Dictionary<int, Mensagem> mensagens = LoadMensagens();

        // tcplistener é o que "ouve" as conexões do TCPClient (os clientes)
        static TcpListener listener;

        static string usersFile = "users.json";
        static string salasFile = "salas.json";
        static string msgsFile = "msgsPorSala.json";

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
                        int id = buscarUltimoIdSala();
                        string username = request["user"];
                        string nome_Sala = request["nomeSala"];
                        string descricao_Sala = request["descricaoSala"];
                        string data_Hoje = request["dataHoje"];
                        Console.WriteLine($"A tentar criar a sala: {nome_Sala} \n - Com a descrição:{descricao_Sala} \n - No dia: {data_Hoje} \n - Criada pelo(a): {username} \n");
                        HandleCriarSala(cliente, username, id, nome_Sala, descricao_Sala, data_Hoje);
                    }else if (request["action"] == "load_salas")
                    {
                        string username = request["user"];
                        Console.WriteLine($"A verificar as salas a que o {username} pertence...");
                        HandleLoadSalas(cliente, username);
                    }else if (request["action"] == "detalhes_da_sala")
                    {
                        string id = request["idSala"];
                        HandleDetalhesSala(cliente, id);
                    }else if (request["action"] == "adicionar_membro_sala")
                    {
                        string id = request["idSala"];
                        string convidar = request["convidar"];
                        string convidado_por = request["convidado_por"];
                        HandleConvidarParaSala(cliente, id, convidar, convidado_por);
                    }else if (request["action"] == "remover_user_da_sala")
                    {
                        int id = int.Parse(request["idSala"]);
                        string user_removido = request["user_a_remover"];
                        string removido_por = request["removido_por"];
                        HandleRemoverUserSala(cliente, id, user_removido, removido_por);
                    }else if (request["action"] == "atualizar_role")
                    {
                        int id = int.Parse(request["idSala"]);
                        string user_a_atualizar = request["user_a_atualizar"];
                        string role_escolhida = request["role_escolhida"];
                        string atualizado_por = request["atualizado_por"];
                        HandleAtualizarRole(cliente, id, user_a_atualizar, role_escolhida, atualizado_por);
                    }else if (request["action"] == "enviar_msg_sala")
                    {
                        int id = buscarUltimoIdMsg();
                        string idSala = request["idSala"];
                        string emissor = request["user"];
                        string dataHora = request["dataHora"];
                        string msg = request["mensagem"];
                        HandleEnviarMsg(cliente, idSala, id, emissor, dataHora, msg);
                    }else if (request["action"] == "load_msgs_sala")
                    {
                        string idSala = request["idSala"];
                        HandleLoadMsgsSala(cliente, idSala);
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
                        Console.WriteLine($"A tentar remover o client: {client.Client.RemoteEndPoint}");
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
                Dictionary<int, string> salasDoUser = new Dictionary<int, string>();
                Console.WriteLine("Salas encontradas:");
                foreach (var salaEntry in salas)
                {
                    Sala sala = salaEntry.Value;
                    if (sala.Membros.ContainsKey(username))
                    {
                        Console.WriteLine($"- {sala.IdSala} | {sala.NomeSala}");
                        salasDoUser.Add(sala.IdSala, sala.NomeSala);
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

        private static void HandleCriarSala(TcpClient cliente, string username, int id, string nomeSala, string descriSala, string dataHj)
        {
            var role = "admin";

            var salas = LoadSalas();

            salas[id] = new Sala
            {
                IdSala = id,
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

        private static void HandleDetalhesSala(TcpClient cliente, string id)
        {
            Console.WriteLine($"A recolher os dados da sala '{id}'");
            string jsonString = File.ReadAllText(salasFile);
            Dictionary<int, Sala> salas = JsonSerializer.Deserialize<Dictionary<int, Sala>>(jsonString);

            var idNovo = int.Parse(id);

            if (salas.ContainsKey(idNovo))
            {
                var sala = salas[idNovo];
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

        private static void HandleConvidarParaSala(TcpClient cliente, string id, string convidar, string convidado_por)
        {
            Console.WriteLine($"A validar o convite ao user {convidar} efetuado pelo user {convidado_por} para a sala '{id}'");
            string jsonSala = File.ReadAllText(salasFile);
            string jsonUser = File.ReadAllText(usersFile);

            Dictionary<int, Sala> salas = JsonSerializer.Deserialize<Dictionary<int, Sala>>(jsonSala);
            Dictionary<string, string> users = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonUser);

            var idNovo = int.Parse(id);

            if (salas.ContainsKey(idNovo))
            {
                var sala = salas[idNovo];

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

        private static void HandleRemoverUserSala(TcpClient cliente, int id, string user_removido, string removido_por)
        {
            Console.WriteLine($"A validar a remoção do user {user_removido} efetuado pelo user {removido_por} para a sala '{id}'");
            string jsonSala = File.ReadAllText(salasFile);

            Dictionary<int, Sala> salas = JsonSerializer.Deserialize<Dictionary<int, Sala>>(jsonSala);

            if (salas.ContainsKey(id))
            {

                var sala = salas[id];
                string[] user_remover = user_removido.Split(new string[] { " | " }, StringSplitOptions.None);
                Console.WriteLine($"User a remover: {user_remover[0]}");


                if (sala.Membros.ContainsKey(user_remover[0]) && sala.Membros.Count() >= 2)
                {   
                    sala.Membros.Remove(user_remover[0]);

                    if (sala.Membros.Count() == 1)
                    {
                        var ultimo_user = sala.Membros.Last();
                        char[] caracteres_nq = [',', '[', ']', ' '];
                        string[] ult_user = ultimo_user.ToString().Split(caracteres_nq);
                        Console.WriteLine($"{sala.Membros.ContainsKey(ult_user[1])}");
                        Console.WriteLine(ult_user[1]);
                        sala.Membros.Remove(ult_user[1]);
                        sala.Membros.Add(ult_user[1], "admin");
                        SaveSalas(salas);
                    }
                    
                    if (sala.Membros.Count() >= 2 && !sala.Membros.ContainsValue("admin"))
                    {
                        Console.WriteLine("faz alguém de admin antes de saires!");
                        sala.Membros.Add(user_remover[0], "admin");
                        SendResponse(cliente, new { status = "Erro", message = $"Não restam admins na sala se tu saires!" });
                    }

                    SaveSalas(salas);
                    SendResponse(cliente, new { status = "Sucesso", message = $"O user {user_removido[0]} foi removido da sala com sucesso!" });
                }else if (sala.Membros.ContainsKey(user_remover[0]) && sala.Membros.Count() == 1)
                {
                    Console.WriteLine("A apagar sala...");
                    sala.Membros.Remove(user_remover[0]);
                    salas.Remove(id);
                    SaveSalas(salas);
                    SendResponse(cliente, new { status = "Sucesso", message = $"A sala {id} foi apagado com sucesso!" });
                }
                else
                {
                    SendResponse(cliente, new { status = "Erro", message = $"Erro ao tentar remover o user {user_removido}..." });
                }
            }
        }

        private static void HandleAtualizarRole(TcpClient cliente, int id, string user_a_atualizar, string role_escolhida, string atualizado_por)
        {
            Console.WriteLine($"A atualizar a role do {user_a_atualizar} para {role_escolhida} pelo {atualizado_por} na sala {id}");
            string jsonSala = File.ReadAllText(salasFile);

            Dictionary<int, Sala> salas = JsonSerializer.Deserialize<Dictionary<int, Sala>>(jsonSala);

            if (salas.ContainsKey(id))
            {
                var sala = salas[id];

                string[] user_atualizar = user_a_atualizar.Split(new string[] { " | " }, StringSplitOptions.None);
                var user = sala.Membros.ContainsKey(user_atualizar[0]);

                if (sala.Membros.ContainsKey(user_atualizar[0]))
                {
                    if (role_escolhida == "admin" && user_atualizar[1] == "user")
                    {
                        sala.Membros.Remove(user_atualizar[0]);
                        sala.Membros.Add(user_atualizar[0], "admin");
                        SaveSalas(salas);
                        Console.WriteLine($"User atualizado para admin: {user}");
                        SendResponse(cliente, new { status = "Sucesso", message = $"O user {user_atualizar[0]} foi atualizado com sucesso para admin!" });
                    }
                    else if(role_escolhida == "user" && user_atualizar[1] == "admin")
                    {
                        sala.Membros.Remove(user_atualizar[0]);
                        sala.Membros.Add(user_atualizar[0], "user");
                        SaveSalas(salas);
                        Console.WriteLine($"User atualizado para user: {user}");
                        SendResponse(cliente, new { status = "Sucesso", message = $"O user {user_atualizar[0]} foi atualizado com sucesso para user!" });
                    }
                    else
                    {
                        Console.WriteLine("ele ja tem essa role");
                    }
                }
                else
                {
                    SendResponse(cliente, new { status = "Erro", message = $"Erro ao tentar remover o user {user_atualizar}..." });
                }
            }
        }

        private static int buscarUltimoIdMsg()
        {
            string jsonMsg = File.ReadAllText(msgsFile);
            Dictionary<int, Mensagem> mensagens = JsonSerializer.Deserialize<Dictionary<int, Mensagem>>(jsonMsg);

            if (string.IsNullOrWhiteSpace(jsonMsg))
            {
                return 1;
            }

            if (mensagens == null || mensagens.Count == 0)
            {
                return 1;
            }

            int lastId = mensagens.Keys.Max();
            return lastId + 1;
        }

        private static int buscarUltimoIdSala()
        {
            string jsonSalas = File.ReadAllText(salasFile);
            Dictionary<int, Sala> salas = JsonSerializer.Deserialize<Dictionary<int, Sala>>(jsonSalas);

            if (string.IsNullOrWhiteSpace(jsonSalas))
            {
                return 1;
            }

            if (salas == null || salas.Count == 0)
            {
                return 1;
            }

            int lastId = salas.Keys.Max();
            return lastId + 1;
        }

        private static void HandleLoadMsgsSala(TcpClient cliente, string idSala)
        {
            
        }


        private static void HandleEnviarMsg(TcpClient cliente, string idSala, int id, string emissor, string dataHoraHj, string msg)
        {
            Console.WriteLine($"{id} A processar a mensagem '{msg}' enviada por '{emissor}' na sala '{idSala} ás {dataHoraHj}'");

            var idSalaNovo = int.Parse(idSala);

            var mensagens = LoadMensagens();
            mensagens[id] = new Mensagem
            {
                IdMsg = id,
                IdSala = idSalaNovo,
                User = emissor,
                DataEnvio = dataHoraHj,
                Msg = msg
            };

            SaveMensagens(mensagens);
            Console.WriteLine("A mensagem foi guardada com sucesso!");
            SendResponse(cliente, new { status = "sucesso", idDaSala = $"{idSalaNovo}", emissor = $"{emissor}", mensagem = $"{msg}" });
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
        private static Dictionary<int, Sala> LoadSalas()
        {
            if (!File.Exists(salasFile)) return new Dictionary<int, Sala>();
            string json = File.ReadAllText(salasFile);
            return JsonSerializer.Deserialize<Dictionary<int, Sala>>(json) ?? new Dictionary<int, Sala>();
        }

        private static void SaveSalas(Dictionary<int, Sala> salas)
        {
            string json = JsonSerializer.Serialize(salas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(salasFile, json);
        }

        public static Dictionary<int, Mensagem> LoadMensagens()
        {
            if (!File.Exists(msgsFile)) return new Dictionary<int, Mensagem>();
            string json = File.ReadAllText(msgsFile);
            return JsonSerializer.Deserialize<Dictionary<int, Mensagem>>(json) ?? new Dictionary<int, Mensagem>();
        }

        private static void SaveMensagens(Dictionary<int, Mensagem> mensagens)
        {
            string json = JsonSerializer.Serialize(mensagens, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(msgsFile, json);
        }

        private static void SendResponse(TcpClient client, object data)
        {
            string json = JsonSerializer.Serialize(data);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            client.GetStream().Write(buffer, 0, buffer.Length);
        }
    }
}

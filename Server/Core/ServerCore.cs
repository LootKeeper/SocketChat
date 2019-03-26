using Communication;
using Communication.Model;
using Server.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core
{
    public class ServerCore
    {
        private ConcurrentBag<ClientBase> _anons;
        private Dictionary<string, ClientBase> _users;
        private ServerListner _listner;

        public ServerCore()
        {
            _anons = new ConcurrentBag<ClientBase>();
            _users = new Dictionary<string, ClientBase>();
            _listner = new ServerListner();                       
        }

        public async Task Start()
        {
            _listner.Start();
            _listner.ClientAcceptHandler += ClientAcceptHandler;
            Console.WriteLine("Server started");
            Console.ReadKey();
        }

        private async Task Broadcust(Message message)
        {
            var clients = _users.Select(user => user.Value).Where(client => client.Info.Id != message.Id);

            foreach (var client in clients)
            {                
                client.Send(message);
            }
        }

        private void ClientAcceptHandler(object sender, TcpClient client)
        {
            ClientBase entry = new ClientBase(client);
            entry.OnMessageRecieve += MessageRecieved;
            _anons.Add(entry);
            Console.WriteLine($"{client.Client.LocalEndPoint.ToString()} connected");            
        }

        private void MessageRecieved(object sender, Message message)
        {
            if (message.Type == MessageType.Auth)
            {
                if(_users.TryAdd(message.Text, sender as ClientBase)) { 
                    Console.WriteLine($"{message.Text} entered to chat");                    
                }

                return;
            }

            var source = _anons.FirstOrDefault(client => client == sender);
            if (source != null)
            {
                source.Info.Id = message.Id;
                source.Info.Name = message.Name;
            }
            this.Broadcust(message);
        }
    }
}

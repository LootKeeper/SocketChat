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
        private ConcurrentBag<ClientEntry> _anons;
        private Dictionary<string, ClientEntry> _users;
        private ServerListner _listner;

        public ServerCore()
        {
            _anons = new ConcurrentBag<ClientEntry>();
            _users = new Dictionary<string, ClientEntry>();
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
            ClientEntry entry = new ClientEntry(client);
            entry.OnMessageRecieve += MessageRecieved;
            _anons.Add(entry);
            Console.WriteLine($"{client.Client.LocalEndPoint.ToString()} connected");            
        }

        private void MessageRecieved(object sender, Message message)
        {
            if (message.Type == MessageType.Auth)
            {
                HandleAuth(sender, message);
            }
            else
            {
                var source = _anons.FirstOrDefault(client => client == sender);
                if (source != null)
                {
                    source.Info.Id = message.Id;
                    source.Info.Name = message.Name;
                }
                this.Broadcust(message);
            }
        }

        private void HandleAuth(object sender, Message msg)
        {
            if (_users.TryAdd(msg.Text, sender as ClientEntry))
            {
                Console.WriteLine($"{msg.Text} entered to chat");
            }
        }
    }
}

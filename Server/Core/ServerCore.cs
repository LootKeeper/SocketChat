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
            var clients = _users.Where(client => client.Key != message.Id);

            foreach (var client in clients.Select(client => client.Value))
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
            else if(message.Type == MessageType.Quit)
            {
                HandleLogOut(sender, message);
            }
            else
            {
                HandleMessage(sender, message);
            }
        }

        private void HandleAuth(object sender, Message msg)
        {
            if (_users.TryAdd(msg.Id, sender as ClientEntry))
            {
                Console.WriteLine($"{msg.Text} entered to chat");
            }
        }

        private void HandleLogOut(object sender, Message msg)
        {
            if(_users.Any(user => user.Key == msg.Id))
            {
                _users.Remove(msg.Id);
            }
        }

        private void HandleMessage(object sender, Message msg)
        {
            this.Broadcust(msg);
        }

        public void ShutDown(object sender, EventArgs args)
        {
            _listner.Stop();
            foreach (var user in _users)
            {
                user.Value.ShutDown(sender, args);
            }
        }
    }
}

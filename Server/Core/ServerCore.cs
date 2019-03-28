using Communication;
using Communication.Model;
using Server.Client;
using Server.MessageHandler;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core
{
    public class ServerCore : IServerHandle
    {
        private ConcurrentDictionary<string, ClientEntry> _anons;
        private ConcurrentDictionary<string, ClientEntry> _users;
        private MessageHandlerChain _messageHandler;
        private ServerListner _listner;
        private bool keepAlive = true;

        public ServerCore()
        {
            _anons = new ConcurrentDictionary<string, ClientEntry>();
            _users = new ConcurrentDictionary<string, ClientEntry>();
            _listner = new ServerListner();
            _messageHandler = new MessageHandlerChain(this);
        }

        public async Task Start()
        {
            _listner.Start();
            _listner.ClientAcceptHandler += ClientAcceptHandler;
            Console.WriteLine("Server started");
            while (keepAlive)
            {
                char key = Console.ReadKey().KeyChar;
                if(key == 'c')
                {
                    keepAlive = false;
                }
            }
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
            if (_anons.TryAdd(entry.Ip, entry))
            {
                Console.WriteLine($"{client.Client.LocalEndPoint.ToString()} connected");
            }
        }

        private void MessageRecieved(object sender, Message message)
        {
            _messageHandler.Handle(sender, message);
        }

        public void HandleAuth(object sender, Message msg)
        {
            ClientEntry entry = sender as ClientEntry;

            if(_anons.TryRemove(entry.Ip, out entry))
            {
                if (_users.TryAdd(msg.Id, entry))
                {
                    Console.WriteLine($"{msg.Text} entered to chat");
                }
            }            
        }

        public void HandleLogOut(object sender, Message msg)
        {
            if(_users.Any(user => user.Key == msg.Id))
            {
                ClientEntry user = null;
                _users.Remove(msg.Id, out user);
                user.ShutDown(this, null);
                user.OnMessageRecieve -= HandleMessage;
                Console.WriteLine($"{msg.Name} logout");
            }

            ClientEntry entry = null;
            if(_anons.TryRemove((sender as ClientEntry).Ip, out entry))
            {
                entry.ShutDown(this, null);
                entry.OnMessageRecieve -= HandleMessage;
            }            
        }

        public void HandleMessage(object sender, Message msg)
        {
            this.Broadcust(msg);
        }

        public void ShutDown(object sender, EventArgs args)
        {
            _listner.Stop();
            _listner.ClientAcceptHandler -= ClientAcceptHandler;
            foreach (var user in _users)
            {
                user.Value.ShutDown(sender, args);
            }
        }
    }
}

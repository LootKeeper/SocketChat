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
        private ConcurrentStack<ClientBase> _nonAuthClients;
        private ConcurrentDictionary<string, ClientBase> _clients;
        private ServerListner _listner;

        public ServerCore()
        {
            _nonAuthClients = new ConcurrentStack<ClientBase>();
            _clients = new ConcurrentDictionary<string, ClientBase>();
            _listner = new ServerListner();                       
        }

        public async Task Start()
        {
            _listner.Start();
            _listner.ClientAcceptHandler += ClientAcceptHandler;
            Console.ReadKey();
        }

        private async Task Broadcust(Message message)
        {
            var clients = GetClients().Where(client => client.Info.Id != message.Id);

            foreach (var client in clients)
            {                
                client.Send(message);
            }
        }

        private IEnumerable<ClientBase> GetClients()
        {
            return _clients.Select(client => client.Value);
        }

        private void ClientAcceptHandler(object sender, TcpClient client)
        {
            ClientBase entry = new ClientBase(client);
            entry.OnMessageRecieve += MessageRecieved;
            this._nonAuthClients.Push(entry);            
            Console.WriteLine($"{entry.Info.Id} connected");            
        }

        private void MessageRecieved(object sender, Message message)
        {
            if (message.Type == MessageType.Message)
            {
                Console.WriteLine($"message from: {message.Name}");
                this.Broadcust(message);
            }
            else
            {
                this.GetClients().First(client => client.Info.Id == message.Id).Info.Name = message.Text;
                if (this._clients.TryAdd(message.Id, this._nonAuthClients.FirstOrDefault(entry => entry == sender)))
                {
                    message.Type = MessageType.Message;
                    message.Text = $"{message.Name} entred to chat";
                    Console.WriteLine(message.Text);
                    this.Broadcust(message);
                }
            }
        }
    }
}

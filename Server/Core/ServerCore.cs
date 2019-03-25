using Communication;
using Server.ActionHandler;
using Server.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core
{
    public class ServerCore
    {
        private ConcurrentDictionary<string, ClientEntry> _clients;
        private ServerListner _listner;
        private ActionHandlerManager _handlerManager;

        public ServerCore()
        {
            _clients = new ConcurrentDictionary<string, ClientEntry>();
            _listner = new ServerListner();
            
        }

        public async Task Start()
        {
            _listner.Start();
            _listner.ClientAcceptHandler += ClientAcceptHandler;
        }

        private async Task Broadcust(Message message)
        {
            foreach (var client in _clients)
            {
                client.Value.Send(message);
            }
        }

        private void ClientAcceptHandler(object sender, TcpClient client)
        {
            ClientEntry entry = new ClientEntry(client, new ServerActionHandlerManager());
            entry.OnMessageRecieve += MessageRecieved;
            this._clients.TryAdd(client.Client.LocalEndPoint.ToString(), entry);            
        }

        private void MessageRecieved(object sender, Message message)
        {

        }
    }
}

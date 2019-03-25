using Client.Core;
using Communication;
using Communication.Decoder;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Client
{
    public class ClientEntry
    {
        private TcpClient _clientConnection;
        private MessageDecoder _decoder;        
        private ActionHandlerManager _handlerManager;

        public ClientInfo Client;

        public event EventHandler<Message> OnMessageRecieve;

        public ClientEntry(TcpClient client, ActionHandlerManager handlerManager)
        {
            Client = new ClientInfo();
            _clientConnection = client;
            _handlerManager = handlerManager;
            _decoder = new MessageDecoder(client.GetStream());
            _decoder.MessageRecieved += OnMessageRecieve;
            _decoder.MessageRecieved += HandleAction;   
        }

        public void Send(Message message)
        {
            MessageDecoder decoder = new MessageDecoder(_clientConnection.GetStream());
            decoder.WriteMessage(message);
        }

        public void HandleAction(object sender, Message msg)
        {
            Task.Run(async () => _handlerManager.Handle(msg));
        }
    }
}

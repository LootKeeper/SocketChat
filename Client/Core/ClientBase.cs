using Client.Core;
using Communication;
using Communication.Decoder;
using Communication.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Client
{
    public abstract class ClientBase : Reciever
    {
        private TcpClient _clientConnection;
        private MessageDecoder _decoder;        

        public ClientInfo Info;

        public event EventHandler<Message> OnMessageRecieve;

        public ClientBase(TcpClient client)
        {
            Info = new ClientInfo(new Random().Next().ToString());
            _clientConnection = client;
            _decoder = new MessageDecoder(this, client.GetStream());
            _decoder.MessageRecieved += OnMessageRecieve;
            _decoder.MessageRecieved += HandleAction;   
        }

        // for client only
        public async Task<bool> Start(IPAddress ip, int port)
        {
            if (!_clientConnection.Connected)
            {
                await this._clientConnection.ConnectAsync(ip, port);
                return true;
            }

            return false;            
        }

        public void Send(string message)
        {
            Send(message, MessageType.Message);
        }

        public void Send(string message, MessageType type)
        {
            Send(new Message(Info.Id, type, Info.Name, message));
        }

        public void Send(Message message)
        {
            _decoder.WriteMessage(message);
        }

        public override void HandleAction(object sender, Message msg)
        {
            Task.Run(async () => Console.WriteLine($"{msg.Name}: {msg.Text}"));
        }
    }
}

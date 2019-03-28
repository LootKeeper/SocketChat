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
        protected TcpClient _clientConnection;
        protected MessageDecoder _decoder;        

        public ClientInfo Info;

        public ClientBase(TcpClient client)
        {
            Info = new ClientInfo(new Random().Next().ToString());
            _clientConnection = client;
        }       

        public Message Send(string message)
        {
            return Send(message, MessageType.Message);
        }

        public Message Send(string message, MessageType type)
        {
            return Send(new Message(Info.Id, type, Info.Name, message));
        }

        public Message Send(Message message)
        {
            _decoder.WriteMessage(message);
            return message;
        }        

        public abstract void HandleAction(object sender, Message msg);
    }
}

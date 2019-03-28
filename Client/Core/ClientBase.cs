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
               
        public ClientBase(TcpClient client)
        {            
            _clientConnection = client;
        }     

        public async Task<Message> Send(Message message)
        {
            await _decoder.WriteMessage(message);
            return message;
        }

        public abstract void ShutDown(object sender, EventArgs args);

        public abstract void HandleAction(object sender, Message msg);
    }
}

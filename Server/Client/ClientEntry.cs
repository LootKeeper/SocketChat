using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Communication.Model;

namespace Server.Client
{
    public class ClientEntry : ClientBase
    {
        public event EventHandler<Message> OnMessageRecieve;

        public ClientEntry(TcpClient client) : base(client)
        {
            this._decoder = new Communication.Decoder.MessageDecoder(client.GetStream());
            _decoder.MessageRecieved += HandleAction;
        }

        public override void HandleAction(object sender, Message msg)
        {
            OnMessageRecieve?.Invoke(this, msg);    
        }

        public override void ShutDown(object sender, EventArgs args)
        {
            _clientConnection.Close();
        }
    }
}

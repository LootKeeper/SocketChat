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

        public string Ip { get; set; }

        public ClientEntry(TcpClient client) : base(client)
        {
            Ip = client.Client.LocalEndPoint.ToString();
            this._decoder = new Communication.Decoder.MessageDecoder(client.GetStream());
            _decoder.MessageRecieved += HandleAction;
        }

        public override void HandleAction(object sender, Message msg)
        {
            OnMessageRecieve?.Invoke(this, msg);    
        }

        public override void ShutDown(object sender, EventArgs args)
        {
            _decoder.MessageRecieved -= HandleAction;
            _decoder.Stop();
        }
    }
}

using Client.Chat;
using Communication.Decoder;
using Communication.Model;
using Server.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ChatClient : ClientBase
    {
        private ChatEngine _chat;

        public ChatClient() : base(new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(ConfigurationManager.AppSettings["port"]))))
        {
            _chat = new ChatEngine();
        }       
        
        public void Start()
        {
            Task.Run(async () => { Start(IPAddress.Parse("127.0.0.1"), 8080); });
            Auth();
            WaitInput();
        }

        public async Task Start(IPAddress ip, int port)
        {
            if (!_clientConnection.Connected)
            {                
                await this._clientConnection.ConnectAsync(ip, port);
                DecoderInit();                
            }            
        }

        private void DecoderInit()
        {
            if (_clientConnection.Connected)
            {
                _decoder = new MessageDecoder(_clientConnection.GetStream());
                _decoder.MessageRecieved += HandleAction;
            }
        }

        private void Auth()
        {
            Console.Write("\r\nEnter your name: ");
            string name = Console.ReadLine();
            Info.Name = name;
            Send(name, Communication.Model.MessageType.Auth);
        }

        public void WaitInput()
        {
            Console.Write("\r\n>: ");
            string msg = Console.ReadLine();
            _chat.PushMessage(Send(msg), true);
            WaitInput();
        }

        public override void HandleAction(object sender, Message msg)
        {
            Task.Run(async () => {
                _chat.PushMessage(msg);
                WaitInput();
            });
        }
    }
}

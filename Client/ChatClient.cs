using Client.Chat;
using Client.Core;
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
        private ClientInfo _info;

        public ChatClient() : base(new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(ConfigurationManager.AppSettings["port"]))))
        {
            _info = new ClientInfo(new Random().Next().ToString());
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
            _info.Name = name;
            Send(name, Communication.Model.MessageType.Auth);
        }

        public async Task WaitInput()
        {
            Console.Write("\r\n>: ");
            string msg = Console.ReadLine();
            _chat.PushMessage(await Send(msg), true);
            await WaitInput();
        }

        public async Task<Message> Send(string message)
        {
            return await Send(message, MessageType.Message);
        }

        public async Task<Message> Send(string message, MessageType type)
        {
            return await Send(new Message(_info?.Id ?? "", type, _info?.Name ?? "", message));
        }

        public override void HandleAction(object sender, Message msg)
        {
            Task.Run(async () => {
                _chat.PushMessage(msg);
                WaitInput();
            });
        }

        public override void ShutDown(object sender, EventArgs args)
        {
            Send("", MessageType.Quit).Wait();
            _decoder.MessageRecieved -= HandleAction;
            _decoder.Stop();
        }
    }
}

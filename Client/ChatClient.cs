using Server.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;
 

namespace Client
{
    public class ChatClient
    {
        private ClientBase _client;        

        public ChatClient()
        {
            _client = new ClientBase(new TcpClient(new IPEndPoint(IPAddress.Parse("127.0.0.1"), int.Parse(ConfigurationManager.AppSettings["port"]))));
        }

        public void Start()
        {
            bool isConnected = _client.Start(IPAddress.Parse("127.0.0.1"), 8080).GetAwaiter().GetResult();
            if (isConnected)
            {
                Console.WriteLine("Connected to server");
                Auth();
                WaitInput();
            }
        }
        
        private void Auth()
        {
            Console.Write("\r\nEnter your name: ");
            string name = Console.ReadLine();
            _client.Info.Name = name;
            _client.Send(name, Communication.Model.MessageType.Auth);
        }

        private void WaitInput()
        {
            _client.WaitInput();
        }
    }
}

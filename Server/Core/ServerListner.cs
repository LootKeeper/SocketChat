using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Core
{
    public class ServerListner
    {
        public event EventHandler<TcpClient> ClientAcceptHandler;

        TcpListener _listner;

        public ServerListner()
        {
            _listner = new TcpListener(IPAddress.Parse("127.0.0.1") ,8080);
        }

        public async Task Start()
        {
            _listner.Start();

            while (true)
            {
                AcceptClientAsync(_listner.AcceptTcpClientAsync());
            }
        }

        public void Stop()
        {
            _listner.Stop();
        }

        private async Task AcceptClientAsync(Task<TcpClient> client) 
        {
            ClientAcceptHandler?.Invoke(this, await client);
        }
    }
}

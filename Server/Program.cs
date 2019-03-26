using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Core.ServerCore server = new Core.ServerCore();
            server.Start();
        }
    }
}

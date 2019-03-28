using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server.Core.ServerCore server = new Core.ServerCore();
            AppDomain.CurrentDomain.ProcessExit += server.ShutDown;
            server.Start();
        }
    }
}

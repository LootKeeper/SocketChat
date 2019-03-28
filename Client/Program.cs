using System;
using System.Net;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatClient client = new ChatClient();
            client.Start();
        }
    }
}

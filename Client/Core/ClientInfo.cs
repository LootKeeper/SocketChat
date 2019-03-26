using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Core
{
    public class ClientInfo
    {
        public ClientInfo() { }

        public ClientInfo(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}

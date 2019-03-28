using Communication.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Chat
{
    public class MessageHolder
    {
        public Message Message { get; private set; }
        
        public bool IsOwner { get; private set; }

        public MessageHolder(Message message, bool isOwner = false)
        {
            Message = message;
            IsOwner = isOwner;
        }
    }
}

using Communication.Model;
using Server.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.MessageHandler
{
    public abstract class MessageHandler
    {
        private MessageHandler _successor;
        protected IServerHandle server;

        public MessageHandler(IServerHandle server, MessageHandler successor)
        {
            _successor = successor;
            this.server = server;
        }

        public abstract void Handle(object sender, Message message);

        protected void PassHandle(object sender, Message message)
        {
            _successor.Handle(sender, message);
        }
    }
}

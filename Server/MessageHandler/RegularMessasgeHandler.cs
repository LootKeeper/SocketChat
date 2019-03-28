using System;
using System.Collections.Generic;
using System.Text;
using Communication.Model;
using Server.Core;

namespace Server.MessageHandler
{
    public class RegularMessasgeHandler : MessageHandler
    {
        public RegularMessasgeHandler(IServerHandle server, MessageHandler successor) : base(server, successor)
        {
        }

        public override void Handle(object sender, Message message)
        {
            if(message.Type == MessageType.Message)
            {
                server.HandleMessage(sender, message);
            }
            else
            {
                base.PassHandle(sender, message);
            }
        }
    }
}

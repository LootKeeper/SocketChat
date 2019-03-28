using System;
using System.Collections.Generic;
using System.Text;
using Communication.Model;
using Server.Core;

namespace Server.MessageHandler
{
    public class LogoutMessageHandler : MessageHandler
    {
        public LogoutMessageHandler(IServerHandle server, MessageHandler successor) : base(server, successor)
        {
        }

        public override void Handle(object sender, Message message)
        {
            if(message.Type == MessageType.Quit || message == null)
            {
                server.HandleLogOut(sender, message);
            }
            else
            {
                base.PassHandle(sender, message);
            }
        }
    }
}

using Communication.Model;
using Server.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.MessageHandler
{
    public class MessageHandlerChain
    {
        MessageHandler _messageHandler;

        public MessageHandlerChain(IServerHandle server)
        {
            _messageHandler = 
                new RegularMessasgeHandler(server,
                    new AuthMessageHandler(server,
                        new LogoutMessageHandler(server, null)));
        }

        public void Handle(object sender, Message message)
        {
            Task.Run(async () => this._messageHandler.Handle(sender, message));
        }
    }
}

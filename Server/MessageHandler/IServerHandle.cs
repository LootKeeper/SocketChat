using Communication.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.MessageHandler
{
    public interface IServerHandle
    {
        void HandleAuth(object sender, Message msg);
        void HandleLogOut(object sender, Message msg);
        void HandleMessage(object sender, Message msg);
    }
}

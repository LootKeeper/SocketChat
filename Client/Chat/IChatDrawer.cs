using Communication.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Chat
{
    public interface IChatDrawer
    {
        void Draw(Stack<MessageHolder> messages);
    }
}

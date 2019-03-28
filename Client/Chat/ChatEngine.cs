using Communication.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Client.Chat
{
    public class ChatEngine
    {
        private ChatDrawer _drawer;
        private ConcurrentStack<MessageHolder> _messagesHistory;

        public ChatEngine()
        {
            _drawer = new ChatDrawer();
            _messagesHistory = new ConcurrentStack<MessageHolder>();
        }

        public void PushMessage(Message msg, bool isOwner = false)
        {
            _messagesHistory.Push(new MessageHolder(msg, isOwner));
            Draw();
        }

        private void Draw()
        {
            _drawer.Draw(new Stack<MessageHolder>(_messagesHistory));
        }
    }
}

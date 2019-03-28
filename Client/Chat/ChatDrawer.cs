using System;
using System.Collections.Generic;
using System.Text;
using Communication.Model;

namespace Client.Chat
{
    public class ChatDrawer : IChatDrawer
    {
        public void Draw(Stack<MessageHolder> messages)
        {
            CleanConsole();
            ResetCursorPossition();
            DrawLines(messages);
        }

        private void CleanConsole()
        {
            Console.Clear();
        }

        private void ResetCursorPossition()
        {
            Console.SetCursorPosition(0, 0);
        }

        private void DrawLines(Stack<MessageHolder> messages)
        {
            while(messages.Count > 0)
            {
                DrawLine(messages.Pop());
            }
        } 

        private void DrawLine(MessageHolder message)
        {
            if (message.IsOwner)
            {
                DrawOwnerMsg(message.Message);
            }
            else
            {
                DrawRegularMsg(message.Message);
            }
        }          
        
        private void DrawOwnerMsg(Message msg)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            DrawMsg(msg);
        }

        private void DrawRegularMsg(Message msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            DrawMsg(msg);
        }

        private void DrawMsg(Message msg)
        {
            Console.Write($"{msg.Name}: ");
            Console.ResetColor();
            Console.Write($"{msg.Text}\r\n");
        }
    }
}

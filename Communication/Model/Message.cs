using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Model
{
    public class Message
    {
        public string Id { get; set; }
        public MessageType Type { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }

        public Message() { }

        public Message(string id, MessageType type, string name, string text)
        {
            this.Id = id;
            this.Type = type;
            this.Name = name;
            this.Text = text;
        }
    }
}

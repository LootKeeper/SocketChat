using Communication.Actions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Communication
{
    public class Message
    {
        public ActionType ActionType { get; set; }
        public ActionInfo ActionInfo { get; set; }

        public Message(ActionType type, ActionInfo actionInfo)
        {
            this.ActionType = type;
            this.ActionInfo = actionInfo;
        }

        public Message() { }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Communication.Actions
{
    public class ActionInfo
    {
        public string Body { get; set; }

        public ActionInfo(string body)
        {
            this.Body = body;
        }

        public ActionInfo() { }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Model
{
    public abstract class Reciever
    {
        public abstract void HandleAction(object sender, Message message);
    }
}

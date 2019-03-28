using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Model
{
    public interface Reciever
    {
        void HandleAction(object sender, Message message);
    }
}

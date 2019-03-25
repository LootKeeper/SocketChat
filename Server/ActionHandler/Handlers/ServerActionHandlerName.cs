using Communication.ActionHandler.Handlers;
using Communication.Actions;
using Server.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.ActionHandler.Handlers
{
    public class ServerActionHandlerName : ActionHandlerBase
    {
        private ClientEntry _entry;

        public ServerActionHandlerName(ClientEntry entry)
        {
            _entry = entry;
        }

        public async Task Handle(ActionInfo actionInfo)
        {
            _entry.Client.Name = actionInfo.Body;
        }
    }
}

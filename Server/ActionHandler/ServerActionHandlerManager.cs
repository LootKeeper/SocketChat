using Client.Core;
using Communication;
using Communication.ActionHandler;
using Communication.ActionHandler.Handlers;
using Communication.Actions;
using Server.ActionHandler.Handlers;
using Server.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.ActionHandler
{
    public class ServerActionHandlerManager : ActionHandlerManager
    {
        public ServerActionHandlerManager(ClientEntry entry)
        {
            base._actionHandlers = new Dictionary<ActionType, ActionHandlerBase>()
            {
                [ActionType.Name] = new ServerActionHandlerName(entry)
            };
        }

        public override async Task Handle(Message message)
        {
             this._actionHandlers[message.ActionType].Handle(message.ActionInfo);
        }
    }
}

using Communication.ActionHandler.Handlers;
using Communication.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public abstract class ActionHandlerManager
    {
        protected Dictionary<ActionType, ActionHandlerBase> _actionHandlers;

        public abstract Task Handle(Message message);
    }
}

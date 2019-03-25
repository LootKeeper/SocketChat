using Communication.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Communication.ActionHandler.Handlers
{
    public interface ActionHandlerBase
    {
        Task Handle(ActionInfo actionInfo);
    }
}

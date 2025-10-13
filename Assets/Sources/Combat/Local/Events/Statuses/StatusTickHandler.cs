using Combat.Common.ValueObjects;

using Combat.Local.Domain.UseCases;

using System;

namespace Combat.Local.Events
{
    public class StatusTickHandler : IStatusTickEventHandler
    {
        public event Action<StatusId> Ticked;

        public void HandleEvent(StatusId statusId)
        {
            Ticked?.Invoke(statusId);
        }
    }
}

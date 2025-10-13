using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

using System;

namespace Combat.Local.Events
{
    public class StatusRemoveHandler : IRemoveStatusEventHandler
    {
        public event Action<StatusId> Removed;

        public void HandleEvent(StatusId target)
        {
            Removed?.Invoke(target);
        }
    }
}

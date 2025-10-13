using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

using System;

namespace Combat.Local.Events
{
    public class StatusExpireHandler : IStatusExpiredEventHandler
    {
        public event Action<StatusId> Expired;

        public void HandleEvent(StatusId statusId)
        {
            Expired?.Invoke(statusId);
        }
    }
}

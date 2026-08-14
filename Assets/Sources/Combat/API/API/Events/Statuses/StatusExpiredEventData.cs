using Combat.API.Contexts;
using Combat.Common.ValueObjects;

namespace Combat.API.Events
{
    public readonly struct StatusExpiredEventData : IEventData
    {
        public readonly StatusId StatusId;

        public StatusExpiredEventData(StatusId statusId)
        {
            StatusId = statusId;
        }
    }
}

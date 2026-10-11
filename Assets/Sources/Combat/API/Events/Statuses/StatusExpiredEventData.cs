using Combat.API.Contexts;
using Combat.Common.Primitives;

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

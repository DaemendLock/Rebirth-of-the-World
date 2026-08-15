using Combat.API.Contexts;
using Combat.Common.ValueObjects;

namespace Combat.API.Events
{
    public readonly struct StatusRemovedEventData : IEventData
    {
        public readonly StatusId StatusId;

        public StatusRemovedEventData(StatusId statusId)
        {
            StatusId = statusId;
        }
    }
}

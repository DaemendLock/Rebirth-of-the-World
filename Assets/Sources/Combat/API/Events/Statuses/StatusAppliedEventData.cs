using Combat.API.Contexts;
using Combat.Common.Primitives;

namespace Combat.API.Events
{
    public readonly struct StatusAppliedEventData : IEventData
    {
        public readonly StatusId StatusId;
        public readonly UnitId Parent;
        public readonly AbilityKey? Source;

        public StatusAppliedEventData(UnitId parent, AbilityKey? source, StatusId statusId)
        {
            StatusId = statusId;
            Parent = parent;
            Source = source;
        }
    }
}

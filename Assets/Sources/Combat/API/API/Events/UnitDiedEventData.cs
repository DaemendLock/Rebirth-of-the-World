using Combat.API.Contexts;
using Combat.Common.Primitives;

namespace Combat.API.Events
{
    public readonly struct UnitDiedEventData : IEventData
    {
        public readonly UnitId Victim;

        public UnitDiedEventData(UnitId victim)
        {
            Victim = victim;
        }
    }
}

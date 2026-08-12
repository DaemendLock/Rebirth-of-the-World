using Combat.API.Contexts;
using Combat.Common.ValueObjects;

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

    public readonly struct ObjectiveCompletedEventData : IEventData
    {
        public readonly ObjectiveId ObjectiveId;

        public ObjectiveCompletedEventData(ObjectiveId objectiveId)
        {
            ObjectiveId = objectiveId;
        }
    }

    public readonly struct DealDamageEventData : IEventData
    {
        public DealDamageEventData(float finalDamage)
        {
            FinalDamage = finalDamage;
        }

        public readonly float FinalDamage { get; }
    }
}

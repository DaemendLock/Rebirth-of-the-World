using Combat.API.Contexts;

namespace Combat.API.Events
{

    public readonly struct DealDamageEventData : IEventData
    {
        public DealDamageEventData(float finalDamage)
        {
            FinalDamage = finalDamage;
        }

        public readonly float FinalDamage { get; }
    }
}

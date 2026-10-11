using Combat.API.Contexts;

namespace Combat.API.Events
{
    public readonly struct TakeDamageEventData : IEventData
    {
        public TakeDamageEventData(float finalDamage)
        {
            FinalDamage = finalDamage;
        }

        public readonly float FinalDamage { get; }
    }
}

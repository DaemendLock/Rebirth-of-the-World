using Combat.API.Contexts;

namespace Combat.API.Events
{

    public readonly struct DealDamageEventData : IEventData
    {
        public DealDamageEventData(float finalHealthDamage, float finalComposureDamage)
        {
            FinalHealthDamage = finalHealthDamage;
            FinalComposureDamage = finalComposureDamage;
        }

        public float FinalHealthDamage { get; }
        public float FinalComposureDamage { get; }
        public float FinalDamage => FinalHealthDamage;
    }
}

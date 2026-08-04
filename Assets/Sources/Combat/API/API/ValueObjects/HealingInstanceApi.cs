using Combat.Common.Flags;

namespace Combat.API.ValueObjects
{
    public ref struct HealingInstanceApi
    {
        private const float PercentConvertionRation = 0.001f;

        public HealingInstanceApi(IUnit healee, IUnit healer, IAbilityApi source, float healing, HealingFlags flags)
        {
            Healer = healer;
            Target = healee;
            Source = source;
            OriginalHealing = healing;
            Flags = flags;
            BaseHealing = healing;
            HealingPercent = 100f;
        }

        public IUnit Target { get; }
        public IUnit Healer { get; }
        public IAbilityApi Source { get; }
        public float OriginalHealing { get; }

        public HealingFlags Flags { get; set; }
        public float BaseHealing { get; set; }
        public float HealingPercent { get; set; }

        public float GetCurrentHealing()
        {
            if (Target == null)
            {
                return 0;
            }

            float healerVersalityBonus = Healer == null ? 1 : Healer.GetVersalityModifier();

            return System.Math.Max(0, BaseHealing * HealingPercent * PercentConvertionRation * healerVersalityBonus / Target.GetVersalityModifier());
        }
    }
}

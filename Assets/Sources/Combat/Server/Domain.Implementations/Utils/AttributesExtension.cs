using Server.Combat.Domain.Attributes;
using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.Implementations.Utils.Extenstions
{
    public static class AttributesExtension
    {
        private const float HasteEffiency = 0.00075f;
        private const float VersalityEffiency = 0.002f;

        public static float EvaluateHasteMultiplier(this Unit attributesOwner) =>
            System.Math.Clamp(1 + attributesOwner.GetAttributeValue(Attribute.Haste) * HasteEffiency, 0.5f, 2f);

        public static float EvaluateVersalityMultiplier(this Unit statsOwner) =>
            1 + System.Math.Max(0, statsOwner.GetAttributeValue(Attribute.Versality) * VersalityEffiency);

        public static float EvaluateDamageDealMultiplier(this Unit statsOwner) =>
            1 + statsOwner.GetAttributeValue(Attribute.OutcomeDamage);

        public static float EvaluateDamageRecivedMultiplier(this Unit statsOwner) =>
            1 + statsOwner.GetAttributeValue(Attribute.IncomeDamage);

        public static float EvaluateHealingOutcomeMultiplier(this Unit statsOwner) =>
            1 + statsOwner.GetAttributeValue(Attribute.OutcomeHealing);

        public static float EvaluateHealingRecivedMultiplier(this Unit statsOwner) =>
            1 + statsOwner.GetAttributeValue(Attribute.IncomeHealing);

        public static float EvaluateMaxHealth(this Unit statsOwner) =>
            statsOwner.GetAttributeValue(Attribute.Endurance);
    }
}

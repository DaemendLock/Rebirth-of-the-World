using Combat.Common.Flags;

namespace Combat.Local.Gateways.Models
{
    public ref struct HealingModification
    {
        public HealingModification(float bonusHealing, float bonusHealingPercent, HealingFlags bonusFlags)
        {
            BonusHealing = bonusHealing;
            BonusHealingPercent = bonusHealingPercent;
            BonusFlags = bonusFlags;
        }

        public float BonusHealing { get; set; }

        public float BonusHealingPercent { get; set; }

        public HealingFlags BonusFlags { get; set; }
    }

    public ref struct DamageModifiaction
    {
        public DamageModifiaction(float bonusDamage, float bonusDamagePercent, DamageFlags bonusFlags)
        {
            BonusDamage = bonusDamage;
            BonusDamagePercent = bonusDamagePercent;
            BonusFlags = bonusFlags;
        }

        public float BonusDamage { get; set; }
        public float BonusDamagePercent { get; set; }
        public DamageFlags BonusFlags { get; set; }
    }
}

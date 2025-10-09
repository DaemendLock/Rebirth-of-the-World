using Combat.Common.Flags;

namespace Combat.Local.Data.Models
{
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

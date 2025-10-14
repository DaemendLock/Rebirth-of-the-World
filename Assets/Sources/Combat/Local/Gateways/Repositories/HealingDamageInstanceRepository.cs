using Combat.Common.Flags;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

namespace Combat.Local.Gateways.Repositories
{
    public class HealingDamageInstanceRepository : IHealingDamageInstanceRepository
    {
        private readonly IDamageModificationDataSource _damageModificationDataSource;
        private readonly IHealingModificationDataSource _healingModificationDataSource;

        public HealingDamageInstanceRepository(IDamageModificationDataSource damageModificationDataSource, IHealingModificationDataSource healDamageModificationDataSource)
        {
            _damageModificationDataSource = damageModificationDataSource;
            _healingModificationDataSource = healDamageModificationDataSource;
        }

        public DamageInstance GetDamageInstance(DamageInstance value)
        {
            DamageInstanceData data = new(value.Target, value.Damage, value.Flags, value.Attacker, value.Source.Skill, value.Source.Unit);
            DamageModifiaction modifiaction = _damageModificationDataSource.GetDamageInstanceModification(data);

            float finalDamage = (value.OriginalDamage + modifiaction.BonusDamage) * (1 + modifiaction.BonusDamagePercent * 0.01f);
            DamageFlags finalFlags = value.Flags | modifiaction.BonusFlags;

            return new(data.Target, value.OriginalDamage, finalDamage, finalFlags, value.Attacker, value.Source);
        }

        public HealingInstance GetHealingInstance(HealingInstance value)
        {
            HealingInstanceData data = new(value.Target, value.OriginalHealing, value.Flags, value.Healer, value.Source.Skill, value.Source.Unit);
            HealingModification modification = _healingModificationDataSource.GetHealingInstanceModification(data);

            float finalHealing = (value.OriginalHealing + modification.BonusHealing) * (1 + modification.BonusHealingPercent * 0.01f);
            HealingFlags finalFlags = data.Flags | modification.BonusFlags;

            return new(value.Target, value.OriginalHealing, finalHealing, finalFlags, value.Healer, value.Source);
        }
    }
}

using Combat.Common.Flags;
using Combat.Local.Data.Models;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Gateways.DataSources;

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

            float finalDamage = (value.Damage + modifiaction.BonusDamage) * (1 + modifiaction.BonusDamagePercent * 0.01f);
            DamageFlags finalFlags = value.Flags | modifiaction.BonusFlags;

            return new(data.Target, value.Damage, finalDamage, finalFlags, data.Attacker, new(data.Caster, data.Skill));
        }

        public HealingInstance GetHealingInstance(HealingInstance value)
        {
            HealingInstanceData data = new(value.Target, value.OriginalHealing, value.Flags, value.Healer, value.Source.Skill, value.Source.Unit);
            data = _healingModificationDataSource.GetHealingInstanceModification(data);
            return new(data.Target, value.OriginalHealing, data.Healing, data.Flags, data.Healer, new(data.Caster, data.Skill));
        }
    }
}

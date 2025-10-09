using Combat.API;
using Combat.API.Controllers;
using Combat.API.Statuses;
using Combat.API.ValueObjects;
using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Lookup;
using Combat.Local.Data.Models;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Data.Databases
{
    public class DamageModificationDataSource : IDamageModificationDataSource
    {
        private readonly UnitApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly StatusLookup _statusLookupService;

        public DamageModificationDataSource(UnitApiProvider unitApiProvider, SkillApiProvider skillApiProvider, StatusLookup statusLookupService)
        {
            _unitApiProvider = unitApiProvider;
            _skillApiProvider = skillApiProvider;
            _statusLookupService = statusLookupService;
        }

        public DamageModifiaction GetDamageInstanceModification(DamageInstanceData data)
        {
            DamageInstanceApi instance = CreateDamageInstanceApi(data);
            DamageModifiaction attackerBonus = new();

            if (data.Attacker.HasValue)
            {
                attackerBonus = HandleAttackerDamageModification(data.Attacker.Value, instance);
            }

            DamageModifiaction targetBonus = HandleDefenderDamageModification(data.Target, instance);

            return new(attackerBonus.BonusDamage + targetBonus.BonusDamage, attackerBonus.BonusDamagePercent + targetBonus.BonusDamagePercent, attackerBonus.BonusFlags | targetBonus.BonusFlags);
        }

        private DamageInstanceApi CreateDamageInstanceApi(DamageInstanceData data)
        {
            Unit target = _unitApiProvider.Get(data.Target);
            SkillApi skill = data.Skill.HasValue ? _skillApiProvider.Get(data.Skill.Value, data.Caster) : null;
            Unit attacker = data.Attacker.HasValue ? _unitApiProvider.Get(data.Attacker.Value) : null;

            return new(target, attacker, skill, data.Damage, data.Flags);
        }

        private DamageModifiaction HandleAttackerDamageModification(EntityId target, DamageInstanceApi instance)
        {
            DamageModifiaction result = new(0, 0, DamageFlags.None);
            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(target);

            foreach (StatusApi effect in effects)
            {
                if (effect.TryGetProperty(out IOutgoingHealDamageModifier outgoingHealDamageModifier) == false)
                {
                    continue;
                }

                result.BonusDamage += outgoingHealDamageModifier.GetBonusDamageDealth(instance);
                result.BonusDamagePercent += outgoingHealDamageModifier.GetBonusDamageDealthPercent(instance);
                result.BonusFlags |= outgoingHealDamageModifier.GetDamageFlagMask(instance);
            }

            return result;
        }

        private DamageModifiaction HandleDefenderDamageModification(EntityId target, DamageInstanceApi instance)
        {
            DamageModifiaction result = new(0, 0, DamageFlags.None);
            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(target);

            foreach (StatusApi effect in effects)
            {
                if (effect.TryGetProperty(out IIncomingHealDamageModifier incomingHalDamageModifier) == false)
                {
                    continue;
                }

                result.BonusDamage += incomingHalDamageModifier.GetBonusDamageRecived(instance);
                result.BonusDamagePercent += incomingHalDamageModifier.GetBonusDamageRecivedPercent(instance);
                result.BonusFlags |= incomingHalDamageModifier.GetDamageFlagMask(instance);
            }

            return result;
        }
    }
}

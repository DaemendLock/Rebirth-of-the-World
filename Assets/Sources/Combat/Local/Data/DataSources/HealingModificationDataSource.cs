using Combat.API;
using Combat.API.Controllers;
using Combat.API.Statuses;
using Combat.API.ValueObjects;

using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using Combat.Local.Data.Lookup;

using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

namespace Combat.Local.Data.Databases
{

    public class HealingModificationDataSource : IHealingModificationDataSource
    {
        private readonly CharacterApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly StatusLookup _statusLookupService;

        public HealingModificationDataSource(CharacterApiProvider unitApiProvider, SkillApiProvider skillApiProvider, StatusLookup statusLookupService)
        {
            _unitApiProvider = unitApiProvider;
            _skillApiProvider = skillApiProvider;
            _statusLookupService = statusLookupService;
        }

        public HealingModification GetHealingInstanceModification(HealingInstanceData data)
        {
            HealingModification result = new(0, 0, HealingFlags.None);
            HealingInstanceApi instance = CreateHealingInstance(data);

            if (data.Healer.HasValue)
            {
                HealingModification healerBonus = HandleHealerHealingModification(data.Healer.Value, instance);
                result.BonusHealing += healerBonus.BonusHealing;
                result.BonusHealingPercent += healerBonus.BonusHealingPercent;
                result.BonusFlags |= healerBonus.BonusFlags;
            }

            HealingModification healeeBonus = HandleHealeeHealingModification(data.Target, instance);
            result.BonusHealing += healeeBonus.BonusHealing;
            result.BonusHealingPercent += healeeBonus.BonusHealingPercent;
            result.BonusFlags |= healeeBonus.BonusFlags;

            return result;
        }

        private HealingInstanceApi CreateHealingInstance(HealingInstanceData data)
        {
            Unit target = _unitApiProvider.Get(data.Target);
            SkillApi skill = data.Skill.HasValue ? _skillApiProvider.Get(data.Skill.Value, data.Caster) : null;
            Unit healer = data.Healer.HasValue ? _unitApiProvider.Get(data.Healer.Value) : null;

            return new(target, healer, skill, data.Healing, data.Flags);
        }

        private HealingModification HandleHealerHealingModification(EntityId target, HealingInstanceApi instance)
        {
            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(target);
            HealingInstanceApi originalInstance = instance;
            HealingModification result = new(0, 0, HealingFlags.None);

            foreach (StatusApi effect in effects)
            {
                if (effect.TryGetProperty(out IOutgoingHealDamageModifier modifier) == false)
                {
                    continue;
                }

                result.BonusHealing += modifier.GetBonusHealingDealth(originalInstance);
                result.BonusHealingPercent += modifier.GetBonusHealingDealthPercent(originalInstance);
                result.BonusFlags |= modifier.GetHealingFlagMask(originalInstance);
            }

            return result;
        }

        private HealingModification HandleHealeeHealingModification(EntityId target, HealingInstanceApi instance)
        {
            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(target);
            HealingInstanceApi originalInstance = instance;
            HealingModification result = new(0, 0, HealingFlags.None);

            foreach (StatusApi effect in effects)
            {
                if (effect.TryGetProperty(out IIncomingHealDamageModifier modifier) == false)
                {
                    continue;
                }

                result.BonusHealing += modifier.GetBonusHealingRecived(originalInstance);
                result.BonusHealingPercent += modifier.GetBonusHealingRecivedPercent(originalInstance);
                result.BonusFlags |= modifier.GetHealingFlagMask(originalInstance);
            }

            return result;
        }
    }
}

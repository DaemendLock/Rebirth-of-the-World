using Combat.API;
using Combat.API.Controllers;
using Combat.API.Statuses;
using Combat.API.ValueObjects;
using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Lookup;
using Combat.Local.Data.Models;
using Combat.Local.Gateways.DataSources;

using System;
using System.Collections.Generic;

namespace Combat.Local.Data.Databases
{

    public class HealingModificationDataSource : IHealingModificationDataSource
    {
        private readonly UnitApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly StatusLookup _statusLookupService;

        public HealingModificationDataSource(UnitApiProvider unitApiProvider, SkillApiProvider skillApiProvider, StatusLookup statusLookupService)
        {
            _unitApiProvider = unitApiProvider;
            _skillApiProvider = skillApiProvider;
            _statusLookupService = statusLookupService;
        }

        public HealingInstanceData GetHealingInstanceModification(HealingInstanceData data)
        {
            HealingInstanceApi instance = CreateHealingInstance(data);

            if (data.Healer.HasValue)
            {
                instance = HandleHealerHealingModification(data.Healer.Value, instance);
            }

            instance = HandleHealeeHealingModification(data.Target, instance);

            HealingFlags flags = instance.Flags;
            float finalHealing = instance.GetCurrentHealing();

            return new(instance.Target.Id, finalHealing, flags, instance.Healer?.Id, data.Skill, data.Caster);
        }

        private HealingInstanceApi CreateHealingInstance(HealingInstanceData data)
        {
            Unit target = _unitApiProvider.Get(data.Target);
            SkillApi skill = data.Skill.HasValue ? _skillApiProvider.Get(data.Skill.Value, data.Caster) : null;
            Unit healer = data.Healer.HasValue ? _unitApiProvider.Get(data.Healer.Value) : null;

            return new(target, healer, skill, data.Healing, data.Flags);
        }

        private HealingInstanceApi HandleHealerHealingModification(EntityId target, HealingInstanceApi instance)
        {
            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(target);
            HealingInstanceApi originalInstance = instance;

            foreach (StatusApi effect in effects)
            {
                if (effect.TryGetProperty(out IOutgoingHealDamageModifier modifier) == false)
                {
                    continue;
                }

                instance.BaseHealing += modifier.GetBonusHealingDealth(originalInstance);
                instance.HealingPercent += modifier.GetBonusHealingDealthPercent(originalInstance);
                instance.Flags |= modifier.GetHealingFlagMask(originalInstance);
            }

            return instance;
        }

        private HealingInstanceApi HandleHealeeHealingModification(EntityId target, HealingInstanceApi instance)
        {
            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(target);
            HealingInstanceApi originalInstance = instance;

            foreach (StatusApi effect in effects)
            {
                if (effect.TryGetProperty(out IIncomingHealDamageModifier modifier) == false)
                {
                    continue;
                }

                instance.BaseHealing += modifier.GetBonusHealingRecived(originalInstance);
                instance.HealingPercent += modifier.GetBonusHealingRecivedPercent(originalInstance);
                instance.Flags |= modifier.GetHealingFlagMask(originalInstance);
            }

            return instance;
        }
    }
}

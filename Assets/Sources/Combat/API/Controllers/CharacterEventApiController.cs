using Combat.API.DTO;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Local.Events;

using System.Collections.Generic;

namespace Combat.API.Controllers
{
    public interface IStatusLookupService
    {
        ICollection<StatusApi> FindStatusesOnUnit(EntityId target);
    }

    public class CharacterEventApiController
    {
        private readonly IStatusLookupService _statusLookup;

        private readonly UnitApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;

        public CharacterEventApiController(IStatusLookupService statusLookup, UnitApiProvider unitApiProvider, SkillApiProvider skillApiProvider)
        {
            _statusLookup = statusLookup;
            _unitApiProvider = unitApiProvider;
            _skillApiProvider = skillApiProvider;
        }

        public void HandleDamageRecived(DamageInfo info)
        {
            DamageRecord @event = CreateDamageRecord(info);

            IEnumerable<StatusApi> targetEffects = _statusLookup.FindStatusesOnUnit(info.Target);

            foreach (StatusApi effect in targetEffects)
            {
                if (effect.TryGetProperty(out IIncomingHealDamageHandler defenderHandler) == false)
                {
                    continue;
                }

                defenderHandler.OnTakeDamage(@event);
            }
        }

        public void HandleDamageDealth(DamageInfo info)
        {
            if (!info.Attacker.HasValue)
            {
                return;
            }

            DamageRecord @event = CreateDamageRecord(info);

            IEnumerable<StatusApi> attackerEffects = _statusLookup.FindStatusesOnUnit(info.Attacker.Value);

            foreach (StatusApi effect in attackerEffects)
            {
                if (effect.TryGetProperty(out IOutgoingHealDamageHandler attackerHandler) == false)
                {
                    continue;
                }

                attackerHandler.OnDealDamage(@event);
            }
        }

        public void HandleHealingDealth(HealingInfo info)
        {
            if (info.Healer.HasValue == false)
            {
                return;
            }

            HealingRecord @event = CreateHealingRecord(info);

            IEnumerable<StatusApi> effects = _statusLookup.FindStatusesOnUnit(info.Healer.Value);

            foreach (StatusApi effect in effects)
            {
                if (effect.TryGetProperty(out IOutgoingHealDamageHandler modifier) == false)
                {
                    continue;
                }

                modifier.OnDealHealing(@event);
            }
        }

        public void HandleHealingRecived(HealingInfo info)
        {
            HealingRecord @event = CreateHealingRecord(info);

            IEnumerable<StatusApi> efects = _statusLookup.FindStatusesOnUnit(info.Target);

            foreach (StatusApi effect in efects)
            {
                if (effect.TryGetProperty(out IIncomingHealDamageHandler modifier) == false)
                {
                    continue;
                }

                modifier.OnTakeHealing(@event);
            }
        }

        public void HandleResourceGained(EntityId target, ResourceChangeRecord @event)
        {
            IEnumerable<StatusApi> effects = _statusLookup.FindStatusesOnUnit(target);

            foreach (StatusApi status in effects)
            {
                if (status.TryGetProperty(out IResourceGainSpendHandler handler) == false)
                {
                    continue;
                }

                handler.OnGainResource(@event);
            }
        }

        public void HandleResourceSpent(EntityId target, ResourceChangeRecord record)
        {
            IEnumerable<StatusApi> effects = _statusLookup.FindStatusesOnUnit(target);

            foreach (StatusApi status in effects)
            {
                if (status.TryGetProperty(out IResourceGainSpendHandler handler) == false)
                {
                    continue;
                }

                handler.OnSpendResource(record);
            }
        }

        private DamageRecord CreateDamageRecord(DamageInfo instance)
        {
            Unit target = _unitApiProvider.Get(instance.Target);

            Unit attacker;
            SkillApi scriptedSkill;

            if (instance.Skill.HasValue)
            {
                scriptedSkill = _skillApiProvider.Get(instance.Skill.Value, instance.Caster);
            }
            else
            {
                scriptedSkill = null;
            }

            if (instance.Attacker.HasValue)
            {
                attacker = _unitApiProvider.Get(instance.Attacker.Value);
            }
            else
            {
                attacker = null;
            }

            return new(target, instance.OriginalDamage, instance.FinalDamage, instance.Flags, attacker, scriptedSkill);
        }

        private HealingRecord CreateHealingRecord(HealingInfo info)
        {
            Unit targetApi = _unitApiProvider.Get(info.Target);

            Unit healerApi;
            SkillApi sourceSkill;

            if (info.Healer.HasValue)
            {
                healerApi = _unitApiProvider.Get(info.Healer.Value);
            }
            else
            {
                healerApi = null;
            }

            if (info.Skill.HasValue)
            {
                sourceSkill = _skillApiProvider.Get(info.Skill.Value, info.Caster);
            }
            else
            {
                sourceSkill = null;
            }

            return new(targetApi, healerApi, sourceSkill, info.OriginalHealing, info.FinalHealing, info.Flags);
        }
    }
}

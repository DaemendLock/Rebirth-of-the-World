using Combat.API.DTO;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.API.Controllers
{
    public interface IStatusLookupService
    {
        ICollection<StatusApi> FindStatusesOnUnit(EntityId target);
    }

    public class UnitEventApiController
    {
        private readonly IStatusLookupService _statusLookup;

        public UnitEventApiController(IStatusLookupService statusLookup)
        {
            _statusLookup = statusLookup;
        }

        public void HandleDamageRecived(EntityId target, DamageRecord @event)
        {
            IEnumerable<StatusApi> targetEffects = _statusLookup.FindStatusesOnUnit(target);

            foreach (StatusApi effect in targetEffects)
            {
                if (effect.TryGetProperty(out IIncomingHealDamageHandler defenderHandler) == false)
                {
                    continue;
                }

                defenderHandler.OnTakeDamage(@event);
            }
        }

        public void HandleDamageDealth(EntityId target, DamageRecord @event)
        {
            IEnumerable<StatusApi> attackerEffects = _statusLookup.FindStatusesOnUnit(target);

            foreach (StatusApi effect in attackerEffects)
            {
                if (effect.TryGetProperty(out IOutgoingHealDamageHandler attackerHandler) == false)
                {
                    continue;
                }

                attackerHandler.OnDealDamage(@event);
            }
        }

        public void HandleHealingDealth(EntityId target, HealingRecord @event)
        {
            IEnumerable<StatusApi> effects = _statusLookup.FindStatusesOnUnit(target);

            foreach (StatusApi effect in effects)
            {
                if (effect.TryGetProperty(out IOutgoingHealDamageHandler modifier) == false)
                {
                    continue;
                }

                modifier.OnDealHealing(@event);
            }
        }

        public void HandleHealingRecived(EntityId target, HealingRecord @event)
        {
            IEnumerable<StatusApi> efects = _statusLookup.FindStatusesOnUnit(target);

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
    }
}

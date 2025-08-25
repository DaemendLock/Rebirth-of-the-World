using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.API.Statuses
{
    public interface IOutgoingHealDamageHandler : IStatusPropery
    {
        void OnDealDamage(DamageRecord @event) { }
        void OnDealHealing(HealingRecord @event) { }
    }
}

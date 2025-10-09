using Combat.API.DTO;

namespace Combat.API.Statuses
{
    public interface IOutgoingHealDamageHandler : IStatusPropery
    {
        void OnDealDamage(DamageRecord @event) { }
        void OnDealHealing(HealingRecord @event) { }
    }
}

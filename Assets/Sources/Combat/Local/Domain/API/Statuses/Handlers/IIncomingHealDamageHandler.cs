using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.API.Statuses
{
    public interface IIncomingHealDamageHandler : IStatusPropery
    {
        void OnTakeDamage(DamageRecord @event);
        void OnTakeHealing(HealingRecord @event);
    }
}

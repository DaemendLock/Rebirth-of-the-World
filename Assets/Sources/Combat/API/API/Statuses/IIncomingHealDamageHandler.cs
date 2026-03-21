using Combat.API.DTO;

namespace Combat.API.Statuses
{
    public interface IIncomingHealDamageHandler : IStatusPropery
    {
        void OnTakeDamage(DamageRecord record); 
    }
}

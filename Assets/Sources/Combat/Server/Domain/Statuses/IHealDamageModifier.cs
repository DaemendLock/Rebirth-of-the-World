using Server.Combat.Domain.Events;

namespace Server.Combat.Domain.Statuses.StatusEffects
{
    //TODO:
    /*
     * GetDamageEventModification(DamageEvent?); 
     * GetStatsBonus(StatsTable defaultStats);
     * 
     */
    public interface IHealDamageModifier
    {
        void OnDamageRecived(ref HealthChangeEvent @event);
        void OnHealRecived(ref HealthChangeEvent @event);
    }
}

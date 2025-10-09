using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Repositories
{
    public interface IHealingDamageInstanceRepository
    {
        HealingInstance GetHealingInstance(HealingInstance value);

        DamageInstance GetDamageInstance(DamageInstance value);
    }
}

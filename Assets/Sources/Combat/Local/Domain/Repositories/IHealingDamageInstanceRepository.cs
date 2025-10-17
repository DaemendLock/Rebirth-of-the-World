using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Repositories
{
    public interface IHealingDamageInstanceRepository
    {
        HealingInstance GetHealingInstance(HealingInstance value);

        DamageInstance GetDamageInstance(DamageInstance value);
    }
}

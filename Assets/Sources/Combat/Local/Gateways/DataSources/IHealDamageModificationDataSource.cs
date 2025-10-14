using Combat.Local.Gateways.Models;

namespace Combat.Local.Gateways.DataSources
{
    public interface IDamageModificationDataSource
    {
        DamageModifiaction GetDamageInstanceModification(DamageInstanceData data);
    }

    public interface IHealingModificationDataSource
    {
        HealingModification GetHealingInstanceModification(HealingInstanceData data);
    }
}

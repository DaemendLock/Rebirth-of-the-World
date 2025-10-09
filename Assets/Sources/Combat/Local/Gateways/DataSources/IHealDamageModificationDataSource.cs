using Combat.Local.Data.Models;

namespace Combat.Local.Gateways.DataSources
{
    public interface IDamageModificationDataSource
    {
        DamageModifiaction GetDamageInstanceModification(DamageInstanceData data);
    }

    public interface IHealingModificationDataSource
    {
        HealingInstanceData GetHealingInstanceModification(HealingInstanceData original);
    }
}

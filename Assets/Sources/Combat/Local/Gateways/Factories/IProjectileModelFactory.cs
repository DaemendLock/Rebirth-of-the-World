using Combat.Common.ValueObjects;
using Combat.Local.Gateways.Models;

namespace Combat.Local.Gateways.Factories
{
    public interface IProjectileModelFactory
    {
        ProjectileModel Create(ModelName modelName);
    }
}

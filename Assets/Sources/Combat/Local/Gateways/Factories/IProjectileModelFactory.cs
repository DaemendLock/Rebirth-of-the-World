using Combat.Common.Primitives;
using Combat.Local.Gateways.Models;

namespace Combat.Local.Gateways.Factories
{
    public interface IProjectileModelFactory
    {
        ProjectileModel Create(ModelName modelName);
    }
}

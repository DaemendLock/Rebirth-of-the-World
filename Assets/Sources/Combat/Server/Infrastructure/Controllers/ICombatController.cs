using Server.Combat.Infrastructure.Factories;

using Zenject;

namespace Server.Combat.Infrastructure.Controllers
{
    public interface ICombatController : ITickable
    {
        void CreateUnit(IUnitModelFactory.UnitModelCreationData data);
    }
}
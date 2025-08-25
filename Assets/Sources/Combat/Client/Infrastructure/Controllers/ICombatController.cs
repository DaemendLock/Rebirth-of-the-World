using Zenject;

namespace Client.Combat.Infrastructure.Controllers
{
    public interface ICombatController : ITickable
    {
        void Add(IUnitController unitController);
    }
}

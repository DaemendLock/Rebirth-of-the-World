using Client.Combat.Infrastructure.Controllers;

namespace Client.Combat.Infrastructure.Factories
{
    public interface IUnitControllerFactory
    {
        IUnitController Create();
    }
}

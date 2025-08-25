using Server.Combat.Domain.Actions;
using Server.Combat.Domain.Entities;

namespace Server.Combat.Infrastructure.Repositories
{
    public interface IActionHandlerRepository : IUpdatable
    {
        void Add(IActionHandler actionHandler);
        IActionHandler Get(Unit actor);
    }
}

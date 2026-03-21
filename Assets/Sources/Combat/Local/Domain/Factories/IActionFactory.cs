using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Factories
{

    public interface IActionFactory
    {
        IAction CreateCastAction(ActionId actionId, EntityId actorId);
    }
}

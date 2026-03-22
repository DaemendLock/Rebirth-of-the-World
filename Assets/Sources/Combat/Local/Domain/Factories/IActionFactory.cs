using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface IActionFactory
    {
        Action CreateCastAction(ActionId actionId, EntityId actorId);
    }
}

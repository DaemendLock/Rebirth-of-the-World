using Combat.Common.ValueObjects;

namespace Combat.API.Controllers.Factories
{
    public interface IUnitApiFactory
    {
        Unit Create(EntityId id);
    }
}

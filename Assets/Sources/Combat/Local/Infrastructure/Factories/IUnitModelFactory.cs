using Combat.Common.ValueObjects;

namespace Combat.Local.Factories
{
    public interface IUnitModelFactory
    {
        EntityId Create(IUnitControllerFactory.UnitModelCreationData context);
    }
}

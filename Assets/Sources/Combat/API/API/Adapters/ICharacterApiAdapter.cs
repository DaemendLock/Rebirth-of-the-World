using Combat.Common.ValueObjects;

namespace Combat.API.Adapters
{
    public interface ICharacterApiAdapter
    {
        IUnit Adaptee(UnitId id);
    }
}
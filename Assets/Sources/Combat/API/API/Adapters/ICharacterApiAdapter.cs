using Combat.Common.ValueObjects;

namespace Combat.API.Adapters
{
    public interface ICharacterApiAdapter
    {
        Unit Adaptee(UnitId id);
    }
}
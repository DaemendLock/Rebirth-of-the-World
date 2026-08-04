using Combat.Common.ValueObjects;

namespace Combat.API.Adapters
{
    public interface IStatusApiAdapter
    {
        StatusApi Adaptee(StatusId id, UnitId parent, AbilityKey? abilityKey);
    }
}
using Combat.Common.ValueObjects;

namespace Combat.API.Adapters
{
    public interface IStatusApiAdapter
    {
        IStatusApi Adaptee(StatusId id, UnitId parent, AbilityKey? abilityKey);
    }
}
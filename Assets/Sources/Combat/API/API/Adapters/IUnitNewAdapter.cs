using Combat.Common.ValueObjects;

namespace Combat.API.Adapters
{
    public interface IUnitNewAdapter
    {
        ITargetable Adaptee(UnitId id);
    }
}
using Combat.Common.ValueObjects;

namespace Combat.API.Contexts
{
    public interface IEnvironmentContext
    {
        bool TryGetTarget(UnitId id, out ITargetable targetable);
    }
}

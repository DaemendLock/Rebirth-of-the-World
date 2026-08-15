using Combat.Common.Primitives;

namespace Combat.API.Contexts
{
    public interface IEnvironmentContext
    {
        bool TryGetTarget(UnitId id, out ITargetable targetable);
    }
}

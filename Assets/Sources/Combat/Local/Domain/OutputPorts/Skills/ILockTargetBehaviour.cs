using Combat.Common.Primitives;

namespace Combat.Local.Domain.Endpoints.Skills
{
    public interface ILockTargetBehaviour
    {
        bool Handle(UnitId entityId);
    }
}

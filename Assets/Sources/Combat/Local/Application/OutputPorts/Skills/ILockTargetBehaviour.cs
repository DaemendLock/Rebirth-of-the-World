using Combat.Common.Primitives;

namespace Combat.Local.Application.Endpoints.Skills
{
    public interface ILockTargetBehaviour
    {
        bool Handle(UnitId entityId);
    }
}

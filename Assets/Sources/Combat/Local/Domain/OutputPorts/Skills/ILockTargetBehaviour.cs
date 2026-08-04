using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Endpoints.Skills
{
    public interface ILockTargetBehaviour
    {
        bool Handle(UnitId entityId);
    }
}

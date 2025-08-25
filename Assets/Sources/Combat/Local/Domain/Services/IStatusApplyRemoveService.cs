using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Services
{

    public interface IStatusService
    {
        void ApplyStatus(StatusName statusName, EntityId parent, EntityId caster, SkillId source, float duration, int stackCount);
        void RemoveStatus(StatusId effect);
    }
}

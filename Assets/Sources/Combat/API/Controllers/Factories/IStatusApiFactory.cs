using Combat.Common.ValueObjects;

namespace Combat.API.Controllers.Factories
{
    public interface IStatusApiFactory
    {
        StatusApi Create(StatusId id, EntityId parent, StatusName statusName, SkillId? source, EntityId? caster);
    }
}

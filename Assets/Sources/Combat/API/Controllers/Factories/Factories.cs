using Combat.Common.ValueObjects;

namespace Combat.API.Controllers.Factories
{
    public interface ISkillApiFactory
    {
        SkillApi Create(SkillId id, EntityId? owner);
    }

    public interface IUnitApiFactory
    {
        Unit Create(EntityId id);
    }

    public interface IStatusApiFactory
    {
        StatusApi Create(StatusId id, EntityId parent, StatusName statusName, SkillId? source, EntityId? caster);
    }
}

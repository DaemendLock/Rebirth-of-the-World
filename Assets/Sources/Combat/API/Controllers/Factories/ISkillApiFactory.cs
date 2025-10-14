using Combat.Common.ValueObjects;

namespace Combat.API.Controllers.Factories
{
    public interface ISkillApiFactory
    {
        SkillApi Create(SkillId id, EntityId? owner);
    }
}

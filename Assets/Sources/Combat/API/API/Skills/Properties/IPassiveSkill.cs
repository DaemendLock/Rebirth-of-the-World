using Combat.Common.ValueObjects;

namespace Combat.API.Skills
{
    public interface IPassiveSkill : ISkillProperty
    {
        StatusName PassiveStatusName { get; }
    }
}

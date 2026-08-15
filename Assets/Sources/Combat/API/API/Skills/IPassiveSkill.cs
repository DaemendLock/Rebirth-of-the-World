using Combat.Common.Primitives;

namespace Combat.API.Skills
{
    public interface IPassiveSkill : ISkillProperty
    {
        StatusType PassiveStatusName { get; }
    }
}

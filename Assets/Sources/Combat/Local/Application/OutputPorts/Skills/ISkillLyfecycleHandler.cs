using Combat.Common.Primitives;

namespace Combat.Local.Application.Endpoints.Skills
{
    public interface ISkillLyfecycleHandler
    {
        void Give(AbilityKey abilityKey);
        void Remove(AbilityKey abilityKey);
    }
}

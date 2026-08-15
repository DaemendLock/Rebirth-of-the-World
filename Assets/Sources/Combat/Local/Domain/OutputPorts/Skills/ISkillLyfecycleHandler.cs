using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Endpoints.Skills
{
    public interface ISkillLyfecycleHandler
    {
        void Give(AbilityKey abilityKey);
        void Remove(AbilityKey abilityKey);
    }
}

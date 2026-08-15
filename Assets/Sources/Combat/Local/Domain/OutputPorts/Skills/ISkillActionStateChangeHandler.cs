using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Endpoints.Skills
{
    public interface ISkillActionStateChangeHandler
    {
        void Handle(AbilityKey abilityKey, ActionState newState);
    }
}

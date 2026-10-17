using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.Local.Application.Endpoints.Skills
{
    public interface ISkillActionStateChangeHandler
    {
        void Handle(AbilityKey abilityKey, ActionState newState);
    }
}

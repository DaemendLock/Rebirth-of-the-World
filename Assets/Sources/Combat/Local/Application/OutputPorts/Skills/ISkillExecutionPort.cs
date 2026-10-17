using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.Local.Application.Endpoints.Skills
{
    public interface ISkillExecutionPort
    {
        CastFailReason CanCast(AbilityKey abilityKey);
        bool BeginCast(AbilityKey abilityKey);
    }
}

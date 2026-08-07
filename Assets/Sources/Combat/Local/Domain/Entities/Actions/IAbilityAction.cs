using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public interface IAbilityAction
    {
        SkillId Source { get; }
        ActionState State { get; }
    }
}

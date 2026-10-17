using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.Local.Application.Entities
{
    public interface IAbilityAction
    {
        SkillId Source { get; }
        ActionState State { get; }
    }
}

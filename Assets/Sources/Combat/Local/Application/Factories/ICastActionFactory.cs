using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.ValueObjects;

namespace Combat.Local.Application.Factories
{
    public interface ICastActionFactory
    {
        CastActivity Create(ActionId actionId, SkillId skillId, SkillFlags skillFlags, ActionFlags flags);
    }
}
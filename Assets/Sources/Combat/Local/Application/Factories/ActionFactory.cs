using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.Entities;

namespace Combat.Local.Application.Factories
{
    public interface IAbilityActionStrategyFactory
    {
        CastActivity Create(ActionId id, SkillId source, bool holdable);
    }

    public class ActionFactory
    {
        private readonly IAbilityActionStrategyFactory _actionStrategyFactory;

        public ActionFactory(IAbilityActionStrategyFactory actionStrategyFactory)
        {
            _actionStrategyFactory = actionStrategyFactory;
        }

        public CastActivity CreateCastAction(ActionId actionId, SkillId skillId, SkillFlags skillFlags, out ActionFlags flags)
        {
            flags = ActionFlags.None;

            if (skillFlags.HasFlag(SkillFlags.DontRestrictMovement))
            {
                flags |= ActionFlags.AllowMovement;
            }

            if (skillFlags.HasFlag(SkillFlags.CanHold))
            {
                flags |= ActionFlags.Holdable;
            }

            var result = _actionStrategyFactory.Create(actionId, skillId, flags.HasFlag(ActionFlags.Holdable));

            return result;
        }
    }
}

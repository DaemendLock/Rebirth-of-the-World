using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface IAbilityActionStrategyFactory
    {
        IActionStrategy Create(ActionId id, SkillId source, bool holdable);
    }

    public class ActionFactory
    {
        private readonly IAbilityActionStrategyFactory _actionStrategyFactory;

        public ActionFactory(IAbilityActionStrategyFactory actionStrategyFactory)
        {
            _actionStrategyFactory = actionStrategyFactory;
        }

        public Action CreateCastAction(ActionId actionId, SkillId skillId, SkillFlags skillFlags)
        {
            ActionFlags flags = ActionFlags.None;

            if (skillFlags.HasFlag(SkillFlags.DontRestrictMovement))
            {
                flags |= ActionFlags.AllowMovement;
            }

            if (skillFlags.HasFlag(SkillFlags.CanHold))
            {
                flags |= ActionFlags.Holdable;
            }

            IActionStrategy strategy = _actionStrategyFactory.Create(actionId, skillId, flags.HasFlag(ActionFlags.Holdable));

            return new Action(actionId, flags, strategy);
        }
    }
}

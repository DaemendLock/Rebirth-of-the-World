using Combat.Common.Flags;
using Combat.Common.ValueObjects;
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

        public Action CreateCastAction(ActionId actionId, Ability ability)
        {
            ActionFlags flags = ActionFlags.None;

            if (ability.AllowMoment)
            {
                flags |= ActionFlags.AllowMovement;
            }

            if (ability.Flags.HasFlag(SkillFlags.CanHold))
            {
                flags |= ActionFlags.Holdable;
            }

            IActionStrategy strategy = _actionStrategyFactory.Create(actionId, ability.SkillId, flags.HasFlag(ActionFlags.Holdable));

            return new Action(actionId, flags, strategy);
        }
    }
}

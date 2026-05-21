using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface IActionStrategyFactory
    {
        IActionStrategy Create(ActionId id);
    }

    public class ActionFactory
    {
        private readonly IActionStrategyFactory _actionStrategyFactory;

        public ActionFactory(IActionStrategyFactory actionStrategyFactory)
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

            IActionStrategy strategy = _actionStrategyFactory.Create(actionId);

            return new Action(actionId, ability.SkillId, flags, strategy);
        }
    }
}

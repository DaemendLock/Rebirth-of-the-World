using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.Factories
{
    public interface IActionStrategyFactory
    {
        IActionStrategy Create(ActionId id);
    }

    public class ActionFactory
    {
        private readonly ISkillRepository _skillRepository;
        private readonly IActionStrategyFactory _actionStrategyFactory;

        public ActionFactory(ISkillRepository skillRepository, IActionStrategyFactory actionStrategyFactory)
        {
            _skillRepository = skillRepository;
            _actionStrategyFactory = actionStrategyFactory;
        }

        public Action CreateCastAction(ActionId actionId, EntityId actorId)
        {
            Skill skill = _skillRepository.Get(new(actionId.Value), actorId);
            ActionFlags flags = ActionFlags.None;

            if (skill.AllowMoment)
            {
                flags |= ActionFlags.AllowMovement;
            }

            if (skill.Flags.HasFlag(SkillFlags.CanHold))
            {
                flags |= ActionFlags.Holdable;
            }

            IActionStrategy strategy = _actionStrategyFactory.Create(actionId);

            return new Action(actionId, skill.Id, flags, strategy);
        }
    }
}

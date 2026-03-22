using CastStateSkill;

using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

namespace Testing.Local.Temp.Factories
{
    public class ActionFactory : IActionFactory
    {
        private readonly ISkillRepository _skillRepository;
        private readonly ISkillDataBase _skillDataBase;

        public ActionFactory(ISkillRepository skillRepository, ISkillDataBase skillDataBase)
        {
            _skillRepository = skillRepository;
            _skillDataBase = skillDataBase;
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

            IActionStrategy strategy = null;

            if (_skillDataBase.TryGetActionData(actionId, out var actionData))
            {
                strategy = new CastActionStrategy(actionData.FrameData);
            }

            return new Action(actionId, skill.Id, flags, strategy);
        }
    }
}

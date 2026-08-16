using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public sealed class ActorReleaseSkillFromSlotUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IActorRepository _actorRepository;

        public ActorReleaseSkillFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _actorRepository = actorRepository;
        }

        public void Execute(UnitId caster, int slot)
        {
            if (_skillOwnerRepository.TryGet(caster, out SkillOwner skillOwner) == false)
            {
                return;
            }

            SkillId? skillId = skillOwner.GetSkill(slot);

            if (skillId.HasValue == false)
            {
                return;
            }

            if (_actorRepository.TryGet(caster, out Actor actor) == false)
            {
                return;
            }

            Action action = actor.CurrentAction;

            if (action == null || action.TryGet(out IAbilityAction abilityAction) == false ||
                abilityAction.Source != skillId.Value)
            {
                return;
            }

            if (action != null && action.TryGet(out IReleasableAction releasable))
            {
                releasable.Release();
            }
            _actorRepository.Update(actor);
        }
    }
}

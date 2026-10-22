using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.Entities;
using Combat.Local.Application.Repositories;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Application.UseCases
{
    public sealed class ActorReleaseSkillFromSlotUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IActorRepository _actorRepository;

        public ActorReleaseSkillFromSlotUseCase(IUnitStore units, IActorRepository actorRepository)
        {
            _unitStore = units;
            _actorRepository = actorRepository;
        }

        public void Execute(UnitId caster, int slot)
        {
            ref UnitContext unit = ref _unitStore.Get(caster);

            SkillId? skillId = unit.Skills.GetSkill(slot);

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

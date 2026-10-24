using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Application.UseCases.Scene
{
    public sealed class HitsHandleUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ISkillHitHandler _skillHitHandler;

        public HitsHandleUseCase(ISkillHitHandler skillHitHandler, IUnitStore unitStore)
        {
            _skillHitHandler = skillHitHandler;
            _unitStore = unitStore;
        }

        public void Execute(HitRecord record)
        {
            UnitId target = record.HitboxOwner;

            ref UnitContext unit = ref _unitStore.Get(target);

            if (unit.Actor.CurrentAction == null)
            {
                return;
            }

            if (unit.Actor.CurrentAction.TryGet(out IAbilityAction currentAction) == false ||
                currentAction.State != ActionState.Active)
            {
                return;
            }

            _skillHitHandler.HandleHit(new(unit.Actor.Id, currentAction.Source), record);
        }
    }
}

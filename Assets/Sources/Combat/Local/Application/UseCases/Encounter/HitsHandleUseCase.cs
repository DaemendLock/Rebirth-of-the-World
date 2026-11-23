using CastStateSkill;

using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.Entities;

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

            UnitContext unit = _unitStore.Get(target);

            if (unit.Actor.CurrentActivity.Type != ActivityType.Casting)
            {
                return;
            }

            ref CastActivity cast = ref unit.Actor.CastActivity;

            if (cast.FrameType != SkillCastState.Active)
            {
                return;
            }

            _skillHitHandler.HandleHit(new(unit.Id, cast.Skill), record);
        }
    }
}

using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases
{
    public sealed class ActorReleaseSkillFromSlotUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IActorActionUpdateOutput _actionUpdateOutput;

        public ActorReleaseSkillFromSlotUseCase(IUnitStore units, IActorActionUpdateOutput actionUpdateOutput)
        {
            _unitStore = units;
            _actionUpdateOutput = actionUpdateOutput;
        }

        public void Execute(UnitId caster, int slot)
        {
            UnitContext unit = _unitStore.Get(caster);

            if (unit.Actor.CurrentActivity.Type != ActivityType.Casting)
            {
                return;
            }

            ref CastActivity cast = ref unit.Actor.CastActivity;
            SkillId? skillId = unit.Skills.GetSkill(slot);

            if ((skillId.HasValue == false) || cast.Skill != skillId.Value)
            {
                return;
            }

            cast.Release();

            if (cast.FrameType == CastStateSkill.SkillCastState.Recovery)
            {
                _actionUpdateOutput.Release(caster);
            }
        }
    }
}

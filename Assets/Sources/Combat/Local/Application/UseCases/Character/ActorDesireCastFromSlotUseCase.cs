using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.Repositories;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Application.UseCases
{
    public class ActorDesireCastFromSlotUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IActorRepository _actorRepository;
        private readonly IDesireCastOutput _desireCastOutput;

        public ActorDesireCastFromSlotUseCase(IUnitStore units, IActorRepository actorRepository, IDesireCastOutput desireCastOutput)
        {
            _unitStore = units;
            _actorRepository = actorRepository;
            _desireCastOutput = desireCastOutput;
        }

        public void Execute(UnitId caster, int slot)
        {
            if (TryGetSkillId(caster, slot, out SkillId skillId) == false)
            {
                _desireCastOutput.Present(DesireCastFailReason.NoSkillFound);
                return;
            }

            if (_actorRepository.TryGet(caster, out Actor actor) == false)
            {
                return;
            }

            actor.DesireCast(skillId);
            _actorRepository.Update(actor);
        }

        private bool TryGetSkillId(UnitId owner, int slot, out SkillId result)
        {
            try
            {
                SkillOwner unit = _unitStore.Get(owner).Skills;
                SkillId? skillId = unit.GetSkill(slot);

                if (skillId.HasValue == false)
                {
                    result = default;
                    return false;
                }

                result = skillId.Value;
                return true;

            }
            catch
            {
                result = default;
                return false;
            }
        }
    }

    public interface IDesireCastOutput
    {
        void Present(DesireCastFailReason failReason);
    }
}

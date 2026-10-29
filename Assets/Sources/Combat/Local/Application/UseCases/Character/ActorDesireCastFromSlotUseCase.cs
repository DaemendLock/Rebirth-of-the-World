using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.Queries;
using Combat.Local.Application.ValueObjects;

namespace Combat.Local.Application.UseCases
{
    public class ActorDesireCastFromSlotUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IInputQueue _inputQueue;
        private readonly IDesireCastOutput _desireCastOutput;

        public ActorDesireCastFromSlotUseCase(IUnitStore units, IDesireCastOutput desireCastOutput, IInputQueue inputQueue)
        {
            _unitStore = units;
            _desireCastOutput = desireCastOutput;
            _inputQueue = inputQueue;
        }

        public void Execute(UnitId caster, int slot)
        {
            UnitContext unit = _unitStore.Get(caster);

            if (TryGetSkillId(unit, slot, out SkillId skillId) == false)
            {
                _desireCastOutput.Present(DesireCastFailReason.NoSkillFound);
                return;
            }

            _inputQueue.Enqueue(new CastSkillCommand(caster, slot));
        }

        private bool TryGetSkillId(UnitContext unit, int slot, out SkillId result)
        {
            SkillId? skillId = unit.Skills.GetSkill(slot);

            if (skillId.HasValue == false)
            {
                result = default;
                return false;
            }

            result = skillId.Value;
            return true;
        }
    }

    public interface IDesireCastOutput
    {
        void Present(DesireCastFailReason failReason);
    }
}

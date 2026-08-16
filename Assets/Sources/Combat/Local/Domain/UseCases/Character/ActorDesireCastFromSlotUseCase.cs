using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public enum DesireCastFailReason
    {
        None,
        NoSkillFound,
    }

    public class ActorDesireCastFromSlotUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IDesireCastOutput _desireCastOutput;

        public ActorDesireCastFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository, IDesireCastOutput desireCastOutput)
        {
            _skillOwnerRepository = skillOwnerRepository;
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
            if (_skillOwnerRepository.TryGet(owner, out SkillOwner skillOwner) == false)
            {
                result = default;
                return false;
            }

            SkillId? skillId = skillOwner.GetSkill(slot);

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

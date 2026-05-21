using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public enum DesireCastFailReason
    {
        None,
        NoSkillFound,
    }

    public class DesireCastFromSlotUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IAbilityRepository _abilityRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IDesireCastOutput _desireCastOutput;

        public DesireCastFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository, IPlayerRepository playerRepository, IAbilityRepository abilityRepository, IDesireCastOutput desireCastOutput)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _actorRepository = actorRepository;
            _playerRepository = playerRepository;
            _abilityRepository = abilityRepository;
            _desireCastOutput = desireCastOutput;
        }

        public void Execute(PlayerId playerId, int slot)
        {
            Player player = _playerRepository.Get(playerId);

            if (player.ControlledEntity.HasValue == false)
            {
                return;
            }

            UnitId caster = player.ControlledEntity.Value;
            //caster = player.ControlledEntity ?? throw new System.InvalidOperationException();

            if (TryGetSkillId(caster, slot, out SkillId skillId) == false)
            {
                _desireCastOutput.Present(DesireCastFailReason.NoSkillFound);
                return;
            }

            Actor actor = _actorRepository.Get(caster);

            actor.DesireCast(skillId);
            _actorRepository.Update(actor);
        }

        private bool TryGetSkillId(UnitId owner, int slot, out SkillId result)
        {
            SkillOwner skillOwner = _skillOwnerRepository.Get(owner);

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

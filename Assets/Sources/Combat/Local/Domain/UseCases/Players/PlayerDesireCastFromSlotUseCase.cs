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

    public class PlayerDesireCastFromSlotUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IDesireCastOutput _desireCastOutput;

        public PlayerDesireCastFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository, IPlayerRepository playerRepository, IDesireCastOutput desireCastOutput)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _actorRepository = actorRepository;
            _playerRepository = playerRepository;
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

    public sealed class PlayerReleaseSkillFromSlotUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IPlayerRepository _playerRepository;

        public PlayerReleaseSkillFromSlotUseCase(ISkillOwnerRepository skillOwnerRepository, IActorRepository actorRepository,
            IPlayerRepository playerRepository)
        {
            _skillOwnerRepository = skillOwnerRepository;
            _actorRepository = actorRepository;
            _playerRepository = playerRepository;
        }

        public void Execute(PlayerId playerId, int slot)
        {
            Player player = _playerRepository.Get(playerId);

            if (player.ControlledEntity.HasValue == false)
            {
                return;
            }

            UnitId caster = player.ControlledEntity.Value;

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

    public interface IDesireCastOutput
    {
        void Present(DesireCastFailReason failReason);
    }
}

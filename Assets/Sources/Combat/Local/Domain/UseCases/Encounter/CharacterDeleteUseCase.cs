using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases.Scene
{
    public sealed class CharacterDeleteUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IAligmentRepository _aligmentRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IResourceOwnerRepository _resourceRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IAbilityRepository _abilityRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;

        private readonly IPlayerRepository _playerRepository;
        public void Execute(UnitId target)
        {
            if (_playerRepository.TryFindOnwer(target, out Player player))
            {
                player.ControlledEntity = default;
                _playerRepository.Update(player);
            }

            _characterUpdateList.Delete(target);
            _healthRepository.Delete(target);
            _aligmentRepository.Delete(target);
            _attributesRepository.Delete(target);
            _resourceRepository.Delete(target);
            _skillOwnerRepository.Delete(target);
            _actorRepository.Delete(target);

            _positionableRepository.Delete(target);
        }
    }
}

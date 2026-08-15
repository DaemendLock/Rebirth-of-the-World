using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases.Scene
{
    public sealed class CharacterDeleteUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IResourceOwnerRepository _resourceRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IMovementEffectOwnerRepository _movementEffectOwnerRepository;

        private readonly IHurtableRepository _hurtableRepository;
        private readonly IHitboxOwnerRepository _hitboxOwnerRepository;

        private readonly IAbilityRepository _abilityRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;

        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;

        private readonly IPlayerRepository _playerRepository;
        private readonly ICharacterRemoveOutput _outputPort;

        public CharacterDeleteUseCase(IHealthRepository healthRepository, IAttributesRepository attributesRepository,
                                      IResourceOwnerRepository resourceRepository, ISkillOwnerRepository skillOwnerRepository,
                                      IPositionableRepository positionableRepository, IActorRepository actorRepository,
                                      IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateRepository characterUpdateList,
                                      IHurtableRepository hurtableRepository, IHitboxOwnerRepository hitboxOwnerRepository,
                                      IAbilityRepository abilityRepository, IStatusRepository statusRepository,
                                      IStatusTimerRepository statusTimerRepository, IPlayerRepository playerRepository,
                                       ISkillLyfecycleHandler skillLyfecycleHandler, IStatusLifecycleHandler statusLifecycleHandler,
                                       IMovementEffectOwnerRepository movementEffectOwnerRepository)
        {
            _healthRepository = healthRepository;
            _attributesRepository = attributesRepository;
            _resourceRepository = resourceRepository;
            _skillOwnerRepository = skillOwnerRepository;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterUpdateList = characterUpdateList;
            _hurtableRepository = hurtableRepository;
            _hitboxOwnerRepository = hitboxOwnerRepository;
            _abilityRepository = abilityRepository;
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
            _playerRepository = playerRepository;
            _skillLyfecycleHandler = skillLyfecycleHandler;
            _statusLifecycleHandler = statusLifecycleHandler;
            _movementEffectOwnerRepository = movementEffectOwnerRepository;
        }

        public void Execute(UnitId target)
        {
            if (_playerRepository.TryFindOwner(target, out Player player))
            {
                player.ControlledEntity = default;
                _playerRepository.Update(player);
            }

            if (_skillOwnerRepository.TryGet(target, out SkillOwner skillOwner))
            {
                foreach (var item in skillOwner.Skills)
                {
                    AbilityKey abilityKey = new(target, item);
                    _skillLyfecycleHandler.Remove(abilityKey);
                    _abilityRepository.Delete(abilityKey);
                }
            }

            if (_statusOwnerRepository.TryGet(target, out StatusOwner statusOwner))
            {
                foreach (var item in statusOwner.GetAll())
                {
                    _statusLifecycleHandler.Remove(item);
                    _statusTimerRepository.Delete(item);
                    _statusRepository.Delete(item);
                }
            }

            if (_actorRepository.TryGet(target, out Actor actor))
            {
                actor.CurrentAction?.Interrupt(InterruptReason.Forced);
            }

            _skillOwnerRepository.Delete(target);
            _statusOwnerRepository.Delete(target);
            _healthRepository.Delete(target);
            _characterUpdateList.Delete(target);

            _hitboxOwnerRepository.Delete(target);
            _hurtableRepository.Delete(target);
            _actorRepository.Delete(target);
            _resourceRepository.Delete(target);
            _movementEffectOwnerRepository.Delete(target);

            _attributesRepository.Delete(target);
            _positionableRepository.Delete(target);
        }
    }
}

using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Application.UseCases.Scene
{
    public sealed class UnitDeleteUseCase
    {
        private readonly IHealthOwnerRepository _healthRepository;
        private readonly IAttributesRepository _attributesRepository;
        private readonly IResourceOwnerRepository _resourceRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IMovementEffectOwnerRepository _movementEffectOwnerRepository;
        private readonly IScaleEffectOwnerRepository _scaleEffectOwnerRepository;

        private readonly IHurtableRepository _hurtableRepository;
        private readonly IHitboxOwnerRepository _hitboxOwnerRepository;

        private readonly IStatusTimerRepository _statusTimerRepository;

        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;

        private readonly PlayerSession _playerSession;

        private readonly ICharacterRemoveOutput _outputPort;

        public UnitDeleteUseCase(IHealthOwnerRepository healthRepository, IAttributesRepository attributesRepository,
                                      IResourceOwnerRepository resourceRepository, ISkillOwnerRepository skillOwnerRepository,
                                      IPositionableRepository positionableRepository, IActorRepository actorRepository,
                                      IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateRepository characterUpdateList,
                                      IHurtableRepository hurtableRepository, IHitboxOwnerRepository hitboxOwnerRepository, IStatusTimerRepository statusTimerRepository,
                                      ISkillLyfecycleHandler skillLyfecycleHandler, IStatusLifecycleHandler statusLifecycleHandler,
                                      IMovementEffectOwnerRepository movementEffectOwnerRepository, IScaleEffectOwnerRepository scaleEffectOwnerRepository, PlayerSession playerSession)
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
            _statusTimerRepository = statusTimerRepository;
            _skillLyfecycleHandler = skillLyfecycleHandler;
            _statusLifecycleHandler = statusLifecycleHandler;
            _movementEffectOwnerRepository = movementEffectOwnerRepository;
            _scaleEffectOwnerRepository = scaleEffectOwnerRepository;
            _playerSession = playerSession;
        }

        public void Execute(UnitId target)
        {
            if (_playerSession.ControlledUnitId.HasValue && _playerSession.ControlledUnitId.Value == target)
            {
                _playerSession.ControlledUnitId = default;
            }

            if (_skillOwnerRepository.TryGet(target, out SkillOwner skillOwner))
            {
                foreach (var item in skillOwner.Skills)
                {
                    AbilityKey abilityKey = new(target, item);
                    _skillLyfecycleHandler.Remove(abilityKey);
                }
            }

            if (_statusOwnerRepository.TryGet(target, out StatusOwner statusOwner))
            {
                foreach (var item in statusOwner.GetAll())
                {
                    _statusTimerRepository.Delete(item.Id);
                    try
                    {
                        _statusLifecycleHandler.Remove(item.Id);
                    }
                    catch { }
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
            _scaleEffectOwnerRepository.Delete(target);

            _attributesRepository.Delete(target);
            _positionableRepository.Delete(target);
        }
    }
}

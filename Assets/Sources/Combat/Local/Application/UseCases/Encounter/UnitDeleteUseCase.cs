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
    public sealed class UnitCleanupUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IPositionableRepository _positionableRepository;
        private readonly ICharacterUpdateQueue _characterUpdateList;
        private readonly IMovementEffectOwnerRepository _movementEffectOwnerRepository;
        private readonly IScaleEffectOwnerRepository _scaleEffectOwnerRepository;

        private readonly IHurtableRepository _hurtableRepository;
        private readonly IHitboxOwnerRepository _hitboxOwnerRepository;

        private readonly IStatusTimerRepository _statusTimerRepository;

        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;

        private readonly ICharacterDeleteQueue _characterDeleteQueue;

        private readonly PlayerSession _playerSession;

        private readonly ICharacterRemoveOutput _outputPort;

        public UnitCleanupUseCase(IUnitStore unitStore, IPositionableRepository positionableRepository,
                                      ICharacterUpdateQueue characterUpdateList, IHurtableRepository hurtableRepository,
                                      IHitboxOwnerRepository hitboxOwnerRepository, IStatusTimerRepository statusTimerRepository,
                                      ISkillLyfecycleHandler skillLyfecycleHandler, IStatusLifecycleHandler statusLifecycleHandler,
                                      IMovementEffectOwnerRepository movementEffectOwnerRepository, IScaleEffectOwnerRepository scaleEffectOwnerRepository, PlayerSession playerSession, ICharacterDeleteQueue characterDeleteQueue)
        {
            _unitStore = unitStore;
            _positionableRepository = positionableRepository;
            _characterUpdateList = characterUpdateList;
            _hurtableRepository = hurtableRepository;
            _hitboxOwnerRepository = hitboxOwnerRepository;
            _statusTimerRepository = statusTimerRepository;
            _skillLyfecycleHandler = skillLyfecycleHandler;
            _statusLifecycleHandler = statusLifecycleHandler;
            _movementEffectOwnerRepository = movementEffectOwnerRepository;
            _scaleEffectOwnerRepository = scaleEffectOwnerRepository;
            _playerSession = playerSession;
            _characterDeleteQueue = characterDeleteQueue;
        }

        public void Execute()
        {
            while (_characterDeleteQueue.TryDequeue(out var characterDeleteQueue))
            {
                HandleTarget(characterDeleteQueue);
            }
        }

        private void HandleTarget(UnitId target)
        {
            if (_unitStore.TryGet(target, out UnitContext unit) == false)
            {
                return;
            }

            if (_playerSession.ControlledUnitId.HasValue && _playerSession.ControlledUnitId.Value == target)
            {
                _playerSession.ControlledUnitId = default;
            }

            foreach (var item in unit.Skills.Skills)
            {
                AbilityKey abilityKey = new(target, item);

                try
                {
                    _skillLyfecycleHandler.Remove(abilityKey);
                }
                catch (System.Exception e) { UnityEngine.Debug.LogException(e); }
            }

            foreach (var item in unit.Statuses.GetAll())
            {
                _statusTimerRepository.Delete(item.Id);

                try
                {
                    _statusLifecycleHandler.Remove(item.Id);
                }
                catch (System.Exception e) { UnityEngine.Debug.LogException(e); }
            }

            unit.Actor.CurrentAction?.Interrupt(InterruptReason.Forced);

            _hitboxOwnerRepository.Delete(target);
            _hurtableRepository.Delete(target);
            _movementEffectOwnerRepository.Delete(target);
            _scaleEffectOwnerRepository.Delete(target);
            _unitStore.Delete(target);
            _characterUpdateList.Delete(target);

            _positionableRepository.Delete(target);
        }
    }

    public sealed class UnitDeleteUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IPositionableRepository _positionableRepository;
        private readonly ICharacterUpdateQueue _characterUpdateList;
        private readonly IMovementEffectOwnerRepository _movementEffectOwnerRepository;
        private readonly IScaleEffectOwnerRepository _scaleEffectOwnerRepository;

        private readonly IHurtableRepository _hurtableRepository;
        private readonly IHitboxOwnerRepository _hitboxOwnerRepository;

        private readonly IStatusTimerRepository _statusTimerRepository;

        private readonly ISkillLyfecycleHandler _skillLyfecycleHandler;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;

        private readonly PlayerSession _playerSession;

        private readonly ICharacterRemoveOutput _outputPort;

        public UnitDeleteUseCase(IUnitStore unitStore, IPositionableRepository positionableRepository,
                                      ICharacterUpdateQueue characterUpdateList, IHurtableRepository hurtableRepository,
                                      IHitboxOwnerRepository hitboxOwnerRepository, IStatusTimerRepository statusTimerRepository,
                                      ISkillLyfecycleHandler skillLyfecycleHandler, IStatusLifecycleHandler statusLifecycleHandler,
                                      IMovementEffectOwnerRepository movementEffectOwnerRepository, IScaleEffectOwnerRepository scaleEffectOwnerRepository, PlayerSession playerSession)
        {
            _unitStore = unitStore;
            _positionableRepository = positionableRepository;
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
            if (_unitStore.TryGet(target, out UnitContext unit) == false)
            {
                return;
            }

            if (_playerSession.ControlledUnitId.HasValue && _playerSession.ControlledUnitId.Value == target)
            {
                _playerSession.ControlledUnitId = default;
            }

            foreach (var item in unit.Skills.Skills)
            {
                AbilityKey abilityKey = new(target, item);

                try
                {
                    _skillLyfecycleHandler.Remove(abilityKey);
                }
                catch (System.Exception e) { UnityEngine.Debug.LogException(e); }
            }

            foreach (var item in unit.Statuses.GetAll())
            {
                _statusTimerRepository.Delete(item.Id);

                try
                {
                    _statusLifecycleHandler.Remove(item.Id);
                }
                catch (System.Exception e) { UnityEngine.Debug.LogException(e); }
            }

            unit.Actor.CurrentAction?.Interrupt(InterruptReason.Forced);

            _hitboxOwnerRepository.Delete(target);
            _hurtableRepository.Delete(target);
            _movementEffectOwnerRepository.Delete(target);
            _scaleEffectOwnerRepository.Delete(target);
            _unitStore.Delete(target);
            _characterUpdateList.Delete(target);

            _positionableRepository.Delete(target);
        }
    }
}

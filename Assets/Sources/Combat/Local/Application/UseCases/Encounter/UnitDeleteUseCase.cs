using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.Repositories;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Application.UseCases.Scene
{
    public sealed class UnitDeleteUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IActorRepository _actorRepository;
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

        public UnitDeleteUseCase(IUnitStore unitStore,
                                      IPositionableRepository positionableRepository, IActorRepository actorRepository,
                                      ICharacterUpdateRepository characterUpdateList,
                                      IHurtableRepository hurtableRepository, IHitboxOwnerRepository hitboxOwnerRepository, IStatusTimerRepository statusTimerRepository,
                                      ISkillLyfecycleHandler skillLyfecycleHandler, IStatusLifecycleHandler statusLifecycleHandler,
                                      IMovementEffectOwnerRepository movementEffectOwnerRepository, IScaleEffectOwnerRepository scaleEffectOwnerRepository, PlayerSession playerSession)
        {
            _unitStore = unitStore;
            _positionableRepository = positionableRepository;
            _actorRepository = actorRepository;
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
            try
            {
                ref UnitContext unit = ref _unitStore.Get(target);

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
                    catch { }
                }

                foreach (var item in unit.Statuses.GetAll())
                {
                    _statusTimerRepository.Delete(item.Id);
                    try
                    {
                        _statusLifecycleHandler.Remove(item.Id);
                    }
                    catch { }
                }

                if (_actorRepository.TryGet(target, out Actor actor))
                {
                    actor.CurrentAction?.Interrupt(InterruptReason.Forced);
                }

                _hitboxOwnerRepository.Delete(target);
                _hurtableRepository.Delete(target);
                _actorRepository.Delete(target);
                _movementEffectOwnerRepository.Delete(target);
                _scaleEffectOwnerRepository.Delete(target);
                _unitStore.Delete(target);
                _characterUpdateList.Delete(target);

                _positionableRepository.Delete(target);
            }
            catch { }
        }
    }
}

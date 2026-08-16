using Combat.Common.Primitives;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.Facades
{
    public readonly struct CharacterFacade
    {
        private readonly ResourceGiveUseCase _giveResourceUseCase;
        private readonly ResourceSpendUseCase _spendResourceUseCase;

        private readonly ActorForceKillUseCase _killUnitUseCase;
        private readonly ActorReviveUseCase _reviveUnitUseCase;
        private readonly AddMovementEffectUseCase _addMovementEffectUseCase;
        private readonly PositionableStartScaleUseCase _startScaleUseCase;
        private readonly PositionableStopScaleUseCase _stopScaleUseCase;

        private readonly StatusOwnerFindStatusUseCase _findStatusUseCase;
        private readonly StatusOwnerApplyUseCase _applyStatusUseCase;
        private readonly IActorRepository _killableRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IResourceOwnerRepository _resourceRepository;

        public CharacterFacade(ResourceGiveUseCase giveResourceUseCase, ResourceSpendUseCase spendResourceUseCase,
                               ActorForceKillUseCase killUnitUseCase, StatusOwnerFindStatusUseCase findStatusUseCase,
                               IActorRepository killableRepository, IPositionableRepository positionableRepository,
                               IResourceOwnerRepository resourceRepository, StatusOwnerApplyUseCase applyStatusUseCase,
                               ActorDesireMoveInDirectionUseCase moveInDirectionUseCase, AddMovementEffectUseCase addMovementEffectUseCase,
                               ActorReviveUseCase reviveUnitUseCase,
                               PositionableStartScaleUseCase startScaleUseCase, PositionableStopScaleUseCase stopScaleUseCase)
        {
            _giveResourceUseCase = giveResourceUseCase;
            _spendResourceUseCase = spendResourceUseCase;
            _killUnitUseCase = killUnitUseCase;
            _findStatusUseCase = findStatusUseCase;
            _killableRepository = killableRepository;
            _positionableRepository = positionableRepository;
            _resourceRepository = resourceRepository;
            _applyStatusUseCase = applyStatusUseCase;
            _addMovementEffectUseCase = addMovementEffectUseCase;
            _reviveUnitUseCase = reviveUnitUseCase;
            _startScaleUseCase = startScaleUseCase;
            _stopScaleUseCase = stopScaleUseCase;
        }

        public float GetResourceValue(UnitId target, ResourceId resource) => _resourceRepository.Get(target).GetResource(resource).Value;

        public void GiveResource(UnitId target, ResourceId resource, float value, AbilityKey? abilityId)
        {
            _giveResourceUseCase.Execute(target, resource, value, abilityId);
        }

        public void SpendResource(UnitId target, ResourceId resource, float value, AbilityKey? abilityId)
        {
            _spendResourceUseCase.Execute(target, resource, value, abilityId);
        }

        public bool IsAlive(UnitId target)
        {
            if (_killableRepository.TryGet(target, out var value))
            {
                return value.ConsciousState == ConsciousState.Alive;
            }

            return false;
        }

        public void Kill(UnitId target, AbilityKey? source) => _killUnitUseCase.Execute(target, source);

        public void Revive(UnitId target, AbilityKey? source) => _reviveUnitUseCase.Execute(target, source);

        public bool HasStatus(UnitId target, StatusType statusName) => _findStatusUseCase.FindStatus(target, statusName).HasValue;

        public Team GetTeam(UnitId target) => _positionableRepository.Get(target).Team;

        public PositionDTO GetPosition(UnitId target)
        {
            Positionable value = _positionableRepository.Get(target);
            return new(value.Position, value.Rotation, value.Scale, value.ModelName);
        }

        public void ApplyStatus(UnitId target, StatusType name, int stackCount, float duration, AbilityKey? source)
        {
            ApplStatusDTO dto = new(target, name, duration, stackCount, source);
            _applyStatusUseCase.Execute(dto);
        }

        public void AddMoveInDirectionEffect(UnitId target, Vector3 direction, float speed, bool isRelative, float maxDuration = 10f)
        {
            _addMovementEffectUseCase.Execute(target, direction, speed, isRelative, maxDuration);
        }

        public ScaleEffectId StartScaleOverTime(UnitId target, float rate) => _startScaleUseCase.Execute(target, rate);

        public void StopScaleOverTime(ScaleEffectId effectId) => _stopScaleUseCase.Execute(effectId);
    }

    public readonly ref struct PositionDTO
    {
        public PositionDTO(Vector3 position, Quaternion roation, float scale, ModelName modelName)
        {
            Position = position;
            Roation = roation;
            Scale = scale;
            ModelName = modelName;
        }

        public Vector3 Position { get; }
        public Quaternion Roation { get; }
        public float Scale { get; }
        public ModelName ModelName { get; }
    }
}

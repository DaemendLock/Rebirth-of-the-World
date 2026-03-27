using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;

using UnityEngine;

namespace Combat.Local.Domain.Facades
{
    public readonly struct CharacterFacade
    {
        private readonly GiveResourceUseCase _giveResourceUseCase;
        private readonly SpendResourceUseCase _spendResourceUseCase;

        private readonly ForceKillUseCase _killUnitUseCase;
        private readonly ReviveUseCase _reviveUnitUseCase;
        private readonly AddMovementEffectUseCase _addMovementEffectUseCase;

        private readonly FindStatusUseCase _findStatusUseCase;
        private readonly ApplyStatusUseCase _applyStatusUseCase;
        private readonly IAligmentRepository _aligmentRepository;
        private readonly IStateRepository _killableRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IResourceRepository _resourceRepository;

        public CharacterFacade(GiveResourceUseCase giveResourceUseCase, SpendResourceUseCase spendResourceUseCase, ForceKillUseCase killUnitUseCase, FindStatusUseCase findStatusUseCase,
            IAligmentRepository aligmentRepository, IStateRepository killableRepository, IPositionableRepository positionableRepository, IResourceRepository resourceRepository, ApplyStatusUseCase applyStatusUseCase, MoveInDirectionUseCase moveInDirectionUseCase, AddMovementEffectUseCase addMovementEffectUseCase, ReviveUseCase reviveUnitUseCase)
        {
            _giveResourceUseCase = giveResourceUseCase;
            _spendResourceUseCase = spendResourceUseCase;
            _killUnitUseCase = killUnitUseCase;
            _findStatusUseCase = findStatusUseCase;
            _aligmentRepository = aligmentRepository;
            _killableRepository = killableRepository;
            _positionableRepository = positionableRepository;
            _resourceRepository = resourceRepository;
            _applyStatusUseCase = applyStatusUseCase;
            _addMovementEffectUseCase = addMovementEffectUseCase;
            _reviveUnitUseCase = reviveUnitUseCase;
        }

        public float GetResourceValue(EntityId target, ResourceId resource) => _resourceRepository.Get(target, resource).CurrentValue;

        public void GiveResource(EntityId target, ResourceId resource, float value, SkillId? skill, EntityId? caster)
        {
            _giveResourceUseCase.Execute(target, resource, value, new(caster, skill));
        }

        public void SpendResource(EntityId target, ResourceId resource, float value, SkillId? skill, EntityId? caster)
        {
            _spendResourceUseCase.Execute(target, resource, value, new(caster, skill));
        }

        public bool IsAlive(EntityId target) => _killableRepository.Get(target).ConsciousState == ConsciousState.Alive;

        public void Kill(EntityId target, SkillId? skill, EntityId? caster) => _killUnitUseCase.Execute(target, new(caster, skill));

        public void Revive(EntityId target, SkillId? skill, EntityId? caster) => _reviveUnitUseCase.Execute(target, new(caster, skill));

        public bool HasStatus(EntityId target, StatusName statusName)
        {
            return _findStatusUseCase.FindStatus(target, statusName).HasValue;
        }

        public Team GetTeam(EntityId target) => _aligmentRepository.Get(target).Team;

        public PositionDTO GetPosition(EntityId target)
        {
            Positionable value = _positionableRepository.Get(target);
            return new(value.Position, value.Rotation, value.Scale, value.ModelName);
        }

        public void ApplyStatus(EntityId target, StatusName name, int stackCount, float duration, SkillId? source, EntityId? caster)
        {
            ApplStatusDTO dto = new(target, name, duration, stackCount, source, caster);
            _applyStatusUseCase.Execute(dto);
        }

        public void AddMoveInDirectionEffect(EntityId target, Vector3 direction, float speed, bool isRelative, float maxDuration = 10f)
        {
            _addMovementEffectUseCase.Execute(target, direction, speed, isRelative, maxDuration);
        }
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

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;

using UnityEngine;

namespace Combat.Local.Controllers
{
    public class CharacterController
    {
        private readonly ApplyStatusUseCase _applyStatusUseCase;

        private readonly GiveResourceUseCase _giveResourceUseCase;
        private readonly SpendResourceUseCase _spendResourceUseCase;

        private readonly KillUnitUseCase _killUnitUseCase;
        private readonly IReviveUnitUseCase _reviveUnitUseCase;

        private readonly FindStatusUseCase _findStatusUseCase;

        private readonly IAligmentRepository _aligmentRepository;
        private readonly IKillableRepository _killableRepository;
        private readonly IPositionableRepository _positionableRepository;
        private readonly IResourceRepository _resourceRepository;

        public CharacterController(ApplyStatusUseCase applyStatusUseCase, GiveResourceUseCase giveResourceUseCase, SpendResourceUseCase spendResourceUseCase, KillUnitUseCase killUnitUseCase, FindStatusUseCase findStatusUseCase,
            IAligmentRepository aligmentRepository, IKillableRepository killableRepository, IPositionableRepository positionableRepository, IResourceRepository resourceRepository)
        {
            _applyStatusUseCase = applyStatusUseCase;
            _giveResourceUseCase = giveResourceUseCase;
            _spendResourceUseCase = spendResourceUseCase;
            _killUnitUseCase = killUnitUseCase;
            _findStatusUseCase = findStatusUseCase;
            _aligmentRepository = aligmentRepository;
            _killableRepository = killableRepository;
            _positionableRepository = positionableRepository;
            _resourceRepository = resourceRepository;
        }

        public void ApplyStatus(EntityId target, StatusName name, int stackCount, float duration, SkillId? skill, EntityId? caster)
        {
            _applyStatusUseCase.Execute(target, name, stackCount, duration, new(caster, skill));
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

        public bool IsAlive(EntityId target) => _killableRepository.Get(target).Alive;

        public void Kill(EntityId target, SkillId? skill, EntityId? caster)
        {
            _killUnitUseCase.Execute(target, new(caster, skill));
        }

        public void Revive(EntityId target, SkillId? skill, EntityId? caster)
        {
            _reviveUnitUseCase.Execute(target, new(caster, skill));
        }

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

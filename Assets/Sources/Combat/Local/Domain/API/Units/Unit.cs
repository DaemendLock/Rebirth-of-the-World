using System.Linq;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;
using Combat.Local.Domain.ValueObjects;

using UnityEngine;

namespace Combat.Local.Domain.API
{
    public sealed partial class Unit
    {
        private readonly IPositionRepository _positionRepository;

        private readonly IStatusService _statusService;
        private readonly IAttributeEvaluationService _attributeEvaluationService;
        private readonly ResourceService _giveSpendResourceService;
        private readonly IStatusLookupService _statusLookupService;

        public Unit(EntityId id,
            IPositionRepository positionRepository,
            HealthService healDamageApplicationService,
            IKillReviveService killReviveService,
            IStatusService statusApplicationRemovalService,
            IAttributeEvaluationService attributeEvaluationService,
            ResourceService giveSpendResourceService,
            IStatusLookupService statusLookupService)
        {
            Id = id;
            _positionRepository = positionRepository;

            _healthService = healDamageApplicationService;
            _killReviveService = killReviveService;
            _statusService = statusApplicationRemovalService;
            _attributeEvaluationService = attributeEvaluationService;
            _giveSpendResourceService = giveSpendResourceService;
            _statusLookupService = statusLookupService;
        }

        public EntityId Id { get; }

        public Team Team => _positionRepository.Get(Id).Team;

        public ModelName UnitId => _positionRepository.Get(Id).ModelName;

        public float Scale
        {
            get => _positionRepository.Get(Id).Scale;
            set
            {
                Entities.Units.Transform positionable = _positionRepository.Get(Id);
                positionable.Scale = value;
                _positionRepository.Update(positionable);
            }
        }

        public Vector3 Position => _positionRepository.Get(Id).Position;

        public float GetCooldown(SkillId skillId) => 0;

        public float GetAttributeValue(Attribute attribute) => _attributeEvaluationService.GetAttributeValue(Id, attribute);

        public float GetVersalityModifier() => _attributeEvaluationService.GetVersalityModifier(Id);

        public float GetHasteModifier() => _attributeEvaluationService.GetHasteModifier(Id);

        public bool CanHurt(Unit target) => Team != target.Team;

        public void ApplyStatus(StatusApplicationData statusData)
        {
            _statusService.ApplyStatus(statusData.Name, Id, statusData.Source.Caster.Id, statusData.Source.Id, statusData.Duration, statusData.StackCount);
        }

        public bool HasStatus(StatusName name) => _statusLookupService.FindStatusesOnUnit(Id).Any(value => value.Name == name);
    }
}

using Combat.Common.ValueObjects;

using Combat.Local.Domain.API;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;

using Testing.Local.Temp.Services;

namespace Testing.Local.Temp.Factories
{
    public class UnitApiFactory
    {
        private readonly IPositionRepository _positionRepository;

        private readonly HealthService _healDamageService;
        private readonly IKillReviveService _killReviveService;
        private readonly IStatusService _statusApplicationRemovalService;
        private readonly IAttributeEvaluationService _attributeEvaluationService;
        private readonly ResourceService _giveSpendResourceService;
        private readonly IStatusLookupService _statusLookupService;

        public UnitApiFactory(IPositionRepository positionRepository,
            HealthService healDamageService,
            IKillReviveService killReviveService,
            IStatusService statusApplicationRemovalService,
            IAttributeEvaluationService attributeEvaluationService,
            ResourceService giveSpendResourceService,
            IStatusLookupService statusLookupService)
        {
            _positionRepository = positionRepository;
            _healDamageService = healDamageService;
            _killReviveService = killReviveService;
            _statusApplicationRemovalService = statusApplicationRemovalService;
            _attributeEvaluationService = attributeEvaluationService;
            _giveSpendResourceService = giveSpendResourceService;
            _statusLookupService = statusLookupService;
        }

        public Unit Create(EntityId id)
        {
            Unit result = new(id,
            _positionRepository, _healDamageService, _killReviveService,
            _statusApplicationRemovalService, _attributeEvaluationService, _giveSpendResourceService, _statusLookupService);

            return result;
        }
    }
}

using Combat.Local.Factories;
using Combat.Local.Infrastructure.Controllers;
using Combat.Local.Infrastructure.Presenters;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.Services;
using Combat.Local.Domain.Repositories;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.IDK;
using Combat.Local.Domain.API;

namespace Testing.Local.Temp.Factories
{
    public class UnitControllerFactory : IUnitControllerFactory
    {
        private readonly ISkillAnimationRepository _skillAnimationRepository;
        private readonly ICastableRepository _castableRepository;

        private readonly ISkillCastService _skillCastService;
        private readonly IMovementService _movementService;

        private readonly IUnitModelFactory _modelFactory;
        private readonly IUnitViewFactory _viewFactory;
        private readonly UnitApiFactory _unitApiFactory;
        private readonly SkillApiFactory _skillApiFactory;

        private readonly UnitApiRepository _unitApiRepository;
        private readonly SkillApiRepository _skillApiRepository;

        public UnitControllerFactory(IUnitModelFactory modelFactory, IUnitViewFactory viewFactory, ISkillAnimationRepository skillDataRepository, IMovementService movementService, ISkillCastService skillCastService, ICastableRepository castableRepository, UnitApiFactory unitApiFactory, SkillApiFactory skillApiFactory, SkillApiRepository skillApiRepository, UnitApiRepository unitApiRepository)
        {
            _modelFactory = modelFactory;
            _viewFactory = viewFactory;
            _skillAnimationRepository = skillDataRepository;
            _movementService = movementService;
            _skillCastService = skillCastService;
            _castableRepository = castableRepository;

            _unitApiFactory = unitApiFactory;
            _skillApiFactory = skillApiFactory;

            _skillApiRepository = skillApiRepository;
            _unitApiRepository = unitApiRepository;
        }

        public UnitController Create(IUnitControllerFactory.UnitModelCreationData context)
        {
            EntityId entityId = _modelFactory.Create(context);
            IUnitPresenter view = _viewFactory.Create(new(entityId, context.UnitId, context.Parent, context.Position));
            Unit api = _unitApiFactory.Create(entityId);

            _unitApiRepository.Create(api);

            int i = 0;

            foreach (SkillId skillId in context.Skills)
            {
                _skillApiRepository.Create(_skillApiFactory.Create(entityId, skillId));
                _castableRepository.Create(i++, entityId, skillId);
            }

            return new UnitController(entityId, view, _skillAnimationRepository, _castableRepository, _movementService, _skillCastService);
        }
    }
}

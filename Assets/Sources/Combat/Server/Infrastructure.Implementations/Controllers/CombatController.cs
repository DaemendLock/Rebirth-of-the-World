using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Repositories;
using Server.Combat.Infrastructure.Controllers;
using Server.Combat.Infrastructure.Factories;
using Server.Combat.Infrastructure.Repositories;

namespace Server.Combat.Infrastructure.Implementations.Controllers
{
    public class CombatController : ICombatController
    {
        private readonly IAttributesRepository _attributesRepository;
        private readonly IUnitControllerRepository _unitControllerRespository;
        private readonly IStatusRepository _statusRepository;
        private readonly IActionHandlerRepository _actionRepository;
        private readonly IUnitModelFactory _unitModelFactory;

        public CombatController(IActionHandlerRepository skillHandlerRepository, IAttributesRepository attributesRepository, IStatusRepository statusRepository, IUnitModelFactory unitModelFactory, IUnitControllerRepository unitControllerRespository)
        {
            _actionRepository = skillHandlerRepository;
            _attributesRepository = attributesRepository;
            _statusRepository = statusRepository;
            _unitModelFactory = unitModelFactory;
            _unitControllerRespository = unitControllerRespository;
        }

        public void CreateUnit(IUnitModelFactory.UnitModelCreationData data)
        {
            Unit model = _unitModelFactory.Create(data);
            _unitControllerRespository.Add(model.Id, new UnitController(model));
        }

        public void Tick()
        {
            float deltaTime = UnityEngine.Time.deltaTime;
            Domain.Common.Time.CurrentTime = (long) (UnityEngine.Time.time * 1000);

            _attributesRepository.Reset();
            //_statusRepository.Update(deltaTime);
            _actionRepository.Update(deltaTime);
        }
    }
}

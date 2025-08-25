using System.Collections.Generic;

using Combat.Local.Domain.Services;
using Combat.Local.Factories;

using Zenject;

namespace Combat.Local.Infrastructure.Controllers
{
    public class CombatController : ITickable
    {
        private readonly IModelUpdateService _modelUpdateService;
        private readonly List<UnitController> _unitToUpdate = new();

        private readonly IUnitControllerFactory _unitModelFactory;

        public CombatController(IUnitControllerFactory unitModelFactory, IModelUpdateService modelUpdateService)
        {
            _unitModelFactory = unitModelFactory;
            _modelUpdateService = modelUpdateService;
        }

        public void CreateUnit(IUnitControllerFactory.UnitModelCreationData data)
        {
            UnitController unit = _unitModelFactory.Create(data);
            _unitToUpdate.Add(unit);
            //_unitControllerRespository.Add(model.Id, new UnitController(model));
        }

        public void Tick()
        {
            float deltaTime = UnityEngine.Time.deltaTime;

            _modelUpdateService.Update(deltaTime);

            _unitToUpdate.ForEach(value => value.Update());
        }
    }
}
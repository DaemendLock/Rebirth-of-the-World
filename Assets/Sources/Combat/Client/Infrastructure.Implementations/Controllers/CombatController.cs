using System.Collections.Generic;

using Client.Combat.Infrastructure.Controllers;

namespace Client.Combat.Infrastructure.Implementations.Controllers
{
    public class CombatController : ICombatController
    {
        private readonly HashSet<IUnitController> _unitControllers;

        public CombatController()
        {
            _unitControllers = new();
        }

        public void Add(IUnitController unitController) => _unitControllers.Add(unitController);

        public void Tick()
        {
            foreach (IUnitController value in _unitControllers)
            {
                value.Update();
            }
        }
    }
}

using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Implementations.Actions;
using Server.Combat.Domain.Units;

using Server.Combat.Infrastructure.Controllers;
using Server.Combat.Infrastructure.Factories;
using Server.Combat.Infrastructure.Implementations.Controllers;
using Server.Combat.Infrastructure.Repositories;
using Server.Combat.Infrastructure.Services;

namespace Server.Combat.Infrastructure.Implementations.Factories
{
    //TODO: deside on layer. probably mistake
    public class UnitControllerFactory : IUnitControllerFactory
    {
        private readonly IUnitControllerRepository _unitControllerRepository;
        private readonly ISkillCastService _skillCastService;

        public UnitControllerFactory(IUnitControllerRepository unitControllerRepository, ISkillCastService skillCastService)
        {
            _unitControllerRepository = unitControllerRepository;

            _skillCastService = skillCastService;
        }

        public IUnitController Create(Unit model)
        {
            IUnitController result = new UnitController(model);

            for (int i = 0; i < 3; i++)
            {
                //result.SetAction(i, new CastAction(i, _skillCastService));
            }

            //result.SetAction(9, new JumpAction(3f));

            _unitControllerRepository.Add(model.Id, result);

            return result;
        }
    }
}

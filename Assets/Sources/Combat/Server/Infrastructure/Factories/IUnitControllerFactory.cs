using Server.Combat.Domain.Entities;
using Server.Combat.Infrastructure.Controllers;

namespace Server.Combat.Infrastructure.Factories
{
    public interface IUnitControllerFactory
    {
        public readonly ref struct UnitCreationData
        {
            public readonly IUnitModelFactory.UnitModelCreationData ModelCreationData;

            public UnitCreationData(IUnitModelFactory.UnitModelCreationData modelData)
            {
                ModelCreationData = modelData;
            }
        }

        IUnitController Create(Unit model);
    }
}

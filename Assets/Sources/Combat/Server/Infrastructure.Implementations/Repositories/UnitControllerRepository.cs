using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Units.ValueObjects;
using Server.Combat.Infrastructure.Controllers;
using Server.Combat.Infrastructure.Repositories;

using Utils.Patterns.Repository;

namespace Server.Combat.Infrastructure.Implementations.Repositories
{
    public class UnitControllerRepository : DictionaryRepository<IUnitController, EntityId>, IUnitControllerRepository
    { }
}

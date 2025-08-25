using System.Collections.Generic;

using Server.Combat.Domain.Units.ValueObjects;
using Server.Combat.Infrastructure.Controllers;

using Utils.Patterns.Repository;

namespace Server.Combat.Infrastructure.Repositories
{
    public interface IUnitControllerRepository : IRepository<IUnitController, EntityId>, IEnumerable<KeyValuePair<EntityId, IUnitController>>
    {
    }
}

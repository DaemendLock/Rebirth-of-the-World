using System.Collections.Generic;

using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.API.Statuses
{
    public interface IStatusLookupService
    {
        IEnumerable<StatusApi> FindStatusesOnUnit(EntityId owner);
        void Update();
    }
}
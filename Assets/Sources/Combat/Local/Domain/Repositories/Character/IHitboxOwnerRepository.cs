using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories
{
    public interface IHitboxOwnerRepository
    {
        void Create(UnitId id);
        void Delete(UnitId id);

        IEnumerable<Queue<HitRecord>> GetHits(UnitId id);
    }
}

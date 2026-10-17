using Combat.Common.Primitives;
using Combat.Domain.Entities;

using System.Collections.Generic;

namespace Combat.Domain.Repositories
{
    public interface IObjectiveRepository
    {
        void Create(Objective objective);
        bool TryGet(ObjectiveId objectiveId, out Objective objective);
        IReadOnlyCollection<ObjectiveId> GetAllIds();
        void Delete(ObjectiveId id);
        void Update(Objective objective);
    }
}

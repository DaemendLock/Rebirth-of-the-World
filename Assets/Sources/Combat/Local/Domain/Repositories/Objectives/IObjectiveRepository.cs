using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories.Objectives
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

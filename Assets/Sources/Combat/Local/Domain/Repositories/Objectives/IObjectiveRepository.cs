using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories.Objectives
{
    public interface IObjectiveRepository
    {
        void Create(Objective objective);
        bool TryGet(ObjectiveId objectiveId, out Objective objective);
        void Delete(ObjectiveId id);
        void Update(Objective objective);
    }
}

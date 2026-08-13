using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts
{
    public interface IObjectiveCreateHandler
    {
        void Create(Objective objective);
    }

    public interface IObjectiveCompleteHandler
    {
        void Complete(ObjectiveId objectiveId);
        void Cancel(ObjectiveId id);
        void Fail(ObjectiveId id);
    }
}

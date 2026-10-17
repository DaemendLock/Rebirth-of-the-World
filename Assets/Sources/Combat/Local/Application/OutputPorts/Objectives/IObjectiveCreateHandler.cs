using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;

namespace Combat.Local.Application.OutputPorts
{
    public interface IObjectiveCreateHandler
    {
        void Create(Objective objective);
    }

    public interface IObjectiveFinalizeHandler
    {
        void Finilize(ObjectiveId objectiveId, ObjectiveState state);
    }
}

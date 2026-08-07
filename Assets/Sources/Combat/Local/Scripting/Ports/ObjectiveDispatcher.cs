using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Ports
{
    public interface IObjectiveRuntimeFactory
    {
        RuntimeObjectiveContainer Create(Objective objective);
    }

    public sealed class ObjectiveDispatcher
    {
        private readonly ObjectiveRuntimeRegistry _objectiveContainer;
        private readonly IEventContext _eventContext;
        private readonly IObjectiveRuntimeFactory _factory;

        public ObjectiveDispatcher(ObjectiveRuntimeRegistry objectiveContainer)
        {
            _objectiveContainer = objectiveContainer;
        }

        public void Create(Objective value)
        {
            RuntimeObjectiveContainer objectiveRuntime = _factory.Create(value);
            _objectiveContainer.Create(value.Id, objectiveRuntime);
            objectiveRuntime.CombatObjective.OnStart(null, objectiveRuntime.Context);
        }

        public void Complete(ObjectiveId id)
        {
            if (_objectiveContainer.TryGet(id, out var value) == false)
            {
                return;
            }

            value.CombatObjective.OnComplete(value.Context);
            _eventContext.Publish(new GameEvent<ObjectiveCompletedEventData>(new(value.Context.Id)));
            value.Context.Dispose();
            _objectiveContainer.Remove(id);
        }

        public void Cancel(ObjectiveId id)
        {
            if (_objectiveContainer.TryGet(id, out var value) == false)
            {
                return;
            }

            value.CombatObjective.OnCancel(value.Context);
            value.Context.Dispose();
            _objectiveContainer.Remove(id);
        }
    }
}

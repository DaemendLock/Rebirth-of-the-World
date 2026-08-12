using Combat.API.Contexts;
using Combat.API.Events;
using Combat.API.Objectives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Ports
{
    public interface IObjectiveCompletionHandler
    {
        void Complete(ObjectiveId id);
        void Cancel(ObjectiveId id);
        void Fail(ObjectiveId id);
    }

    public sealed class ObjectiveDispatcher : IObjectiveCompletionHandler, IObjectiveCreateHandler
    {
        private readonly ObjectiveRuntimeRegistry _objectiveContainer;
        private readonly IObjectiveMemoryRepository _memoryRepository;
        private readonly IEventContext _eventContext;

        public ObjectiveDispatcher(ObjectiveRuntimeRegistry objectiveContainer, IObjectiveMemoryRepository memoryRepository, IEventContext eventContext)
        {
            _objectiveContainer = objectiveContainer;
            _memoryRepository = memoryRepository;
            _eventContext = eventContext;
        }

        public void Create(Objective value)
        {
            RuntimeObjectiveContainer objectiveRuntime = CreateRuntime(value);

            _objectiveContainer.Create(value.Id, objectiveRuntime);
            objectiveRuntime.CombatObjective.OnStart(null, objectiveRuntime.Context);
        }

        public void Complete(ObjectiveId id)
        {
            if (_objectiveContainer.TryGet(id, out var value) == false)
            {
                return;
            }

            try
            {
                value.CombatObjective.OnComplete(value.Context);
                _eventContext.Publish(new GameEvent<ObjectiveCompletedEventData>(new(id)));
            }
            finally
            {
                DisposeAndRemove(id, value);
            }
        }

        public void Cancel(ObjectiveId id)
        {
            if (_objectiveContainer.TryGet(id, out var value) == false)
            {
                return;
            }

            try
            {
                value.CombatObjective.OnCancel(value.Context);
            }
            finally
            {
                DisposeAndRemove(id, value);
            }
        }

        public void Fail(ObjectiveId id)
        {
            if (_objectiveContainer.TryGet(id, out var value) == false)
            {
                return;
            }

            try
            {
                value.CombatObjective.OnFail(value.Context);
            }
            finally
            {
                DisposeAndRemove(id, value);
            }
        }

        private RuntimeObjectiveContainer CreateRuntime(Objective objective)
        {
            ObjectiveContext objectiveContext = new(objective.Id, _memoryRepository, _eventContext, this);

            return new(new DealDamageObjective(), objectiveContext);
        }

        private void DisposeAndRemove(ObjectiveId id, RuntimeObjectiveContainer value)
        {
            try
            {
                value.Context.Dispose();
            }
            finally
            {
                _objectiveContainer.Remove(id);
            }
        }
    }
}

using Combat.API.Contexts;
using Combat.API.Events;
using Combat.API.Objectives;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Factories;
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
        private readonly IEncounterContext _context;
        private readonly IEventContext _eventContext;
        private readonly IObjectiveScriptFactory _scriptFactory;

        public ObjectiveDispatcher(ObjectiveRuntimeRegistry objectiveContainer, IObjectiveMemoryRepository memoryRepository, IEventContext eventContext, IEncounterContext context, IObjectiveScriptFactory scriptFactory)
        {
            _objectiveContainer = objectiveContainer;
            _memoryRepository = memoryRepository;
            _eventContext = eventContext;
            _context = context;
            _scriptFactory = scriptFactory;
        }

        public void Create(Objective value)
        {
            RuntimeObjectiveContainer objectiveRuntime = CreateRuntime(value);

            _objectiveContainer.Create(value.Id, objectiveRuntime);
            objectiveRuntime.CombatObjective.OnStart(objectiveRuntime.Context);
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
            ICombatObjective script = _scriptFactory.Create(objective);
            ObjectiveContext context = new(objective.Id, _memoryRepository, _eventContext, this, _context);

            return new(script, context);
        }

        private void DisposeAndRemove(ObjectiveId id, RuntimeObjectiveContainer value)
        {
            try
            {
                value.Context.Dispose();
            }
            finally
            {
                _memoryRepository.Delete(id);
                _objectiveContainer.Remove(id);
            }
        }
    }
}

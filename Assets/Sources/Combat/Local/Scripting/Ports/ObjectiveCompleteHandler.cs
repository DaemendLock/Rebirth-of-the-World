using Combat.API.Contexts;
using Combat.API.Events;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Factories;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Ports
{
    public sealed class ObjectiveCreateHandler : IObjectiveCreateHandler
    {
        private readonly ObjectiveRuntimeRegistry _objectiveContainer;
        private readonly IObjectiveScriptFactory _scriptFactory;
        private readonly IObjectiveMemoryRepository _memoryRepository;
        private readonly ObjectiveCompleteFacade _facade;
        private readonly IEncounterContext _context;
        private readonly IEventContext _eventContext;

        public ObjectiveCreateHandler(ObjectiveRuntimeRegistry objectiveContainer, IObjectiveScriptFactory scriptFactory, IObjectiveMemoryRepository memoryRepository, ObjectiveCompleteFacade facade, IEncounterContext context, IEventContext eventContext)
        {
            _objectiveContainer = objectiveContainer;
            _scriptFactory = scriptFactory;
            _memoryRepository = memoryRepository;
            _facade = facade;
            _context = context;
            _eventContext = eventContext;
        }

        public void Create(Objective value)
        {
            RuntimeObjectiveContainer objectiveRuntime = CreateRuntime(value);

            _objectiveContainer.Create(value.Id, objectiveRuntime);
            objectiveRuntime.CombatObjective.OnStart(objectiveRuntime.Context);
        }

        private RuntimeObjectiveContainer CreateRuntime(Objective objective)
        {
            ICombatObjective script = _scriptFactory.Create(objective);
            ObjectiveContext context = new(objective.Id, _memoryRepository, _eventContext, _context, _facade);

            return new(script, context);
        }
    }

    public sealed class ObjectiveCompleteHandler : IObjectiveFinalizeHandler
    {
        private readonly ObjectiveRuntimeRegistry _objectiveContainer;
        private readonly IObjectiveMemoryRepository _memoryRepository;
        private readonly IEventContext _eventContext;

        public ObjectiveCompleteHandler(ObjectiveRuntimeRegistry objectiveContainer, IObjectiveMemoryRepository memoryRepository, IEventContext eventContext)
        {
            _objectiveContainer = objectiveContainer;
            _memoryRepository = memoryRepository;
            _eventContext = eventContext;
        }

        public void Finilize(ObjectiveId id, ObjectiveState state)
        {
            if (_objectiveContainer.TryGet(id, out var value) == false)
            {
                return;
            }

            try
            {
                switch (state)
                {
                    case ObjectiveState.Completed:
                        value.CombatObjective.OnComplete(value.Context);
                        _eventContext.Publish(new GameEvent<ObjectiveCompletedEventData>(new(id)));
                        return;
                    case ObjectiveState.Failed:
                        value.CombatObjective.OnFail(value.Context);
                        return;
                    case ObjectiveState.Cancelled:
                        value.CombatObjective.OnCancel(value.Context);
                        return;
                    default:
                        return;
                }
            }
            finally
            {
                DisposeAndRemove(id, value);
            }
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

using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Local.Domain.UseCases.Objectives
{
    public sealed class ObjectiveFinalizeUseCase
    {
        private readonly IObjectiveRepository _objectiveRepository;
        private readonly IObjectiveFinalizeHandler _handler;

        public ObjectiveFinalizeUseCase(IObjectiveRepository objectiveRepository, IObjectiveFinalizeHandler handler)
        {
            _objectiveRepository = objectiveRepository;
            _handler = handler;
        }

        public void Execute(ObjectiveId id, ObjectiveState state)
        {
            if (_objectiveRepository.TryGet(id, out var value) == false)
            {
                throw new InvalidOperationException("Objective is not registered");
            }

            if (value.TryTransition(state) == false)
            {
                throw new InvalidOperationException("Unable to finalize objective.");
            }

            _objectiveRepository.Update(value);
            _handler.Finilize(id, value.State);
        }
    }

    public sealed class ObjectiveFinalizeAllUseCase
    {
        private readonly IObjectiveRepository _objectiveRepository;
        private readonly ObjectiveFinalizeUseCase _finalizeObjective;

        public ObjectiveFinalizeAllUseCase(IObjectiveRepository objectiveRepository,
                                           ObjectiveFinalizeUseCase finalizeObjective)
        {
            _objectiveRepository = objectiveRepository;
            _finalizeObjective = finalizeObjective;
        }

        public void Execute(ObjectiveState finalState)
        {
            ObjectiveId[] ids = _objectiveRepository.GetAllIds().ToArray();
            List<Exception> errors = null;

            foreach (ObjectiveId id in ids)
            {
                try
                {
                    if (_objectiveRepository.TryGet(id, out var objective) &&
                        objective.State == ObjectiveState.Running)
                    {
                        _finalizeObjective.Execute(id, finalState);
                    }
                }
                catch (Exception exception)
                {
                    errors ??= new List<Exception>();
                    errors.Add(exception);
                }
                finally
                {
                    _objectiveRepository.Delete(id);
                }
            }

            if (errors != null)
            {
                throw new AggregateException("One or more objectives failed to finalize.", errors);
            }
        }
    }
}

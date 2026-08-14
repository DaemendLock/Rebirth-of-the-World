using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases.Objectives;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Combat.Local.Domain.UseCases.Scene
{
    public sealed class EncounterFilalizeUseCase
    {
        private readonly IEncounterStateMachine _stateMachine;
        private readonly ObjectiveFinalizeAllUseCase _finalizeObjectives;
        private readonly ICharacterDeleteQueue _characterDeleteQueue;
        private readonly ICharacterUpdateRepository _characterUpdateRepository;
        private readonly CharacterDeleteUseCase _deleteCharacter;
        private readonly IEncounterEndOutput _output;

        public EncounterFilalizeUseCase(IEncounterStateMachine stateMachine, ObjectiveFinalizeAllUseCase finalizeObjectives,
                                   ICharacterDeleteQueue characterDeleteQueue, ICharacterUpdateRepository characterUpdateRepository,
                                   CharacterDeleteUseCase deleteCharacter, IEncounterEndOutput output)
        {
            _stateMachine = stateMachine;
            _finalizeObjectives = finalizeObjectives;
            _characterDeleteQueue = characterDeleteQueue;
            _characterUpdateRepository = characterUpdateRepository;
            _deleteCharacter = deleteCharacter;
            _output = output;
        }

        public void Execute(EncounterState reason)
        {
            ObjectiveState objectiveState = ToObjectiveState(reason);

            if (_stateMachine.TryFinalize() == false)
            {
                return;
            }

            List<Exception> errors = null;

            try
            {
                _finalizeObjectives.Execute(objectiveState);
            }
            catch (Exception exception)
            {
                errors = new List<Exception> { exception };
            }

            try
            {
                FlushCharacterCleanup();
            }
            catch (Exception exception)
            {
                errors ??= new List<Exception>();
                errors.Add(exception);
            }

            if (errors != null)
            {
                throw new AggregateException("Encounter cleanup failed. Scene transition was cancelled.", errors);
            }

            _output.Present(reason);
        }

        private void FlushCharacterCleanup()
        {
            HashSet<UnitId> targets = new();

            while (_characterDeleteQueue.TryDequeue(out UnitId queuedTarget))
            {
                targets.Add(queuedTarget);
            }

            foreach (var target in _characterUpdateRepository.GetAll().ToArray())
            {
                targets.Add(target.Id);
            }

            List<Exception> errors = null;

            foreach (UnitId target in targets)
            {
                try
                {
                    _deleteCharacter.Execute(target);
                }
                catch (Exception exception)
                {
                    errors ??= new List<Exception>();
                    errors.Add(exception);
                }
            }

            if (errors != null)
            {
                throw new AggregateException("One or more characters failed to clean up.", errors);
            }
        }

        private static ObjectiveState ToObjectiveState(EncounterState encounterState) => encounterState switch
        {
            EncounterState.Completed => ObjectiveState.Completed,
            EncounterState.Failed => ObjectiveState.Failed,
            EncounterState.Cancelled => ObjectiveState.Cancelled,
            _ => throw new ArgumentOutOfRangeException(nameof(encounterState), encounterState, null)
        };
    }
}

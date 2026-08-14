using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.UseCases;
using Combat.Local.Domain.UseCases.Scene;
using Combat.Local.Domain.ValueObjects;

using System;

using UnityEngine;

namespace Combat.Local.Controllers
{
    public readonly ref struct UnitCreationInfo
    {
        public readonly ModelName ModelName;
        public readonly Vector3 Position;
        public readonly Team Team;
        public readonly float BaseHealth;
        public readonly float CurrentHealth;
        public readonly Span<AttributeValue> DefaultAttributes;
        public readonly Span<ResourceValue> DefaultResources;
        public readonly Span<SkillId> Skills;

        public UnitCreationInfo(ModelName modelName, Team team, Vector3 position, float baseHealth, float currentHealth, Span<AttributeValue> defaultAttributes, Span<ResourceValue> defaultResources, Span<SkillId> skills)
        {
            ModelName = modelName;
            Position = position;
            Team = team;
            BaseHealth = baseHealth;
            CurrentHealth = currentHealth;
            DefaultAttributes = defaultAttributes;
            Skills = skills;
            DefaultResources = defaultResources;
        }
    }

    public sealed class EncounterController
    {
        private readonly CharacterCreateUseCase _createUnitUseCase;
        private readonly EncounterFilalizeUseCase _endEncounterUseCase;
        private readonly IEncounterStateMachine _stateMachine;

        public EncounterController(CharacterCreateUseCase createUnitUseCase,
                                   EncounterFilalizeUseCase endEncounterUseCase,
                                   IEncounterStateMachine stateMachine)
        {
            _createUnitUseCase = createUnitUseCase;
            _endEncounterUseCase = endEncounterUseCase;
            _stateMachine = stateMachine;
        }

        public void Start()
        {
            if (_stateMachine.TryStart() == false)
            {
                throw new InvalidOperationException($"Encounter cannot start from state {_stateMachine.State}.");
            }
        }

        public EncounterState State => _stateMachine.State;

        public bool Pause() => _stateMachine.TryPause();

        public bool Resume() => _stateMachine.TryResume();

        public void Finalize(EncounterState reason)
        {
            _endEncounterUseCase.Execute(reason);
        }

        public UnitId CreateUnit(UnitCreationInfo data)
        {
            CreateCharacterDTO unitCreationDTO = new(data.ModelName, data.Team, data.Position, data.CurrentHealth, data.BaseHealth, data.DefaultAttributes, data.DefaultResources, data.Skills);
            return _createUnitUseCase.Execute(unitCreationDTO);
        }

        public void CreateUnit(UnitId targetId, UnitCreationInfo data)
        {
            CreateCharacterDTO unitCreationDTO = new(data.ModelName, data.Team, data.Position, data.CurrentHealth, data.BaseHealth, data.DefaultAttributes, data.DefaultResources, data.Skills);
            _createUnitUseCase.Execute(targetId, unitCreationDTO);
        }
    }
}

using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Local.Application.UseCases;
using Combat.Local.Application.UseCases.Encounter;
using Combat.Local.Application.UseCases.Scene;

using System;
using System.Collections.Generic;
using System.Linq;

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
        public readonly float BaseComposure;
        public readonly float CurrentComposure;
        public readonly float BaseMovespeed;
        public readonly Span<AttributeValue> DefaultAttributes;
        public readonly Span<ResourceValue> DefaultResources;
        public readonly Span<SkillId> Skills;

        public UnitCreationInfo(ModelName modelName, Team team, Vector3 position, float baseHealth, float currentHealth, Span<AttributeValue> defaultAttributes, Span<ResourceValue> defaultResources, Span<SkillId> skills,
            float baseComposure, float currentComposure, float baseMovespeed)
        {
            ModelName = modelName;
            Position = position;
            Team = team;
            BaseHealth = baseHealth;
            CurrentHealth = currentHealth;
            BaseComposure = baseComposure;
            CurrentComposure = currentComposure;
            BaseMovespeed = baseMovespeed;
            DefaultAttributes = defaultAttributes;
            Skills = skills;
            DefaultResources = defaultResources;
        }
    }

    public sealed class EncounterController
    {
        private readonly UnitCreateUseCase _createUnitUseCase;
        private readonly CreateCharacterUseCase _createCharacterUseCase;
        private readonly EncounterFinalizeUseCase _endEncounterUseCase;
        private readonly IEncounterStateMachine _stateMachine;

        public EncounterController(UnitCreateUseCase createUnitUseCase,
                                   EncounterFinalizeUseCase endEncounterUseCase,
                                   IEncounterStateMachine stateMachine,
                                   CreateCharacterUseCase createCharacterUseCase)
        {
            _createUnitUseCase = createUnitUseCase;
            _endEncounterUseCase = endEncounterUseCase;
            _stateMachine = stateMachine;
            _createCharacterUseCase = createCharacterUseCase;
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

        public void Finalize(EncounterState reason) => _endEncounterUseCase.Execute(reason);

        public UnitId CreateUnit(UnitCreationInfo data)
        {
            Application.DTO.UnitCreationInfo unitCreationDTO = new(data.ModelName, data.Team, data.Position, data.BaseHealth, data.BaseComposure, data.BaseMovespeed, data.DefaultResources, data.DefaultAttributes, data.Skills,
                initialHealth: data.CurrentHealth, initialComposure: data.CurrentComposure);
            return _createUnitUseCase.Execute(unitCreationDTO);
        }

        public void CreateUnit(UnitId targetId, UnitCreationInfo data)
        {
            Application.DTO.UnitCreationInfo unitCreationDTO = new(data.ModelName, data.Team, data.Position, data.BaseHealth, data.BaseComposure, data.BaseMovespeed, data.DefaultResources, data.DefaultAttributes, data.Skills,
                initialHealth: data.CurrentHealth, initialComposure: data.CurrentComposure);
            _createUnitUseCase.Execute(targetId, unitCreationDTO);
        }

        public UnitId CreateCharacter(CharacterKey charcterKey, Team team, UnityEngine.Vector3 position, float? baseHealth = null,
            IReadOnlyCollection<int> skills = null, float? baseComposure = null)
        {
            SkillId[] skillIds = skills?.Select(value => new SkillId(value)).ToArray();

            return _createCharacterUseCase.Execute(charcterKey, team, position, baseHealth, skillIds, baseComposure);
        }
    }
}

using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.UseCases;

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
        public readonly Span<SkillId> Skills;

        public UnitCreationInfo(ModelName modelName, Team team, Vector3 position, float baseHealth, float currentHealth, Span<AttributeValue> defaultAttributes, Span<SkillId> skills)
        {
            ModelName = modelName;
            Position = position;
            Team = team;
            BaseHealth = baseHealth;
            CurrentHealth = currentHealth;
            DefaultAttributes = defaultAttributes;
            Skills = skills;
        }
    }

    public class CombatController
    {
        private readonly CreateCharacterUseCase _createUnitUseCase;

        public CombatController(CreateCharacterUseCase createUnitUseCase)
        {
            _createUnitUseCase = createUnitUseCase;
        }

        public EntityId CreateUnit(UnitCreationInfo data)
        {
            CreateCharacterDTO unitCreationDTO = new(data.ModelName, data.Team, data.Position, data.CurrentHealth, data.BaseHealth, data.DefaultAttributes, data.Skills);
            return _createUnitUseCase.Execute(unitCreationDTO);
        }

        public void CreateUnit(UnitCreationInfo data, Transform parent)
        {
            CreateCharacterDTO unitCreationDTO = new(data.ModelName, data.Team, data.Position, data.CurrentHealth, data.BaseHealth, data.DefaultAttributes, data.Skills);
            _createUnitUseCase.Execute(unitCreationDTO, parent);
        }
    }
}
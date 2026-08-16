using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases.Encounter
{
    public readonly ref struct CharacterDefinition
    {
        public readonly ModelName Model;
        public readonly float BaseHealth;
        public readonly AttributeValue[] BaseAttributes;
        public readonly CharacterResourceDefinition[] Resources;
        public readonly SkillId[] Skills;

        public CharacterDefinition(ModelName model, float baseHealth, AttributeValue[] baseAttributes, CharacterResourceDefinition[] resources, SkillId[] skills)
        {
            Model = model;
            BaseHealth = baseHealth;
            BaseAttributes = baseAttributes;
            Resources = resources;
            Skills = skills;
        }
    }

    public readonly struct CharacterResourceDefinition
    {
        public readonly ResourceId Type;
        public readonly float MaxValue;

        public CharacterResourceDefinition(ResourceId type, float maxValue)
        {
            Type = type;
            MaxValue = maxValue;
        }
    }

    public sealed class CreateCharacterUseCase
    {
        private readonly UnitCreateUseCase _unitCreateUseCase;
        private readonly ICharacterDefinitionProvider _characterDefinitionProvider;

        public CreateCharacterUseCase(UnitCreateUseCase unitCreateUseCase, ICharacterDefinitionProvider characterDefinitionProvider)
        {
            _unitCreateUseCase = unitCreateUseCase;
            _characterDefinitionProvider = characterDefinitionProvider;
        }

        public UnitId Execute(CharacterKey charcterKey, Team team, UnityEngine.Vector3 position, float? baseHealth = null)
        {
            CharacterDefinition characterDefinition = _characterDefinitionProvider.Get(charcterKey);
            Span<ResourceValue> resources = stackalloc ResourceValue[characterDefinition.Resources.Length];

            for (int i = 0; i < resources.Length; i++)
            {
                CharacterResourceDefinition resource = characterDefinition.Resources[i];
                resources[i] = new(resource.Type, resource.MaxValue, resource.MaxValue);
            }

            UnitCreationInfo unitInfo = new(
                    characterDefinition.Model,
                    team,
                    position,
                    -1,
                    baseHealth ?? characterDefinition.BaseHealth,
                    characterDefinition.BaseAttributes,
                    resources,
                    characterDefinition.Skills
                );

            return _unitCreateUseCase.Execute(unitInfo);
        }
    }
}

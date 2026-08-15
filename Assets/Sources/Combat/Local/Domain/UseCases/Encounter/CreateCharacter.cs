using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.UseCases.Encounter
{
    public readonly struct CharacterDefinition
    {
        public readonly ModelName Model;
        public readonly float BaseHealth;
        public readonly CharacterResourceDefinition[] Resources;
        public readonly SkillId[] Skills;

        public float GetBaseHealth()
        {
            return BaseHealth;
        }

        public ReadOnlySpan<AttributeValue> GetBaseAttributes()
        {
            return Span<AttributeValue>.Empty;
        }
    }

    public readonly struct CharacterResourceDefinition
    {
        public readonly ResourceId Type;
        public readonly float MaxValue;
    }

    public sealed class CreateCharacter
    {
        private readonly UnitCreateUseCase _unitCreateUseCase;
        private readonly ICharacterDefinitionProvider _characterDefinitionProvider;

        public void Execute(CharacterKey charcterId, Team team, UnityEngine.Vector3 position)
        {
            CharacterDefinition characterDefinition = _characterDefinitionProvider.Get(charcterId);
            Span<ResourceValue> resources = stackalloc ResourceValue[characterDefinition.Resources.Length];

            for (int i = 0; i < resources.Length; i++)
            {
                CharacterResourceDefinition resource = characterDefinition.Resources[i];
                resources[i] = new(resource.Type, resource.MaxValue, resource.MaxValue);
            }

            UnitCreationInfo characterDTO = new(
                    characterDefinition.Model,
                    team,
                    position,
                    0,
                    characterDefinition.GetBaseHealth(),
                    characterDefinition.GetBaseAttributes(),
                    resources,
                    characterDefinition.Skills
                );
        }
    }
}

using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Repositories;

using System;

namespace Combat.Local.Application.UseCases.Encounter
{
    public sealed class CreateCharacterUseCase
    {
        private readonly UnitCreateUseCase _unitCreateUseCase;
        private readonly ICharacterDefinitionProvider _characterDefinitionProvider;

        public CreateCharacterUseCase(UnitCreateUseCase unitCreateUseCase, ICharacterDefinitionProvider characterDefinitionProvider)
        {
            _unitCreateUseCase = unitCreateUseCase;
            _characterDefinitionProvider = characterDefinitionProvider;
        }

        public UnitId Execute(CharacterKey charcterKey, Team team, UnityEngine.Vector3 position, float? baseHealth = null, SkillId[] skills = null,
            float? baseComposure = null, float? movespeed = null)
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
                    baseHealth ?? characterDefinition.BaseHealth,
                    defaultComposure: baseComposure ?? characterDefinition.BaseComposure,
                    baseMovespeed: movespeed ?? characterDefinition.BaseMovespeed
,
                    defaultResources: resources,
                    defaultAttributes: characterDefinition.BaseAttributes,
                    skills: skills ?? characterDefinition.Skills,
                    initialHealth: -1);

            return _unitCreateUseCase.Execute(unitInfo);
        }
    }
}

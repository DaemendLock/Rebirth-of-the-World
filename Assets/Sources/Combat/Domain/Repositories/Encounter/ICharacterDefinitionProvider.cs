using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.Local.Application.Repositories
{
    public readonly ref struct CharacterDefinition
    {
        public readonly ModelName Model;
        public readonly float BaseHealth;
        public readonly float BaseComposure;
        public readonly float BaseMovespeed;
        public readonly AttributeValue[] BaseAttributes;
        public readonly CharacterResourceDefinition[] Resources;
        public readonly SkillId[] Skills;

        public CharacterDefinition(ModelName model, float baseHealth, AttributeValue[] baseAttributes, CharacterResourceDefinition[] resources, SkillId[] skills,
            float baseComposure, float baseMovespeed)
        {
            Model = model;
            BaseHealth = baseHealth;
            BaseComposure = baseComposure;
            BaseMovespeed = baseMovespeed;
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

    public interface ICharacterDefinitionProvider
    {
        CharacterDefinition Get(CharacterKey key);
    }
}

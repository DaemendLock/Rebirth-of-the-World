using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Gateways.Models
{
    public readonly struct CharacterDefinitionModel
    {
        public readonly ModelName Model;
        public readonly float BaseHealth;
        public readonly float BaseComposure;
        public readonly float BaseMovespeed;
        public readonly AttributeValue[] Attributes;
        public readonly ResourceValue[] Resources;
        public readonly SkillId[] Skills;

        public CharacterDefinitionModel(ModelName model, float baseHealth, float baseComposure, float baseMovespeed, AttributeValue[] attributes,
            ResourceValue[] resources, SkillId[] skills)
        {
            Model = model;
            BaseHealth = baseHealth;
            BaseComposure = baseComposure;
            BaseMovespeed = baseMovespeed;
            Attributes = attributes;
            Resources = resources;
            Skills = skills;
        }

        public CharacterDefinition Parse()
        {
            CharacterResourceDefinition[] resources = new CharacterResourceDefinition[Resources.Length];

            for (int i = 0; i < resources.Length; i++)
            {
                var item = Resources[i];
                resources[i] = new(item.ResourceId, item.MaxValue);
            }

            return new(Model, BaseHealth, Attributes, resources, Skills, BaseComposure, BaseMovespeed);
        }
    }
}

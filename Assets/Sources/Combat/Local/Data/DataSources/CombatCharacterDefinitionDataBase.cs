using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Databases;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Models;

using Data.Characters;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.DataSources
{
    public sealed class CombatCharacterDefinitionDataBase : ICombatCharacterDataBase
    {
        private readonly Dictionary<CharacterKey, CharacterDefinitionModel> _values;

        private readonly SkillDataBase _skillDataBase;

        public CombatCharacterDefinitionDataBase(SkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
            _values = new();

            foreach (var data in Resources.LoadAll<CombatCharacter>("Characters"))
            {
                Load(data);
            }
        }

        public void Load(CombatCharacter combatCharacter)
        {
            _values[combatCharacter.CharacterKey] = Adapt(combatCharacter);
        }

        public bool TryGet(CharacterKey key, out CharacterDefinitionModel result) => _values.TryGetValue(key, out result);

        private CharacterDefinitionModel Adapt(CombatCharacter data)
        {
            var attributesInfo = data.AttributeInfo;
            AttributeValue[] attributeValues = new AttributeValue[(int)UnitAttribute.PARRY + 1];

            foreach (var item in attributesInfo)
            {
                attributeValues[(int)item.Attribute] = new(item.Value, 100f);
            }

            CharacterResourceInfo[] resourceInfos = data.Resources;
            ResourceValue[] resources = new ResourceValue[resourceInfos.Length];

            for (int i = 0; i < resources.Length; i++)
            {
                var item = resourceInfos[i];
                resources[i] = new(item.ResourceId, item.MaxValue, item.MaxValue);
            }

            SkillId[] skills = new SkillId[data.Skills.Length];

            for (int i = 0; i < skills.Length; i++)
            {
                _skillDataBase.Load(data.Skills[i]);
                skills[i] = data.Skills[i].Id;
            }

            return new(data.ModelName, data.BaseHealth, attributeValues, resources, skills);
        }
    }
}

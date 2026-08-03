using Combat.API.Adapters;
using Combat.API.API.Skills;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Repositories.Skills;

using System.Runtime.Serialization;

namespace Combat.Local.Gateways.Factories
{

    public sealed class CustomScriptSkillStrategyFactory : ISkillStrategyFactory
    {
        private readonly ISkillDataBase _skillDataBase;
        private readonly CharacterApiAdapter _characterApiProvider;
        private readonly AbilityApiAdapter _skillApiAdapter;
        private readonly SceneApiAdapter _sceneApiAdapter;

        public CustomScriptSkillStrategyFactory(ISkillDataBase skillDataBase, CharacterApiAdapter characterApiProvider, AbilityApiAdapter skillApiProvider, SceneApiAdapter sceneApiProvider)
        {
            _skillDataBase = skillDataBase;
            _characterApiProvider = characterApiProvider;
            _skillApiAdapter = skillApiProvider;
            _sceneApiAdapter = sceneApiProvider;
        }

        public bool CanHandle(SkillId skillId) => _skillDataBase.GetScriptType(skillId) != null;

        public IAbilityPropertyContainer Create(UnitId? owner, SkillId skillType)
        {
            if (TryCreateEmpty(skillType, out SkillScript script) == false)
            {
                return null;
            }

            script.Init(_skillApiAdapter.Adaptee(new(owner, skillType)));
            return new ApiScriptDrivenAbilityPropertyContainer(skillType, script, _characterApiProvider, _skillApiAdapter, _sceneApiAdapter);
        }

        public bool TryCreateEmpty(SkillId id, out SkillScript value)
        {
            System.Type type = _skillDataBase.GetScriptType(id);

            if (_skillDataBase.Get(id) == null)
            {
                value = default;
                return false;
            }

            value = (SkillScript)FormatterServices.GetUninitializedObject(type);
            return true;
        }
    }
}

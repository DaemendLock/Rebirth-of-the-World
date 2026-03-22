using Combat.API.Adapters;
using Combat.API.Controllers.Misc;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Databases;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;

using System.Runtime.Serialization;

namespace Temp.Domain.Implementations
{
    public class CustomScriptSkillStrategyFactory : ISkillStrategyFactory
    {
        private readonly SkillDataBase _skillDataBase;
        private readonly CharacterApiAdapter _characterApiProvider;
        private readonly SkillApiAdapter _skillApiAdapter;
        private readonly SceneApiAdapter _sceneApiAdapter;

        public CustomScriptSkillStrategyFactory(SkillDataBase skillDataBase, CharacterApiAdapter characterApiProvider, SkillApiAdapter skillApiProvider, SceneApiAdapter sceneApiProvider)
        {
            _skillDataBase = skillDataBase;
            _characterApiProvider = characterApiProvider;
            _skillApiAdapter = skillApiProvider;
            _sceneApiAdapter = sceneApiProvider;
        }

        public bool CanHandle(SkillId skillId) => _skillDataBase.GetScriptType(skillId) != null;

        public ISkillStrategy Create(SkillId skillId, EntityId? owner)
        {
            if (TryCreateEmpty(skillId, out SkillScript script) == false)
            {
                return null;
            }

            var skillApi = _skillApiAdapter.Adaptee(skillId, owner);
            script.Init(skillApi);

            return new DataDrivenSkillStrategy(skillId, script, _characterApiProvider, _skillApiAdapter, _sceneApiAdapter);
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

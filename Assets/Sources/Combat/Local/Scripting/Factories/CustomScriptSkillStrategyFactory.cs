using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Runtime;

using System.Runtime.Serialization;

namespace Combat.Local.Scripting.Factories
{
    public sealed class CustomScriptSkillStrategyFactory : ISkillRuntimeFactory
    {
        private readonly ISkillScriptTypeProvider _skillDataBase;
        private readonly AbilityApiAdapter _skillApiAdapter;
        private readonly CharacterApiAdapter _characterApiAdapter;

        public CustomScriptSkillStrategyFactory(ISkillScriptTypeProvider skillDataBase, AbilityApiAdapter skillApiProvider, CharacterApiAdapter characterApiAdapter)
        {
            _skillDataBase = skillDataBase;
            _skillApiAdapter = skillApiProvider;
            _characterApiAdapter = characterApiAdapter;
        }

        public bool CanHandle(SkillId skillId) => _skillDataBase.GetScriptType(skillId) != null;

        public SkillRuntime Create(UnitId? owner, SkillId skillType)
        {
            if (TryCreateEmpty(skillType, out SkillScript script) == false)
            {
                return default;
            }

            script.Init(_skillApiAdapter.Adaptee(new(owner, skillType)));
            return new(null, new OldSkillCapabilityProvider(script, _characterApiAdapter));
        }

        private bool TryCreateEmpty(SkillId id, out SkillScript value)
        {
            System.Type type = _skillDataBase.GetScriptType(id);

            if (_skillDataBase.GetScriptType(id) == null)
            {
                value = default;
                return false;
            }

            value = (SkillScript)FormatterServices.GetUninitializedObject(type);
            return true;
        }
    }
}

using Combat.API;
using Combat.API.Adapters;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Idk;

using System.Runtime.Serialization;

namespace Combat.Local.Scripting.Factories
{
    public sealed class CustomScriptSkillStrategyFactory : ISkillPropertyContainerFactory
    {
        private readonly ISkillScriptTypeProvider _skillDataBase;
        private readonly AbilityApiAdapter _skillApiAdapter;

        public CustomScriptSkillStrategyFactory(ISkillScriptTypeProvider skillDataBase, AbilityApiAdapter skillApiProvider)
        {
            _skillDataBase = skillDataBase;
            _skillApiAdapter = skillApiProvider;
        }

        public bool CanHandle(SkillId skillId) => _skillDataBase.GetScriptType(skillId) != null;

        public IAbilityPropertyContainer Create(UnitId? owner, SkillId skillType)
        {
            if (TryCreateEmpty(skillType, out SkillScript script) == false)
            {
                return null;
            }

            script.Init(_skillApiAdapter.Adaptee(new(owner, skillType)));
            return new ApiScriptDrivenAbilityPropertyContainer(script);
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

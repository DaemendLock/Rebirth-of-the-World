using Combat.API;
using Combat.API.API.Skills;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Skill;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Idk
{
    public sealed class NewScriptAbilityPropertyContainer : IAbilityPropertyContainer
    {
        private readonly AbilityKey _id;
        private readonly ICastableSkill _castableSkill;

        public NewScriptAbilityPropertyContainer(AbilityKey id, ISkillScriptNew script, UnitNewAdapter unitNewAdapter, ISkillMemoryRepository skillMemoryRepository)
        {
            _id = id;
            UnitNew unitNew = null;

            if (id.Owner.HasValue )
            {
                unitNew = unitNewAdapter.Adaptee(id.Owner.Value) as UnitNew;
            }

            _castableSkill = new NewCast(id, skillMemoryRepository, script, unitNew);
        }

        public bool TryGet(out HandleSkillHitCapability result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out HandleSkillActionStateChangeCapability result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out EvaluateSkillTargetCapability result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out ExecuteSkillCapability result)
        {
            result = new(_castableSkill);
            return true;
        }

        private sealed class NewCast : ICastableSkill
        {
            private readonly AbilityKey _abilityKey;
            private readonly ISkillMemoryRepository _memoryRepository;
            private readonly ISkillScriptNew _skillScript;
            private readonly UnitNew _unitNew;

            public NewCast(AbilityKey abilityKey, ISkillMemoryRepository memoryRepository, ISkillScriptNew skillScript, UnitNew unitNew)
            {
                _abilityKey = abilityKey;
                _memoryRepository = memoryRepository;
                _skillScript = skillScript;
                _unitNew = unitNew;
            }

            public void OnCast()
            {
                DomainSkillContext context = new(_memoryRepository, _abilityKey);

                _skillScript.OnCast(_unitNew, context);
            }
        }
    }
}

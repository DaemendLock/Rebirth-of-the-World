using Combat.API.Adapters;
using Combat.API.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Repositories.Skill;

namespace Assets.Sources.Combat.Local.Gateways.Repositories.Skills
{
    public sealed class NewScriptAbilityPropertyContainer : IAbilityPropertyContainer
    {
        private readonly AbilityKey _id;

        private readonly NewScriptCastStrategy _newScriptCastStrategy;

        public NewScriptAbilityPropertyContainer(AbilityKey id, TestScript testScript, ISkillMemoryRepository skillMemoryRepository)
        {
            _id = id;
            _newScriptCastStrategy = new(testScript, skillMemoryRepository, id);
        }

        public void Give() { }

        public void Remove() { }

        public bool TryGet(out SkillCastEffect result)
        {
            result = new(_id.Skill, _newScriptCastStrategy);
            return true;
        }

        public bool TryGet(out ISkillHitStrategy result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out SkillActionStateChangeEffect result)
        {
            result = default;
            return false;
        }

        public bool TryGet(out LockTargetSkillEffect result)
        {
            result = default;
            return false;
        }

        private sealed class NewScriptCastStrategy : ISkillCastStrategy
        {
            private readonly TestScript _testScript;
            private readonly ISkillMemoryRepository _skillMemoryRepository;
            private readonly UnitNewAdapter _unitNewAdapter;
            private readonly AbilityKey _skillId;

            public NewScriptCastStrategy(TestScript testScript, ISkillMemoryRepository skillMemoryRepository, AbilityKey skillId)
            {
                _testScript = testScript;
                _skillMemoryRepository = skillMemoryRepository;
                _skillId = skillId;
            }

            public CastFailReason CanCast() => CastFailReason.Success;

            public void Execute()
            {
                _testScript.OnCast(null, new CoreUnitSkillContext(_skillMemoryRepository, _skillId));
            }
        }
    }
}

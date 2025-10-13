using Combat.API.Scripting;
using Combat.Common.Flags;
using Combat.Common.ValueObjects;

namespace Combat.API
{
    public sealed class SkillApi
    {
        private readonly Unit _owner;
        private readonly SkillId _skillId;
        private readonly SkillFlags _flags;
        private readonly SkillScript _skillScript;

        public SkillApi(SkillId skillId, SkillFlags flags, Unit owner, SceneApi scene, SkillScript skillScript)
        {
            _owner = owner;
            _skillId = skillId;
            _flags = flags;
            _skillScript = skillScript;

            if (skillScript != null)
            {
                _skillScript.Init(new(this, owner, scene));
            }
        }

        public EntityId? OwnerId => _owner?.Id;

        public Unit Owner => _owner;

        public SkillId SkillId => _skillId;

        public SkillFlags Flags => _flags;

        public bool HasFlag(SkillFlags skillFlags) => _flags.HasFlag(skillFlags);

        public bool TryGetProperty<T>(out T result) where T : ISkillProperty
        {
            if (_skillScript is not T property)
            {
                result = default;
                return false;
            }

            result = property;
            return true;
        }
    }
}

using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.API
{
    public sealed class SkillApi
    {
        private readonly Unit _owner;
        private readonly SkillFacade _skillFacede;

        public SkillApi(SkillId skillId, Unit owner, SkillFacade skillFacede, SceneApi scene)
        {
            _owner = owner;
            SkillId = skillId;

            Scene = scene;
            _skillFacede = skillFacede;
        }

        public EntityId? OwnerId => _owner?.Id;

        public Unit Owner => _owner;

        public SceneApi Scene { get; }

        public SkillId SkillId { get; }

        public bool HasFlag(SkillFlags skillFlags) => _skillFacede.GetFlags(SkillId, _owner?.Id).HasFlag(skillFlags);
    }
}

using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public readonly ref struct SkillCastInfo
    {
        public SkillCastInfo(SkillId skillId, EntityId? caster)
        {
            SkillId = skillId;
            Caster = caster;
        }

        public SkillId SkillId { get; }
        public EntityId? Caster { get; }
    }

    public class SkillCastHandler : ISkillCastEventHandler
    {
        public delegate void Handler(SkillCastInfo info);

        public event Handler SkillCasted;

        public void HandleEvent(EntityId? casterId, SkillId skillId)
        {
            SkillCasted?.Invoke(new(skillId, casterId));
        }
    }
}

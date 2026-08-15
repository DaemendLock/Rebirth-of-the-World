using Combat.API;
using Combat.API.Contexts;
using Combat.Common.Flags;
using Combat.Common.Primitives;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class DomainAbilityContext : IAbilityContext
    {
        private readonly SkillFlags _flags;

        public DomainAbilityContext(SkillId id, Unit owner, SkillFlags flags, EncounterApi scene)
        {
            SkillId = id;
            Owner = owner;
            _flags = flags;
            Scene = scene;
        }

        public SkillId SkillId { get; }

        public Unit Owner { get; }

        public EncounterApi Scene { get; }

        public bool HasFlag(SkillFlags skillFlags) => _flags.HasFlag(skillFlags);
    }
}

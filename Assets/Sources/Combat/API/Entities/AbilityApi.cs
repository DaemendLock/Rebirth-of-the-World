using Combat.API.Contexts;
using Combat.Common.Flags;
using Combat.Common.Primitives;

namespace Combat.API
{
    public sealed class AbilityApi
    {
        private readonly IAbilityContext _context;

        public AbilityApi(IAbilityContext context)
        {
            _context = context;
        }

        public AbilityKey AbilityKey => new(Owner?.Id, SkillId);

        public Unit Owner => _context.Owner;

        public EncounterApi Scene => _context.Scene;

        public SkillId SkillId => _context.SkillId;

        public bool HasFlag(SkillFlags skillFlags) => _context.HasFlag(skillFlags);
    }
}

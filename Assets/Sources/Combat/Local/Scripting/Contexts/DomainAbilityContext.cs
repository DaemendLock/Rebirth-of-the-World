using Combat.API;
using Combat.API.Contexts;
using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class DomainAbilityContext : IAbilityContext
    {
        private readonly AbilityFacade _skillFacade;

        public DomainAbilityContext(SkillId id, Unit owner, AbilityFacade skillFacede, EncounterApi scene)
        {
            _skillFacade = skillFacede;
            SkillId = id;

            Owner = owner;
            Scene = scene;
        }

        public SkillId SkillId { get; }

        public Unit Owner { get; }

        public EncounterApi Scene { get; }

        public bool HasFlag(SkillFlags skillFlags) => _skillFacade.GetFlags(Owner.Id, SkillId).HasFlag(skillFlags);
    }
}

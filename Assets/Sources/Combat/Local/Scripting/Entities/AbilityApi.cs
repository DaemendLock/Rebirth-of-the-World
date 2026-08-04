using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.API
{
    public sealed class AbilityApi : IAbilityApi
    {
        private readonly AbilityFacade _skillFacade;

        public AbilityApi(SkillId id, IUnit owner, AbilityFacade skillFacede, IEncounterApi scene)
        {
            _skillFacade = skillFacede;
            SkillId = id;

            Owner = owner;
            Scene = scene;
        }

        public SkillId SkillId { get; }

        public IUnit Owner { get; }

        public IEncounterApi Scene { get; }

        public AbilityKey AbilityKey => new(Owner?.Id, SkillId);

        public bool HasFlag(SkillFlags skillFlags) => _skillFacade.GetFlags(Owner.Id, SkillId).HasFlag(skillFlags);
    }
}

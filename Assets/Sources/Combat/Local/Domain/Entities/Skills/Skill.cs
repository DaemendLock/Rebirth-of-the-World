using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Skills.Effects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public interface ISkillStrategy
    {
        void Give(EntityId owner);
        void Remove(EntityId owner);
        bool TryGetEffect(out SkillCastEffect result);
        bool TryGetEffect(out SkillHitEffect result);
        bool TryGetEffect(out SkillActionStateChangeEffect result);
    }

    public readonly ref struct Skill
    {
        private readonly ISkillStrategy _skillStrategy;

        public Skill(SkillId id, SkillFlags flags, EntityId? owner, IReadOnlyCollection<ActionId> actions, ISkillStrategy skillStrategy)
        {
            Id = id;
            Flags = flags;
            Owner = owner;
            Actions = actions;
            _skillStrategy = skillStrategy;
        }

        public SkillId Id { get; }

        public SkillFlags Flags { get; }

        public EntityId? Owner { get; }

        public ISkillStrategy Strategy => _skillStrategy;

        public IReadOnlyCollection<ActionId> Actions { get; }

        public bool AllowMoment => Flags.HasFlag(SkillFlags.DontRestrictMovement);

        public bool TryGetEffect(out SkillCastEffect result) => _skillStrategy.TryGetEffect(out result);

        public bool TryGetEffect(out SkillHitEffect result) => _skillStrategy.TryGetEffect(out result);

        public bool TryGetEffect(out SkillActionStateChangeEffect result) => _skillStrategy.TryGetEffect(out result);
    }
}

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Entities.Skills.Effects
{
    public interface ISkillHitStrategy
    {
        void HandleHit(Hitbox hitbox, Hurtbox hurtbox, UnityEngine.Vector3 location);
    }

    public readonly ref struct SkillHitEffect
    {
        private readonly ISkillHitStrategy _strategy;

        public SkillHitEffect(SkillId skill, ISkillHitStrategy strategy)
        {
            Skill = skill;
            _strategy = strategy;
        }

        public SkillId Skill { get; }

        public void HandleHit(Hitbox hitbox, Hurtbox hurtbox, UnityEngine.Vector3 location) => _strategy.HandleHit(hitbox, hurtbox, location);
    }
}

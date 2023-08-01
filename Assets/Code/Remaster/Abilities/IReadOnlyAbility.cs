using Remaster.Utils;

namespace Remaster.Abilities
{
    public interface IReadOnlyAbility
    {
        public Spell Spell { get; }
        public Duration Cooldown { get; }
        public float CooldownTime { get; }
        public bool OnCooldown { get; }
        public SchoolType Type { get; }
    }
}

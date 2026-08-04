using Combat.Common.ValueObjects;

namespace Combat.API.Skills
{
    public readonly ref struct CastEvent
    {
        public CastEvent(Unit caster, AbilityApi skill, EncounterApi scene)
        {
            Caster = caster;
            Skill = skill;
            Scene = scene;
        }

        public Unit Caster { get; }
        public AbilityApi Skill { get; }
        public EncounterApi Scene { get; }
    }

    public interface ICastableSkill : ISkillProperty
    {
        /// <summary>
        /// Called when casting skill to determine if cast is possible.
        /// </summary>
        /// <returns><see cref="CastFailReason.Success"/> if cast is possible.</returns>
        CastFailReason CanCast() => CastFailReason.Success;
        void OnCast() { }
    }
}

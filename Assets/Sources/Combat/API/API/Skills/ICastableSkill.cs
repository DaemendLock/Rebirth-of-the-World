using Combat.Common.ValueObjects;

namespace Combat.API.Skills
{
    public readonly ref struct CastEvent
    {
        public CastEvent(Unit caster, SkillApi skill, SceneApi scene)
        {
            Caster = caster;
            Skill = skill;
            Scene = scene;
        }

        public Unit Caster { get; }
        public SkillApi Skill { get; }
        public SceneApi Scene { get; }
    }

    public interface ICastableSkill : ISkillProperty
    {
        /// <summary>
        /// Called when casting skill to determine if cast is possible.
        /// </summary>
        /// <returns><see cref="CastFailReason.Success"/> if cast is possible.</returns>
        CastFailReason CanCast(CastEvent @event) => CastFailReason.Success;
        void OnCast(CastEvent @event) { }
    }
}

using Combat.Common.ValueObjects;

namespace Combat.API.Skills
{
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

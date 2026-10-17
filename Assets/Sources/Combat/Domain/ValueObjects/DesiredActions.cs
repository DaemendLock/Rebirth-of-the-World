using Combat.Common.Primitives;

using System.Numerics;


namespace Combat.Local.Application.ValueObjects
{
    public readonly struct DesiredActions
    {
        public DesiredActions(SkillId? skill, Vector2 movementDirection)
        {
            Skill = skill;
            MovementDirection = movementDirection;
        }

        public SkillId? Skill { get; }
        public Vector2 MovementDirection { get; }
    }
}

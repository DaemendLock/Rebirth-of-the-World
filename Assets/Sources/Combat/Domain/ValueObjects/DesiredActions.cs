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

    public interface IInputCommand
    {
        UnitId Target { get; }
    }

    public readonly struct CastCommand : IInputCommand
    {
        public readonly int SkillSlot;

        public CastCommand(UnitId target, int skillSlot)
        {
            Target = target;
            SkillSlot = skillSlot;
        }

        public readonly UnitId Target { get; }
    }

    public readonly struct ReleaseCommand : IInputCommand
    {
        public readonly int SkillSlot;

        public ReleaseCommand(UnitId target, int skillSlot)
        {
            SkillSlot = skillSlot;
            Target = target;
        }

        public readonly UnitId Target { get; }
    }
}

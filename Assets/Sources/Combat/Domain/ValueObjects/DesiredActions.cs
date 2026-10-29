using Combat.Common.Primitives;

using System.Numerics;

namespace Combat.Local.Application.ValueObjects
{
    public readonly struct DesiredActions
    {
        public DesiredActions(Vector2 movementDirection)
        {
            MovementDirection = movementDirection;
        }
        public Vector2 MovementDirection { get; }
    }

    public interface IInputCommand
    {
        UnitId Target { get; }
    }

    public readonly struct CastSkillCommand : IInputCommand
    {
        public readonly int SkillSlot;

        public CastSkillCommand(UnitId target, int skillSlot)
        {
            Target = target;
            SkillSlot = skillSlot;
        }

        public readonly UnitId Target { get; }
    }

    public readonly struct ReleaseSkillCommand : IInputCommand
    {
        public readonly int SkillSlot;

        public ReleaseSkillCommand(UnitId target, int skillSlot)
        {
            SkillSlot = skillSlot;
            Target = target;
        }

        public readonly UnitId Target { get; }
    }
}

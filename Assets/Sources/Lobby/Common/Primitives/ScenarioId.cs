using System;

namespace Lobby.Common.Primitives
{
    public readonly struct ScenarioId : IEquatable<ScenarioId>
    {
        public readonly Guid Value;

        public ScenarioId(Guid value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is ScenarioId id && Equals(id);
        public bool Equals(ScenarioId other) => Value.Equals(other.Value);
        public override int GetHashCode() => HashCode.Combine(Value);

        public static bool operator ==(ScenarioId left, ScenarioId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ScenarioId left, ScenarioId right)
        {
            return !(left == right);
        }

        public override string ToString() => Value.ToString();
    }
}
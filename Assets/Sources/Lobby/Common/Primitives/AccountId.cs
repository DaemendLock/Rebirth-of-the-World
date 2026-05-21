using System;

namespace Lobby.Common.Primitives
{
    public readonly struct GroupId
    {
        public readonly Guid Value;
    }

    public readonly struct AccountId : IEquatable<AccountId>
    {
        public readonly long Value;

        public AccountId(long value)
        {
            Value = value;
        }

        public override bool Equals(object obj) => obj is AccountId id && Equals(id);
        public bool Equals(AccountId other) => Value.Equals(other.Value);
        public override int GetHashCode() => HashCode.Combine(Value);

        public static bool operator ==(AccountId left, AccountId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AccountId left, AccountId right)
        {
            return !(left == right);
        }

        public override string ToString() => Value.ToString();
    }
}
using System;

using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.OldAttributes
{
    public interface IAttributeCollection<T> where T : struct, Enum
    {
        void Add(IAttributeCollection<T> attribute);

        AttributeValue this[T attribute] { get; set; }
    }
}

using System;

namespace Server.Combat.Domain.Attributes
{

    public interface IAttributeCollection<T> where T : struct, Enum
    {
        void Add(IAttributeCollection<T> attribute);

        AttributeValue this[T attribute] { get; set; }
    }
}

using System;

namespace Client.Combat.Domain.Attributes
{

    public interface IAttributeCollection<T> where T : struct, Enum
    {
        void Add(IAttributeCollection<T> attribute);

        AttributeValue this[T attribute] { get; set; }
    }
}

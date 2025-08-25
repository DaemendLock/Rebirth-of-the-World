using System;
using System.Runtime.Serialization;

using Utils.ByteHelper;

namespace Utils.Serializer
{
    public static class Serializer
    {
        public static object Deserialize(Type type, ByteReader source)
        {
            return Activator.CreateInstance(type, source) ?? throw new SerializationException(type.Name + " deserializtion constructor is not defined.");
        }
    }
}

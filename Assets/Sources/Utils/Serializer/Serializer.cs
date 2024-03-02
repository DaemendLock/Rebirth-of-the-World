using System;
using System.IO;
using System.Runtime.Serialization;
using Utils.Interfaces;

namespace Utils.Serializer
{
    public static class Serializer
    {
        public static void SerializeEffect(SerializableInterface effect, BinaryWriter destination)
        {
            effect.Serialize(destination);
        }

        public static object Deserialize(Type type, BinaryReader source)
        {
            return Activator.CreateInstance(type, source) ?? throw new SerializationException(type.Name + " deserializtion constructor is not defined.");
        }

        public static void SerializeStruct<T>(T @struct, BinaryWriter destination) where T : struct
        {

        }
    }
}

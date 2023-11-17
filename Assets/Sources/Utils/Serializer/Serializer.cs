using System;
using System.IO;
using System.Runtime.Serialization;
using Utils.Interfaces;

namespace Utils.Serializer
{
    public static class Serializer
    {
        public static void SerializeEffect<T>(T effect, BinaryWriter destination) where T : SerializableInterface
        {
            effect.Serialize(destination);
        }

        public static T Deserialize<T>(BinaryReader source) where T : SerializableInterface
        {
            string typeName  = source.ReadString();
            Type type = Type.GetType(typeName) ?? throw new SerializationException("Can't read type. "+ typeName);

            return (T) Activator.CreateInstance(type, source) ?? throw new SerializationException(type.Name + " deserializtion constructor is not defined.");
        }

        public static void SerializeStruct<T>(T @struct, BinaryWriter destination) where T : struct
        {
            
        }
    }
}

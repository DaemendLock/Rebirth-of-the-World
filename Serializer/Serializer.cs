﻿using System;
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
            Type type = Type.GetType(source.ReadString()) ?? throw new SerializationException("Can't read type.");
            
            if(type == null)
            {
                throw new SerializationException(type.Name + " deserializtion constructor is not defined.");
            }

            return (T) Activator.CreateInstance(type, source);
        }
    }
}

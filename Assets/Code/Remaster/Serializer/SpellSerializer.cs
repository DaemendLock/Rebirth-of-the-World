using Remaster.AuraEffects;
using Remaster.SpellEffects;
using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization;
using Remaster.Interfaces;

namespace Remaster.Data.Serializer
{
    public static class SpellSerializer
    {
        public static byte[] Serialize(SpellData data)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (BinaryWriter buffer = new BinaryWriter(memoryStream))
            {
                buffer.Write(data.Id);
                buffer.Write(data.Cost.Left);
                buffer.Write(data.Cost.Right);
                buffer.Write((byte) data.TargetTeam);
                buffer.Write(data.Range);
                buffer.Write(data.CastTime);
                buffer.Write(data.Cooldown);
                buffer.Write(data.GCD);
                buffer.Write((byte) data.GcdCategory);
                buffer.Write(data.Duration);
                buffer.Write((ushort) data.School);
                buffer.Write((int) data.Mechanic);
                buffer.Write((int) data.DispellType);
                buffer.Write((long) data.Flags);
                SerializeSpellEffects(buffer, data.Effects);
                buffer.Write(data.Script);
            }

            byte[] result = memoryStream.ToArray();
            memoryStream.Dispose();

            return result;
        }

        public static SpellData Deserialize(byte[] data)
        {
            int id;
            float leftCost;
            float rightCost;
            TargetTeam team;
            float range;
            float castTime;
            float cooldown;
            float gcd;
            GcdCategory gcdCategory;
            float duration;
            SchoolType school;
            Mechanic mechanic;
            DispellType dispellType;
            SpellFlags flags;
            SpellEffect[] effects;
            Type script;

            MemoryStream memoryStream = new MemoryStream(data);

            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
            {
                id = binaryReader.ReadInt32();
                leftCost = binaryReader.ReadSingle();
                rightCost = binaryReader.ReadSingle();
                team = (TargetTeam) binaryReader.ReadByte();
                range = binaryReader.ReadSingle();
                castTime = binaryReader.ReadSingle();
                cooldown = binaryReader.ReadSingle();
                gcd = binaryReader.ReadSingle();
                gcdCategory = (GcdCategory) binaryReader.ReadByte();
                duration = binaryReader.ReadSingle();
                school = (SchoolType) binaryReader.ReadUInt16();
                mechanic = (Mechanic) binaryReader.ReadInt32();
                dispellType = (DispellType) binaryReader.ReadInt32();
                flags = (SpellFlags) binaryReader.ReadInt64();
                effects = DeserializeSpellEffects(binaryReader);
                script = Type.GetType(binaryReader.ReadString()) ?? typeof(Spell);
            }

            return new SpellData(id, "", new AbilityCost(leftCost, rightCost), team, range, castTime, cooldown, gcd, gcdCategory, duration, dispellType, school, mechanic, effects, flags, script);
        }

        private static void SerializeSpellEffects(BinaryWriter buffer, SpellEffect[] effects)
        {
            buffer.Write(effects.Length);

            foreach (SpellEffect effect in effects)
            {
                Serializer.SerializeEffect(effect, buffer);
            }
        }

        private static SpellEffect[] DeserializeSpellEffects(BinaryReader readed)
        {
            SpellEffect[] spellEffects = new SpellEffect[readed.ReadInt32()];

            Debug.Log(spellEffects.Length);

            for (int i = 0; i < spellEffects.Length; i++)
            {
                spellEffects[i] = Serializer.Deserialize<SpellEffect>(readed);
            }

            return spellEffects;
        }
    }

    public static class Serializer
    {
        public static void SerializeEffect<T>(T effect, BinaryWriter destination) where T : SerializableInterface
        {
            effect.Serialize(destination);
        }

        public static T Deserialize<T>(BinaryReader source) where T : SerializableInterface
        {
            Type type = Type.GetType(source.ReadString()) ?? throw new SerializationException("Can't read type.");
            return (T) Activator.CreateInstance(type, source) ?? throw new SerializationException(type.Name + " deserializtion constructor is not defined.");
        }
    }
}

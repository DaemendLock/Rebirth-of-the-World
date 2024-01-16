using Core.Combat.Abilities;
using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Statuses.Auras;
using Core.Combat.Utils.ValueSources;
using System;
using System.IO;
using System.Runtime.Serialization;
using Utils.DataTypes;

namespace Core.Combat.Utils.Serialization
{
    internal enum AuraEffectType
    {
        IMMMUNITY,
        MODIFY_COOLDOWN,
        MODIFY_HEALTH_REGEN,
        MODIFY_STAT,
        PERIODICALLY_TRIGGER_SPELL,
        PROC_TRIGGER_SPELL,
        REACTION_CAST,
        MODIFY_SPELL_EFFECT
    }

    internal enum AuraEffectValue
    {
        CONSTANT,
        STAT_PROVIDER,
        VALUE_SUM,
        VALUE_MULTIPLY
    }

    public class AuraSerializer
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
                buffer.Write((ushort) data.School);
                buffer.Write((int) data.Mechanic);
                buffer.Write((long) data.Flags);
                buffer.Write((byte) data.Script);

                //SerializeSpellEffects(buffer, data.Effects);
            }

            byte[] result = memoryStream.ToArray();
            memoryStream.Dispose();

            return result;
        }

        public static Aura Deserialize(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream(data);

            using (BinaryReader binaryReader = new BinaryReader(memoryStream))
            {
                //id = binaryReader.ReadInt32();
                //leftCost = binaryReader.ReadSingle();
                //rightCost = binaryReader.ReadSingle();
                //team = (TargetTeam) binaryReader.ReadByte();
                //range = binaryReader.ReadSingle();
                //castTime = binaryReader.ReadSingle();
                //cooldown = binaryReader.ReadSingle();
                //gcd = binaryReader.ReadSingle();
                //gcdCategory = (GcdCategory) binaryReader.ReadByte();
                //duration = binaryReader.ReadSingle();
                //school = (SchoolType) binaryReader.ReadUInt16();
                //mechanic = (Mechanic) binaryReader.ReadInt32();
                //dispellType = (DispellType) binaryReader.ReadInt32();
                //flags = (SpellFlags) binaryReader.ReadInt64();
                //script = (SpellType) binaryReader.ReadByte();
                //effects = DeserializeSpellEffects(binaryReader);
            }
            memoryStream.Dispose();
            //return new SpellData(id, new AbilityCost(leftCost, rightCost), team, range, castTime, cooldown, gcd, gcdCategory, school, mechanic, effects, flags, script);
            throw new NotImplementedException();
        }

        internal static AuraEffect DeserializeAuraEffect(BinaryReader readed) => (AuraEffectType) readed.ReadByte() switch
        {
            AuraEffectType.IMMMUNITY => new Immunity(readed),
            AuraEffectType.MODIFY_COOLDOWN => new ModCooldown(readed),
            AuraEffectType.MODIFY_HEALTH_REGEN => new ModHealthRegen(readed),
            AuraEffectType.MODIFY_STAT => new ModStat(readed),
            AuraEffectType.PERIODICALLY_TRIGGER_SPELL => new PeriodicallyTriggerSpell(readed),
            AuraEffectType.PROC_TRIGGER_SPELL => new ProcTriggerSpell(readed),
            AuraEffectType.REACTION_CAST => new ReactionCast(readed),
            AuraEffectType.MODIFY_SPELL_EFFECT => new ModifySpellEffect(readed),
            _ => throw new SerializationException("Unknown aura effect type."),
        };

        internal static AuraValueSource DeserializeAuraValue(BinaryReader readed) => (AuraEffectValue) readed.ReadByte() switch
        {
            AuraEffectValue.CONSTANT => new Constant(readed),
            AuraEffectValue.STAT_PROVIDER => new StatProvider(readed),
            AuraEffectValue.VALUE_SUM => new ValueSum(readed),
            AuraEffectValue.VALUE_MULTIPLY => new ValueMultiplication(readed),
            _ => throw new SerializationException("Unknown aura value type."),
        };
    }
}

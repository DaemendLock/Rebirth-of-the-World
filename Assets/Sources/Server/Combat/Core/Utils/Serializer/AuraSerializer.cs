using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Statuses.Auras;
using Core.Combat.Statuses.Auras.AuraEffects;
using Core.Combat.Utils.ValueSources;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace Core.Combat.Utils.Serialization
{
    internal enum AuraEffectType
    {
        Immunity,
        ModifyCooldown,
        ModifyHealthRegen,
        ModifyStat,
        PERIODICALLY_TRIGGER_SPELL,
        PROC_TRIGGER_SPELL,
        OnEventCast,
        ModifySpellEffect
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
        public static byte[] Serialize(AuraData data)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (BinaryWriter buffer = new BinaryWriter(memoryStream))
            {

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
            AuraEffectType.Immunity => new Immunity(readed),
            AuraEffectType.ModifyCooldown => new ModCooldown(readed),
            //AuraEffectType.ModifyHealthRegen => new ModHealthRegen(readed),
            AuraEffectType.ModifyStat => new ModStat(readed),
            //AuraEffectType.PERIODICALLY_TRIGGER_SPELL => new PeriodicallyTriggerSpell(readed),
            //AuraEffectType.PROC_TRIGGER_SPELL => new ProcTriggerSpell(readed),
            //AuraEffectType.OnEventCast => new ReactionCast(readed),
            AuraEffectType.ModifySpellEffect => new ModifySpellEffect(readed),
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

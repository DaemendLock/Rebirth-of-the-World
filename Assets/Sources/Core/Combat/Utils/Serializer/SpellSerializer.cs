using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Statuses.AuraEffects;
using Core.Combat.Utils.ValueSources;
using System;
using System.IO;
using System.Runtime.Serialization;
using Utils.DataTypes;

namespace Utils.Serializer
{
    internal enum SpellEffectType : byte
    {
        DUMMY,
        ABSORB_DAMAGE,
        APPLY_AURA,
        GIVE_RESOURCE,
        HEAL,
        INTERRUPT,
        REDUCE_COOLDOWN,
        RESURRECT,
        SCHOOL_DAMAGE,
        SUMMON,
        TAUNT,
        TELEPORT,
        TRIGGER_SPELL,
        CONSUME_MARK,
    }

    internal enum SpellEffectValue
    {
        FIXED_VALUE,
        CASTER_STAT,
        CASTER_RESOURCE,
        MULTIPLY,
    }

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
                buffer.Write(data.Script);

                SerializeSpellEffects(buffer, data.Effects);
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
            Type script;
            SpellEffect[] effects;

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
                script = Type.GetType(binaryReader.ReadString()) ?? typeof(Spell);
                effects = DeserializeSpellEffects(binaryReader);
            }

            memoryStream.Dispose();

            return new SpellData(id, new AbilityCost(leftCost, rightCost), team, range, castTime, cooldown, gcd, gcdCategory, duration, dispellType, school, mechanic, effects, flags, script);
        }

        internal static Spell FromSpellData(SpellData data)
        {
            Type type = Type.GetType(data.Script) ?? throw new SerializationException("Can't read type.");
            return (Spell) Activator.CreateInstance(type, data) ?? throw new SerializationException("Unknow error occured...");
        }

        internal static SpellValueSource DeserializeSpellValue(BinaryReader readed) => (SpellEffectValue) readed.ReadByte() switch
        {
            SpellEffectValue.FIXED_VALUE => new FixedValue(readed),
            SpellEffectValue.CASTER_STAT => new StatValue(readed),
            SpellEffectValue.CASTER_RESOURCE => new CasterResourceValue(readed),
            SpellEffectValue.MULTIPLY => new MultiplyValue(readed),
            _ => throw new SerializationException("Unknown spell value type."),
        };

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
            _ => throw new SerializationException("Unknown spell value type."),
        };

        internal static Core.Combat.Utils.ValueSources.AuraValueSource DeserializeAuraValue(BinaryReader readed) => (AuraEffectValue) readed.ReadByte() switch
        {
            AuraEffectValue.CONSTANT => new Constant(readed),
            AuraEffectValue.STAT_PROVIDER => new StatProvider(readed),
            AuraEffectValue.VALUE_SUM => new ValueSum(readed),
            AuraEffectValue.VALUE_MULTIPLY => new ValueMultiplication(readed),
            _ => throw new SerializationException("Unknown spell value type."),
        };

        private static void SerializeSpellEffects(BinaryWriter buffer, SpellEffect[] effects)
        {
            buffer.Write(effects.Length);

            foreach (SpellEffect effect in effects)
            {
                effect.Serialize(buffer);
            }
        }

        private static SpellEffect[] DeserializeSpellEffects(BinaryReader readed)
        {
            SpellEffect[] spellEffects = new SpellEffect[readed.ReadInt32()];

            for (int i = 0; i < spellEffects.Length; i++)
            {
                spellEffects[i] = DeserilizeSpellEffect(readed);
            }

            return spellEffects;
        }

        public static SpellEffect DeserilizeSpellEffect(BinaryReader readed) => (SpellEffectType) readed.ReadByte() switch
        {
            SpellEffectType.DUMMY => new Dummy(readed),
            SpellEffectType.ABSORB_DAMAGE => new AbsorbDamage(readed),
            SpellEffectType.APPLY_AURA => new ApplyAura(readed),
            SpellEffectType.GIVE_RESOURCE => new GiveResource(readed),
            SpellEffectType.HEAL => new Heal(readed),
            SpellEffectType.INTERRUPT => new Interrupt(readed),
            SpellEffectType.REDUCE_COOLDOWN => new ReduceCooldown(readed),
            SpellEffectType.RESURRECT => new Resurrect(readed),
            SpellEffectType.SCHOOL_DAMAGE => new SchoolDamage(readed),
            SpellEffectType.SUMMON => new Summon(readed),
            SpellEffectType.TAUNT => new Taunt(readed),
            SpellEffectType.TELEPORT => new Teleport(readed),
            SpellEffectType.TRIGGER_SPELL => new TriggerSpell(readed),
            _ => throw new SerializationException("Unknown spell value type."),
        };
    }
}

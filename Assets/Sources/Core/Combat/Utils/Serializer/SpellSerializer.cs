using Core.Combat.Abilities;
using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Abilities.SpellScripts;
using System.IO;
using System.Runtime.Serialization;
using Utils.DataTypes;

namespace Core.Combat.Utils.Serialization
{
    public enum SpellType : byte
    {
        Default,
        Aoe,
        Cleave,
        Splash,
        Selfcast
    }

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
                buffer.Write((ushort) data.School);
                buffer.Write((int) data.Mechanic);
                buffer.Write((long) data.Flags);
                buffer.Write((byte) data.Script);

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

            SchoolType school;
            Mechanic mechanic;
            SpellFlags flags;
            SpellType script;
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
                school = (SchoolType) binaryReader.ReadUInt16();
                mechanic = (Mechanic) binaryReader.ReadInt32();
                flags = (SpellFlags) binaryReader.ReadInt64();
                script = (SpellType) binaryReader.ReadByte();
                effects = DeserializeSpellEffects(binaryReader);
            }

            memoryStream.Dispose();

            return new SpellData(id, new AbilityCost(leftCost, rightCost), team, range, castTime, cooldown, gcd, gcdCategory, school, mechanic, effects, flags, script);
        }

        internal static Spell FromSpellData(SpellData data) => data.Script switch
        {
            SpellType.Default => new Spell(data),
            SpellType.Splash => new SplashSpell(data),
            SpellType.Aoe => new AoeSpell(data),
            SpellType.Selfcast => new SelfcastSpell(data),
            SpellType.Cleave => new CleaveSpell(data),
            _ => throw new SerializationException("Unknow spell type."),
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

        internal static SpellValueSource DeserializeSpellValue(BinaryReader readed) => (SpellEffectValue) readed.ReadByte() switch
        {
            SpellEffectValue.FIXED_VALUE => new FixedValue(readed),
            SpellEffectValue.CASTER_STAT => new StatValue(readed),
            SpellEffectValue.CASTER_RESOURCE => new CasterResourceValue(readed),
            SpellEffectValue.MULTIPLY => new MultiplyValue(readed),
            _ => throw new SerializationException("Unknown spell value type."),
        };

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

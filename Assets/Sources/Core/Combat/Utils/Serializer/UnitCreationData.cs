using Core.Combat.Abilities;
using Core.Combat.Stats;
using Core.Combat.Units;
using Core.Combat.Units.Components;
using System;
using System.Runtime.CompilerServices;
using Utils.DataTypes;

namespace Core.Combat.Utils
{
    public readonly struct UnitCreationData
    {
        public readonly int Id;
        public readonly Spellcasting Spellcasting;
        public readonly UnitState UnitState;
        //TODO: Model
        //TODO: Gear 

        public UnitCreationData(Spellcasting spellcasting, UnitState state) : this()
        {
            Spellcasting = spellcasting;
            UnitState = state;
            Id = state.EntityId;
        }

        public static UnitCreationData Parse(byte[] data, ref int start)
        {
            Spellcasting spellcasting = ParseSpellCast(data, ref start);
            UnitState state = ParseUnitState(data, ref start);
            return new(spellcasting, state);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Spellcasting ParseSpellCast(byte[] source, ref int index)
        {
            Spellcasting result = new();

            int spellCount = source[index++];

            Spell spell;

            for (int i = 0; i < spellCount; i++)
            {
                spell = SpellLibrary.SpellLib.GetSpell((SpellId) BitConverter.ToInt32(source, index));
                index += sizeof(int);

                if (spell == null)
                {
                    continue;
                }

                result.GiveAbility(spell);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static UnitState ParseUnitState(byte[] source, ref int index)
        {
            StatsTable stats = ParseStatsTable(source, ref index);
            CastResources resources = ParseCastResources(source, ref index);
            Position position = ParsePosition(source, ref index);
            Team.Team team = (Team.Team) source[index++];
            int id = BitConverter.ToInt32(source, index);
            index += sizeof(int);
            UnitState result = new(stats, resources, position, team, id);
            
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StatsTable ParseStatsTable(byte[] source, ref int index)
        {
            StatsTable result = StatsTable.UNIT_DEFAULT;

            for (int i = 0; i < StatsTable.STATS_COUNT; i++)
            {
                float value = BitConverter.ToSingle(source, index);
                float percent = BitConverter.ToSingle(source, index + sizeof(float));

                result[i] = new PercentModifiedValue(value, percent);
                index += sizeof(float) * 2;
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CastResources ParseCastResources(byte[] source, ref int index)
        {
            float leftResource = BitConverter.ToSingle(source, index);
            float rightResource = BitConverter.ToSingle(source, index + sizeof(float));
            index += sizeof(float) * 2;

            ResourceType leftType = (ResourceType) BitConverter.ToUInt16(source, index);
            ResourceType rightType = (ResourceType) BitConverter.ToUInt16(source, index + sizeof(ushort));
            index += sizeof(ushort)*2;

            return new(leftResource, rightResource, leftType, rightType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Position ParsePosition(byte[] source, ref int index)
        {
            float positionX = BitConverter.ToSingle(source, index);
            float positionY = BitConverter.ToSingle(source, index += sizeof(float));
            float positionZ = BitConverter.ToSingle(source, index += sizeof(float));

            float rotation = BitConverter.ToSingle(source, index += sizeof(float));

            index += sizeof(float);

            return new Position() { Location = new(positionX, positionY, positionZ), Rotation = rotation};
        }
    }
}


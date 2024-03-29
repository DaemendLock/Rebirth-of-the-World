﻿using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utils.DataStructure;

namespace Utils.DataTypes
{
    public class UnitCreationData
    {
        public readonly int Id;
        public readonly ModelData Model;
        public readonly ViewData Veiw;

        public readonly byte ControlGroup = 0;
        //TODO: Model
        //TODO: Gear 
        public readonly struct ModelData
        {
            public readonly SpellId[] Spells;
            public readonly StatsTable Stats;

            public readonly PositionData PositionData;
            public readonly CastResourceData CastResourceData;

            public readonly byte Team;

            public ModelData(SpellId[] spells, StatsTable stats, PositionData positionData, CastResourceData castResourceData, byte team)
            {
                Spells = spells;
                Stats = stats;
                PositionData = positionData;
                CastResourceData = castResourceData;
                Team = team;
            }
        }

        public readonly struct PositionData
        {
            public readonly Vector3 Location;
            public readonly float Rotation;

            public PositionData(Vector3 location, float rotation)
            {
                Location = location;
                Rotation = rotation;
            }
        }

        public readonly struct CastResourceData
        {
            public readonly float LeftResourceMaxValue;
            public readonly float RightResourceMaxValue;
            public readonly ResourceType LeftResourceType;
            public readonly ResourceType RightResourceType;

            public CastResourceData(float leftResource, float rightResource, ResourceType leftType, ResourceType rightType)
            {
                LeftResourceMaxValue = leftResource;
                RightResourceMaxValue = rightResource;
                LeftResourceType = leftType;
                RightResourceType = rightType;
            }
        }

        public readonly struct ViewData
        {
            public readonly int CharacterId;
            public readonly int CharacterViewSet;

            public ViewData(int characterId, int characterViewSet)
            {
                CharacterId = characterId;
                CharacterViewSet = characterViewSet;
            }
        }

#if DEBUG
        public
#else
        private
#endif
        UnitCreationData(int id, ModelData modelData, ViewData veiwData)
        {
            Id = id;
            Model = modelData;
            Veiw = veiwData;
        }

        public static UnitCreationData Parse(byte[] data, int start)
        {
            ModelData modelData = ParseModelData(data, ref start);
            Debug.Log(start);
            ViewData viewData = ParseViewData(data, ref start);
            int id = BitConverter.ToInt32(data, start);
            start += sizeof(int);

            return new(id, modelData, viewData);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ModelData ParseModelData(byte[] source, ref int index)
        {
            SpellId[] spells = ParseSpells(source, ref index);
            StatsTable stats = ParseStatsTable(source, ref index);
            CastResourceData resources = ParseCastResources(source, ref index);
            PositionData position = ParsePosition(source, ref index);
            byte team = source[index++];

            ModelData result = new(spells, stats, position, resources, team);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ViewData ParseViewData(byte[] source, ref int index)
        {
            int characterId = BitConverter.ToInt32(source, index);
            byte viewSet = source[index + sizeof(int)];
            index += sizeof(int) + 1;

            return new(characterId, viewSet);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SpellId[] ParseSpells(byte[] source, ref int index)
        {
            int spellCount = source[index++];

            SpellId[] result = new SpellId[spellCount];

            for (int i = 0; i < spellCount; i++)
            {
                result[i] = (SpellId) BitConverter.ToInt32(source, index);
                index += sizeof(int);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StatsTable ParseStatsTable(byte[] source, ref int index)
        {
            StatsTable result = new StatsTable();

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
        private static CastResourceData ParseCastResources(byte[] source, ref int index)
        {
            float leftResource = BitConverter.ToSingle(source, index);
            float rightResource = BitConverter.ToSingle(source, index + sizeof(float));
            index += sizeof(float) * 2;

            ushort leftType = BitConverter.ToUInt16(source, index);
            ushort rightType = BitConverter.ToUInt16(source, index + sizeof(ushort));
            index += sizeof(ushort) * 2;

            return new(leftResource, rightResource, (ResourceType) leftType, (ResourceType) rightType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static PositionData ParsePosition(byte[] source, ref int index)
        {
            float positionX = BitConverter.ToSingle(source, index);
            float positionY = BitConverter.ToSingle(source, index += sizeof(float));
            float positionZ = BitConverter.ToSingle(source, index += sizeof(float));

            float rotation = BitConverter.ToSingle(source, index += sizeof(float));

            index += sizeof(float);

            return new(new(positionX, positionY, positionZ), rotation);
        }
    }
}


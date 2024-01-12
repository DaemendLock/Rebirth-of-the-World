using System.Runtime.CompilerServices;
using Utils.DataStructure;

namespace Utils.DataTypes
{
    public class UnitCreationData
    {
        public readonly int Id;
        public readonly ModelData Model;
        public readonly ViewData Veiw;

        public readonly byte ControlGroup;
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
        UnitCreationData(int id, ModelData modelData, ViewData veiwData, byte contolGroup)
        {
            Id = id;
            Model = modelData;
            Veiw = veiwData;
            ControlGroup = contolGroup;
        }

        public static UnitCreationData Parse(ByteReader source)
        {
            ModelData modelData = ParseModelData(source);

            ViewData viewData = ParseViewData(source);
            byte group = source.ReadByte();
            int id = source.ReadInt();

            return new(id, modelData, viewData, group);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ModelData ParseModelData(ByteReader source)
        {
            SpellId[] spells = ParseSpells(source);
            StatsTable stats = ParseStatsTable(source);
            CastResourceData resources = ParseCastResources(source);
            PositionData position = ParsePosition(source);
            byte team = source.ReadByte();

            ModelData result = new(spells, stats, position, resources, team);

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ViewData ParseViewData(ByteReader source)
        {
            int characterId = source.ReadInt();
            byte viewSet = source.ReadByte();

            return new(characterId, viewSet);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static SpellId[] ParseSpells(ByteReader source)
        {
            int spellCount = source.ReadByte();

            SpellId[] result = new SpellId[spellCount];

            for (int i = 0; i < spellCount; i++)
            {
                result[i] = (SpellId) source.ReadInt();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StatsTable ParseStatsTable(ByteReader source)
        {
            StatsTable result = new StatsTable();

            for (int i = 0; i < StatsTable.STATS_COUNT; i++)
            {
                float value = source.ReadFloat();
                float percent = source.ReadFloat();

                result[i] = new PercentModifiedValue(value, percent);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static CastResourceData ParseCastResources(ByteReader source)
        {
            float leftResource = source.ReadFloat();
            float rightResource = source.ReadFloat();

            ushort leftType = source.ReadUShort();
            ushort rightType = source.ReadUShort();

            return new(leftResource, rightResource, (ResourceType) leftType, (ResourceType) rightType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static PositionData ParsePosition(ByteReader source)
        {
            Vector3 position = Vector3.Parse(source);
            float rotation = source.ReadFloat();

            return new(position, rotation);
        }
    }
}


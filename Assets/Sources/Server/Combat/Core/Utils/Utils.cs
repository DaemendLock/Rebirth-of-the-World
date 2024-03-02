using Core.Combat.Engine;
using Utils.DataTypes;

namespace Server.Combat.Core.Utils
{
    public enum HealthChangeFailType
    {
        NONE,
        SUCCESS,
        EVADE,
        PARRY,
        BLOCK,
        ABSORB,
    }

    public enum CommandResult : byte
    {
        SUCCES,
        ON_COOLDOWN,
        INVALID_TARGET,
        NOT_IN_LOS,
        NOT_ENOUGHT_RESOURCE,
        OUT_OF_RANGE,
        MOVING
    }

    public static class Time
    {
        public static long time => ModelUpdate.LastUpdate;
    }

    public readonly struct Position
    {
        public readonly Vector3 Location;
        public readonly float Rotation;

        public Position(UnitCreationData.PositionData data) : this(data.Location, data.Rotation)
        {
        }

        public Position(Vector3 location, float rotation)
        {
            Location = location;
            Rotation = rotation;
        }
    }
}

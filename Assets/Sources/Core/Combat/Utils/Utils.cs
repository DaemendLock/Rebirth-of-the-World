using Core.Combat.Units;
using Utils.DataTypes;

namespace Core.Combat.Utils
{
    public enum HealthChangeProcessingType
    {
        INITIAL_VALUE,
        CONSTANT_MODIFICATION,
        PERCENT_MODIFICATION,
        EVADE,
        PARRY,
        BLOCK,
        ABSORB,

    }

    public enum HealthChangeFailType
    {
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
        NOT_ENOUGHT_RESOURCE
    }

    public readonly struct HealthChangePrecessingData
    {
        public readonly Unit Appier;
        public readonly HealthChangeProcessingType Action;
        public readonly float Value;
    }

    public static class Time
    {
        public static float time => (float) CombatTime.Time / 1000;
    }

    public struct Position
    {
        public Vector3 Location;
        public Vector3 ViewDirection;
    }
}

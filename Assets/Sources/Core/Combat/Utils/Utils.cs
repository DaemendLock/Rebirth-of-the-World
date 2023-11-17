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
        RESIST,
        AMPLIFY,
    }

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
        OUT_OF_RANGE
    }

    public readonly struct HealthChangeEventModification
    {
        public readonly int Caster;
        public readonly HealthChangeProcessingType Action;
        public readonly float Value;

        public HealthChangeEventModification(int caster, HealthChangeProcessingType action, float value)
        {
            Caster = caster;
            Action = action;
            Value = value;
        }
    }

    public static class Time
    {
        public static float time => (float) CombatTime.Time / 1000;
    }

    public struct Position
    {
        public Vector3 Location;
        public float Rotation;
    }
}

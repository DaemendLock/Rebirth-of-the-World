using Core.Combat.Utils;

namespace Core.Combat.Engine
{
    public static class Combat
    {
        public static bool Running { get; private set; } = false;

        public static void Start()
        {
            if (Running)
            {
                return;
            }

            Running = true;

            /*while (_running)
            {
                Update();
            }

            Reset();*/
        }

        public static void Stop()
        {
            if (Running == false)
            {
                return;
            }

            Running = false;
        }

        public static void Reset()
        {
            ModelUpdate.Clear();
            Units.Clear();
            CombatTime.Reset();
        }

        public static float GetManaRestoreRate() => 0.1f;
        public static float GetEnergyRechargeRate() => 15f;
        public static float GetConcentrationRestoreRate() => 100f;
    }
}

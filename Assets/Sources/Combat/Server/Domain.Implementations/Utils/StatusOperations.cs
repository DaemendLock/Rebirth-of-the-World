using System.Reflection;

using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Implementations.Statuses.Attributes;

namespace Server.Combat.Domain.Implementations.Utils.Extenstions
{
    public static class StatusExtenstions
    {
        public static void IncrementStackCount(this StatusEffect status) => status.StackCount++;
        public static void DecrementStackCount(this StatusEffect status) => status.StackCount--;

        public static float GetFullDuration(this StatusEffect status) => status.Duration;
        public static string GetName(this StatusEffect status) => status.GetType().GetCustomAttribute<StatusNameAttribute>()?.Name ?? throw new System.Exception($"Status name for {status} is not assigned.");
        public static int AuraId(this StatusEffect status) => status.GetType().GetCustomAttribute<AuraIdAttribute>()?.Id ?? throw new System.Exception($"Aura id for {status} is not assigned.");
    }
}

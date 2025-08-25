using Server.Combat.Domain.Entities;

namespace Server.Combat.Domain.Implementations.Utils
{
    public static class TeamOwnerExtestion
    {
        public static bool CanHurt(this Unit unit, Unit target) => unit != null && target != null && unit.Team != target.Team;

        public static bool CanHelp(this Unit unit, Unit target) => unit != null && target != null && unit.Team == target.Team;
    }
}

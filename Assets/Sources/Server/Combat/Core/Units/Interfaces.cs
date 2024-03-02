using Utils.DataStructure;
using Utils.DataTypes;

namespace Core.Combat.Units.Interfaces
{
    public interface UnitAssignable
    {
        public void AssignTo(Unit unit);
    }

    public interface TeamOwner
    {
        public Team.Team Team { get; }

        public bool CanHelp(TeamOwner teamOwner);

        public bool CanHurt(TeamOwner teamOwner);
    }

    public interface StatsOwner
    {
        int EvaluateStatValue(UnitStat stat);

        PercentModifiedValue EvaluateStat(UnitStat stat);
    }
}
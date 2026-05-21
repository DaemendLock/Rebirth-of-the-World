using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Data.Models
{
    public readonly struct AligmentData
    {
        public AligmentData(Aligment aligment)
        {
            Team = aligment.Team;
        }

        public Team Team { get; }
    }
}

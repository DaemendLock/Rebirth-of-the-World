using Combat.API.Objectives;
using Combat.API.Scripting;

namespace Combat.Local.Scripting.Runtime
{
    public sealed class RuntimeObjectiveContainer
    {
        private readonly ICombatObjective _combatObjective;
        private readonly IObjectiveContext _context;

        public RuntimeObjectiveContainer(ICombatObjective combatObjective, IObjectiveContext context)
        {
            _combatObjective = combatObjective;
            _context = context;
        }

        public ICombatObjective CombatObjective => _combatObjective;

        public IObjectiveContext Context => _context;
    }
}

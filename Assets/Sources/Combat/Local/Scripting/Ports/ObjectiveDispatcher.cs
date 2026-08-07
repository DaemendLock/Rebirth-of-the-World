using Combat.Common.ValueObjects;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Ports
{
    public sealed class ObjectiveDispatcher
    {
        private readonly ObjectiveRuntimeRegistry _objectiveContainer;

        public void Cancel(ObjectiveId id)
        {
            if (_objectiveContainer.TryGet(id, out var value) == false)
            {
                return;
            }

            value.CombatObjective.OnComplete(value.Context);
            _objectiveContainer.Remove(id);
        }
    }
}

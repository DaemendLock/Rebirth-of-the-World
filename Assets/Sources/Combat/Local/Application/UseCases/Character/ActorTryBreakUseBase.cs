using Combat.Common;
using Combat.Common.Primitives;
using Combat.Domain.Repositories;

namespace Combat.Local.Application.UseCases.Character
{
    public sealed class ActorTryBreakUseBase
    {
        private readonly IUnitStore _unitStore;

        public bool Execute(UnitId target, ActorState type, float duration, float minComposure = 0)
        {
            if (_unitStore.TryGet(target, out var unit) == false)
            {
                return false;
            }

            if (unit.Composure.Current > minComposure)
            {
                return false;
            }

            unit.Actor.ApplyBreak(type, duration);
            unit.Composure.Current = 0;

            return true;
        }
    }
}

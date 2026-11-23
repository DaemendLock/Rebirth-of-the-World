using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;

using System.Numerics;

namespace Combat.Local.Application.UseCases
{
    public class ActorDesireMoveInDirectionUseCase
    {
        private readonly IUnitStore _unitStore;

        public ActorDesireMoveInDirectionUseCase(IUnitStore unitStore)
        {
            _unitStore = unitStore;
        }

        public void Execute(UnitId target, Vector2 relativeDirection)
        {
            if (_unitStore.TryGet(target, out UnitContext unit) == false)
            {
                return;
            }

            if (relativeDirection.LengthSquared() > 1)
            {
                relativeDirection = Vector2.Normalize(relativeDirection);
            }

            unit.Actor.DesireMoveDirection(relativeDirection);
        }
    }
}

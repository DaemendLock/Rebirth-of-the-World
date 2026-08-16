using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Numerics;

namespace Combat.Local.Domain.UseCases
{
    public class DesireMoveInDirectionUseCase
    {
        private readonly IActorRepository _actorRepository;

        public DesireMoveInDirectionUseCase(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public void Execute(UnitId target, Vector2 relativeDirection)
        {
            if (_actorRepository.TryGet(target, out Actor actor) == false)
            {
                return;
            }

            actor.DesireMoveDirection(relativeDirection);
            _actorRepository.Update(actor);
        }
    }
}

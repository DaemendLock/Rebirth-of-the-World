using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.Repositories;

using System.Numerics;

namespace Combat.Local.Application.UseCases
{
    public class ActorDesireMoveInDirectionUseCase
    {
        private readonly IActorRepository _actorRepository;

        public ActorDesireMoveInDirectionUseCase(IActorRepository actorRepository)
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

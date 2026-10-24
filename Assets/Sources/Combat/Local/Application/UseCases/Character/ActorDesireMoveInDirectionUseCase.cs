using Combat.Common.Primitives;
using Combat.Domain.Repositories;

using System.Numerics;

namespace Combat.Local.Application.UseCases
{
    public class ActorDesireMoveInDirectionUseCase
    {
        private readonly IUnitStore _actorRepository;

        public ActorDesireMoveInDirectionUseCase(IUnitStore actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public void Execute(UnitId target, Vector2 relativeDirection)
        {
            _actorRepository.Get(target).Actor.DesireMoveDirection(relativeDirection);
        }
    }
}

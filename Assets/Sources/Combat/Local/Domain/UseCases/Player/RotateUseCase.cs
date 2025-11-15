using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class RotateUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly IPositionableRepository _positionableRepository;

        public void Execute(EntityId entityId, float angle)
        {
            Actor actor = _actorRepository.Get(entityId);

            if (actor.CanMove == false)
            {
                return;
            }

            Positionable positionable = _positionableRepository.Get(entityId);

            

            _positionableRepository.Update(positionable);
        }
    }
}

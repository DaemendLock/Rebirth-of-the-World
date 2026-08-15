using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Numerics;

namespace Combat.Local.Domain.UseCases
{
    public class DesireMoveInDirectionUseCase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IActorRepository _actorRepository;

        public DesireMoveInDirectionUseCase(IPlayerRepository playerRepository, IActorRepository actorRepository)
        {
            _playerRepository = playerRepository;
            _actorRepository = actorRepository;
        }

        public void Execute(PlayerId target, Vector2 relativeDirection)
        {
            Player player = _playerRepository.Get(target);

            if (player.ControlledEntity.HasValue == false)
            {
                return;
            }

            UnitId id = player.ControlledEntity.Value;

            if (_actorRepository.TryGet(id, out Actor actor) == false)
            {
                return;
            }

            actor.DesireMoveDirection(relativeDirection);
            _actorRepository.Update(actor);
        }
    }
}

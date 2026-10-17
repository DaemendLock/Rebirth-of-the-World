using Combat.Common;
using Combat.Domain.Entities;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Application.Services.Skills
{
    public sealed class ActorOperations
    {
        private readonly IActorRepository _actorRepository;

        public ActorOperations(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public bool TryKill(Actor actor)
        {
            if (actor.ConsciousState == ConsciousState.Dead)
            {
                return false;
            }

            actor.Kill();
            _actorRepository.Update(actor);
            return true;
        }

        public bool Revive(Actor actor)
        {
            if (actor.ConsciousState == ConsciousState.Alive)
            {
                return false;
            }

            actor.Revive();
            _actorRepository.Update(actor);
            return true;
        }
    }
}

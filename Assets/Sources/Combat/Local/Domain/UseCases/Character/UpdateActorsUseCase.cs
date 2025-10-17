using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

using System.Linq;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct UpdateActorsUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly IActionStateChangeEventHandler _actionStateChangeEventHandler;

        public UpdateActorsUseCase(IActorRepository actorRepository, IActionStateChangeEventHandler actionStateChangeEventHandler)
        {
            _actorRepository = actorRepository;
            _actionStateChangeEventHandler = actionStateChangeEventHandler;
        }

        public void Execute(float deltaTime)
        {
            foreach (EntityId id in _actorRepository.GetAll().ToArray())
            {
                Actor actor = _actorRepository.Get(id);
                UpdateActor(actor, deltaTime);
            }
        }

        private void UpdateActor(Actor actor, float deltaTime)
        {
            IAction action = actor.CurrentAction;

            if (action == null)
            {
                return;
            }

            if (action.IsActive == false)
            {
                actor.CurrentAction = null;
                _actorRepository.Update(actor);
                return;
            }

            ActionState actionState = action.CurrentState;

            float time = action.ActiveTime + deltaTime;

            ActionData data = new(action.Skill, action.IsActive, time, time, action.AllowMovement);
            action.Update(data);

            if (action.CurrentState == actionState)
            {
                return;
            }

            _actionStateChangeEventHandler.HandleEvent(actor.Id, data.Skill, action.CurrentState);
        }
    }
}

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Linq;

using UnityEngine;

namespace Combat.Local.Domain.UseCases
{

    public readonly ref struct ActionDTO
    {
        public EntityId ActorId { get; }
        public AnimationClip Clip { get; }
        public float StartTime { get; }
        public float HasteModifier { get; }
    }

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

    public interface IActionOutput
    {
        void Present(ActionDTO actionDTO);
    }

    public interface IActionStateChangeEventHandler
    {
        void HandleEvent(EntityId actorId, SkillId skill, ActionState newState);
    }
}

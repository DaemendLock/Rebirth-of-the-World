using Combat.API;
using Combat.API.Controllers;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

namespace Testing.Local.Temp.DomainOutputs
{
    public class ActionStateChangeHandler : IActionStateChangeEventHandler
    {
        private readonly SkillApiProvider _skillApiProvider;
        private readonly IActorRepository _actorRepository;

        public ActionStateChangeHandler(SkillApiProvider skillApiProvider, IActorRepository actorRepository)
        {
            _skillApiProvider = skillApiProvider;
            _actorRepository = actorRepository;
        }

        public void HandleEvent(EntityId actorId, ActionState newState)
        {
            Actor actor = _actorRepository.Get(actorId);
            SkillApi skillApi = _skillApiProvider.Get(new(actor.CurrentAction.Id.Value), actorId);

            if (skillApi == null || (skillApi.TryGetProperty(out ICastStateChangeHandler handler) == false))
            {
                return;
            }

            switch (newState)
            {
                case ActionState.Startup:
                    handler.OnStartup();
                    break;

                case ActionState.Active:
                    handler.OnActive();
                    break;

                case ActionState.Gap:
                    handler.OnGapStart();
                    break;

                case ActionState.Recovery:
                    handler.OnRecovery();
                    break;

                case ActionState.Inactive:
                    handler.OnEnds();
                    break;

                default:
                    throw new System.InvalidOperationException($"Can't find skill state \"{newState}\".");
            }
        }
    }
}

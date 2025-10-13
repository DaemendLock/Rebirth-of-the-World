using Combat.API;
using Combat.API.Controllers;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class ActionStateChangeHandler : IActionStateChangeEventHandler
    {
        private readonly SkillApiProvider _skillApiProvider;

        public ActionStateChangeHandler(SkillApiProvider skillApiProvider)
        {
            _skillApiProvider = skillApiProvider;
        }

        public void HandleEvent(EntityId actorId, SkillId skillId, ActionState newState)
        {
            SkillApi skillApi = _skillApiProvider.Get(skillId, actorId);

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

using System.Collections.Generic;

using CastStateSkill;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.API;
using Combat.Local.Domain.API.Skills;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Services;

namespace Assets.Sources.Combat.Local.Domain.API
{
    public class ApiContainer
    {
        private readonly Scene _environment;
        private readonly IStatusLookupService _statusLookupService;
        private readonly HealthService _healDamageApplicationService;

        public ApiContainer(IStatusLookupService statusLookupService, HealthService healDamageApplicationService)
        {
            if (Api != null)
            {
                throw new System.InvalidOperationException();
            }

            _statusLookupService = statusLookupService;
            _healDamageApplicationService = healDamageApplicationService;

            _castStateChangeHandlers = new();
            
            Api = this;
        }

        public static ApiContainer Api { get; private set; }

        private readonly Dictionary<EntityId, ICastStateChangeHandler> _castStateChangeHandlers;

        public void GetMovementModifiers()
        {

        }

        public void HandleActionStateChange(IAction action, SkillCastState newState)
        {
            ICastStateChangeHandler stateChangeHandler = _castStateChangeHandlers[action.Actor];

            switch (newState)
            {
                case SkillCastState.Startup:
                    stateChangeHandler.OnStartup();
                    break;

                case SkillCastState.Active:
                    stateChangeHandler.OnActive();
                    break;

                case SkillCastState.Gap:
                    stateChangeHandler.OnGapStart();
                    break;

                case SkillCastState.Recovery:
                    stateChangeHandler.OnRecovery();
                    break;

                case SkillCastState.Inactive:
                    stateChangeHandler.OnEnds();
                    break;

                default:
                    throw new System.InvalidOperationException($"Can't find skill state \"{newState}\".");
            }
        }

        public StatusApi RegisterStatus(Status statusEffect) => throw new System.NotImplementedException();
    }
}

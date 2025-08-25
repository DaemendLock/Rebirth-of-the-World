using CastStateSkill;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.Skills;
using Combat.Local.Domain.Entities.Units;

using UnityEngine;

namespace Combat.Local.Domain.Entities
{
    public class CastAction : IAction
    {
        private readonly ICastStateChangeHandler _stateChangeHandler;
        private readonly IHitHandler _hitHandler;
        private readonly IFrameData _frameData;

        private float _timeMultiplier;
        private SkillCastState _state;

        public CastAction(EntityId caster, float timeMultiplier, IFrameData frameData, bool allowMoment, ICastStateChangeHandler stateChangeHandler, IHitHandler hitHandler)
        {
            _stateChangeHandler = stateChangeHandler;
            _hitHandler = hitHandler;
            _frameData = frameData;
            _timeMultiplier = timeMultiplier;

            AllowMovement = allowMoment;
            Actor = caster;
            ActiveTime = 0;
        }

        public EntityId Actor { get; }

        public float ActiveTime { get; set; }

        public bool AllowMovement { get; }

        public bool IsActive => _state != SkillCastState.Inactive;

        public void Start()
        {
            if (IsActive)
            {
                return;
            }

            _state = SkillCastState.Startup;
            _stateChangeHandler.OnStartup();
        }

        public void Update()
        {
            if (IsActive == false)
            {
                return;
            }

            SkillCastState currentState = _frameData.GetCastState(ActiveTime * _timeMultiplier);

            if (currentState == _state)
            {
                return;
            }

            _state = currentState;

            //_api.OnCastStateChange();
            switch (_state)
            {
                case SkillCastState.Startup:
                    _stateChangeHandler.OnStartup();
                    break;

                case SkillCastState.Active:
                    _stateChangeHandler.OnActive();
                    break;

                case SkillCastState.Gap:
                    _stateChangeHandler.OnGapStart();
                    break;

                case SkillCastState.Recovery:
                    _stateChangeHandler.OnRecovery();
                    break;

                case SkillCastState.Inactive:
                    _stateChangeHandler.OnEnds();
                    break;

                default:
                    throw new System.InvalidOperationException($"Can't find skill state \"{_state}\".");
            }
        }

        public bool HandleHit(Hitbox hitbox, Hurtbox hurtbox, Vector3 position)
        {
            if (_hitHandler == null)
                return false;
            return false;
        }
    }
}

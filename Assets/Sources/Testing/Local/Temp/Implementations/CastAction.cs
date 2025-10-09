using CastStateSkill;

using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public class CastAction : IAction
    {
        private readonly List<EntityId> _hittedTargets;

        private readonly IFrameData _frameData;

        private float _timeMultiplier;
        private ActionState _state;

        private ActionData _data;

        public CastAction(ActionData data, float timeMultiplier, IFrameData frameData)
        {
            _frameData = frameData;
            _timeMultiplier = timeMultiplier;
            _state = ActionState.Inactive;
            _data = data;

            _hittedTargets = new();
        }

        public ICollection<EntityId> HittedTargets => _hittedTargets;

        public float ActiveTime => _data.ActiveTime;

        public bool AllowMovement => _data.AllowMovement;

        public bool IsActive => _state != ActionState.Inactive;

        public ActionState CurrentState => _state;

        public SkillId Skill => _data.Skill;

        public ActionData Data => new(_data.Skill, IsActive, ActiveTime, ActiveTime * _timeMultiplier, AllowMovement);

        public void Start()
        {
            if (IsActive)
            {
                return;
            }

            _state = ActionState.Startup;
        }

        public void Update(ActionData data)
        {
            _data = data;

            if (_state == ActionState.Inactive)
            {
                return;
            }

            _state = (ActionState)_frameData.GetCastState(data.ActiveTime * _timeMultiplier);
        }
    }
}

using CastStateSkill;

using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Entities
{
    public class CastAction : IAction
    {
        private readonly List<EntityId> _hittedTargets;
        private readonly IFrameData _frameData;

        private ActionState _state;
        private ActionData _data;

        public CastAction(ActionId id, ActionFlags flags, IFrameData frameData)
        {
            _frameData = frameData;
            _state = ActionState.Inactive;
            Id = id;
            _data = new(false, 0, 0);
            Flags = flags;

            _hittedTargets = new();
        }

        public ActionId Id { get; }

        public ActionFlags Flags { get; }

        public float ActiveTime => _data.ActiveTime;

        public float EffectiveTime => _data.EffectiveTime;

        public ActionState CurrentState => _state;

        public ICollection<EntityId> HittedTargets => _hittedTargets;

        public void Start()
        {
            if (_state != ActionState.Inactive)
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

            _state = (ActionState)_frameData.GetCastState(data.EffectiveTime);
        }
    }
}

using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Presentation.Providers;

using DaeAnimator;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public sealed class ActorUpdatePresenter : IActorActionUpdateOutput
    {
        private readonly ITransformProvider _transformProvider;
        private readonly IActionDataContainer _actionDataContainer;

        public ActorUpdatePresenter(ITransformProvider transformProvider, IActionDataContainer actionDataContainer)
        {
            _transformProvider = transformProvider;
            _actionDataContainer = actionDataContainer;
        }

        public void Present(UnitId target, ActionId action, ActionFlags flags, float activeTime)
        {
            if (TryGetAnimator(target, out var casterView) == false)
            {
                return;
            }

            var data = _actionDataContainer.Get(action);

            if (data.Animation == null)
                return;

            bool isHoldable = flags.HasFlag(ActionFlags.Holdable);

            CharacterAnimation animation = isHoldable
                ? new(
                    data.Animation,
                    0,
                    0,
                    true,
                    data.FrameData.LastActiveEnterTime,
                    data.FrameData.LastActiveExitTime)
                : new(data.Animation, 0, 0, false);

            casterView.PlayCastAnimation(animation, activeTime);
        }

        public void Clear(UnitId target)
        {
            if (TryGetAnimator(target, out var animator) == false)
                return;

            animator.StopCastAnimation();
        }

        public void Release(UnitId target)
        {
            if (TryGetAnimator(target, out var animator) == false)
                return;

            animator.ReleaseCastAnimation();
        }

        private bool TryGetAnimator(UnitId target, out CharacterAnimator animator)
        {
            animator = null;

            return _transformProvider.TryGet(target, out var model) && model.TryGetComponent(out animator);
        }
    }
}

using Combat.Common.Primitives;
using Combat.Local.Domain.UseCases;
using Combat.Local.Presentation.Views;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public interface ITransformProvider
    {
        bool TryGet(UnitId unitId, out Transform transform);
    }

    public sealed class PlayerPresenter : ITakeControllOutput, IDesireCastOutput
    {
        private readonly ITransformProvider _sceneObjectDataSource;
        private readonly PlayerViewComponent _view;

        public PlayerPresenter(ITransformProvider sceneObjectDataSource)
        {
            _view = Camera.main.gameObject.AddComponent<PlayerViewComponent>();
            _sceneObjectDataSource = sceneObjectDataSource;
        }

        void ITakeControllOutput.Present(UnitId? id)
        {
            UnityEngine.Debug.Log($"Assumed control over character(Id: {id})");

            if (id.HasValue == false)
            {
                _view.Follow(null);
            }

            if (_sceneObjectDataSource.TryGet(id.Value, out var model))
            {
                _view.Follow(model);
            }
        }

        void IDesireCastOutput.Present(DesireCastFailReason failReason)
        {
            switch (failReason)
            {
                case DesireCastFailReason.NoSkillFound:
                    UnityEngine.Debug.Log("No skill in slot");
                    return;

                default:
                    return;
            }
        }
    }
}

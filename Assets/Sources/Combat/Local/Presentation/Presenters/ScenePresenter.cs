using Combat.Common.Primitives;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Presentation.Providers;
using Combat.Local.Presentation.Views;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public class ScenePresenter : ICharacterCreateOutput, ICharacterRemoveOutput
    {
        private readonly ITransformProvider _transformProvider;

        public ScenePresenter(ITransformProvider transformProvider)
        {
            _transformProvider = transformProvider;
        }

        void ICharacterCreateOutput.Present(UnitId value)
        {
            UnityEngine.Debug.Log($"Character created: Id - {value}");

            if (_transformProvider.TryGet(value, out var transform))
            {
                transform.gameObject.AddComponent<CharacterView>();
                transform.gameObject.AddComponent<PositionableView>();
                transform.gameObject.AddComponent<ScalableViewComponent>();
            }
        }

        void ICharacterRemoveOutput.Present(UnitId value)
        {
            UnityEngine.Debug.Log($"Character removed: Id - {value}");

            if (_transformProvider.TryGet(value, out var transform))
            {
                Object.Destroy(transform.gameObject);
            }
        }
    }
}

using Combat.Common.Primitives;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;
using Combat.Local.Presentation.Providers;

using UnityEngine;

namespace Combat.Local.Presentation.Presenters
{
    public class ScenePresenter : ICharacterCreateOutput, ICharacterRemoveOutput
    {
        public ScenePresenter()
        {
        }

        void ICharacterCreateOutput.Present(UnitId value)
        {
            UnityEngine.Debug.Log($"Character created: Id - {value}");
        }

        void ICharacterRemoveOutput.Present(UnitId value)
        {
            UnityEngine.Debug.Log($"Character removed: Id - {value}");
        }
    }
}

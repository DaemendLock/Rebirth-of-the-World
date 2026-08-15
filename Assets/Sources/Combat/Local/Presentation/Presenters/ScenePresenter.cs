using Combat.Common.Primitives;
using Combat.Local.Domain.OutputPorts;

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

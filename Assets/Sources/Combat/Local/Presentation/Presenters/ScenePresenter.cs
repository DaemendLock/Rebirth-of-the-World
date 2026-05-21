using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts;

namespace Combat.Local.Presentation.Presenters
{
    public class ScenePresenter : ICharacterCreateOutput
    {

        public ScenePresenter()
        {
        }

        void ICharacterCreateOutput.Present(UnitId value)
        {
            UnityEngine.Debug.Log($"Character created: Id - {value}");
        }
    }
}

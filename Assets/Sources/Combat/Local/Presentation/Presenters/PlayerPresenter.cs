using Combat.Common.Primitives;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Presentation.Presenters
{
    public class PlayerPresenter : ITakeControllOutput, IDesireCastOutput
    {
        public PlayerPresenter()
        {
        }

        void ITakeControllOutput.Present(UnitId? id)
        {
            UnityEngine.Debug.Log($"Assumed control over character(Id: {id})");
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

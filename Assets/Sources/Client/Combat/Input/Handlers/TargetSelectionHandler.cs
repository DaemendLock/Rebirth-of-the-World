using Client.Combat.Adapters;

using Input;

namespace Client.Combat.Input.Handlers
{
    public class TargetSelectionHandler
    {
        private CombatInput.CombatTargetingActions _targetSellectionAction;
        private readonly Selection.SelectionInfo _selection;

        public TargetSelectionHandler(CombatInput source)
        {
            _targetSellectionAction = source.CombatTargeting;
        }

        public void Enable()
        {
            _targetSellectionAction.ControlNext.performed += ctx => ControlNext();
            _targetSellectionAction.TargetNext.performed += ctx => TargetNext();

            _targetSellectionAction.Enable();
        }

        public void Disable()
        {
            _targetSellectionAction.Disable();

            _targetSellectionAction.ControlNext.performed -= ctx => ControlNext();
            _targetSellectionAction.TargetNext.performed -= ctx => TargetNext();
        }

        private void ControlNext() => _selection.SelectNext();

        private void TargetNext()
        {
            _selection.TargetNext();
            Networking.Combat.Send(new TargetData(_selection.CurrentSelection, _selection.CurrentTarget).GetBytes());
        }
    }
}

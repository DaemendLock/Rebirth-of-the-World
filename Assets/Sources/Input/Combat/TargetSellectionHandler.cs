using Adapters.Combat;

namespace Input.Combat
{
    public class TargetSellectionHandler
    {
        private CombatInput.CombatTargetingActions _targetSellectionAction;

        public TargetSellectionHandler(CombatInput source)
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

        private void ControlNext()
        {
            SelectionInfo.ControlNext();
            UserInputAdapter.HandleSelectionChange();
        }

        private void TargetNext()
        {
            SelectionInfo.TargetNext();
            UserInputAdapter.HandleTargetChange();
            Networking.Combat.Send(new TargetData(SelectionInfo.SelectionId, SelectionInfo.TargetId));
        }
    }
}

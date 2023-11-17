using Utils.DataTypes;

namespace Input.Combat
{
    internal class CancelInputHandler
    {
        private CombatInput.CombatTargetingActions _targetSellectionAction;

        public CancelInputHandler(CombatInput source)
        {
            _targetSellectionAction = source.CombatTargeting;
        }

        public void Enable()
        {
            _targetSellectionAction.ControlNext.performed += ctx => SellectionInfo.ControlNext();
            _targetSellectionAction.TargetNext.performed += ctx =>
            { SellectionInfo.TargetNext(); SendTargetInput(SellectionInfo.TargetedUnitId); };

            _targetSellectionAction.Enable();
        }

        public void Disable()
        {
            _targetSellectionAction.Disable();

            _targetSellectionAction.ControlNext.performed -= ctx => SellectionInfo.ControlNext();
            _targetSellectionAction.TargetNext.performed -= ctx =>
            { SellectionInfo.TargetNext(); SendTargetInput(SellectionInfo.TargetedUnitId); };
        }

        private void SendTargetInput(int targetId)
        {
            Networking.Combat.Send(new TargetCommand(SellectionInfo.TargetedUnitId, targetId));
        }
    }
}       

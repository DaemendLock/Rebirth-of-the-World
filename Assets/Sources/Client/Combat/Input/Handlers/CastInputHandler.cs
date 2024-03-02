using Client.Combat.Adapters;
using Client.Combat.Input.Selection;

using Input;

using System;

namespace Client.Combat.Input.Handlers
{
    public class CastInputHandler
    {
        private readonly CombatInput.CombatCastingActions _castingActions;
        private readonly SelectionInfo _selection;

        public CastInputHandler(CombatInput source, SelectionInfo selectionInfo)
        {
            _castingActions = source.CombatCasting;
            _selection = selectionInfo ?? throw new ArgumentNullException(nameof(selectionInfo));
        }

        public void Enable()
        {
            _castingActions.Enable();
            OnEnable();
        }

        public void Disable()
        {
            _castingActions.Disable();
            OnDisable();
        }

        private void OnEnable()
        {
            _castingActions.CastAbility1.performed += ctx => CastAbility(_selection, 0);
            _castingActions.CastAbility2.performed += ctx => CastAbility(_selection, 1);
            _castingActions.CastAbility3.performed += ctx => CastAbility(_selection, 2);
            _castingActions.CastAbility4.performed += ctx => CastAbility(_selection, 3);
            _castingActions.CastAbility5.performed += ctx => CastAbility(_selection, 4);
            _castingActions.CastAbility6.performed += ctx => CastAbility(_selection, 5);

            _castingActions.Enable();
        }

        private void OnDisable()
        {
            _castingActions.Disable();

            _castingActions.CastAbility1.performed -= ctx => CastAbility(_selection, 0);
            _castingActions.CastAbility2.performed -= ctx => CastAbility(_selection, 1);
            _castingActions.CastAbility3.performed -= ctx => CastAbility(_selection, 2);
            _castingActions.CastAbility4.performed -= ctx => CastAbility(_selection, 3);
            _castingActions.CastAbility5.performed -= ctx => CastAbility(_selection, 4);
            _castingActions.CastAbility6.performed -= ctx => CastAbility(_selection, 5);
        }

        private static void CastAbility(SelectionInfo selection, byte spellSlot)
        {
            CastData request = new(selection.CurrentSelection, selection.CurrentTarget, spellSlot);
            Networking.Combat.Send(request.GetBytes());
        }
    }
}

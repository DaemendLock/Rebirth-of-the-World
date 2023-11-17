using Core.Combat.Abilities;
using Utils.DataTypes;

namespace Input.Combat
{
    public class CastInputHandler
    {
        private CombatInput.CombatCastingActions _castingActions;

        public CastInputHandler(CombatInput source)
        {
            _castingActions = source.CombatCasting;
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
            _castingActions.CastAbility1.performed += ctx => CastAbility(SpellSlot.FIRST);
            _castingActions.CastAbility2.performed += ctx => CastAbility(SpellSlot.SECOND);
            _castingActions.CastAbility3.performed += ctx => CastAbility(SpellSlot.THIRD);
            _castingActions.CastAbility4.performed += ctx => CastAbility(SpellSlot.FOURTH);
            _castingActions.CastAbility5.performed += ctx => CastAbility(SpellSlot.FIVETH);
            _castingActions.CastAbility6.performed += ctx => CastAbility(SpellSlot.SIXTH);

            _castingActions.Enable();
        }

        private void OnDisable()
        {
            _castingActions.Disable();

            _castingActions.CastAbility1.performed -= ctx => CastAbility(SpellSlot.FIRST);
            _castingActions.CastAbility2.performed -= ctx => CastAbility(SpellSlot.SECOND);
            _castingActions.CastAbility3.performed -= ctx => CastAbility(SpellSlot.THIRD);
            _castingActions.CastAbility4.performed -= ctx => CastAbility(SpellSlot.FOURTH);
            _castingActions.CastAbility5.performed -= ctx => CastAbility(SpellSlot.FIVETH);
            _castingActions.CastAbility6.performed -= ctx => CastAbility(SpellSlot.SIXTH);
        }

        private void CastAbility(SpellSlot spellSlot)
        {
            CastCommand request = new CastCommand(SellectionInfo.ContolledUnitId, SellectionInfo.TargetedUnitId, (byte)spellSlot);
            Networking.Combat.Send(request);
        }
    }
}

using Core.Combat.Units;
using Utils.DataTypes;

namespace Core.Combat.Abilities.ActionRecords
{
    public class CastActionRecord : IActionRecordContainer, ActionRecord
    {
        private readonly Unit _caster;
        private readonly Unit _target;
        private readonly SpellId _spell;
        private readonly IActionRecordContainer _container;

        public CastActionRecord(Unit caster, Unit target, SpellId spell)
        {
            _caster = caster;
            _target = target;
            _spell = spell;
            _container = new ActionRecordContainer();
        }

        public ActionType Type => ActionType.Dummy;

        public int Target => _target.Id;

        public SpellId Source => _spell;

        public void AddAction(ActionRecord action)
        {
            _container.AddAction(action);
        }

        public byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }
    }
}
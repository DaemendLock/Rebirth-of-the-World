using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units.CastingBehaviors;
using Core.Combat.Utils;

namespace Core.Combat.CastingBehaviors
{
    public class Idle : CastingBehavior
    {
        public Idle() : base()
        {
        }

        public bool CanInterrupt => false;

        public bool Finished => false;

        public bool AllowAutoattack => true;

        public Duration Duration => new Duration(float.PositiveInfinity);

        public Spell Spell => null;

        public void OnExit()
        {
        }

        public void OnEnter()
        {
        }

        public void Update()
        {
        }

        public void Update(IActionRecordContainer container)
        {
        }
    }
}

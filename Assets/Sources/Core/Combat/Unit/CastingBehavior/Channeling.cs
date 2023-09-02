using Core.Combat.Abilities;
using Core.Combat.Utils;

namespace Core.Combat.CastingBehaviors
{
    public sealed class Channeling : CastingBehavior
    {
        private Duration _timer;
        private float _procTime;
        private Castable _ability;

        public Channeling(EventData data, SpellModification modification, float procTime) : base(data, modification)
        {
            _procTime = procTime;
            _timer = new Duration(_procTime);

            _ability = data.Caster.FindAbility(data.Spell);
            _ability ??= data.Spell;
        }

        public override void OnCastBegins()
        { }

        public override void OnCastEnd()
        { }

        public override void OnUpdate()
        {
            if (_timer.Expired)
            {
                _timer += _procTime;
                ProcSpell(_ability);
            }
        }
    }
}

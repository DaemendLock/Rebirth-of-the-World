using Core.Combat.Abilities;
using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Engine.Services;
using Core.Combat.Units.CastingBehaviors;
using Core.Combat.Utils;

namespace Core.Combat.CastingBehaviors
{
    internal sealed class Precast : CastingBehavior
    {
        private readonly CastData _data;

        internal Precast(CastData data, float time)
        {
            Duration = new Duration(time);
            _data = data;
        }

        public Duration Duration { get; }

        public Spell Spell => _data.Ability.Spell;

        public bool CanInterrupt => Spell.Flags.HasFlag(SpellFlags.CANT_INTERRUPT) == false;

        public bool Finished => Duration.Expired;

        public bool AllowAutoattack => false;

        public void OnEnter()
        {
        }

        public void OnExit()
        {
        }

        public void Update(IActionRecordContainer container)
        {

            if (Finished)
                container.AddAction(_data.Ability.Cast(_data.Caster, _data.Target, _data.Caster.GetSpellValues(Spell.Id)));
        }
    }
}
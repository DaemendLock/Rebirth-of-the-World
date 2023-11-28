using Core.Combat.Units;
using Core.Combat.Utils;

namespace Core.Combat.Abilities.SpellScripts
{
    public class AoeSpell : Spell
    {
        private const byte AOE_RADIUS_INDEX = 0;

        public AoeSpell(SpellData data) : base(data)
        {
        }

        public override void Cast(CastEventData data, SpellModification modification)
        {
            Unit[] targets = new Unit[0];
            //targets = GetUnitsInRadius(range, team, data.Target.Position)

            foreach (Unit target in targets)
            {
                for (int i = 0; i < EffectsCount; i++)
                {
                    ApplyEffect(i, modification.EffectsModifications[i], new CastEventData(data.Caster, target, data.Spell, data.TriggerTime));
                }
            }
        }
    }
}

using Remaster.Events;
using Remaster.Utils;

namespace Remaster.SpellScripts
{
    public class CleaveSpell : Spell
    {
        public const int CLEAVE_EFFECT_INDEX = 0;

        public CleaveSpell(SpellData data) : base(data)
        {
        }

        public override void Cast(EventData data, SpellModification modification)
        {
            float angle = GetEffectValue(CLEAVE_EFFECT_INDEX, modification.EffectsModificationList[CLEAVE_EFFECT_INDEX]);

            Unit[] targets = new Unit[0];
            //targets = GetUnitsInRadius(range, team, data.Caster.Position)

            foreach (Unit target in targets)
            {
                //Check is in view angle
                if (true)
                {
                    continue;
                }

                for (int i = 0; i < EffectsCount; i++)
                {
                    ApplyEffect(i, modification.EffectsModificationList[i], new EventData(data.Caster, target, data.Spell));
                }
            }

            data.Caster?.InformCast(data, CommandResult.SUCCES);
        }
    }
}

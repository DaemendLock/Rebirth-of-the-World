using Core.Combat.Units;
using Core.Combat.Utils;

namespace Core.Combat.Abilities.SpellScripts
{
    public class SplashSpell : Spell
    {
        public SplashSpell(SpellData data) : base(data)
        {
        }

        public override void Cast(EventData data, SpellModification modification)
        {
            Unit[] targets = new Unit[0];
            //targets = GetUnitsInRadius(range, team, data.Caster.Position)

            foreach (Unit target in targets)
            {
                for (int i = 0; i < EffectsCount; i++)
                {
                    ApplyEffect(i, modification.EffectsModificationList[i], new EventData(data.Caster, target, data.Spell));
                }
            }

            data.Caster?.InformCast(data, CommandResult.SUCCES);
        }
    }
}
using Remaster.Events;
using Remaster.Utils;

namespace Remaster.SpellScripts
{
    public class AoeSpell : Spell
    {
        private const byte AOE_RADIUS_INDEX = 0;

        public AoeSpell(SpellData data) : base(data)
        {
        }

        public override void Cast(EventData data, SpellModification modification)
        {
            Unit[] targets = new Unit[0];
            //targets = GetUnitsInRadius(range, team, data.Target.Position)

            foreach(Unit target in targets)
            {
                for (int i = 0; i < EffectsCount; i++)
                {
                    ApplyEffect(i, modification.EffectsModificationList[i], new EventData(data.Caster, target, data.Spell, data.TriggerTime));
                }
            }

            data.Caster?.InformCast(data, CommandResult.SUCCES);
        }
    }
}

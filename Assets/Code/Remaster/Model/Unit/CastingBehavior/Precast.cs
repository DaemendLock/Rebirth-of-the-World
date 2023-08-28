using Remaster.Events;

namespace Remaster.CastingBehaviors
{
    public sealed class Precast : CastingBehavior
    {
        public Precast(EventData data, SpellModification modification) : base(data, modification)
        {
        }

        public override void OnCastBegins()
        {
            Logger.Log($"Start precasting {Spell.Id}({FullTime} sec.): {Caster}");
        }

        public override void OnUpdate()
        {
        }

        public override void OnCastEnd()
        {
            ProcSpell();
        }
    }
}
using Core.Combat.Abilities;
using Core.Combat.Utils;

namespace Core.Combat.CastingBehaviors
{
    public sealed class Precast : CastingBehavior
    {
        public Precast(CastEventData data, SpellModification modification) : base(data, modification)
        {
        }

        public override void OnCastBegins()
        {

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
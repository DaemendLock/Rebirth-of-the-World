namespace Core.Combat.CastingBehaviors
{
    public class Idle : CastingBehavior
    {
        public Idle() : base()
        {
        }

        public override bool CanInterrupt => false;

        public override void OnCastBegins()
        {
        }

        public override void OnCastEnd()
        {
        }

        public override void OnUpdate()
        {
        }
    }
}

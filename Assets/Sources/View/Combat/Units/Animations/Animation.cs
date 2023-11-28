using System;

namespace View.Combat.Units.Animations
{
    public abstract class Animation
    {
        public abstract AnimationType Type { get; }

        public abstract void Apply(AnimationController unit);
    }

    public class AttackAnimation : Animation
    {
        private readonly bool _useMainHand;

        public AttackAnimation(bool useMainHand, Unit target)
        {
            _useMainHand = useMainHand;
        }

        public override AnimationType Type => _useMainHand ? AnimationType.ATTACK_RIGHT : AnimationType.ATTACK_LEFT;

        public override void Apply(AnimationController unit)
        {
            throw new NotImplementedException();
        }
    }
}

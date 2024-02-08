using Core.Combat.Abilities;
using Core.Combat.Engine;
using Core.Combat.Utils;
using Utils.DataStructure.StateMachine;

namespace Core.Combat.Units.CastingBehaviors
{
    internal interface CastingBehavior : IState, Updatable
    {
        public Duration Duration { get; }
        public Spell Spell { get; }

        public bool CanInterrupt { get; }

        public bool Finished { get; }

        public bool AllowAutoattack { get; }
    }
}
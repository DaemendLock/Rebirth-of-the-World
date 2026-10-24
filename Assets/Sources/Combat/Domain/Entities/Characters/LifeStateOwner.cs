using Combat.Common;
using Combat.Common.Primitives;

namespace Combat.Domain.Entities
{
    public struct LifeStateOwner
    {
        public const float MaxDamageSeverity = 2f;

        public LifeStateOwner(UnitId id)
        {
            Id = id;
            State = ConsciousState.Alive;
        }

        public UnitId Id { get; }

        public ConsciousState State { readonly get; private set; }

        public bool TryEnterDead()
        {
            if (State == ConsciousState.Dead)
            {
                return false;
            }

            State = ConsciousState.Dead;
            return true;
        }

        public bool TryEnterDowned()
        {
            if (State != ConsciousState.Alive)
            {
                return false;
            }

            State = ConsciousState.Downed;
            return true;
        }

        public bool TryRevive()
        {
            if (State == ConsciousState.Alive)
            {
                return false;
            }

            State = ConsciousState.Alive;
            return true;
        }
    }
}

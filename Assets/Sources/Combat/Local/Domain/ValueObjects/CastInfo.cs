using System;

namespace Combat.Local.Domain.ValueObjects
{
    public enum ConsciousState
    {
        Alive,
        Dead,
    }

    [Flags]
    public enum ActorState
    {
        None = 0,
        Silenced = 1,
        Rooted = 2,
    }
}

using System;

namespace Combat.Common
{
    public enum ConsciousState
    {
        Alive,
        Downed,
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

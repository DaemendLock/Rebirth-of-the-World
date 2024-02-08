﻿namespace Adapters.Combat
{
    public enum ServerCommand : byte
    {
        Cast,
        Move,
        Stop,
        Target,
        CreateUnit,
        StartCombat,
        Kill,
        Resurrect,
        TakeDamage
    }
}

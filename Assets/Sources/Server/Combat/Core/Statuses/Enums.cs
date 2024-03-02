using System;

namespace Core.Combat.Statuses
{
    public enum DispellType : byte
    {
        None,
        Magic,
        Bleed
    }

    [Flags]
    public enum AuraFlags
    {
        None = 0,
        IgnorDeath = 1,
        SeparateStatuses = 2,
        DontDestroyOnBreak = 4,
        Hidden = 8,
    }
}

namespace Utils.DataTypes
{
    public enum ActionType : byte
    {
        Dummy,
        HealthModification,
        ApplyAura,
        RemoveAura,
        Kill,
        Resurrect,
        StartCast,
        StopCast,
        ModifyResource,
        Cast,
        Movement
    }
}

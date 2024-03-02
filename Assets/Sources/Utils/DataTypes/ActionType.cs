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
        Precast,
        StopCast,
        ModifyResource,
        Cast,
        Movement
    }
}

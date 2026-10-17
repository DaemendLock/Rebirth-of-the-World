namespace Combat.Common.ValueObjects
{
    public enum DesireCastFailReason
    {
        None,
        NoSkillFound,
    }

    public enum CastFailReason
    {
        Success = 0,
        UnknownSkill,
        OnCooldown,
        CastInProgress,
        NotCastable,
        CantCast,
        Custom
    }
}

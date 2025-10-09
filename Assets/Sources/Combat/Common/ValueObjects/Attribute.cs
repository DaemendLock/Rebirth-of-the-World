namespace Combat.Common.ValueObjects
{
    public enum ActionState : byte
    {
        Inactive,
        Startup,
        Active,
        Gap,
        Recovery,
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

    public enum Attribute : int
    {
        Atk,
        Spellpower,
        Haste,
        Lethality,
        Versality,
        LIFESTEAL,
        Speed,
        AOERESIST,
        Endurance,
        OutcomeDamage,
        IncomeDamage,
        OutcomeHealing,
        IncomeHealing,
        BLOCK,
        EVADE,
        PARRY,
    }
}

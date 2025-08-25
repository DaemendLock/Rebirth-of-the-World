namespace Combat.Local.Domain.API.DTO
{
    public readonly ref struct DamageData
    {
        public readonly float Damage;
        public readonly ScriptedSkill Source;
        public readonly Unit Attacker;
        public readonly DamageFlags Flags;

        public DamageData(Unit attacker, ScriptedSkill source, float damage, DamageFlags flags)
        {
            Damage = damage;
            Source = source;
            Attacker = attacker;
            Flags = flags;
        }
    }
}

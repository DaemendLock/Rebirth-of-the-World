using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Services
{
    public interface ISkillCastService
    {
        void Cast(EntityId caster, SkillId skill);
    }
}

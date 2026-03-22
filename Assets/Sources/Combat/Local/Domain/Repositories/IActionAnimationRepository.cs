using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Repositories
{
    public interface ISkillRepository
    {
        void Create(Skill skill);

        Skill Get(SkillId id, EntityId? caster);
    }
}

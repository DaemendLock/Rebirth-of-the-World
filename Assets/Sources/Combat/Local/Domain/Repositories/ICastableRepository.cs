using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Repositories
{
    public interface ICastableRepository
    {
        void Create(int slot, EntityId caster, SkillId skillId);
        SkillId Get(EntityId owner, int slot);
        void Update(int slot, EntityId caster, SkillId skillId);
        void Delete(EntityId caster, int slot);
    }
}

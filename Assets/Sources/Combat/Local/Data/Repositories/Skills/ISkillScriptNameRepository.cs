using Combat.Common.ValueObjects;

namespace Combat.Local.Data.Repositories
{
    public interface ISkillScriptNameRepository
    {
        void Add(SkillId id, string name);
        string Get(SkillId id);
        void Delete(SkillId id);
    }
}

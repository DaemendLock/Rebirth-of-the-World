using System.Reflection;

using Server.Combat.Domain.Skills;

namespace Server.Combat.Infrastructure.Repositories
{
    public interface ISkillScriptConstructorRepository
    {
        public bool TryGet(SkillId skillId, out ConstructorInfo constructorInfo);
    }
}

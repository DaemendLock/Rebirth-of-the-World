using System.Collections.Generic;
using System.Reflection;

using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Skills;
using Server.Combat.Infrastructure.Factories;
using Server.Combat.Infrastructure.Repositories;

namespace Server.Combat.Infrastructure.Implementations.Factories
{
    public class ReflectionSkillScriptFactory : ISkillScriptFactory
    {
        private readonly ISkillScriptConstructorRepository _skillConstructorRepository;

        private readonly Dictionary<int, ConstructorInfo> _constructorsCache;
        private readonly object[] _arguments = new object[1];

        public ReflectionSkillScriptFactory(ISkillScriptConstructorRepository skillConstructorRepository)
        {
            _skillConstructorRepository = skillConstructorRepository;

            _constructorsCache = new();
        }

        public ISkillScript Create(Skill skill, Unit caster)
        {
            SkillId skillId = skill.Id;

            if (_constructorsCache.TryGetValue(skillId.Value, out ConstructorInfo constructorInfo) == false)
            {
                if (_skillConstructorRepository.TryGet(skillId, out constructorInfo) == false)
                {
                    return null;
                }

                _constructorsCache[skillId.Value] = constructorInfo;
            }

            SkillScriptContext skillCastHandlerContext = new(caster, skill);
            _arguments[0] = skillCastHandlerContext;
            return (ISkillScript) constructorInfo.Invoke(_arguments);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Combat.Local.Data.Yes;

using Server.Combat.Data.Entities;
using Server.Combat.Data.Repositories;
using Server.Combat.Domain.DTO;
using Server.Combat.Domain.Implementations.Utils;
using Server.Combat.Domain.Skills;
using Server.Combat.Infrastructure.Repositories;

namespace Server.Combat.Infrastructure.Implementations.Repositories
{
    public class SkillConstructorRepository : ISkillScriptConstructorRepository
    {
        private readonly ISkillDataRepository _skillDataRepository;

        private readonly Type _scriptType = typeof(ISkillScript);
        private readonly Type[] _constructorArgumentsType;

        private readonly Dictionary<string, ConstructorInfo> _constructorsByName;
        private readonly Dictionary<int, ConstructorInfo> _constructorsById;

        public SkillConstructorRepository(ISkillDataRepository skillDataRepository)
        {
            _constructorArgumentsType = new Type[] { typeof(SkillScriptContext) };
            _constructorsById = new();
            _constructorsByName = new();
            _skillDataRepository = skillDataRepository;

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(ISkillScript).IsAssignableFrom(value)))
            {
                Add(type);
            }
        }

        public void Add(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                return;
            }

            if (_scriptType.IsAssignableFrom(type) == false)
            {
                UnityEngine.Debug.LogError($"Can't register skill script not implementing {_scriptType}");
                return;
            }

            ConstructorInfo constructor = type.GetConstructor(_constructorArgumentsType);

            if (constructor == null)
            {
                UnityEngine.Debug.LogError($"Skill script {type.Name} has no valid constructors.");
                return;
            }

            RegisterConstructorBySkillsId(type, constructor);
            RegisterConstructorByName(type, constructor);
        }

        public bool TryGet(SkillId skillId, out ConstructorInfo constructor)
        {
            SkillData skillData = _skillDataRepository.Get(skillId);

            if (skillData == null)
            {
                constructor = null;
                return false;
            }

            return _constructorsByName.TryGetValue(skillData.ScriptName, out constructor) || _constructorsById.TryGetValue(skillData.Id, out constructor);
        }

        private void RegisterConstructorBySkillsId(Type type, ConstructorInfo constructor)
        {
            IEnumerable<ScriptSkillIdAttribute> skillScriptAttributes = type.GetCustomAttributes<ScriptSkillIdAttribute>();

            if (skillScriptAttributes == null || !skillScriptAttributes.Any())
            {
                return;
            }

            foreach (ScriptSkillIdAttribute skillScriptAttribute in skillScriptAttributes)
            {
                _constructorsById[skillScriptAttribute.SkillId] = constructor;
            }
        }

        private void RegisterConstructorByName(Type type, ConstructorInfo constructor)
        {
            SkillScriptNameAttribute skillScriptNameAttribute = type.GetCustomAttribute<SkillScriptNameAttribute>();

            if (skillScriptNameAttribute == null)
            {
                return;
            }

            _constructorsByName[skillScriptNameAttribute.Name] = constructor;
        }
    }
}

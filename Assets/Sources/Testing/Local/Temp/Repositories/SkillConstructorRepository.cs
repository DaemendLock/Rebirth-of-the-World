using System;
using System.Collections.Generic;
using System.Reflection;

using Combat.Common.ValueObjects;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.API.Skills;

namespace Testing.Local.Temp.Factories
{
    public class SkillConstructorRepository : ITypeRepository<SkillId>
    {
        private readonly Type _targetType;
        private readonly ISkillScriptNameRepository _skillScriptNameRepository;
        private readonly Dictionary<string, Type> _typesByName;

        public SkillConstructorRepository(ISkillScriptNameRepository skillScriptNameRepository, Type targetType)
        {
            _typesByName = new();
            _skillScriptNameRepository = skillScriptNameRepository;
            _targetType = targetType;
        }

        public void Register(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                return;
            }

            if (_targetType.IsAssignableFrom(type) == false)
            {
                UnityEngine.Debug.LogError($"Can't register skill script not implementing {_targetType}");
                return;
            }

            SkillScriptNameAttribute skillScriptNameAttribute = type.GetCustomAttribute<SkillScriptNameAttribute>();

            if (skillScriptNameAttribute == null)
            {
                //UnityEngine.Debug.LogError($"Skill script {type.Name} has no name assigned.");
                return;
            }

            _typesByName[skillScriptNameAttribute.Name] = type;
            //ConstructorInfo constructor = type.GetConstructor(_constructorArgumentsType);

            //if (skillScriptNameAttribute == null)
            //{
            //    return;
            //}

            //_constructorsByName[skillScriptNameAttribute.Name] = constructor;
        }

        public bool TryGet(SkillId id, out Type type)
        {
            string name = _skillScriptNameRepository.Get(id);

            if (name == null)
            {
                type = default;
                return false;
            }

            return _typesByName.TryGetValue(name, out type);
        }
    }
}

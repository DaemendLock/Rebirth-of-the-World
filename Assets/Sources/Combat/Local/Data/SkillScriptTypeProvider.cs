using System;
using System.Collections.Generic;
using System.Reflection;

using Combat.API.Skills;

namespace Combat.Local.Data.Databases
{
    public class SkillScriptTypeProvider
    {
        private readonly Type _targetType;
        private readonly Dictionary<string, Type> _typesByName;

        public SkillScriptTypeProvider(Type targetType)
        {
            _targetType = targetType;

            _typesByName = new();
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

        public bool TryGet(string name, out Type type)
        {
            if (name == null)
            {
                type = default;
                return false;
            }

            return _typesByName.TryGetValue(name, out type);
        }
    }
}

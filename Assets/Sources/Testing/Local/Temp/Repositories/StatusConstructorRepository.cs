using System;
using System.Collections.Generic;
using System.Reflection;

using Combat.Common.ValueObjects;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.API.Statuses;

namespace Testing.Local.Temp.Factories
{
    public class StatusConstructorRepository : ITypeRepository<StatusName>
    {
        private readonly Type _scriptType;
        private readonly Dictionary<StatusName, Type> _typesByName;

        public StatusConstructorRepository(Type scriptType)
        {
            _scriptType = scriptType;
            _typesByName = new();
        }

        public void Register(Type type)
        {
            if (type.IsAbstract || type.IsInterface)
            {
                return;
            }

            if (_scriptType.IsAssignableFrom(type) == false)
            {
                UnityEngine.Debug.LogError($"Can't register status: not implementing {_scriptType}");
                return;
            }

            StatusScriptNameAttribute skillScriptNameAttribute = type.GetCustomAttribute<StatusScriptNameAttribute>();

            if (skillScriptNameAttribute == null)
            {
                //UnityEngine.Debug.LogError($"Status script {type.Name} has no name assigned.");
                return;
            }

            _typesByName[skillScriptNameAttribute.Name] = type;

            //ConstructorInfo constructor = type.GetConstructor(_constructorArgumentsType);

            //if (constructor == null)
            //{
            //    UnityEngine.Debug.LogError($"Skill script {type.Name} has no valid constructors.");
            //    return;
            //}

            //if (skillScriptNameAttribute == null)
            //{
            //    return;
            //}

            //_constructorsByName[skillScriptNameAttribute.Name] = constructor;
        }

        public bool TryGet(StatusName key, out Type type) => _typesByName.TryGetValue(key, out type);
    }
}

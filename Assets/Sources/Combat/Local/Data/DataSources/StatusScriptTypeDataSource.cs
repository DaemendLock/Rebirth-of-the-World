using Combat.API.Statuses;
using Combat.Common.Primitives;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Scripting.Idk;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Combat.Local.Data.Databases
{
    public class StatusScriptTypeDataSource : IStatusDataBase, IStatusScriptTypeProvider
    {
        private readonly Type _scriptType;
        private readonly Dictionary<StatusType, Type> _typesByName;

        public StatusScriptTypeDataSource(Type scriptType)
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

        public bool TryGet(StatusType key, out Type type) => _typesByName.TryGetValue(key, out type);
    }
}

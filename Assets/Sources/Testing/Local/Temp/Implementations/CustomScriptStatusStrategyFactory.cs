using Combat.API.Adapters;
using Combat.API.Controllers.Misc;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Databases;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Temp.Domain.Implementations
{
    public class CustomScriptStatusStrategyFactory : IStatusStrategyFactory
    {
        private readonly StatusScriptTypeDataSource _statusApiTypeProvider;
        private readonly CharacterApiAdapter _unitApiAdapter;
        private readonly StatusApiAdapter _statusApiFactory;
        private readonly SkillApiAdapter _skillApiProvider;
        private readonly SceneApiAdapter _sceneApiProvider;
        private readonly IStatusRepository _statusRepository;

        public CustomScriptStatusStrategyFactory(CharacterApiAdapter unitApiAdapter, SkillApiAdapter skillApiProvider, SceneApiAdapter sceneApiProvider, StatusApiAdapter statusApiFactory, IStatusRepository statusRepository)
        {
            _unitApiAdapter = unitApiAdapter;
            _skillApiProvider = skillApiProvider;
            _sceneApiProvider = sceneApiProvider;

            _statusApiTypeProvider = new StatusScriptTypeDataSource(typeof(StatusScript));

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(StatusScript).IsAssignableFrom(value)))
            {
                _statusApiTypeProvider.Register(type);
            }

            _statusApiFactory = statusApiFactory;
            _statusRepository = statusRepository;
        }

        public bool CanHandle(StatusName name) => _statusApiTypeProvider.TryGet(name, out Type _);

        public IStatusStrategy Create(StatusId id, StatusName name, EntityId parent)
        {
            if (TryCreateEmpty(name, out StatusScript script) == false)
            {
                return null;
            }

            return new DataDrivenStatusStrategy(id, script, _unitApiAdapter, _skillApiProvider, _sceneApiProvider, _statusApiFactory, _statusRepository);
        }

        public bool TryCreateEmpty(StatusName name, out StatusScript value)
        {
            if (_statusApiTypeProvider.TryGet(name, out Type targetType) == false)
            {
                value = default;
                return false;
            }

            value = (StatusScript)FormatterServices.GetUninitializedObject(targetType);
            return true;
        }
    }
}

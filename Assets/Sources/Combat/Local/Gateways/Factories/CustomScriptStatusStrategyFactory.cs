using Combat.API.Adapters;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Gateways.Repositories.Statuses;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Combat.Local.Gateways.Factories
{
    public class CustomScriptStatusStrategyFactory : IStatusPropertyContainerFactory
    {
        private readonly IStatusDataBase _statusApiTypeProvider;
        private readonly CharacterApiAdapter _unitApiAdapter;
        private readonly StatusApiAdapter _statusApiFactory;
        private readonly AbilityApiAdapter _skillApiProvider;
        private readonly SceneApiAdapter _sceneApiProvider;
        private readonly IStatusRepository _statusRepository;

        public CustomScriptStatusStrategyFactory(CharacterApiAdapter unitApiAdapter, AbilityApiAdapter skillApiProvider, SceneApiAdapter sceneApiProvider, StatusApiAdapter statusApiFactory, IStatusRepository statusRepository, IStatusDataBase statusApiTypeProvider)
        {
            _unitApiAdapter = unitApiAdapter;
            _skillApiProvider = skillApiProvider;
            _sceneApiProvider = sceneApiProvider;

            _statusApiTypeProvider = statusApiTypeProvider;

            _statusApiFactory = statusApiFactory;
            _statusRepository = statusRepository;

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(StatusScript).IsAssignableFrom(value)))
            {
                _statusApiTypeProvider.Register(type);
            }
        }

        public bool CanHandle(StatusType name) => _statusApiTypeProvider.TryGet(name, out Type _);

        public IStatusPropertyContainer Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source)
        {
            if (TryCreateEmpty(name, out StatusScript script) == false)
            {
                return null;
            }

            script.Init(_statusApiFactory.Adaptee(id, parent, source));
            return new ApiScriptStatusPropertyContainer(id, script, _unitApiAdapter, _skillApiProvider, _sceneApiProvider, _statusApiFactory, _statusRepository);
        }

        public bool TryCreateEmpty(StatusType name, out StatusScript value)
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

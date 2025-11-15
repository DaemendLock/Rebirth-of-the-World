using Combat.API;
using Combat.API.Controllers;
using Combat.API.Controllers.Factories;
using Combat.API.Scripting;

using Combat.Common.ValueObjects;

using Combat.Local.Controllers;
using Combat.Local.Data.Databases;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Combat.Local.Data.Factories
{
    public class StatusApiFactory : IStatusApiFactory
    {
        private readonly StatusScriptTypeDataSource _statusApiTypeProvider;

        private readonly CharacterApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly SceneApiProvider _sceneApiProvider;
        private readonly StatusController _statusController;

        public StatusApiFactory(CharacterApiProvider unitApiRepository, SkillApiProvider skillApiRepository, StatusController statusController, SceneApiProvider sceneApiProvider)
        {
            _unitApiProvider = unitApiRepository;
            _skillApiProvider = skillApiRepository;
            _sceneApiProvider = sceneApiProvider;
            _statusController = statusController;

            _statusApiTypeProvider = new StatusScriptTypeDataSource(typeof(StatusScript));

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(StatusScript).IsAssignableFrom(value)))
            {
                _statusApiTypeProvider.Register(type);
            }
        }

        public StatusApi Create(StatusId id, EntityId parent, StatusName name, SkillId? source, EntityId? caster)
        {
            Unit parentApi = _unitApiProvider.Get(parent) ?? throw new InvalidOperationException("Can't create api status for parent with non-registered api.");

            if (TryCreateEmpty(name, out StatusScript script) == false)
            {
                return default;
            }

            SkillApi skill = null;

            if (source.HasValue)
            {
                skill = _skillApiProvider.Get(source.Value, caster);
            }

            return new(id, parentApi, skill, _sceneApiProvider.Get(), _statusController, script);
        }

        private bool TryCreateEmpty(StatusName name, out StatusScript value)
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

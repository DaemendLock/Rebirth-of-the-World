using Combat.API;
using Combat.API.Controllers;
using Combat.API.Controllers.Factories;
using Combat.API.Scripting;

using Combat.Common.ValueObjects;
using Combat.Local.Controllers;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Combat.Api.Controllers.Factories
{
    public interface IStatusScriptTypeProvider
    {
        void Register(Type type);
        bool TryGet(StatusName name, out Type type);
    }

    public class StatusApiFactory : IStatusApiFactory
    {
        private readonly IStatusScriptTypeProvider _scriptTypeProvider;
        private readonly CharacterApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly SceneApiProvider _sceneApiProvider;
        private readonly StatusController _statusController;

        public StatusApiFactory(CharacterApiProvider unitApiRepository, SkillApiProvider skillApiRepository, StatusController statusController, SceneApiProvider sceneApiProvider, IStatusScriptTypeProvider scriptTypeProvider)
        {
            _unitApiProvider = unitApiRepository;
            _skillApiProvider = skillApiRepository;
            _sceneApiProvider = sceneApiProvider;
            _statusController = statusController;
            _scriptTypeProvider = scriptTypeProvider;

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(StatusScript).IsAssignableFrom(value)))
            {
                _scriptTypeProvider.Register(type);
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
            if (_scriptTypeProvider.TryGet(name, out Type targetType) == false)
            {
                value = default;
                return false;
            }

            value = (StatusScript)FormatterServices.GetUninitializedObject(targetType);
            return true;
        }
    }
}

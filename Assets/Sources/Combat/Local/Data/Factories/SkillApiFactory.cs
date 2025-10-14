using Combat.API;
using Combat.API.Controllers;
using Combat.API.Controllers.Factories;
using Combat.API.Scripting;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Databases;

using Data.Entities;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Combat.Local.Data.Factories
{
    public class SkillApiFactory : ISkillApiFactory
    {
        private readonly CharacterApiProvider _unitApiProvider;
        private readonly SceneApiProvider _sceneProvider;

        private readonly SkillDataBase _skillDataBase;
        private readonly SkillScriptTypeProvider _skillScriptTypeProvider;

        public SkillApiFactory(CharacterApiProvider unitApiRepository, SkillDataBase skillDataBase, SceneApiProvider scene)
        {
            _unitApiProvider = unitApiRepository;
            _skillDataBase = skillDataBase;
            _sceneProvider = scene;

            _skillScriptTypeProvider = new SkillScriptTypeProvider(typeof(SkillScript));

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(SkillScript).IsAssignableFrom(value)))
            {
                _skillScriptTypeProvider.Register(type);
            }
        }

        public SkillApi Create(SkillId skillId, EntityId? owner)
        {
            ISkillData skillData = _skillDataBase.Get(skillId);
            SkillScript script = CreateUninitiailizedScript(skillData.ScriptName);

            Unit ownerApi = owner.HasValue ? _unitApiProvider.Get(owner.Value) ?? throw new InvalidOperationException("Can't create script api for non-registred unit " + owner.Value) : null;
            SceneApi sceneApi = _sceneProvider.Get();

            return new(skillId, skillData.Flags, ownerApi, sceneApi, script);
        }

        private SkillScript CreateUninitiailizedScript(string scriptName)
        {
            if (_skillScriptTypeProvider.TryGet(scriptName, out Type type) == false)
            {
                throw new InvalidOperationException($"No script assigned found with name \"{scriptName}\".");
            }

            return (SkillScript)FormatterServices.GetUninitializedObject(type);
        }
    }
}

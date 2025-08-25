using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Combat.Common.ValueObjects;
using Combat.Local.Data.Repositories;

using Combat.Local.Domain.API;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.IDK;
using Combat.Local.Domain.Repositories;

namespace Testing.Local.Temp.Factories
{
    public class SkillApiFactory
    {
        private delegate void Init(ScriptedSkillContext context);

        private readonly UnitApiRepository _unitApiRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly ITypeRepository<SkillId> _skillScriptTypeRepository;

        private readonly MethodInfo _initMethod;
        private readonly Dictionary<SkillId, Type> _cachedScripts;

        private readonly Scene _scene;

        public SkillApiFactory(ISkillScriptNameRepository skillScriptNameRepository, ISkillRepository skillRepository, UnitApiRepository unitAdapter)
        {
            _skillScriptTypeRepository = new SkillConstructorRepository(skillScriptNameRepository, typeof(ScriptedSkill));

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(ScriptedSkill).IsAssignableFrom(value)))
            {
                _skillScriptTypeRepository.Register(type);
            }

            _cachedScripts = new();
            _skillRepository = skillRepository;
            _unitApiRepository = unitAdapter;
            _scene = new(_unitApiRepository);
            _initMethod = typeof(ScriptedSkill).GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public ScriptedSkill Create(EntityId caster, SkillId skillId)
        {
            Unit unitApi = _unitApiRepository.Get(caster) ?? throw new InvalidOperationException("Can't create script api for non-registred unit " + caster.Value);

            if (_cachedScripts.TryGetValue(skillId, out Type type) == false)
            {
                if (_skillScriptTypeRepository.TryGet(skillId, out type) == false)
                {
                    throw new InvalidOperationException($"No script assigned to skill {skillId}.");
                }

                _cachedScripts.Add(skillId, type);
            }

            ScriptedSkillContext context = new(_skillRepository.Get(skillId), unitApi, _scene);
            object result = FormatterServices.GetUninitializedObject(type);
            Init value = (Init) _initMethod.CreateDelegate(typeof(Init), result);
            value.Invoke(context);
            return (ScriptedSkill) result;
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

using Combat.Common.ValueObjects;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.API;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.IDK;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Testing.Local.Temp.Factories
{
    public class StatusApiFactory
    {
        private delegate void Init(ScriptedStatusContext context, IStatusRepository statusRepository);

        private readonly IStatusRepository _statusRepository;
        private readonly ITypeRepository<StatusName> _typeRepository;

        private readonly MethodInfo _initMethod;

        private readonly UnitApiRepository _unitApiRepository;
        private readonly SkillApiRepository _skillApiRepository;

        public StatusApiFactory(IStatusRepository statusRepository, UnitApiRepository unitApiRepository, SkillApiRepository skillApiRepository)
        {
            _statusRepository = statusRepository;
            _unitApiRepository = unitApiRepository;
            _skillApiRepository = skillApiRepository;

            _typeRepository = new StatusConstructorRepository(typeof(StatusApi));
            _initMethod = typeof(StatusApi).GetMethod("Init", BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(StatusApi).IsAssignableFrom(value)))
            {
                _typeRepository.Register(type);
            }
        }

        public StatusApi Create(StatusId id)
        {
            if (_statusRepository.TryGet(id, out Status data) == false)
            {
                return default;
            }

            if (_typeRepository.TryGet(data.Name, out Type targetType) == false)
            {
                return default;
            }

            ScriptedSkill skill = null;

            if (data.Caster != null && data.Source != null)
            {
                skill = _skillApiRepository.Get(data.Caster.Value, data.Source.Value);
            }

            ScriptedStatusContext context = new(data.Id, data.Name, _unitApiRepository.Get(data.Parent), skill, data.Duration, data.StackCount);

            object result = FormatterServices.GetUninitializedObject(targetType);
            Init value = (Init) _initMethod.CreateDelegate(typeof(Init), result);
            value.Invoke(context, _statusRepository);
            return (StatusApi) result;
        }
    }
}

using Combat.API;
using Combat.API.Controllers;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Factories;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;

namespace Testing.Local.Temp.DomainOutputs
{
    public class UnitCreationHandler : ICreateUnitEventHandler
    {
        private readonly UnitApiFactory _unitApiFactory;
        private readonly UnitApiProvider _unitApiRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly SkillApiProvider _skillApiProvider;
        private readonly SkillApiFactory _skillApiFactory;

        public UnitCreationHandler(UnitApiFactory unitApiFactory, UnitApiProvider unitApiRepository, ISkillOwnerRepository skillOwnerRepository, SkillApiProvider skillApiProvider, SkillApiFactory skillApiFactory)
        {
            _unitApiFactory = unitApiFactory;
            _unitApiRepository = unitApiRepository;
            _skillOwnerRepository = skillOwnerRepository;
            _skillApiProvider = skillApiProvider;
            _skillApiFactory = skillApiFactory;
        }

        public void HandleEvent(EntityId id)
        {
            Unit unitApi = _unitApiFactory.Create(id);
            _unitApiRepository.Register(unitApi);

            SkillOwner skillOwner = _skillOwnerRepository.Get(id);

            foreach (var skill in skillOwner.GetAll())
            {
                if (_skillApiProvider.Get(skill, id) != null)
                {
                    return;
                }

                var value = _skillApiFactory.Create(skill, id);
                _skillApiProvider.Register(value);
            }
        }
    }

    public class ActionStateChangeHandler : IActionStateChangeEventHandler
    {
        private readonly SkillApiProvider _skillApiProvider;

        public ActionStateChangeHandler(SkillApiProvider skillApiProvider)
        {
            _skillApiProvider = skillApiProvider;
        }

        public void HandleEvent(EntityId actorId, SkillId skillId, ActionState newState)
        {
            SkillApi skillApi = _skillApiProvider.Get(skillId, actorId);

            if (skillApi == null || (skillApi.TryGetProperty(out ICastStateChangeHandler handler) == false))
            {
                return;
            }

            switch (newState)
            {
                case ActionState.Startup:
                    handler.OnStartup();
                    break;

                case ActionState.Active:
                    handler.OnActive();
                    break;

                case ActionState.Gap:
                    handler.OnGapStart();
                    break;

                case ActionState.Recovery:
                    handler.OnRecovery();
                    break;

                case ActionState.Inactive:
                    handler.OnEnds();
                    break;

                default:
                    throw new System.InvalidOperationException($"Can't find skill state \"{newState}\".");
            }
        }
    }
}

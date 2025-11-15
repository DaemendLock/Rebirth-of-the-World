using CastStateSkill;

using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Data.Databases;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Presentation.Components;
using Combat.Local.Presentation.Factories;

using Data.Entities.Components;

using UnityEngine;

namespace Testing.Local.Temp.Factories
{
    public class ActionFactory : IActionFactory
    {
        private readonly ISkillRepository _skillRepository;
        private readonly ISkillDataBase _skillDataBase;

        public ActionFactory(ISkillRepository skillRepository, ISkillDataBase skillDataBase)
        {
            _skillRepository = skillRepository;
            _skillDataBase = skillDataBase;
        }

        public IAction CreateCastAction(ActionId actionId, EntityId actorId)
        {
            Skill skill = _skillRepository.Get(new(actionId.Value), actorId);
            ActionFlags flags = ActionFlags.None;

            if (skill.AllowMoment)
            {
                flags |= ActionFlags.AllowMovement;
            }

            if(skill.Flags.HasFlag(SkillFlags.CanHold))
            {
                flags |= ActionFlags.Holdable;
            }

            IFrameData frameData = _skillDataBase.GetFrameData(actionId);

            return new CastAction(actionId, flags, frameData);
        }
    }

    public class CharacterViewFactory : ICharacterViewFactory
    {
        private readonly HitController _hitController;
        private readonly CharacterModelProvider _characterModelProvider;

        public CharacterViewFactory(CharacterModelProvider characterModelRepository, HitController hitController)
        {
            _characterModelProvider = characterModelRepository;

            _hitController = hitController;
        }

        public CharacterView Create(EntityId id, ModelName modelName)
        {
            GameObject prefab = _characterModelProvider.Get(modelName);

            if (prefab == null)
            {
                throw new System.InvalidOperationException();
            }

            GameObject gameObject = Object.Instantiate(prefab);
            CharacterView result = gameObject.GetComponent<CharacterView>() ?? gameObject.AddComponent<CharacterView>();
            return result;
        }

        public void Init(EntityId entityId, CharacterView target)
        {
            Hitbox[] hitboxes = target.GetComponentsInChildren<Hitbox>();
            Hurtbox[] hurtboxes = target.GetComponentsInChildren<Hurtbox>();

            foreach (Hitbox hitboxData in hitboxes)
            {
                Collider collider = hitboxData.GetComponent<Collider>();
                _hitController.CreateHitbox(collider, hitboxData.Type, entityId);
                Object.Destroy(hitboxData);
            }

            foreach (Hurtbox hurtboxData in hurtboxes)
            {
                Collider collider = hurtboxData.GetComponent<Collider>();
                _hitController.CreateHurtbox(collider, hurtboxData.Type, entityId);
                Object.Destroy(hurtboxData);
            }
        }
    }
}

using Combat.Common.ValueObjects;
using Combat.Local.Data.Repositories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services;
using Combat.Local.Infrastructure.Presenters;

using UnityEngine;

namespace Combat.Local.Infrastructure.Controllers
{
    public class UnitController
    {
        private readonly EntityId _id;
        private readonly ICastableRepository _castableRepository;
        private readonly IUnitPresenter _unitPresener;
        private readonly ISkillCastService _skillCastService;
        private readonly IMovementService _movementService;
        private readonly ISkillAnimationRepository _skillAnimationRepository;

        public UnitController(EntityId id, IUnitPresenter unitPresener, ISkillAnimationRepository skillDataRepository, ICastableRepository castableRepository, IMovementService movementService, ISkillCastService skillCastService)
        {
            _id = id;
            _unitPresener = unitPresener;
            _skillAnimationRepository = skillDataRepository;
            _movementService = movementService;
            _skillCastService = skillCastService;
            _castableRepository = castableRepository;
        }

        public void MoveInDirection(Vector2 relativeDirection)
        {
            if (_movementService.TryMoveInDirection(_id, relativeDirection, out Vector3 velocity) == false)
            {
                return;
            }

            _unitPresener.Velocity = new(velocity.x, _unitPresener.Velocity.y, velocity.z);
        }

        public void LookInDirection(Vector2 direction) => throw new System.NotImplementedException();

        public void Cast(int slot)
        {
            SkillId skillId;

            try
            {
                skillId = _castableRepository.Get(_id, slot);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                return;
            }

            _skillCastService.Cast(_id, skillId);

            AnimationClip clip = _skillAnimationRepository.Get(skillId);

            if (clip == null)
            {
                return;
            }

            _unitPresener.PlayAnimation(clip);
        }

        public void Update()
        {
            _movementService.SetPosition(_id, _unitPresener.Position);

            Vector2 movement = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movement += (Vector2.up);
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement += (Vector2.down);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                movement += (Vector2.left);
            }
            if (Input.GetKey(KeyCode.E))
            {
                movement += (Vector2.right);
            }

            MoveInDirection(movement);

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Cast(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Cast(1);
            }
        }
    }
}

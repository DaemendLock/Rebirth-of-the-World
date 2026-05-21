using Combat.API;
using Combat.API.Adapters;
using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories.Skills
{
    public class ApiScriptDrivenAbilityPropertyContainer : IAbilityPropertyContainer
    {
        private readonly SkillId _skillType;
        private readonly SkillScript _script;

        private readonly ISkillCastStrategy _castStrategy;
        private readonly ISkillHitStrategy _hitStrategy;
        private readonly ISkillActionStateChangeStrategy _actionStateChangeStrategy;
        private readonly ILockTargetStrategy _lockTargetStrategy;

        public ApiScriptDrivenAbilityPropertyContainer(SkillId skillId, SkillScript customSkillStrategy, CharacterApiAdapter characterApiAdapter, AbilityApiAdapter skillApiProvider, SceneApiAdapter sceneApiProvider)
        {
            _skillType = skillId;
            _script = customSkillStrategy;

            if (_script is ICastableSkill castable)
            {
                _castStrategy = new DataDrivenCastStrategy(castable);
            }

            if (_script is IHitHandler hitHandler)
            {
                _hitStrategy = new DataDrivenHitStrategy(hitHandler, characterApiAdapter);
            }

            if (_script is ICastStateChangeHandler actionStateChangeHandler)
            {
                _actionStateChangeStrategy = new DataDrivenActionStateChangeStrategy(actionStateChangeHandler);
            }

            if (_script is ITargettableSkill lockTargetHandler)
            {
                _lockTargetStrategy = new DataDrivenLockTargetStrategy(characterApiAdapter, lockTargetHandler);
            }
        }

        public void Give()
        {
        }

        public void Remove()
        {
        }

        public bool TryGet(out SkillCastEffect result)
        {
            if (_castStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_skillType, _castStrategy);
            return true;
        }

        public bool TryGet(out ISkillHitStrategy result)
        {
            if (_hitStrategy == null)
            {
                result = default;
                return false;
            }

            result = _hitStrategy;
            return true;
        }

        public bool TryGet(out SkillActionStateChangeEffect result)
        {
            if (_actionStateChangeStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_skillType, _actionStateChangeStrategy);
            return true;
        }

        public bool TryGet(out LockTargetSkillEffect result)
        {
            if (_lockTargetStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_skillType, _lockTargetStrategy);
            return true;
        }

        private class DataDrivenCastStrategy : ISkillCastStrategy
        {
            private readonly ICastableSkill _castableSkill;

            public DataDrivenCastStrategy(ICastableSkill castableSkill)
            {
                _castableSkill = castableSkill;
            }

            public CastFailReason CanCast() => _castableSkill.CanCast();

            public void Execute() => _castableSkill.OnCast();
        }

        private class DataDrivenHitStrategy : ISkillHitStrategy
        {
            private readonly IHitHandler _handler;
            private readonly List<UnitId> _hittedTargets;
            private readonly CharacterApiAdapter _characterApiProvider;

            public DataDrivenHitStrategy(IHitHandler hitHandler, CharacterApiAdapter characterApiProvider)
            {
                _handler = hitHandler;
                _characterApiProvider = characterApiProvider;
                _hittedTargets = new();
            }

            public void Reset() => _hittedTargets.Clear();

            public void HandleHit(Domain.ValueObjects.HitRecord record)
            {
                try
                {
                    if (_hittedTargets.Contains(record.HurtboxOwner))
                    {
                        return;
                    }

                    _hittedTargets.Add(record.HurtboxOwner);
                    HitRecord @event = CreateHitEvent(record);
                    _handler.OnHit(@event);
                }
                catch (System.Exception exception)
                {
                    UnityEngine.Debug.LogException(exception);
                }
            }

            private HitRecord CreateHitEvent(Domain.ValueObjects.HitRecord record)
            {
                Unit source = _characterApiProvider.Adaptee(record.HitboxOwner);
                Unit target = _characterApiProvider.Adaptee(record.HurtboxOwner);

                return new(source, record.HitboxType, target, record.HurtboxType, record.Location);
            }
        }

        private class DataDrivenActionStateChangeStrategy : ISkillActionStateChangeStrategy
        {
            private readonly ICastStateChangeHandler _handler;

            public DataDrivenActionStateChangeStrategy(ICastStateChangeHandler handler)
            {
                _handler = handler;
            }

            public void Handle(ActionState newState)
            {
                switch (newState)
                {
                    case ActionState.Startup:
                        _handler.OnStartup();
                        break;

                    case ActionState.Active:
                        _handler.OnActive();
                        break;

                    case ActionState.Gap:
                        _handler.OnGapStart();
                        break;

                    case ActionState.Recovery:
                        _handler.OnRecovery();
                        break;

                    case ActionState.Inactive:
                        _handler.OnEnds();
                        break;

                    default:
                        throw new System.InvalidOperationException($"Can't find skill state \"{newState}\".");
                }
            }
        }

        private sealed class DataDrivenLockTargetStrategy : ILockTargetStrategy
        {
            private readonly CharacterApiAdapter _apiAdapter;
            private readonly ITargettableSkill _handler;

            public DataDrivenLockTargetStrategy(CharacterApiAdapter apiAdapter, ITargettableSkill handler)
            {
                _apiAdapter = apiAdapter;
                _handler = handler;
            }

            public bool Handle(UnitId entityId) => _handler.CanTarget(_apiAdapter.Adaptee(entityId));
        }
    }
}

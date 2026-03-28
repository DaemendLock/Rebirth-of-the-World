using Combat.API;
using Combat.API.Adapters;
using Combat.API.DTO;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Skills.Effects;
using Combat.Local.Domain.Entities.Units;

namespace Temp.Domain.Implementations
{
    public class DataDrivenSkillStrategy : ISkillStrategy
    {
        private readonly SkillId _skillId;
        private readonly SkillScript _script;

        private readonly ISkillCastStrategy _castStrategy;
        private readonly ISkillHitStrategy _hitStrategy;
        private readonly ISkillActionStateChangeStrategy _actionStateChangeStrategy;

        public DataDrivenSkillStrategy(SkillId id, SkillScript customSkillStrategy, CharacterApiAdapter characterApiProvider, SkillApiAdapter skillApiProvider, SceneApiAdapter sceneApiProvider)
        {
            _skillId = id;
            _script = customSkillStrategy;

            if (_script is ICastableSkill castable)
            {
                _castStrategy = new DataDrivenCastStrategy(id, castable, characterApiProvider, skillApiProvider, sceneApiProvider);
            }

            if (_script is IHitHandler hitHandler)
            {
                _hitStrategy = new DataDrivenHitStrategy(hitHandler, characterApiProvider);
            }

            if (_script is ICastStateChangeHandler actionStateChangeHandler)
            {
                _actionStateChangeStrategy = new DataDrivenActionStateChangeStrategy(actionStateChangeHandler);
            }
        }

        public void Give(EntityId owner) => throw new System.NotImplementedException();

        public void Remove(EntityId owner) => throw new System.NotImplementedException();

        public bool TryGetEffect(out SkillCastEffect result)
        {
            if (_castStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_skillId, _castStrategy);
            return true;
        }

        public bool TryGetEffect(out SkillHitEffect result)
        {
            if (_hitStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_skillId, _hitStrategy);
            return true;
        }

        public bool TryGetEffect(out SkillActionStateChangeEffect result)
        {
            if (_actionStateChangeStrategy == null)
            {
                result = default;
                return false;
            }

            result = new(_skillId, _actionStateChangeStrategy);
            return true;
        }

        private class DataDrivenCastStrategy : ISkillCastStrategy
        {
            private readonly SkillId _skillId;
            private readonly ICastableSkill _castableSkill;

            private readonly CharacterApiAdapter _characterApiProvider;
            private readonly SkillApiAdapter _skillApiProvider;
            private readonly SceneApiAdapter _sceneApiProvider;

            public DataDrivenCastStrategy(SkillId skillId, ICastableSkill castableSkill, CharacterApiAdapter characterApiProvider, SkillApiAdapter skillApiProvider, SceneApiAdapter sceneApiProvider)
            {
                _skillId = skillId;
                _castableSkill = castableSkill;
                _characterApiProvider = characterApiProvider;
                _skillApiProvider = skillApiProvider;
                _sceneApiProvider = sceneApiProvider;
            }

            public CastFailReason CanCast(EntityId? caster) => _castableSkill.CanCast(CreateCastEvent(caster));

            public void Execute(EntityId? caster) => _castableSkill.OnCast(CreateCastEvent(caster));

            private CastEvent CreateCastEvent(EntityId? caster)
            {
                return new(caster.HasValue ? _characterApiProvider.Adaptee(caster.Value) : null, _skillApiProvider.Adaptee(_skillId, caster), _sceneApiProvider.Get());
            }
        }

        private class DataDrivenHitStrategy : ISkillHitStrategy
        {
            private readonly IHitHandler _handler;
            private readonly CharacterApiAdapter _characterApiProvider;

            public DataDrivenHitStrategy(IHitHandler hitHandler, CharacterApiAdapter characterApiProvider)
            {
                _handler = hitHandler;
                _characterApiProvider = characterApiProvider;
            }

            public void HandleHit(Hitbox hitbox, Hurtbox hurtbox, UnityEngine.Vector3 location)
            {
                try
                {
                    var @event = CreateHitEvent(hitbox, hurtbox, location);
                    _handler.OnHit(@event);
                }
                catch (System.Exception exception)
                {
                    UnityEngine.Debug.LogException(exception);
                }
            }

            private HitRecord CreateHitEvent(Hitbox hitbox, Hurtbox hurtbox, UnityEngine.Vector3 location)
            {
                Unit source = _characterApiProvider.Adaptee(hitbox.Owner);
                Unit target = _characterApiProvider.Adaptee(hurtbox.Owner);

                return new(source, hitbox.Type, target, hurtbox.Type, location);
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
    }
}

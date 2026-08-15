using Combat.API;
using Combat.API.Contexts;
using Combat.API.Skills;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Capabilities.Skills
{
    public interface ISkillHitCapability
    {
        bool Handle(ISkillContext context, in HitRecord record);
    }

    public sealed class OldHandleSkillHitCapability : ISkillHitCapability
    {
        private readonly IHitHandler _handler;
        private readonly CharacterApiAdapter _characterApiAdapter;

        public OldHandleSkillHitCapability(IHitHandler handler, CharacterApiAdapter characterApiAdapter)
        {
            _handler = handler;
            _characterApiAdapter = characterApiAdapter;
        }

        public bool Handle(ISkillContext context, in HitRecord record)
        {
            Unit source = _characterApiAdapter.Adaptee(record.HitboxOwner);
            Unit target = _characterApiAdapter.Adaptee(record.HurtboxOwner);
            API.DTO.HitRecord apiRecord = new(source, record.HitboxType, target, record.HurtboxType, record.Location);
            return _handler.OnHit(apiRecord);
        }
    }
}

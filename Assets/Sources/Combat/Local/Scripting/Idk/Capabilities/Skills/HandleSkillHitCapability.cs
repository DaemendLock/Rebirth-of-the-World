using Combat.API;
using Combat.API.Skills;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Idk.Capabilities.Skills
{
    public readonly ref struct HandleSkillHitCapability
    {
        private readonly IHitHandler _handler;
        private readonly CharacterApiAdapter _characterApiAdapter;

        public HandleSkillHitCapability(IHitHandler handler, CharacterApiAdapter characterApiAdapter)
        {
            _handler = handler;
            _characterApiAdapter = characterApiAdapter;
        }

        public bool Handle(in HitRecord record)
        {
            Unit source = _characterApiAdapter.Adaptee(record.HitboxOwner);
            Unit target = _characterApiAdapter.Adaptee(record.HurtboxOwner);
            Combat.API.DTO.HitRecord apiRecord = new(source, record.HitboxType, target, record.HurtboxType, record.Location);
            return _handler.OnHit(apiRecord);
        }
    }
}

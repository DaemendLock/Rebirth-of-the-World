using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{

    public interface IStatusPropertyContainer
    {
        bool DestroyOnExpire { get; }

        void Apply();
        void Remove();
        void Expire();

        void Tick();

        bool TryGetProperty(out ITakeDamageEffectStrategy property);
        bool TryGetProperty(out IDealDamageEffectStrategy property);
        bool TryGetProperty(out IModifyParentOutgoingDamageStrategy property);
        bool TryGetProperty(out IModifyParentOutgoingHealingStrategy property);
        bool TryGetProperty(out IModifyParentIncomingDamageStrategy property);
        bool TryGetProperty(out IModifyAttributesStrategy property);
        bool TryGetProperty(out IModifyTimeScaleStrategy property);
    }
}

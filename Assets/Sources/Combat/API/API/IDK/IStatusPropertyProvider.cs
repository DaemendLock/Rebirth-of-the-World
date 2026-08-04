using Combat.API.Statuses;

namespace Combat.Local.API.IDK
{
    public interface IStatusPropertyContainer
    {
        bool DestroyOnExpire { get; }

        void Apply();
        void Remove();
        void Expire();

        void Tick();

        bool TryGetProperty(out IIncomingHealDamageHandler property);
        bool TryGetProperty(out IOutgoingHealDamageHandler property);
        bool TryGetProperty(out IOutgoingDamageModifier property);
        bool TryGetProperty(out IOutgoingHealingModifier property);
        bool TryGetProperty(out IIncomingHealDamageModifier property);
        bool TryGetProperty(out IAttributesModifier property);
        bool TryGetProperty(out ITimeScaleModifier property);
    }
}

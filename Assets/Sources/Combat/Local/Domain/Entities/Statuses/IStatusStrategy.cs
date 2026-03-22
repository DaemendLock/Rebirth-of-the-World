namespace Combat.Local.Domain.Entities.Statuses
{
    public interface IStatusStrategy
    {
        bool DestroyOnExpire { get; }

        void Apply();
        void Remove();
        void Expire();

        void Tick();

        bool TryGetEffect(out TakeDamageEffect effect);

        bool TryGetEffect(out DealDamageEffect effect);

        bool TryGetEffect(out ModifyOutgoingDamageEffect effect);

        bool TryGetEffect(out ModifyOutgoingHealingEffect effect);

        bool TryGetEffect(out ModifyIncomingDamageEffect effect);

        bool TryGetEffect(out ModifyAttributesEffect effect);

        bool TryGetEffect(out ModifyTimeScaleEffect effect);
    }
}

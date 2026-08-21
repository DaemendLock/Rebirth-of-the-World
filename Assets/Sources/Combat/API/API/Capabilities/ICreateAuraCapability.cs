using Combat.Common.Primitives;

namespace Combat.API.Capabilities
{
    public readonly struct CreateAuraInfo
    {
        public float Radius { get; }

        public float LingerDuration { get; }
    }

    public interface ICreateAuraCapability
    {
        AuraId Create(CreateAuraInfo info);
        void Remove(AuraId id);
    }
}

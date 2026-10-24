using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Local.Application.DTO;

namespace Combat.Local.Application.OutputPorts
{
    public interface ICharacterDeathHandler
    {
        void Handle(KillRecord record);
    }

    public interface ICharacterConsciousStateOutput
    {
        void Present(UnitId value, ConsciousState state);
    }

    public interface IActorActionUpdateOutput
    {
        void Present(UnitId target, ActionId action, ActionFlags flags, float activeTime);

        void Release(UnitId target);

        void Clear(UnitId unit);
    }

    public readonly ref struct ActionUpdateInfo
    {
        public UnitId Actor { get; }
        public ActionId? ActionId { get; }
        public ActionFlags Flags { get; }
        public float ActiveTime { get; }
    }
}

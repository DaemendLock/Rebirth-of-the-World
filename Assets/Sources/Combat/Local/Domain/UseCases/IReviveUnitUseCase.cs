using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public interface IReviveUnitUseCase
    {
        void Execute(EntityId target, EventSource source);
    }
}

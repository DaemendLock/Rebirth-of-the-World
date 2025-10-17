using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public class KillUnitUseCase
    {
        public void Execute(EntityId target, EventSource source)
        {

        }
    }
    public interface IReviveUnitUseCase
    {
        void Execute(EntityId target, EventSource source);
    }
}

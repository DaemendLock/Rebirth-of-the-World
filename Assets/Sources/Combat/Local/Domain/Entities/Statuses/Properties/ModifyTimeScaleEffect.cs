using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface IModifyTimeScaleStrategy
    {
        float GetModification();
    }
}

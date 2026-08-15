using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IStatusTickHandler
    {
        void Handle(StatusId statusId);
    }
}

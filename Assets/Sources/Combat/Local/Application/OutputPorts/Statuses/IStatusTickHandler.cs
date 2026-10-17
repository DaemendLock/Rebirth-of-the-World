using Combat.Common.Primitives;

namespace Combat.Local.Application.OutputPorts.Statuses
{
    public interface IStatusTickHandler
    {
        void Handle(StatusId statusId);
    }
}

using Combat.API.DTO;
using Combat.Common.Primitives;

namespace Combat.API.Capabilities
{

    public interface IStatusCapability
    {
        StatusId ApplyStatus(ApplyStatusInfo applyStatusInfo);

        void RemoveStatus(StatusId statusId);
    }
}

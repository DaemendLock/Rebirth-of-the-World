using Combat.API.Capabilities;
using Combat.API.DTO;
using Combat.Common.Primitives;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Scripting.Capabilities.Units
{
    public sealed class StatusApplyRemoveCapabilty : IStatusCapability
    {
        private readonly UnitId _owner;
        private readonly StatusOwnerApplyUseCase _statusOwnerApplyUseCase;
        private readonly StatusOwnerRemoveUseCase _statusRemoveUseCase;

        public StatusApplyRemoveCapabilty(UnitId owner, StatusOwnerApplyUseCase statusOwnerApplyUseCase, StatusOwnerRemoveUseCase statusRemoveUseCase)
        {
            _owner = owner;
            _statusOwnerApplyUseCase = statusOwnerApplyUseCase;
            _statusRemoveUseCase = statusRemoveUseCase;
        }

        public StatusId ApplyStatus(ApplyStatusInfo applyStatusInfo) => _statusOwnerApplyUseCase.Execute(new(_owner, applyStatusInfo.Name, applyStatusInfo.Duration, applyStatusInfo.StackCount, applyStatusInfo.Source));

        public void RemoveStatus(StatusId statusId) => _statusRemoveUseCase.Execute(_owner, statusId);
    }
}

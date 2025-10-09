using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ApplyStatusUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IApplyStatusEventHandler _applyStatusEventHandler;
        private readonly StatusFactory _statusFactory;

        public ApplyStatusUseCase(IStatusRepository statusRepository, StatusFactory statusFactory, IApplyStatusEventHandler applyStatusEventHandler)
        {
            _statusRepository = statusRepository;
            _statusFactory = statusFactory;
            _applyStatusEventHandler = applyStatusEventHandler;
        }

        public void Execute(EntityId parent, StatusName statusName, int stackCount, float duration, EventSource source)
        {
            Status status = _statusFactory.Create(statusName, parent, duration, stackCount, source);
            _statusRepository.Create(status);
            _applyStatusEventHandler.HandleEvent(status);
        }
    }

    public interface IApplyStatusOutput
    {
        void Present(Status status);
    }

    public interface IApplyStatusEventHandler
    {
        void HandleEvent(Status status);
    }
}

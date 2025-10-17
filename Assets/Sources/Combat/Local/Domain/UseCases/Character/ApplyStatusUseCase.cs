using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

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

        public void Execute(ApplStatusDTO data)
        {
            Status status = _statusFactory.Create(data.StatusName, data.Parent, data.InitialDuration, data.InitialStackCount, new(data.Caster, data.Skill));
            _statusRepository.Create(status);
            _applyStatusEventHandler.HandleEvent(status);
        }
    }
}

using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;
using System.Linq;

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
            IReadOnlyCollection<Status> statuses = _statusRepository.FindStatusesWithParent(data.Parent);

            StatusName name = data.StatusName;

            if (statuses.Any(value => value.Name == name))
            {
                Status target = statuses.First(value => value.Name == name);

                if (target.Caster == data.Caster)
                {
                    target.Duration = new(0, data.InitialDuration);
                    
                    return;
                }
            }

            Status status = _statusFactory.Create(data.StatusName, data.Parent, data.InitialDuration, data.InitialStackCount, new(data.Caster, data.Skill));
            _statusRepository.Create(status);
            _applyStatusEventHandler.HandleEvent(status);
        }
    }
}

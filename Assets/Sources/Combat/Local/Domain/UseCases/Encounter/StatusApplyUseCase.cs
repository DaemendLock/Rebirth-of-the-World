using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct StatusApplyUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly StatusFactory _statusFactory;

        public StatusApplyUseCase(IStatusRepository statusRepository, StatusFactory statusFactory, IStatusOwnerRepository statusOwnerRepository)
        {
            _statusRepository = statusRepository;
            _statusFactory = statusFactory;
            _statusOwnerRepository = statusOwnerRepository;
        }

        public void Execute(ApplStatusDTO data)
        {
            if (TryReapplyStatus(data))
            {
                return;
            }

            Status status = _statusFactory.Create(data.StatusName, data.Target, data.InitialDuration, data.InitialStackCount, data.Ability);
            RegisterStatus(data.Target, status);
            status.Properties.Apply();
        }

        private bool TryReapplyStatus(ApplStatusDTO data)
        {
            StatusOwner statusOwner = _statusOwnerRepository.Get(data.Target);

            ReadOnlySpan<StatusId> ids = statusOwner.GetAll();

            StatusType name = data.StatusName;

            foreach (StatusId id in ids)
            {
                if (_statusRepository.TryGet(id, out Status status) == false)
                {
                    continue;
                }

                if (status.Name != name)
                {
                    continue;
                }

                status.RefreshDuration(data.InitialDuration);
                _statusRepository.Update(status);
                return true;
            }

            return false;
        }

        private void RegisterStatus(UnitId parent, Status status)
        {
            StatusOwner statusOwner = _statusOwnerRepository.Get(parent);
            var buffer = statusOwner.GetAll();
            System.Span<StatusId> values = stackalloc StatusId[buffer.Length + 1];
            buffer.CopyTo(values);
            values[^1] = status.Id;
            _statusOwnerRepository.Update(new(statusOwner.Id, values));
            _statusRepository.Create(status);
        }
    }
}

using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Scripting.Ports.StatusPorts
{
    public sealed class StatusPropertyDamageModifcationCalculator : IDamageModifierCalculator
    {
        private readonly IStatusRepository _statusRepository;

        public StatusPropertyDamageModifcationCalculator(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public DamageModification GetAttackerDamageModification(ReadOnlySpan<StatusId> values, in DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRepository.TryGet(id, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out IModifyParentOutgoingDamageStrategy effect) == false)
                {
                    continue;
                }

                result += effect.GetModification(instance);
            }

            return result;
        }

        public DamageModification GetDefenderDamageModification(ReadOnlySpan<StatusId> values, in DamageInstance instance)
        {
            DamageModification result = new(0, 0, 0, DamageFlags.None);

            foreach (StatusId id in values)
            {
                if (_statusRepository.TryGet(id, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out IModifyParentIncomingDamageStrategy effect) == false)
                {
                    continue;
                }

                result += effect.GetModification(instance);
            }

            return result;
        }
    }
}

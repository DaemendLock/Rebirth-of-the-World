using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.UseCases
{
    public sealed class StatusOwnerProgressAllUseCases
    {
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;
        private readonly IStatusTickHandler _statusTickHandler;

        public StatusOwnerProgressAllUseCases(IStatusTimerRepository statusTimerRepository, IStatusOwnerRepository statusOwnerRepository, IStatusTickHandler statusTickHandler, IStatusLifecycleHandler statusLifecycleHandler)
        {
            _statusTimerRepository = statusTimerRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusLifecycleHandler = statusLifecycleHandler;
            _statusTickHandler = statusTickHandler;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float deltaTime)
        {
            foreach (Updatable target in targets)
            {
                ProgressTarget(target.Id, deltaTime * target.TimeScale);
            }
        }

        private void ProgressTarget(UnitId target, float deltaTime)
        {
            ref StatusOwner statusOwner = ref _statusOwnerRepository.Get(target);

            Span<StatusInstance> instances = statusOwner.GetAll();

            for (int i = 0; i < instances.Length; i++)
            {
                ProgressStatus(ref instances[i], deltaTime);
            }

            for (int i = 0; i < instances.Length; i++)
            {
                ProgressTimer(instances[i].StatusId, deltaTime);
            }

            for (int i = 0; i < instances.Length; ++i)
            {
                StatusInstance instance = instances[i];

                if (instance.Duration.Left > 0)
                {
                    continue;
                }

                if (_statusLifecycleHandler.Expire(instance.StatusId))
                {
                    instances[i] = instance.MarkDead();

                    if (statusOwner.NeedCleanup == false)
                    {
                        statusOwner = statusOwner.MarkDirty();
                    }
                }
            }
        }

        private void ProgressStatus(ref StatusInstance instance, float deltaTime)
        {
            Duration duration = instance.Duration.Progress(deltaTime);
            StatusInstance newInstance = new(instance.StatusId, instance.Type, duration);
            instance = newInstance;
        }

        private void ProgressTimer(StatusId id, float deltaTime)
        {
            if (_statusTimerRepository.TryGet(id, out StatusTimer timer))
            {
                timer.TimePassed += deltaTime;

                if (timer.TimePassed > timer.Priod)
                {
                    _statusTickHandler.Handle(id);
                    timer.TimePassed -= timer.Priod;
                }

                _statusTimerRepository.Update(timer);
            }
        }
    }

    public interface IStatusExpiredEventHandler
    {
        void HandleEvent(StatusId statusId);
    }
}

using Combat.Domain.Events;
using Combat.Local.Application.Queries;

namespace Combat.Local.Presentation.Controllers
{
    public sealed class HitController
    {
        //private readonly HitsHandleUseCase

        private readonly IHitRecordQueue _hitRecordQueue;

        public void RegisterHit()
        {
            HitEvent record = new();
            _hitRecordQueue.Enqueue(record);
        }
    }
}

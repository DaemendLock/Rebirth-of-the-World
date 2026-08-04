using Combat.API.Statuses;

namespace Combat.API.Scripting
{
    public class StatusScript : IStatusPropery
    {
        private IStatusApi _instance;

        protected IUnit Parent => _instance.Parent;

        protected IAbilityApi Source => _instance.Source;

        protected IStatusApi Instance => _instance;

        public virtual void OnCreate() { }

        public virtual void OnExpire() { }

        public virtual void OnRemove() { }

        public virtual void OnTick() { }

        public void Init(IStatusApi instacne)
        {
            _instance = instacne;
        }
    }
}

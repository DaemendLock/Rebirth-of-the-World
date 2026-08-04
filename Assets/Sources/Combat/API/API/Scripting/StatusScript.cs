using Combat.API.Statuses;

namespace Combat.API.Scripting
{
    public class StatusScript : IStatusPropery
    {
        private StatusApi _instance;

        protected Unit Parent => _instance.Parent;

        protected AbilityApi Source => _instance.Source;

        protected StatusApi Instance => _instance;

        public virtual void OnCreate() { }

        public virtual void OnExpire() { }

        public virtual void OnRemove() { }

        public virtual void OnTick() { }

        public void Init(StatusApi instacne)
        {
            _instance = instacne;
        }
    }
}

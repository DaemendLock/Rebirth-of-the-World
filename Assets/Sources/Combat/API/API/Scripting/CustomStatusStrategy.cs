using Combat.API.Statuses;

namespace Combat.API.Scripting
{
    public class CustomStatusStrategy : IStatusPropery
    {
        private StatusApi _instance;

        protected Unit Parent => _instance.Parent;

        protected SkillApi Source => _instance.Source;

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

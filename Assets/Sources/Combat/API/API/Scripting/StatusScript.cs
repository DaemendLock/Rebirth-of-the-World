using Combat.API.DTO;
using Combat.API.Statuses;

namespace Combat.API.Scripting
{
    public class StatusScript : IStatusPropery
    {
        private Unit _parent;
        private SkillApi _source;
        private StatusApi _instance;

        protected Unit Parent => _parent;

        protected SkillApi Source => _source;

        protected StatusApi Instance => _instance;

        public virtual void OnCreate() { }

        public virtual void OnExpire() { }

        public virtual void OnRemove() { }

        public virtual void OnTick() { }

        internal void Init(ScriptedStatusContext context)
        {
            _parent = context.Parent;
            _source = context.Source;
            _instance = context.Instacne;
        }
    }
}

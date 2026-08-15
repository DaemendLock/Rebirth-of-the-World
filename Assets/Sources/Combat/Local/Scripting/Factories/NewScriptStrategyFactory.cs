using Combat.API;
using Combat.API.API.Skills;
using Combat.API.Contexts;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Skill;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Factories
{
    public sealed class NewScriptStrategyFactory : ISkillRuntimeFactory
    {
        private readonly ISkillDynamicMemoryRepository _skillMemoryRepository;
        private readonly IEventContext _eventContext;
        private readonly UnitNewAdapter _unitNewAdapter;

        public NewScriptStrategyFactory(ISkillDynamicMemoryRepository skillMemoryRepository, UnitNewAdapter unitNewAdapter, IEventContext eventContext)
        {
            _skillMemoryRepository = skillMemoryRepository;
            _unitNewAdapter = unitNewAdapter;
            _eventContext = eventContext;
        }

        public bool CanHandle(SkillId skillId) => true;

        public SkillRuntime Create(UnitId? owner, SkillId skillId)
        {
            UnitNew unitNew;

            if (owner.HasValue)
            {
                unitNew = _unitNewAdapter.Adaptee(owner.Value);
            }
            else
            {
                unitNew = null;
            }

            DomainSkillContext context = new(new(owner, skillId), _skillMemoryRepository, _eventContext);
            NewScriptCapabilityContainer container = new(unitNew, new TestScript());
            return new(context, container);
        }
    }
}

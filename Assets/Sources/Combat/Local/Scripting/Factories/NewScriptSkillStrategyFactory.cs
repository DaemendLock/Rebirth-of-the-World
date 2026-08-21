using Combat.API.Adapters;
using Combat.API.API.Skills;
using Combat.API.Contexts;
using Combat.Common.Primitives;
using Combat.Local.Domain.Repositories.Skill;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Idk;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Factories
{
    public sealed class NewScriptSkillStrategyFactory : ISkillRuntimeFactory
    {
        private readonly ISkillDynamicMemoryRepository _skillMemoryRepository;
        private readonly IEventContext _eventContext;
        private readonly ISceneApiAdapter _sceneApiAdapter;

        public NewScriptSkillStrategyFactory(ISkillDynamicMemoryRepository skillMemoryRepository, IEventContext eventContext, ISceneApiAdapter sceneApiAdapter)
        {
            _skillMemoryRepository = skillMemoryRepository;
            _eventContext = eventContext;
            _sceneApiAdapter = sceneApiAdapter;
        }

        public bool CanHandle(SkillId skillId) => true;

        public SkillRuntime Create(AbilityKey abilityKey)
        {
            DomainSkillContext context = new(abilityKey, _skillMemoryRepository, _eventContext, _sceneApiAdapter.Get().Context);
            NewScriptCapabilityContainer container = new(new GrowSelfSkillScript());

            return new(context, container);
        }
    }
}

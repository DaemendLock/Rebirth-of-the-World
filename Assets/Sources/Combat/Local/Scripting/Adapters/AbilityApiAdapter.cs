using Combat.API;
using Combat.API.Adapters;
using Combat.Common.Primitives;
using Combat.Domain.Repositories;
using Combat.Domain.ValueObjects;
using Combat.Local.Scripting.Contexts;

namespace Combat.Local.Scripting.Adapters
{
    public readonly struct AbilityApiAdapter
    {
        private readonly CharacterApiAdapter _characterApiProvider;
        private readonly ISceneApiAdapter _sceneApiProvider;
        private readonly ISkillInfoRepository _abilityFactory;

        public AbilityApiAdapter(CharacterApiAdapter characterApiProvider, ISceneApiAdapter sceneApiProvider,
            ISkillInfoRepository abilityFactory)
        {
            _characterApiProvider = characterApiProvider;
            _sceneApiProvider = sceneApiProvider;
            _abilityFactory = abilityFactory;
        }

        public AbilityApi Adaptee(AbilityKey abilityKey)
        {
            SkillInfo ability = _abilityFactory.Get(abilityKey.Skill);
            var context = new DomainAbilityContext(abilityKey.Skill,
                abilityKey.Owner.HasValue ? _characterApiProvider.Adaptee(abilityKey.Owner.Value) : null,
                ability.Flags,
                _sceneApiProvider.Get());
            return new(context);
        }
    }
}

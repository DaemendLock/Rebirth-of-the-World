using Combat.API;
using Combat.API.Adapters;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Scripting.Contexts;

namespace Combat.Local.Scripting.Adapters
{
    public readonly struct AbilityApiAdapter
    {
        private readonly CharacterApiAdapter _characterApiProvider;
        private readonly ISceneApiAdapter _sceneApiProvider;
        private readonly IAbilityFactory _abilityFactory;

        public AbilityApiAdapter(CharacterApiAdapter characterApiProvider, ISceneApiAdapter sceneApiProvider,
            IAbilityFactory abilityFactory)
        {
            _characterApiProvider = characterApiProvider;
            _sceneApiProvider = sceneApiProvider;
            _abilityFactory = abilityFactory;
        }

        public AbilityApi Adaptee(AbilityKey abilityKey)
        {
            Ability ability = _abilityFactory.Create(abilityKey.Skill, abilityKey.Owner);
            var context = new DomainAbilityContext(abilityKey.Skill,
                abilityKey.Owner.HasValue ? _characterApiProvider.Adaptee(abilityKey.Owner.Value) : null,
                ability.Flags,
                _sceneApiProvider.Get());
            return new(context);
        }
    }
}

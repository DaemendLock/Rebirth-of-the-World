using Combat.API;
using Combat.API.Adapters;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Scripting.Contexts;

namespace Combat.Local.Scripting.Adapters
{
    public readonly struct AbilityApiAdapter
    {
        private readonly CharacterApiAdapter _characterApiProvider;
        private readonly ISceneApiAdapter _sceneApiProvider;
        private readonly AbilityFacade _skillFacade;

        public AbilityApiAdapter(CharacterApiAdapter characterApiProvider, ISceneApiAdapter sceneApiProvider, AbilityFacade skillFacade)
        {
            _characterApiProvider = characterApiProvider;
            _sceneApiProvider = sceneApiProvider;
            _skillFacade = skillFacade;
        }

        public AbilityApi Adaptee(AbilityKey abilityKey)
        {
            var context = new DomainAbilityContext(abilityKey.Skill,
                abilityKey.Owner.HasValue ? _characterApiProvider.Adaptee(abilityKey.Owner.Value) : null,
                _skillFacade,
                _sceneApiProvider.Get());
            return new(context);
        }
    }
}

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.API.Adapters
{
    public readonly struct AbilityApiAdapter
    {
        private readonly CharacterApiAdapter _characterApiProvider;
        private readonly SceneApiAdapter _sceneApiProvider;
        private readonly AbilityFacade _skillFacade;

        public AbilityApiAdapter(CharacterApiAdapter characterApiProvider, SceneApiAdapter sceneApiProvider, AbilityFacade skillFacade)
        {
            _characterApiProvider = characterApiProvider;
            _sceneApiProvider = sceneApiProvider;
            _skillFacade = skillFacade;
        }

        public AbilityApi Adaptee(AbilityKey abilityKey)
        {
            return new(abilityKey.Skill, abilityKey.Owner.HasValue ? _characterApiProvider.Adaptee(abilityKey.Owner.Value) : null, _skillFacade, _sceneApiProvider.Get());
        }
    }
}

using Combat.Common.Primitives;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases.Encounter;
using Combat.Local.Gateways.DataSources;

namespace Combat.Local.Gateways.Repositories.Encounter
{
    public sealed class CharacterDefinitionProvider : ICharacterDefinitionProvider
    {
        private readonly ICombatCharacterDataBase _dataSource;

        public CharacterDefinitionProvider(ICombatCharacterDataBase dataSource)
        {
            _dataSource = dataSource;
        }

        public CharacterDefinition Get(CharacterKey key)
        {
            if (_dataSource.TryGet(key, out var value) == false)
            {
                throw new System.InvalidOperationException("Key is not present");
            }

            return value.Parse();
        }
    }
}
